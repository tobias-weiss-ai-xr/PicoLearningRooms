using UnityEditor;
using UnityEngine;
using System.IO;

namespace LearningWorlds.Editor
{
    public static class TutorSetupMenu
    {
        [MenuItem("LearningWorlds/Setup Saia Tutors", false, 100)]
        private static void SetupSaiaTutors()
        {
            // Create a default Tutor Configuration asset
            string path = "Assets/Resources/TutorConfigs/DefaultTutorConfig.asset";
            string fullPath = Path.Combine(Application.dataPath, "Resources/TutorConfigs");
            
            // Ensure directory exists
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
            }

            if (File.Exists(Path.Combine(Application.dataPath, path)))
            {
                // Asset already exists – display asset in Project window
                EditorUtility.FocusProjectWindow();
                var asset = AssetDatabase.LoadAssetAtPath<SaiaTutorConfig>(path);
                if (asset != null)
                {
                    Selection.activeObject = asset;
                    EditorGUIUtility.PingObject(asset);
                }
            }
            else
            {
                // Create and populate Default Tutor Configuration
                SaiaTutorConfig configObject = ScriptableObject.CreateInstance<SaiaTutorConfig>();
                
                // Setup default Chemiker tutor for PseGallery
                var chemikerTutor = new SaiaTutorConfig.TutorConfig
                {
                    world = "PseGallery",
                    name = "Chemiker",
                    systemPrompt = "Du bist ein Chemie-Tutor. Erkläre Konzepte zu chemischen Reaktionen und Elementen auf sokratische Weise.",
                    voiceId = "de-de",
                    spawnPosition = new Vector3(2.0f, 0.0f, 1.0f),
                    spawnEulerAngles = new Vector3(0, 180, 0)
                };

                // Setup default Forscher tutor for ElementRoom
                var researcherTutor = new SaiaTutorConfig.TutorConfig
                {
                    world = "ElementRoom",
                    name = "Forscher",
                    systemPrompt = "Du bist ein Wissenschaftler, der Experimente durchführt. Hilf dem Nutzer, Chemie-Konzepte interaktiv zu erkunden.",
                    voiceId = "de-de",
                    spawnPosition = new Vector3(4.0f, 0.0f, -1.0f),
                    spawnEulerAngles = new Vector3(0, 0, 0)
                };

                // Setup default Therapeut tutor for Arachnophobia
                var therapistTutor = new SaiaTutorConfig.TutorConfig
                {
                    world = "Arachnophobia",
                    name = "Therapeut",
                    systemPrompt = "Du bist ein psychologischer Therapeut, spezialisiert auf Phobien. Hilf dem Nutzer, seine Angst vor Spinnen durch schrittweise Konfrontation zu überwinden.",
                    voiceId = "de-de",
                    spawnPosition = new Vector3(0.0f, 0.0f, 0.0f),
                    spawnEulerAngles = new Vector3(0, 0, 0)
                };

                // Setup default Wegweiser tutor for Hub
                var guideTutor = new SaiaTutorConfig.TutorConfig
                {
                    world = "Hub",
                    name = "Wegweiser",
                    systemPrompt = "Du bist ein Lernbegleiter. Hilf dem Nutzer, den richtigen Lernweg zu finden und die verfügbaren Lernwelten zu verstehen.",
                    voiceId = "de-de",
                    spawnPosition = new Vector3(0.0f, 0.0f, 2.0f),
                    spawnEulerAngles = new Vector3(0, 180, 0)
                };

                // Add tutors to config
                configObject.tutors.Add(chemikerTutor);
                configObject.tutors.Add(researcherTutor);
                configObject.tutors.Add(therapistTutor);
                configObject.tutors.Add(guideTutor);

                // Save the new asset file
                AssetDatabase.CreateAsset(configObject, path);
                AssetDatabase.SaveAssets();

                // Focus on new asset in Project window
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = configObject;
                EditorGUIUtility.PingObject(configObject);

                Debug.Log("[TutorSetupMenu] Default Tutor Config saved at " + path);
            }
        }
    }
}
