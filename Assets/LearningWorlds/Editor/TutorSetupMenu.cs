using UnityEditor;
using UnityEngine;

public partial class TutorSetupMenu
{
    [MenuItem("LearningWorlds/Setup Saia Tutors")]
    private static void SetupSaiaTutors()
    {
        // Create a default Tutor Configuration asset
        string path = "Assets/Resources/TutorConfigs/DefaultTutorConfig.asset";

        if (System.IO.File.Exists(path))
        {
            // Asset already exists – display asset in Project window
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<object>(path);
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            // Create and populate Default Tutor Configuration
            SaiaTutorConfig configObject = ScriptableObject.CreateInstance<SaiaTutorConfig>();
            
            // Setup default Chemiker tutor for PseGallery
            var chemikerTutor = new SaiaTutorConfig.TutorConfig()
            {
                world = "PseGallery",
                name = "Chemiker",
                systemPrompt = "You are a chemistry tutor. Explain concepts about chemical reactions and elements.",
                voiceId = "de-de",
                spawnPosition = new Vector3(2.0f, 0.0f, 1.0f),
                spawnRotation = Quaternion.Euler(0, 180, 0)
            };

            // Setup default Forscher tutor for ElementRoom
            var researcherTutor = new SaiaTutorConfig.TutorConfig()
            {
                world = "ElementRoom",
                name = "Forscher",
                systemPrompt = "You are a scientist conducting experiments. Help the user interactively explore chemistry concepts.",
                voiceId = "de-de",
                spawnPosition = new Vector3(4.0f, 0.0f, -1.0f),
                spawnRotation = Quaternion.Euler(0, 0, 0)
            };

            // Setup default Therapeut tutor for Arachnophobia
            var therapistTutor = new SaiaTutorConfig.TutorConfig()
            {
                world = "Arachnophobia",
                name = "Therapeut",
                systemPrompt = "You are a psychological therapist specializing in phobias. Help the user overcome their fear of spiders through gradual exposure.",
                voiceId = "de-de",
                spawnPosition = new Vector3(0.0f, 0.0f, 0.0f),
                spawnRotation = Quaternion.Euler(0, 0, 0)
            };

            // Add tutors to config
            configObject.tutors.Add(chemikerTutor);
            configObject.tutors.Add(researcherTutor);
            configObject.tutors.Add(therapistTutor);

            // Save the new asset file
            AssetDatabase.CreateAsset(configObject, path);
            AssetDatabase.SaveAssets();

            // Focus on new asset in Project window
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = configObject;

            Debug.Log($"Default Tutor Config Assigned Saved at {path}");
        }
    }
}