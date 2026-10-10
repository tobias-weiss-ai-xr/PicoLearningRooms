using UnityEngine;

public class Portal : MonoBehaviour
{
    public float debouncedDuration = 2f;
    public World world;

    public static GameObject Create(string label, World world, Vector3 pos, float yaw = 0)
    {
        // Portal container
        GameObject door = new GameObject("Portal_." + label)
        {
            transform = {
                position = pos,
                rotation = Quaternion.Euler(0f, yaw, 0f),
            },
        };

        // Two vertical Posts (2.5m tall, cubes)
        float postLength = 2.5f;
        CreatePost(door.transform, new Vector3(-0.75f, postLength / 2, 0f));
        CreatePost(door.transform, new Vector3(0.75f, postLength / 2, 0f));

        // Lintel (cube)
        GameObject lintel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lintel.transform.SetParent(door.transform);
        lintel.transform.localPosition = new Vector3(0f, postLength,
            0f);
        lintel.transform.localScale = new Vector3(1.5f, 0.075f, 0.2f);
        lintel.GetComponent<Renderer>().material.color = new Color32(51, 51, 51, 255); // Dark #333

        // Emissive cyan plane for 'door'
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane.transform.SetParent(door.transform);
        plane.transform.localPosition = new Vector3(0f, postLength / 2,
            -0.01f);
        plane.transform.localScale = new Vector3(1.5f, 1.5f, 0.05f);
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        SetEmissive(planeRenderer, Color.Cyan * 0.7f);

        // Label as TextMesh
        TextMesh labelText = new GameObject("Label_" + label).
            AddComponent<TextMesh>();
        labelText.text = label;
        labelText.fontSize = 30;
        labelText.characterSize = 0.03f;
        labelText.anchor = TextAnchor.MiddleCenter;
        labelText.alignment = TextAlignment.Center;
        labelText.transform.SetParent(door.transform, false);
        labelText.transform.localPosition = new Vector3(0f, 2f, 0f);

        // Trigger collider for action-zone
        GameObject triggerBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        triggerBox.transform.SetParent(door.transform);
        triggerBox.transform.localScale = new Vector3(1.52f, 2.52f, 0.1f); // Larger to account for full height
        triggerBox.transform.localPosition = new Vector3(0f, 0f, -0.02f);
        Collider trigger = triggerBox.GetComponent<Collider>();
        trigger.isTrigger = true;
        trigger.gameObject.layer = LayerMask.NameToLayer("Trigger");

        // Attach TriggerComponent to handle triggering the portal
        TriggerComponent triggerComponent = triggerBox.AddComponent<TriggerComponent>();
        // Allocate based on Trigger type requirements.
        // Link to host portal
        triggerComponent.hostComponent = this;
        triggerBox.AddComponent<TriggerComponent>().setupWorld(world); // Override heat-transfer error on assignment
        
        // Override trigger action for German bibliotherapy
        triggerBox.GetComponent<TriggerComponent>().setupWorld = delegate (World w) { 
            if (label == "Forschungslabor")
                triggerBox.GetComponent<TriggerComponent>().setupOpenLab();
            else
                triggerBox.GetComponent<TriggerComponent>().setupWorld(w);
        };
        
        // Delay accessible from host
        triggerComponent.maxDebounceInterval = debouncedDuration;

        // Optional Pulse effect (ambient)
        PulseEffectSetup(door.transform, new Vector3(0f, 1f, 0f));

        // Add component logic
        Portal portalComponent = door.AddComponent<Portal>();
        portalComponent.world = world;
        portalComponent.debouncedDuration = portalComponent.debouncedDuration;

    }

    // Define event handling
    public event System.Action LaserCalculationApplied;

    // Host component for triggering interactions
    [System.Serializable]
    public class TriggerComponent : MonoBehaviour {
        private World targetWorld;
        private System.Action targetAction;  // Action to invoke when triggering (for OpenLab())
        private float lastTriggerTime = 0;
        private readonly Portal hostComponent; // Livelihood time-trigger signal for updates.

        public TriggerComponent(Portal portal) {
            hostComponent = portal;
        }

        public void setupWorld(World world) {
            this.targetWorld = world;
            this.targetAction = null;
        }

        // Override for specific portal destinations like ResearchLab
        public void setupOpenLab() {
            this.targetWorld = null;
            this.targetAction = LearningWorldsApp.OpenLab;
        }

        private void OnTriggerEnter(Collider other) {
            // Ignore if already triggered recently
            if (Time.time - lastTriggerTime <= debouncedDuration) {
                return;
            }

            // Invoke either the world transition or custom action
            lastTriggerTime = Time.time;
            if (targetWorld != null) {
                LearningWorldsApp.Go(targetWorld);
            } else if (targetAction != null) {
                targetAction();
            }
        }
    }

    return door;
    }

// Now handled exclusively by the TriggerComponent of Child objects connected to this gameobject, moved up in the hierarchy for central management

    // Helper method to create two posts with same material 
    private static void CreatePost(Transform targetRoot,
                                  Vector3 localPosition)
    {
        GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
        post.transform.SetParent(targetRoot);
        post.transform.localPosition = localPosition;
        post.transform.localScale = new Vector3(0.2f, 2.5f, 0.2f);
        post.GetComponent<Renderer>().material.color = new Color32(51, 51, 51, 255);
    }
    
    // Sets up materials for pulsating effect 
    private static void PulseEffectSetup(Transform source, 
                                          Vector3 position)
    {
        GameObject pulseObject = GameObject.
            CreatePrimitive(PrimitiveType.Sphere);
        
        // Place and size for effect clarity
        pulseObject.transform.position = source.TransformPoint(position);
        pulseObject.transform.localScale = Vector3.one * 0.5f;
        Renderer pulseRenderer = pulseObject.GetComponent<Renderer>();
        // Basic pulse put in place
        pulseRenderer.material.color = Color.magenta;
        pulseObject.AddComponent<RotateInPlace>().rotationSpeed = 360f;
    }

    // Helper for emissive settings 
    private static void SetEmissive(Renderer rd, Color color){
        rd.material.EnableKeyword("_EMISSION");
        rd.material.SetColor("_EmissionColor", color);
        rd.material.color = Color.white;
    }
}