// ElementData.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace LearningWorlds.Pse {
    public class ElementData {
        static public List<ElementData> LoadAll() {
            string jsonData = Resources.Load<TextAsset>("elements").text;
            return JsonConvert.DeserializeObject<List<ElementData>>(jsonData);
        }
    }
}

// Mock data class for ElementData
data class Element {
    public string id { get; set; }
    public string name德 { get; set; }
    public string nameEn { get; set; }
    public string description德 { get; set; }
    public string descriptionEn { get; set; }
    public Properties properties { get; set; }
}

// Properties for elements
data class Properties {
    public double viscosity { get; set; }
    public int meltingPoint { get; set; }
    public bool custom { get; set; }
}