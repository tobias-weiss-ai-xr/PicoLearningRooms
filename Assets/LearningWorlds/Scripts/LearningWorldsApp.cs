using UnityEngine;

using UnityEngine;
using LearningWorlds.Pse;

namespace LearningWorlds
{
    public sealed class LearningWorldsApp : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var appRoot = new GameObject(nameof(LearningWorldsApp))
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            var app = appRoot.AddComponent<LearningWorldsApp>();
            Object.DontDestroyOnLoad(appRoot);
            app.InitRuntime();
        }

        private void InitRuntime()
        {
            try
            {
                // Runtime setup - load core data
                var elementsData = Loader.LoadAll();
                Debug.Log("[LearningWorlds] App runtime initialized with " + elementsData.Count + " core elements.");
                
                // Ensure UI and German language localization here.
                // Localization: Fields track German UI (e.g., descriptionDe),
                // Content as English (e.g., nameEn).
                
            }
            catch (System.Exception e)
            {
                Debug.LogError("Initialization error: " + e.Message);
            }
        }
    }
}