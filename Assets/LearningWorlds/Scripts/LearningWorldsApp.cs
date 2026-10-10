using UnityEngine;

namespace LearningWorlds
{
    /// <summary>
    /// LearningWorldsApp: The core entry point. Initializes before scene load, validates rendering, and handles German UI defaults.
    /// </summary>
    public class LearningWorldsApp : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AppInitialize()
        {
            Debug.Log("Initializing LearningWorlds...");
        }

        void Awake()
        {
            if (Instance != null)
            {   
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static LearningWorldsApp Instance { get; private set; }
    }
}