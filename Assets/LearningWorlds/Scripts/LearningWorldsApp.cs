using UnityEngine;

namespace LearningWorlds {
    public class LearningWorldsApp : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        [SerializeField]
        private DefaultFont defaultFont;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            // Setup minimal learning world
            GameObject worldCanvas = GameObject.CreatePrimitive(PrimitiveType.Cube);
            worldCanvas.name = "LearningWorlds-Canvas";
            worldCanvas.transform.localScale = new Vector3(3f, 3f, 3f);
            
            // Add basic runtime configuration here
            Shader universalShader = Shader.Find("Universal Render Pipeline/Lit");
            if (universalShader != null) {
                worldCanvas.GetComponent<Renderer>().sharedMaterial.shader = universalShader;
            } else {
                Debug.LogWarning("URP/Lit shader missing; fallback to Standard.");
                worldCanvas.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Standard");
            }
            
            // Initialize UI Elements
            ApplyRuntimeFont();
        }

        private void ApplyRuntimeFont() {
            // Load a default font for runtime
            Font runtimeFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (runtimeFont != null) {
                Debug.Log("Applied runtime font: LegacyRuntime.ttf.");
            } else {
                Debug.LogWarning("Runtime font missing; using default.");
                runtimeFont = Font.CreateDynamicFontFromOSFont();
            }
        }
    }
    
    [System.Serializable]
    public class DefaultFont {
        public string FontAsset; // Embedded UI Font
    }
}