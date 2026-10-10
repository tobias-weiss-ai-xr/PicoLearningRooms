using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace LearningWorlds.Editor
{
    public static class LearningWorldsMenus
    {
        [MenuItem("LearningWorlds/Open Hub Scene")]
        public static void OpenHub()
        {
            if (Application.isPlaying) return;
            
            // Ensure a clean scene before loading
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            EditorSceneManager.LoadScene("Assets/Scenes/LearningWorlds-Hub.unity");
        }
        
        [MenuItem("LearningWorlds/Register Build Scenes")]
        public static void Register()
        {
            string hubScenePath = "Assets/Scenes/LearningWorlds-Hub.unity";
            
            // Check if the scene is already registered
            bool isRegistered = false;
            var scenes = EditorBuildSettings.scenes;
            
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].path == hubScenePath) {
                    isRegistered = true;
                    break;
                }
            }
            
            // Register scene if it is not already present
            if(!isRegistered) {
                scenes = EditorBuildSettings.scenes;
                Array.Resize(ref scenes, scenes.Length + 1);
                scenes[scenes.Length - 1] = new EditorBuildSettingsScene(hubScenePath, true);
                EditorBuildSettings.scenes = scenes;
                EditorUtility.DisplayDialog("Success", "Scene "+hubScenePath+" registered to build.", "OK");
            } else {
                Debug.Log($"Scene " {hubScenePath} was already registered.");
            }
        }
    }
}