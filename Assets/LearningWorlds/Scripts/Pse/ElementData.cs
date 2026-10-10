using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace LearningWorlds.Pse
{
    [System.Serializable]
    public class ElementData
    {
        public string id;
        public string nameEn;
        public string descriptionDe;
    }

    public static class Loader
    {
        public static List<ElementData> LoadAll()
        {
            string elementsJsonPath = Path.Combine(Application.dataPath, "Assets/LearningWorlds/Data/elements.json");
            string jsonData = File.ReadAllText(elementsJsonPath);
            var data = JsonUtility.FromJson<ElementData[]>(jsonData);
            
            if (data == null)
            {
                Debug.LogWarning("[LearningWorlds] No elements data loaded at " + elementsJsonPath);
                return new List<ElementData>();
            }
            
            return new List<ElementData>(data);
        }
    }
}