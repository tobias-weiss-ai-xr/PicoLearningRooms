using UnityEngine;

namespace LearningWorlds {
    public class LearningWorldsApp : MonoBehaviour {
        // Runtime method to initialize this rig as a singleton and persistent across scenes.
        [RuntimeInitializeOnLoadMethod] 
        private static void Initialize() {
            var appRig = GameObject.CreatePrimitive(PrimitiveType.Cube);
            appRig.name = "LearningWorldsAppRig";
            DontDestroyOnLoad(appRig);
        }
    }
}