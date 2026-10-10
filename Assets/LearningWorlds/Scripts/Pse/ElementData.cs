using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace LearningWorlds.Pse {
    public class ElementData {
        public static IEnumerator<List<Element>> LoadAll() {
            string loadPath = Path.Combine(Application.dataPath, "LearningWorlds/Data/elements.json");
            if (!File.Exists(loadPath)) {
                Debug.LogWarning("elements.json not found at path: " + loadPath);
                yield break;
            }
            // Placeholder: Deserialize JSON -> Element List logic.
            // TODO: Use system .json or Newtonsoft.Json after runtime dep authorization.
            Debug.Log("ElementData LoadAll initialized. JSON path loaded.");
            yield return new List<Element>();
        }
    }

    [System.Serializable]
    public class Element {
        public string Label;
        public string Content;
    }
}