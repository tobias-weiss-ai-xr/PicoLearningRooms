using UnityEngine;

namespace LearningWorlds {
    public class LearningWorldsApp : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeApp() {
            GameObject learningWorldsObject = new GameObject("LearningWorlds");
            DontDestroyOnLoad(learningWorldsObject);

            Debug.Log("Initializing LearningWorldsApp...");
        }
    }
}