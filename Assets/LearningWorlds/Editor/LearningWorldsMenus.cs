using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace LearningWorlds.Editor
{
    public static class LearningWorldsMenus
    {
       #if UNITY_EDITOR // Repro Only
[MenuItem("LearningWorlds/Open Hub Scene", false)]
        public static void OpenHub()
        {
            if (Application.isPlaying) return;
             
            // Ensure a clean scene before loading
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            EditorSceneManager.LoadScene("Assets/Scenes/LearningWorlds-Hub.unity");
        }
        
        [MenuItem("LearningWorlds/Register Build Scenes", false)]
        public static void Register()
        {
            string hubScenePath = "Assets/Scenes/LearningWorlds-Hub.unity";
            bool isRegistered = false;
            var scenes = EditorBuildSettings.scenes;

            for (int i = 0; i < scenes.Length; i++)
            {
                if(scenes[i].path == hubScenePath) 
                {
                    isRegistered = true;
                    break;
                }
            }

            // Register scene if not present
            if(!isRegistered) 
            {
                System.Array.Resize(ref scenes, scenes.Length + 1);
                scenes[scenes.Length - 1] = new EditorBuildSettingsScene(hubScenePath, true);
                EditorBuildSettings.scenes = scenes;
                EditorUtility.DisplayDialog("Success", string.Format("Scene {0} registered to build.", hubScenePath), "OK");
            } else 
            {
                Debug.Log("Scene already registered: "+hubScenePath);
            }
        }
    }
}