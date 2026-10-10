using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Centralizes loading, parsing and accessing of JSON-based element data.
    /// </summary>
    public static class ElementData
    {
        public static List<ElementInfo> LoadAll()
        {
            List<ElementInfo> elements = new List<ElementInfo>();
            string filePath = Path.Combine(Application.dataPath, "../Assets/LearningWorlds/Data/elements.json");
    
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonStr = File.ReadAllText(filePath);
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject<JToken>(jsonStr);

                    foreach (var elem in jsonObject)
                    {
                        var localizedStrings = (JObject)elem.Value["strings"];
                        JObject german = (JObject)localizedStrings["de"], english = (JObject)localizedStrings["en"];

                        elements.Add(new ElementInfo
                        {
                            Name = elem.Name,
                            Type = elem.Value["type"].Value<string>(),
                            Localized = new LocalizedElement
                            {
                                German = german["title"].Value<string>(),
                                English = english["title"].Value<string>(),
                            }
                        });
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Failed to load ElementData: " + e.Message);
                    return new List<ElementInfo>()
                    {
                        new ElementInfo
                        {
                            Name = "LW-EMPTY-BLOCKER",
                            Type = "Debug Failed",
                            Localized = new LocalizedElement
                            {
                                German = "Fehler beim Laden",
                                English = "Unable to load"
                            }
                        }
                    };
                }
            }
            return elements;
        }
    }

    public class ElementInfo
    {
        public string Name;
        public string Type;
        public LocalizedElement Localized;
    }

    public class LocalizedElement
    {
        public string German;
        public string English;
    }
}