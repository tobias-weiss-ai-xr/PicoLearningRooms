using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LearningWorlds.Pse {
    public class ElementData {
        public static List<Element> LoadAll(string filePath = "elements.json") {
            string fullPath = System.IO.Path.Combine("Assets/LearningWorlds/Data", filePath);
            var text = System.IO.File.ReadAllText(fullPath);
            var elements = JsonUtility.FromJson<ElementContainer>(text).Elements;
            return elements;
        }

        [System.Serializable]
        public class Element {
            public string elementId;
            public string name;
            public string content;
        }

        [System.Serializable]
        public class ElementContainer {
            public List<Element> Elements = new List<Element>();
        }
    }
}