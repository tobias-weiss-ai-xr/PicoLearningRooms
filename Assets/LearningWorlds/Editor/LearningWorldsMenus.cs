using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace LearningWorlds.Editor
{
    /// <summary>
    /// Editor menu items for LearningWorlds functionality.
    /// </summary>
    public static class LearningWorldsMenus
    {
        private const string HubScenePath = "Assets/Scenes/LearningWorlds-Hub.unity";
        private const string GalleryScenePath = "Assets/Scenes/LearningWorlds-PseGallery.unity";
        private const string HubSceneGuid = "5a0d4b1b5f7a44249bd9f682b8b866b1";

        /// <summary>
        /// Opens the LearningWorlds Hub scene.
        /// </summary>
        [MenuItem("LearningWorlds/Open Hub Scene", false, 0)]
        public static void OpenHub()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "Cannot load scene while in Play Mode.", "OK");
                return;
            }

            try
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(HubScenePath);
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", "Failed to open Hub scene: " + e.Message, "OK");
            }
        }

        /// <summary>
        /// Opens the PSE Gallery scene.
        /// </summary>
        [MenuItem("LearningWorlds/Open PSE Gallery", false, 1)]
        public static void OpenPseGallery()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "Cannot load scene while in Play Mode.", "OK");
                return;
            }

            try
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(GalleryScenePath);
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", "Failed to open PSE Gallery: " + e.Message, "OK");
            }
        }

        /// <summary>
        /// Registers all LearningWorlds scenes in Build Settings.
        /// </summary>
        [MenuItem("LearningWorlds/Register Build Scenes", false, 10)]
        public static void Register()
        {
            string[] scenePaths = {
                HubScenePath,
                GalleryScenePath
            };

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            bool anyChanges = false;

            foreach (string scenePath in scenePaths)
            {
                bool isRegistered = System.Array.Exists(scenes, s => s.path == scenePath);

                if (!isRegistered)
                {
                    uint currentSize = (uint)scenes.Length;
                    System.Array.Resize(ref scenes, currentSize + 1);
                    scenes[scenes.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
                    anyChanges = true;
                }
            }

            if (anyChanges)
            {
                EditorBuildSettings.scenes = scenes;
                EditorUtility.DisplayDialog("Success", "LearningWorlds scenes registered in Build Settings.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Message", "All LearningWorlds scenes are already registered.", "OK");
            }
        }

        /// <summary>
        /// Creates a new LearningWorlds prefab in the project.
        /// </summary>
        [MenuItem("GameObject/LearningWorlds/Create Hub Prefab", false, 0)]
        public static void CreateHubPrefab()
        {
            GameObject hubPrefab = new GameObject("LearningWorlds-Hub");
            hubPrefab.AddComponent<LearningWorldsApp>();
            
            string prefabPath = "Assets/Prefabs/LearningWorlds-Hub.prefab";
            string prefabDir = System.IO.Path.GetDirectoryName(prefabPath);
            
            if (!System.IO.Directory.Exists(prefabDir))
            {
                System.IO.Directory.CreateDirectory(prefabDir);
            }
            
            PrefabUtility.SaveAsPrefabAsset(hubPrefab, prefabPath);
            GameObject.DestroyImmediate(hubPrefab);
            
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Success", "Hub prefab created at: " + prefabPath, "OK");
        }
    }
}
