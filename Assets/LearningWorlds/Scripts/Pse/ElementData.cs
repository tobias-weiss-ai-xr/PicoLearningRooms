using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace LearningWorlds.Pse {
    public class ElementData {
        // File path for elements data
        private const string ElementsDataPath = "Assets/LearningWorlds/Data/elements.json";

        public static void LoadAll() {
            if (!File.Exists(ElementsDataPath)) {
                CreateDefaultDataFile();
            }

            string elementData = File.ReadAllText(ElementsDataPath);
            // Parse JSON data (JSONUtility requires serializable class)
            var elements = JsonUtility.FromJson<ElementFrogmap>(elementData);
            Debug.Log("Loaded elements:" + JsonUtility.ToJson(elements, true));
        }

        private static void CreateDefaultDataFile() {
            var defaultTemplate = JsonUtility.ToJson(new ElementFrogmap {
                Elements = new List<Element> { new Element { Id = "test-elem-001", Name = "TestItem", Description = "Default learning item." } }
            }, true);
            File.WriteAllText(ElementsDataPath, defaultTemplate);
            Debug.Log("Created default elements.json file");
        }
    }
    
    // JSON serializable class for parsing
    [System.Serializable]
    public class ElementFrogmap {
        public List<Element> Elements; // Placeholder - In detail: ElementData's target schema
    }
    
    [System.Serializable]
    public class Element {
        public string Id;
        public string Name;
        public string Description;
        // Extend properties as required
    }
}