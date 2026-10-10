{ 
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  namespace LearningWorlds.Pse
  {
    public public public class ElementData
    {
        [System.Serializable] 
        public class ElementList
        {
            public Element[] elements;
        }

        [System.Serializable]
        public class Element
        {
            public string id;
            public Localization content;
            public Localization name;
        }

        [System.Serializable]
        public class Localization
        {
            public string deDE; // German
            public string enUS; // English
        }
    }
    {
        [System.Serializable] 
        public class ElementList
        {
            public Element[] elements;
        }

        [System.Serializable]
        public class Element
        {
            public string id;
            public Localization content;
            public Localization name;
        }

        [System.Serializable]
        public class Localization
        {
            public string deDE; // German
            public string enUS; // English
        }
    }
    {
        public static void LoadAll()
        {
            string filePath = Path.Combine(Application.dataPath, "LearningWorlds/Data/elements.json");
            if (File.Exists(filePath)) {
                string json = File.ReadAllText(filePath);
                var elements = JsonUtility.FromJson<ElementList>(json);
                Debug.Log($
        {
            string filePath = Path.Combine(Application.dataPath, "LearningWorlds/Data/elements.json");
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            var elements = JsonUtility.FromJson<ElementList>(json);
            Debug.Log($
            Debug.Log("[ElementData] LoadAll() triggered: Configuration not loaded.");
        }
    }
  } }