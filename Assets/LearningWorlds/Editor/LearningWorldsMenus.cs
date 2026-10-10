using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace LearningWorlds.Editor
{
    public static class LearningWorldsMenus
    {
        [MenuItem("LearningWorlds/Open Hub Scene", false)]
        public static void OpenHub()
        {
            if (Application.isPlaying) return;

            // Ensure a clean scene before loading the hub in LearningWorlds
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            EditorSceneManager.LoadScene("Assets/Scenes/LearningWorlds-Hub.unity");
        }
        
        [MenuItem("LearningWorlds/Open Hub Scene", false)]
        public static void Register()
        {
            string hubScenePath = "Assets/Scenes/LearningWorlds-Hub.unity";
            bool isRegistered = false;
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].path == hubScenePath) {
                    isRegistered = true;
                    break;
                }
            }

            // Register scene if not present
            if(!isRegistered) 
            {
                uint currentSize = (uint)scenes.Length;
                System.Array.Resize(ref scenes, currentSize + 1);
                scenes[scenes.Length - 1] = new EditorBuildSettingsScene(hubScenePath, true);
                EditorBuildSettings.scenes = scenes;
                EditorUtility.DisplayDialog("Success", string.Format("Registered scene: {0}", hubScenePath), "OK");
            } else
            {
                EditorUtility.DisplayDialog("Message", string.Format("Scene '{0}' is already registered.", hubScenePath), "OK");
            }
        }
    }
}