using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

namespace LearningWorlds.Pse {
    public class ElementData {
        public List<Element> Elements = new List<Element>();

        public void LoadAll(string path) {
            string jsonString = File.ReadAllText(path);
            ElementDataParsed parsedData = JsonUtility.FromJson<ElementDataParsed>(jsonString);
            UpdateElementsFromParsed(parsedData);

            Debug.Log("Loaded all elements from " + path);
        }

        private void UpdateElementsFromParsed(ElementDataParsed parsed) {
            if (parsed != null && parsed.elements != null) {
                Elements.AddRange(parsed.elements);
            }
        }

        // Nested helper class to parse JSON data
        [System.Serializable]
        private class ElementDataParsed {
            public int count;
            public Element[] elements;
        }

        [System.Serializable]
        public class Element {
            public string id;
            public string name;
            public string type;
            public string description;
            public string[] labels;
            public float[] attributes;
            // Missing nested property to align with placeholder
        }
    }
}