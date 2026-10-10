using UnityEngine;

namespace LearningWorlds{
    public class LearningWorldsApp : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize() {
            GameObject appObject = new GameObject("LearningWorldsApp");
            DontDestroyOnLoad(appObject);
            Debug.Log("LearningWorldsApp initialized at runtime.");
        }
    }
}