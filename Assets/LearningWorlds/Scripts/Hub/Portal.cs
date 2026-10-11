using UnityEngine;
using LearningWorlds;

namespace LearningWorlds
{
    /// <summary>
    /// Portal system for transitioning between learning worlds.
    /// Creates interactive portals with trigger colliders that load target scenes.
    /// </summary>
    public class Portal : MonoBehaviour
    {
        private float lastTriggerTime = 0f;
        private const float TriggerCooldown = 2.0f;

        /// <summary>
        /// Trigger component attached to portal colliders.
        /// Handles world transitions when players enter the trigger.
        /// </summary>
        [System.Serializable]
        public class TriggerComponent : MonoBehaviour
        {
            public float debounce_duration = TriggerCooldown;
            private World target_world;
            private System.Action trigger_action;

            /// <summary>
            /// Set the target world for this portal.
            /// </summary>
            public void setupWorld(World world)
            {
                target_world = world;
            }

            /// <summary>
            /// Set a custom action to trigger (e.g., OpenLab).
            /// </summary>
            public void setupOpenLab()
            {
                trigger_action = LearningWorldsApp.OpenLab;
            }

            /// <summary>
            /// Show tooltip for the tutor associated with this portal's world.
            /// </summary>
            private void ShowTooltip(World world)
            {
                if (world != World.Hub) // Don't show tooltip for Hub portal
                {
                    string tutorName = SocraticTutorManager.Instance?.GetTutorNameForWorld(world);
                    if (!string.IsNullOrEmpty(tutorName))
                    {
                        Debug.Log("[Portal] Tutor available: " + tutorName);
                    }
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                // Check cooldown to prevent trigger spam
                if (Time.time - lastTriggerTime < debounce_duration)
                    return;

                // Only trigger for player (or objects on default layer)
                if (other.CompareTag("Player") || other.gameObject.layer == 0)
                {
                    // Execute world transition or custom action
                    if (target_world != default(World))
                    {
                        // Use GetAwaiter().GetResult() to wait for async Go() in non-async context
                        var task = LearningWorldsApp.Go(target_world);
                        if (task != null)
                        {
                            task.GetAwaiter().GetResult();
                        }
                    }
                    else if (trigger_action != null)
                    {
                        trigger_action.Invoke();
                    }

                    // Show tooltip after transition
                    ShowTooltip(target_world);

                    // Update cooldown
                    lastTriggerTime = Time.time;
                }
            }

            private void ResetTriggerTime()
            {
                lastTriggerTime = 0f;
            }

            private void OnEnable()
            {
                ResetTriggerTime();
            }
        }

        /// <summary>
        /// Creates a portal object with pillars, lintel, and trigger.
        /// </summary>
        /// <param name="label">Display text for the portal</param>
        /// <param name="world">Target world to transition to</param>
        /// <param name="position">World position for the portal</param>
        /// <param name="yaw">Rotation in degrees (Y-axis)</param>
        /// <returns>Created portal GameObject</returns>
        public static GameObject Create(string label, World world, Vector3 position, float yaw = 0f)
        {
            // Create portal container
            GameObject portalContainer = new GameObject("Portal_" + label);
            portalContainer.transform.position = position;
            portalContainer.transform.rotation = Quaternion.Euler(0f, yaw, 0f);

            // Create left and right pillars
            CreatePortalPost(portalContainer.transform, new Vector3(-0.6f, 1.25f, 0f));
            CreatePortalPost(portalContainer.transform, new Vector3(0.6f, 1.25f, 0f));

            // Create lintel (top bar)
            CreateLintel(portalContainer.transform);

            // Create portal frame
            CreatePortalFrame(portalContainer.transform);

            // Create emissive backdrop
            CreateEmissiveBackdrop(portalContainer.transform);

            // Create label
            CreateLabel(portalContainer.transform, label, new Vector3(0f, 2.45f, -0.5f));

            // Create trigger collider
            CreateTrigger(portalContainer.transform, world);

            // Create pulse effect
            CreatePulseEffect(portalContainer.transform);

            return portalContainer;
        }

        /// <summary>
        /// Creates a portal post (column).
        /// </summary>
        private static void CreatePortalPost(Transform parent, Vector3 localPosition)
        {
            GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
            post.transform.SetParent(parent);
            post.transform.localPosition = localPosition;
            post.transform.localScale = new Vector3(0.15f, 3.4f, 0.15f);
            post.name = "Post";

            DestroyImmediate(post.GetComponent<BoxCollider>());

            Renderer renderer = post.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color32(51, 51, 51, 255); // Dark gray
        }

        /// <summary>
        /// Creates the lintel (top horizontal bar).
        /// </summary>
        private static void CreateLintel(Transform parent)
        {
            GameObject lintel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lintel.transform.SetParent(parent);
            lintel.transform.localPosition = new Vector3(0f, 2.2f, 0f);
            lintel.transform.localScale = new Vector3(1.6f, 0.06f, 0.1f);
            lintel.name = "Lintel";

            DestroyImmediate(lintel.GetComponent<BoxCollider>());

            Renderer renderer = lintel.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color32(51, 51, 51, 255);
        }

        /// <summary>
        /// Creates the portal frame.
        /// </summary>
        private static void CreatePortalFrame(Transform parent)
        {
            GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frame.transform.SetParent(parent);
            frame.transform.localPosition = new Vector3(0f, 1.25f, -0.005f);
            frame.transform.localScale = new Vector3(1.4f, 3.5f, 0.03f);
            frame.name = "Frame";

            DestroyImmediate(frame.GetComponent<BoxCollider>());

            Renderer frameRenderer = frame.GetComponent<Renderer>();
            Material frameMaterial = new Material(Shader.Find("Standard"));
            frameMaterial.EnableKeyword("_EMISSION");
            frameMaterial.SetColor("_EmissionColor", Color.cyan * 0.8f);
            frameRenderer.material = frameMaterial;
        }

        /// <summary>
        /// Creates the emissive cyan backdrop.
        /// </summary>
        private static void CreateEmissiveBackdrop(Transform parent)
        {
            GameObject backdrop = GameObject.CreatePrimitive(PrimitiveType.Plane);
            backdrop.transform.SetParent(parent);
            backdrop.transform.localPosition = new Vector3(0f, 1.5f, -0.12f);
            backdrop.transform.localScale = new Vector3(1.8f, 1.1f, 1f);
            backdrop.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            backdrop.name = "Backdrop";

            DestroyImmediate(backdrop.GetComponent<BoxCollider>());

            Renderer backdropRenderer = backdrop.GetComponent<Renderer>();
            Material emissiveMat = new Material(Shader.Find("Standard"));
            emissiveMat.EnableKeyword("_EMISSION");
            emissiveMat.SetColor("_EmissionColor", Color.cyan * 1.3f);
            backdropRenderer.material = emissiveMat;
        }

        /// <summary>
        /// Creates a text label for the portal.
        /// </summary>
        private static void CreateLabel(Transform parent, string text, Vector3 localPosition)
        {
            GameObject labelObj = new GameObject("Label_" + text);
            TextMesh textMesh = labelObj.AddComponent<TextMesh>();
            labelObj.transform.SetParent(parent);
            labelObj.transform.localPosition = localPosition;

            textMesh.text = text;
            textMesh.characterSize = 0.25f;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;

            // Try to use a built-in font
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font != null)
            {
                textMesh.font = font;
            }
        }

        /// <summary>
        /// Creates the trigger collider for the portal.
        /// </summary>
        private static void CreateTrigger(Transform parent, World world)
        {
            GameObject triggerObj = new GameObject("Trigger");
            triggerObj.transform.SetParent(parent);
            triggerObj.transform.localPosition = new Vector3(0f, 1.6f, -0.5f);
            triggerObj.transform.localScale = new Vector3(1.5f, 3.0f, 0.5f);

            BoxCollider trigger = triggerObj.AddComponent<BoxCollider>();
            trigger.isTrigger = true;

            TriggerComponent triggerComponent = triggerObj.AddComponent<TriggerComponent>();
            triggerComponent.setupWorld(world);
        }

        /// <summary>
        /// Creates a pulsing sphere effect above the portal.
        /// </summary>
        private static void CreatePulseEffect(Transform parent)
        {
            GameObject pulseSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pulseSphere.transform.SetParent(parent);
            pulseSphere.transform.localPosition = new Vector3(0f, 2f, 0f);
            pulseSphere.transform.localScale = Vector3.one * 0.35f;
            pulseSphere.name = "PulseEffect";

            DestroyImmediate(pulseSphere.GetComponent<SphereCollider>());

            Renderer pulseRenderer = pulseSphere.GetComponent<Renderer>();
            pulseRenderer.material = new Material(Shader.Find("Standard"));
            pulseRenderer.material.color = Color.white;

            // Simple rotation for visual effect
            RotateInPlace rotator = pulseSphere.AddComponent<RotateInPlace>();
            if (rotator != null)
            {
                rotator.rotationSpeed = -60f;
            }
        }
    }

    /// <summary>
    /// Simple component to rotate an object in place.
    /// </summary>
    public class RotateInPlace : MonoBehaviour
    {
        public float rotationSpeed = 60f;

        void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
