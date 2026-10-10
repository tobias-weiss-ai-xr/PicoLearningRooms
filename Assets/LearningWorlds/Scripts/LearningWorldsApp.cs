// LearningWorldsApp.cs
using UnityEngine;

namespace LearningWorlds {
    public class LearningWorldsApp : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void InitializeApp() {
            if (!GameObject.Find("LearningWorldsManager")) {
                GameObject appRoot = new GameObject("LearningWorldsManager");
                DontDestroyOnLoad(appRoot);

                // Init basic test UI
                var textObject = new GameObject("TitleUI");
                textObject.transform.SetParent(appRoot.transform);

                var textMesh = textObject.AddComponent<TextMesh>();
                textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                textMesh.text = "Lernwelten App (LearningWorlds)";
                textMesh.characterSize = 1f;
                
                Debug.Log("LearningWorldsApp initialized.");
            }
        }
    }
}