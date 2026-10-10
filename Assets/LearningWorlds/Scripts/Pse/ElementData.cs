using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LearningWorlds.Pse
{
    public class ElementData : MonoBehaviour
    {
        public string dataPath = "Data/elements.json";
        private List<ElementInfo> _elements;

        public class ElementInfo
        {
            public string Id { get; private set; }
            public string NameEn { get; private set; }
            public string DescriptionDe { get; private set; }
            public string Type { get; private set; }

            public ElementInfo(string id, string nameEn, string descriptionDe, string type)
            {
                Id = id;
                NameEn = nameEn;
                DescriptionDe = descriptionDe;
                Type = type;
            }
        }

        public void Start()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            string fullPath = Path.Combine(Application.dataPath.Split(Application.productName)[1], dataPath);

            if (File.Exists(fullPath))
            {
                string jsonData = File.ReadAllText(fullPath);
                var elementsData = JsonUtility.FromJson<JArray>(jsonData);

                _elements = new List<ElementInfo>();
                foreach (var item in elementsData)
                {
                    var jItem = item as JObject;
                    var element = new ElementInfo(
                        (string)jItem["id"],
                        (string)jItem["nameEn"],
                        (string)jItem["descriptionDe"],
                        (string)jItem["type"]
                    );

                    _elements.Add(element);
                }
            }
            else
            {
                Debug.LogError("elements.json must be in the '" + dataPath + "' path.");
            }
        }

        public ElementInfo GetElementById(string id)
        {
            return _elements?.Find(e => e.Id == id);
        }
    }
}