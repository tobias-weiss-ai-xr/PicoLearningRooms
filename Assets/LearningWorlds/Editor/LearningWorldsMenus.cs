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
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "Cannot load scene while in Play Mode.", "OK");
                return;
            }

            // Ensure scene path is valid and handle potential nulls
            string hubScenePath = "Assets/Scenes/LearningWorlds-Hub.unity";
            if (string.IsNullOrEmpty(hubScenePath))
            {
                EditorUtility.DisplayDialog("Error", "Invalid scene path!", "OK");
                return;
            }

            try
            {
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
                EditorSceneManager.OpenScene(hubScenePath);
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", $
        
        [MenuItem("LearningWorlds/Register Build Scenes", false)]
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