using UnityEngine;

namespace LearningWorlds.Hub
{
    /// <summary>
    /// Creates educational signage labels in the Learning Hub.
    /// </summary>
    public static class LearningSignage
    {
        private static GameObject signagePanel;

        /// <summary>
        /// Builds all signage for the hub.
        /// </summary>
        public static GameObject Build()
        {
            // Create signage container object
            GameObject signageBox = new GameObject("SignageHub_Center");
            signagePanel = signageBox;

            // Welcome sign at center
            CreateTextMesh("Welcome_HubCenter",
                "Willkommen at PicoLearningRooms.\nEverything here is about Learning",
                new Vector3(0.25f, 0.85f, -1.2f), 44, 0.0185f, TextAnchor.MiddleCenter);

            // Learning principles sign at left
            CreateTextMesh("Lernprinzipien_KV",
                "Learning Principles at PicoLearningRooms\n• Multi-explorative synthetic learning\n• Adaptive feedback systems\n• Immersive experiences",
                new Vector3(-3f, 0.6f, 0f), 42, 0.018f, TextAnchor.MiddleLeft);

            // Courage training sign at right
            CreateTextMesh("Mut-TrainingKB",
                "Courage Training Principles\n─ Technology | Sport | Exposure Therapy\nExperiences geared for outcomes",
                new Vector3(3f, 0.6f, 0f), 52, 0.02f, TextAnchor.MiddleRight);

            return signageBox;
        }

        /// <summary>
        /// Creates a text mesh with specified parameters.
        /// </summary>
        private static void CreateTextMesh(string name, string text, Vector3 position, 
            int fontSize, float characterSize, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            GameObject panel = new GameObject(name + "_Panel");
            TextMesh tm = panel.AddComponent<TextMesh>();

            tm.text = text;
            tm.anchor = anchor;
            tm.characterSize = characterSize;
            tm.fontSize = fontSize;
            tm.transform.localPosition = position;
            tm.color = Color.white;

            if (signagePanel != null)
            {
                panel.transform.SetParent(signagePanel.transform);
            }
        }
    }
}
