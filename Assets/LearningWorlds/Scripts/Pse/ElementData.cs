using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Data wrapper for JSON-stored elements.
    /// Focuses on runtime asset path parsing and UI content translation.
    /// </summary>
    public static class ElementData
    {
        private static List<Element> _elements = new List<Element>();

        /// <summary>
        /// Load all elements from the JSON data source.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void LoadAll()
        {
            string filePath = Path.Combine(Application.dataPath, "LearningWorlds/Data/elements.json");
            if (!File.Exists(filePath))
            {
                Debug.LogError("[ElementData] Data file not found: " + filePath);
                return;
            }

            string json = File.ReadAllText(filePath);
            _elements = JsonUtility.FromJson<ElementRoot>(json).elements;
            Debug.Log("[ElementData] Loaded " + _elements.Count + " elements from " + filePath);
        }

        /// <summary>
        /// Exposure for content access (English only).
        /// </summary>
        public static IReadOnlyList<Element> Elements => _elements;

        // JSON parser root structure
        [System.Serializable]
        private class ElementRoot
        {
            public List<Element> elements;
        }

        [System.Serializable]
        public class Element
        {
            public string id;
            public string nameEn;
            public string descriptionDe; // German UI text
        }
    }
}