using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using System.IO; // For JSON handling, replace with actual unity built-in if permitted, otherwise use System.Text.Json

namespace LearningWorlds.Pse {
    public class ElementData {
        public static IEnumerator<List<Element>> LoadAll() {
            string loadPath = Path.Combine(Application.dataPath, "LearningWorlds/Data/elements.json");
            if (!File.Exists(loadPath)) {
                Debug.LogWarning("elements.json not found at path: " + loadPath);
                yield break;
            }
            if (File.Exists(loadPath)) {
                string json = File.ReadAllText(loadPath);
                return JsonUtility.FromJson<List<Element>>(json);
            } else {
                Debug.LogError("elements.json not found at: " + loadPath);
                yield break;
            }
        }
    }

    [System.Serializable]
    public class Element {
        public string Label;
        public string Content;
    }
}