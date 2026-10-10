using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace LearningWorlds
{
    /// <summary>
    /// Editor tool for initializing Socratic Tutors
 /// </summary>
    public static class TutorSetupMenu
    {
        [MenuItem("LearningWorlds/Setup Socratic Tutors")]
        public static void Setup()
        {
            // Create scriptable object to hold tutor configurations
            SocraticTutorConfig configAsset = ScriptableObject.CreateInstance<SocraticTutorConfig>();
            configAsset.name = "SocraticTutorConfig";

            // Default configurations for each world
            configAsset.Configs.Add(new SocraticTutorConfig.TutorConfig
            {
                world = SocraticTutorManager.World.Hub,
                characterId = "HubTutor",
                spawnPosition = new Vector3(0f, 1f, -3f),
                spawnRotation = new Vector3(0f, 180f, 0f)
            });

            configAsset.Configs.Add(new SocraticTutorConfig.TutorConfig
            {
                world = SocraticTutorManager.World.PseGallery,
                characterId = "Mendeleev",
                spawnPosition = new Vector3(1f, 1f, 0f),
                spawnRotation = new Vector3(0f, 0f, 0f)
            });

            configAsset.Configs.Add(new SocraticTutorConfig.TutorConfig
            {
                world = SocraticTutorManager.World.ElementRoom,
                characterId = "Einstein",
                spawnPosition = new Vector3(0f, 1f, 0f),
                spawnRotation = new Vector3(0f, 90f, 0f)
            });

            configAsset.Configs.Add(new SocraticTutorConfig.TutorConfig
            {
                world = SocraticTutorManager.World.Arachnophobia,
                characterId = "Spiderman",
                spawnPosition = new Vector3(-4f, 1f, 0f),
                spawnRotation = new Vector3(0f, 270f, 0f)
            });

            // Ensure Resources directory exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            // Save the asset in Resources folder
            string path = "Assets/Resources/SocraticTutorConfig.asset";
            AssetDatabase.CreateAsset(configAsset, path);

            // Create and activate SocraticTutorManager
            if (!GameObject.FindObjectOfType<SocraticTutorManager>())
            {
                GameObject managerObj = new GameObject("SocraticTutorManager");
                managerObj.AddComponent<SocraticTutorManager>();
            }

            AssetDatabase.SaveAssets();
            Debug.Log("Socratic Tutors configuration set up successfully!");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = configAsset;
        }
    }
}