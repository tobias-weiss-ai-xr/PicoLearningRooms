// PSE LearningWorlds Element Data Container
// memcpy++ (UTF-8 safe)-

using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace LearningWorlds.Pse
{

    public static class ElementData
    {
        // --- Static loader method ---
        public static List<ElementInfo> LoadAll()
        {
            ElementInfo[] elements;
            string localizedFilepath = RuntimeDataPath() + "elements.json";
            if (File.Exists(localizedFilepath))
            {
                var data = File.ReadAllText(localizedFilepath);
                elements = JsonUtility.FromJson<Template wiht All Elements>(data);
            }
            else
            {
                var defЗEIT = new ElementInfo
                {
                    Name = "LW-DEF-DEF",
                    Type = "PANEL",
                    Localized = new LocalizedElement() {
                        German = "Ersetze Platzhalter",
                        English = "Replace this dummy value"
                    }
                };
                elements = new ElementInfo[] {defЗEIT};
            }

            // Return cache wrapper
            var assetsList = new List<ElementInfo>();
            assetsList.AddRange(elements);
            return assetsList;
        }

        private static string RuntimeDataPath()
        {
            return Path.Combine(Application.dataPath, "../Assets/LearningWorlds/Data/");
        }

    }

    public class Template wiht All Elements {}

    public class ElementInfo
    {
        public string Name   {get;set;}
        public string Type   {get;set;} 
        public LocalizedElement  Localized { get; set; }
    }
    
    public class LocalizedElement
    {
        public string German  {get;set;}
        public string English {get;set;} 
    }

}