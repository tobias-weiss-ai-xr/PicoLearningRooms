using UnityEngine;

namespace LearningWorlds
{
    public sealed class LearningWorldsApp : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            // Create the root game object and mark it as DontDestroyOnLoad for persistence
            new GameObject(nameof(LearningWorldsApp))
            {
                hideFlags = HideFlags.HideInHierarchy
            }.
            AddComponent<LearningWorldsApp>().
            InitRuntime();
        }

        private void InitRuntime()
        {
            // German UI, English content
            Object.DontDestroyOnLoad(gameObject);
            Debug.Log("[LearningWorlds] App runtime initialized.");
        }
    }
}