using UnityEditor;
using UnityEngine;

public class TutorSetupMenu
{
    [MenuItem("LearningWorlds/Setup Socratic Tutors")]
    private static void Setup()
    {
        string path = EditorUtility.SaveFolderPanel("Save Location", "", "Assets/LearningWorlds/Data");
        if (!string.IsNullOrEmpty(path))
        {
            string configName = "DefaultSocraticTutorConfig";
            string fullPath = System.IO.Path.Combine(path, configName + ".asset");
            SocraticTutorConfig tutorConfig = ScriptableObject.CreateInstance<SocraticTutorConfig>();

            // Adding default tutors for each world
            var hubConfig = new SocraticTutorManager.TutorConfig
            {
                world = SocraticTutorManager.World.Hub,
                characterId = "HubTutor",
                spawnPosition = new Vector3(0f, 0f, 0f),
                spawnRotation = new Vector3(0f, 0f, 0f)
            };

            var pseConfig = new SocraticTutorManager.TutorConfig
            {
                world = SocraticTutorManager.World.PseGallery,
                characterId = "PseTutor",
                spawnPosition = new Vector3(2f, 0f, 0f),
                spawnRotation = new Vector3(0f, 45f, 0f)
            };

            var elementConfig = new SocraticTutorManager.TutorConfig
            {
                world = SocraticTutorManager.World.ElementRoom,
                characterId = "ElementTutor",
                spawnPosition = new Vector3(4f, 0f, 0f),
                spawnRotation = new Vector3(0f, 60f, 0f)
            };

            var arachnoConfig = new SocraticTutorManager.TutorConfig
            {
                world = SocraticTutorManager.World.Arachnophobia,
                characterId = "PhobiaTutor",
                spawnPosition = new Vector3(6f, 0f, 0f),
                spawnRotation = new Vector3(0, 90f, 0f)
            };
            
            tutorConfig.tutorConfigs.Add(hubConfig);
            tutorConfig.tutorConfigs.Add(pseConfig);
            tutorConfig.tutorConfigs.Add(elementConfig);
            tutorConfig.tutorConfigs.Add(arachnoConfig);

            AssetDatabase.CreateAsset(tutorConfig, fullPath);
            Debug.Log("Created default SocraticTutorConfig: " + fullPath);
            EditorUtility.SetDirty(tutorConfig);
        }
    }
}