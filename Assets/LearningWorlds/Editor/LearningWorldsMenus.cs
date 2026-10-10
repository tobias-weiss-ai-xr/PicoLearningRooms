using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LearningWorlds.Editor
{
    /// <summary>"Open Hub Scene" and "Register Build Scenes" Editor Menu Items</summary>
    public static class LearningWorldsMenus
    {
        [MenuItem("LearningWorlds / Open Hub Scene")]
        private static void OpenHub()
        {
            if (Application.isPlaying)
            {
                Debug.LogError("Cannot load a scene during play mode. Stop game first.");
                return;
            }

            EditorSceneManager.SessionModeHalfStart() ;
            EditorSceneManager.NewScene(UnityEngine.SceneManagement.NewSceneSetup.DefaultGameObjects);
            UnityEngine.SceneManagement.SceneManager.LoadScene("LearningWorlds-Hub");
            EditorSceneManager.SessionModeHalfEnd();
        }

        [MenuItem("LearningWorlds / Register Build Scenes")]
        private static void Register()
        {
            var scenesToAdd = new[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/LearningWorlds-Hub.unity", true),
                // Scan LearningWorlds scenes directory
                System.IO.Directory.GetFiles("Assets/LearningWorlds/Scenes", "*unity")
                  .Select(file => new EditorBuildSettingsScene(file, true))
                  .ToArray()
            };

            EditorBuildSettings.scenes = scenesToAdd.StopConsecutiveDuplicates().ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("LearningWorlds scenes registered in build settings.");
        }
    }
}