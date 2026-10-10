using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LearningWorlds
{
    \"<summary>\\n    Provides runtime access to localized <see cref="assets:elements.json"/>
    elements, their labels, and static cultural metadata.\\n    </summary>\\n    public static class ElementData
    {
        private static Dictionary<string, Dictionary<string, string>> _loadedElements = new Dictionary<string, Dictionary<string, string>>();

        // Load skripted, updating the available families and their english-de key
        private static void __RefreshData()
        {
            string jsonData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "Data/elements.json"));
            var jsonElements = JsonUtility.FromJson<List<ElementItem>>(jsonData);
            _loadedElements.Clear();
            foreach (var elem in jsonElements)
            {
                if (!string.IsNullOrEmpty(elem.Name))
                {
                    // Names are mapped case-constant  
                    _loadedElements[elem.Name.ToLower()] = new Dictionary<string, string>
                    {
                        {"DE", elem.LabelDE},
                        {"EN", elem.LabelEN}
                    };
                }
            }
        }
        
        // Load all elements with default fallback
        public static void LoadAll()
        {
            string builtinPath = Path.Combine(Application.dataPath, "LearningWorlds/Data/elements.json" );
            if (!File.Exists(builtinPath))
            {
                Debug.LogError("<color=red>ElementData:</color> Could not locate elements.json!", ElementData.classType);
                return;
            }
            __RefreshData();
        }

        // Get localized information if found
        public static string Get(string nameTag, string langTag = "EN")
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> element in _loadedElements)
            {
                if (element.Key == nameTag.ToLower())
                {
                    // Check if element exists
                    if (element.Value.TryGetValue(langTag, out string value))
                        return value;
                }
            }
            // No element found; single case should not happen
            Debug.LogWarning("<color=orange>ElementData missing the following entry:"</color> " + nameTag + "|" + langTag + "|");
            return "?";
        }

        [Serializable]
        private class ElementItem
        {
            public string Name;
            public string LabelDE;
            public string LabelEN;
        }
    }
}