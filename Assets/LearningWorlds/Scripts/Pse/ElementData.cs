// ElementData.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Represents data for a chemical element.
    /// </summary>
    [Serializable]
    public class ElementData
    {
        public string id;
        public string nameDe;
        public string nameEn;
        public string descriptionDe;
        public string descriptionEn;
        public ElementProperties properties;

        /// <summary>
        /// Loads all element data from JSON resource.
        /// </summary>
        public static List<ElementData> LoadAll()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("elements");
            if (jsonFile != null)
            {
                return JsonConvert.DeserializeObject<List<ElementData>>(jsonFile.text);
            }
            Debug.LogError("ElementData: Could not load elements.json from Resources");
            return new List<ElementData>();
        }
    }

    /// <summary>
    /// Properties for a chemical element.
    /// </summary>
    [Serializable]
    public class ElementProperties
    {
        public double viscosity;
        public int meltingPoint;
        public bool custom;
    }
}
