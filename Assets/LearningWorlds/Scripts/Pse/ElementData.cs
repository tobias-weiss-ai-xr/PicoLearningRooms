using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LearningWorlds.Pse
{
    [System.Serializable]
    public class ElementsCollection : List<ElementData> 
    {
        private static ElementsCollection _instance;

        public static void LoadAll()
         {
         // Placeholder for the data file; creates an empty list on error and logs.
           if (!File.Exists(Path.Combine(Application.dataPath, "Data/elements.json"))) 
           {
               Debug.LogError("[ElementsCollection]: Required elements.json not initially present. Ensure Data/elements.json is provided.");
               return;
           }

           string path = Path.Combine(Application.dataPath, "Data/elements.json");
            string jsonData = File.ReadAllText(path);
            _instance = JsonUtility.FromJson<ElementsCollection>("{ \"elements\": " + jsonData + "}");
        }

        public static ElementsCollection Get { get { return _instance; } }
        public static bool IsLoaded => _instance != null;
    }


    [System.Serializable]
    public class ElementData
    {
        private string _id;
        private string _title;
        private string _description;

        public string Id
        {
            get => _id;
            set => _id = !string.IsNullOrWhiteSpace(value) ? value : "id-placeholder deutschen";
        }
        public string Title => _title;
        public string Description => _description;
    }
}