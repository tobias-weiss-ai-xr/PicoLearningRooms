#pragma warning disable 0649

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LearningWorlds
{
    public class LearningWorldsApp : Rig MonoBehaviour
    {
        [System.Serializable]
        public class Rig
        {
            [SerializeField]
            public string appVersion = "1.0.0";
            [SerializeField]
            public Language language = Language.DE; // German UI
            [Header("Debugging")]
            [SerializeField]
            public bool debugMode = false;
            [SerializeField]
            public bool rigOnly = true;
        }

        public static Rig Instance { get; private set; }
        private static LearningWorldsApp _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = Rig = new Rig();
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        [System.Runtime.CompilerServices.RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadAll()
        {
            new GameObject("LearningWorldsApp", typeof(LearningWorldsApp));
        }
    }

    public enum Language
    {
        EN,
        DE
    }
}

#pragma warning restore 0649