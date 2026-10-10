using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace LearningWorlds.Pse
{
    [System.Serializable]
    public class ElementData
    {
        // Placeholder: Load all elements from JSON
        [System.Serializable]
        public class Element
        {
            public string name;
            public string description;
        }
        
        // Storage for loaded elements
        private static List<Element> elements = new List<Element>();
        
        // Loads elements.json and initializes the static list
        public static void LoadAll()
        {
            string jsonFilePath = "Assets/LearningWorlds/Data/elements.json";
            if (File.Exists(jsonFilePath))
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                Element[] loadedElements = JsonUtility.FromJson<Element[]>(jsonData);
                foreach (var element in loadedElements)
                {
                    elements.Add(element);
                }
            }
            else
            {
                Debug.LogError("Element data file not found: " + jsonFilePath);
            }
        }
    }
}