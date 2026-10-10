using UnityEngine;

public class Portal : MonoBehaviour
{
    public float debouncedDuration = 2f;
    public World world;

    [System.Serializable]
    public class TriggerComponent : MonoBehaviour {
        private World targetWorld;
        private System.Action targetAction; 
        private float lastTriggerTime = 0;

        public float debouncedDuration;
        public float maxDebounceInterval;

        public void setupWorld(World world) {
            this.targetWorld = world;
            this.targetAction = null;
        }

        public void setupOpenLab() {
            this.targetWorld = null;
            this.targetAction = LearningWorldsApp.OpenLab;
        }

        private void OnTriggerEnter(Collider other) {
            // Debounce portal action
            if (Time.time - lastTriggerTime > debouncedDuration)
            {
                lastTriggerTime = Time.time;

                // Invoke either world transition or custom action
                if (targetWorld != null) {
                    LearningWorldsApp.Go(targetWorld);
                } else if (targetAction != null) {
                    targetAction();
                }
                Debug.Log($
    }

    public event System.Action LaserCalculationApplied;

    public static GameObject Create(string label, World world, Vector3 pos, float yaw = 0)
    {
        // Portal container
        GameObject door = new GameObject("Portal_." + label){
            transform = {
                position = pos,
                rotation = Quaternion.Euler(0f, yaw, 0f),
            },
        };
        
        float postLength = 2.5f;
        CreatePost(door.transform, new Vector3(-0.75f, postLength / 2, 0f));
        CreatePost(door.transform, new Vector3(0.75f, postLength / 2, 0f));

        // Lintel
        GameObject lintel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lintel.transform.SetParent(door.transform);
        lintel.transform.localPosition = new Vector3(0f, postLength, 0f);
        lintel.transform.localScale = new Vector3(1.5f, 0.075f, 0.2f);
        lintel.GetComponent<Renderer>().material.color = new Color32(51, 51, 51, 255);

        // Emissive cyan plane
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane.transform.SetParent(door.transform);
        plane.transform.localPosition = new Vector3(0f, postLength / 2, -0.01f);
        plane.transform.localScale = new Vector3(1.5f, 1.5f, 0.05f);
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        SetEmissive(planeRenderer, Color.Cyan * 0.7f);

        // Label
        TextMesh labelText = new GameObject("Label_" + label)
            .AddComponent<TextMesh>();
        labelText.text = label;
        labelText.fontSize = 30;
        labelText.characterSize = 0.03f;
        labelText.anchor = TextAnchor.MiddleCenter;
        labelText.alignment = TextAlignment.Center;
        labelText.transform.SetParent(door.transform, false);
        labelText.transform.localPosition = new Vector3(0f, 2f, 0f);

        // Trigger collider
        GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        trigger.transform.SetParent(door.transform);
        trigger.transform.localScale = new Vector3(1.52f, 2.52f, 0.1f);  
        trigger.transform.localPosition = new Vector3(0f, 0f, -0.02f);
        Collider collider = trigger.GetComponent<Collider>();
        collider.isTrigger = true;
        trigger.gameObject.layer = LayerMask.NameToLayer("Trigger");

        TriggerComponent triggerComponent = trigger.AddComponent<TriggerComponent>();

        // Post triggers
        triggerComponent.setupWorld(world);

        // Check specific portal
        if (label == "Forschungslabor") {
            triggerComponent.setupOpenLab();
        }

        // Optional effect
        PulseEffectSetup(door.transform, new Vector3(0f, 1f, 0f));
        
        return door;
    }
    
    /// <summary>Helper to place portal post</summary>
    private static void CreatePost(Transform targetRoot, Vector3 localPosition)
    {
        GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
        post.transform.SetParent(targetRoot);
        post.transform.localPosition = localPosition;
        post.transform.localScale = new Vector3(0.2f, 2.5f, 0.2f);
        post.GetComponent<Renderer>().material.color = new Color32(51, 51, 51, 255);
    }
    
    /// <summary>Pulsating object helper</summary>
    private static void PulseEffectSetup(Transform source, Vector3 offset)
    {
        GameObject pulse = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pulse.transform.position = source.TransformPoint(offset);
        pulse.transform.localScale = Vector3.one * 0.5f;
        Renderer pulseRenderer = pulse.GetComponent<Renderer>();
        pulseRenderer.material.color = Color.magenta;
        pulse.AddComponent<RotateInPlace>().rotationSpeed = 360f;
    }
    
    /// <summary>Helps for emissive materials</summary>
    private static void SetEmissive(Renderer render, Color color)
    {
        render.material.EnableKeyword("_EMISSION");
        render.material.SetColor("_EmissionColor", color);
        render.material.color = Color.white;
    }
}