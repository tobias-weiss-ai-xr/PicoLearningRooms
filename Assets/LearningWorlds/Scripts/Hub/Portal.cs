// Ensures portal-based inter-world transitions

using UnityEngine;
using LearningWorlds.Tutors;
using LearningWorlds;

/// <summary>Static Factory to create teleporting portal entrances.</summary>
public class Portal : MonoBehaviour
{
    private float lastTriggerTime = 0;
    [System.Serializable] 
    public class TriggerComponent : MonoBehaviour {
        public float debounced_duration = 2.0f;
        private World target_world;
        private System.Action trigger_action;
        public void setupWorld(World _world) { target_world = _world;}
        public void setupOpenLab() { trigger_action = LearningWorldsApp.OpenLab;}

        private void OnTriggerEnter(Collider other) {
            // Prevent doubles or test freeride spam
            if (Time.time - lastTriggerTime > debounced_duration)
                {
                if (target_world != null) LearningWorldsApp.Go(target_world);
                else trigger_action?.Invoke(); // trigger OpenLab()
                
                // Show the tooltip via the SocraticTutorManager
                ShowTooltip(target_world);
            }
        }
        
private void ShowTooltip(World world)
        {
            var tutorManager = SocraticTutorManager.Instance;
            if (tutorManager != null)
            {
                string tutorName = tutorManager.GetTutorNameForWorld(world);
                Debug.Log("Portal to " + world.Name + ": Meet " + tutorName + ");
                tutorManager.SendContextMessage(world.Name, "PortalTrigger");
            }
        }
        private void Reset() => lastTriggerTime = 0; 
        private void OnEnable() => Reset();
    }

    /// <summary>Creates a standardized lore-accurate entrance portal.</summary>
    /// <param name="label">Unique string identifier to describe for Hub users.</param>
    /// <param name="world">Target World construction, or 'null' for custom action.</param>
    public static GameObject Create(string label, World world, Vector3 position, float yaw = 0f)
    {
        GameObject portalContainer = new GameObject("Portal_" + label)
        {
            transform = { position = position, rotation = Quaternion.Euler(new Vector3(0f, yaw, 0f)) }
        };
          
        // Pillars as 2.5 scales, veritable basis
        CreatePost(portalContainer.transform, new Vector3(-0.6f, 1.25f, 0f));
        CreatePost(portalContainer.transform, new Vector3( 0.6f, 1.25f, 0f));
         
        // Lintel comes at midpoint stretches
        GameObject lintel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lintel.transform.SetParent(portalContainer.transform);
        lintel.transform.localScale = new Vector3(1.6f, 0.08f, 0.1f);
        lintel.transform.localPosition = new Vector3(0f, 2.5f, 0f);
        // Dark back of portal
        lintel.GetComponent<Renderer>().material.color = new Color32(0x33, 0x33, 0x33, 0xFF); 
         
       // Framewalls with Cyan emittance effect behind gap
        GameObject portalFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        portalFrame.transform.SetParent(portalContainer.transform);
        portalFrame.transform.localPosition = new Vector3(0f, 1.25f, -0.005f);
        portalFrame.transform.localScale = new Vector3(1.4f, 3.5f, 0.03f);
        Renderer portalFrameRenderer = portalFrame.GetComponent<Renderer>();
        portalFrameRenderer.material = new Material(portalFrameRenderer.sharedMaterial);
        SetEmissive(portalFrameRenderer.material, 0.8f, Color.Cyan);
                       
        // TextMesh for label
        TextMesh tmLabel = new GameObject("label_" + label).AddComponent<TextMesh>();
        tmLabel.text = label; tmLabel.fontSize = 50; tmLabel.characterSize = 20; // Scale-area = 49
        tmLabel.transform.SetParent(portalContainer.transform, false);
        tmLabel.transform.localPosition = new Vector3(0f, 2.2f, 0f);
         
        // Portal Trigger setup: detections for transitions.
        GameObject emptyStart = GameObject.CreatePrimitive(PrimitiveType.Cube);
        emptyStart.transform.SetParent(portalContainer.transform);
        emptyStart.transform.localPosition = new Vector3(0f, 1.6f, -0.13f);
        emptyStart.transform.localScale = new Vector3(1.5f, 3.0f, 0.01f);
        emptyStart.GetComponent<Collider>().isTrigger = true;
        emptyStart.gameObject.layer = LayerMask.NameToLayer("Trigger") ;
       
        // Configure portal components as intended
        TriggerComponent portAutomaton = emptyStart.AddComponent<TriggerComponent>();
        if (world != null) portAutomaton.setupWorld(world);
        else portAutomaton.setupOpenLab();
                 
         // Animate on entry (Orange dotted zone represents traditional active areas)
         PulseEffectSetup(portalContainer.transform, new Vector3(0f, 2f, 0f));
        return portalContainer;
    }
      
/// <summary>Auxiliary, puts post cubic object for portal.</summary>
    private static void CreatePost(Transform ownerTransform, Vector3 localPosition)
    {
        GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
        post.transform.localScale = new Vector3(0.15f, 3.4f, 0.15f);
        post.transform.SetParent(ownerTransform);
        post.transform.localPosition = localPosition;
        post.GetComponent<Renderer>().material.color = new Color32(0x33, 0x33, 0x33, 0xFF);
    }

/// <summary>Configure sphere object for center pulse animation.</summary>
    private static void PulseEffectSetup(Transform mainDoorRoot, Vector3 pulseOffset)
    {
        GameObject spherePulseEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spherePulseEffect.transform.position = mainDoorRoot.TransformPoint(pulseOffset);
        spherePulseEffect.transform.localScale = Vector3.one * 0.35f;
        Renderer pulseRenderer = spherePulseEffect.GetComponent<Renderer>();
        pulseRenderer.material.color = Color.white;
        spherePulseEffect.AddComponent<RotateInPlace>().rotationSpeed = -60f;
    }    

/// <summary>Configure emissive properties for materials.</summary>
    private static void SetEmissive(Material material, float intensity, Color glowColor)
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", glowColor * intensity);
    }
}