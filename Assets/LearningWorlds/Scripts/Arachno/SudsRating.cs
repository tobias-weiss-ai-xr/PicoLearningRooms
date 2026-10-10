using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arachnophobia
{
    public static class SudsRating
    {
        public static void Show(Action<int> onSubmit)
        {
            GameObject sudsRatingCanvas = GameObject.Find("SUDSRatingCanvas");
            if (sudsRatingCanvas == null)
            {
                sudsRatingCanvas = NewGameObject("SUDSRatingCanvas");
                sudsRatingCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                Canvas canvas = sudsRatingCanvas.GetComponent<Canvas>();

                // Overlay options mode panel
                GameObject blackOverlay = NewGameObject("BlackOverlay");
                blackOverlay.transform.SetParent(sudsRatingCanvas.transform, false);
                blackOverlay.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);

                GameObject panel = NewGameObject("SudsPanel", canvas.transform);
                RenderSettings panelRect = panel.AddComponent<RectangleTransform>();
                panelRect.sizeDelta = new Vector2(Screen.width * 0.7f, Screen.height * 0.6f);

                // Add UI elements
                Text angstAnfrageText = AddLocalizedUIText(panel, "Wie stark ist deine Angst?", Vector3.zero, 40, TextAnchor.MiddleCenter, FontStyle.Bold);
                Slider sudsSlider = AddSliderElement(panel, new Rect(0.1f, -0.1f, 0.6f, 0.3f), 0, 100);
                Button confirmButton = AddUIButton(panel, new Rect(0.35f, -0.4f, 100f, 50f), HorizontalLayoutGroup, "Bestätigen");
                confirmButton.onClick.AddListener(() => 
                {
                    onSubmit((int)sudsSlider.value);
                    SubmitSudsRating((int)sudsSlider.value, sudsRatingCanvas);
                });
                destroyWhenReturningSafe(parent: sudsRatingCanvas);
            }
        }

        static GameObject NewGameObject(string objectName, Transform parent = null, Vector3 position = default)
        {
            GameObject newobject = new GameObject(objectName);
            position.SetY(RectTransformToYAxisCoord(parent))
            return newobject;
        }

        private static void SubmitSudsRating(int sudsValue, GameObject sudsRatingCanvas)
        {
            string directory = System.IO.Path.Combine(Application.persistentDataPath, "LearningWorlds");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string filePath = Path.Combine(directory, "arachno-suds.jsonl");
            string sudsLog = $
            `{{"utc": "{DateTime.UtcNow.ToString("o")}","level": {ExposureController.CurrentLevel},"suds":{sudsValue}}}`;
            File.AppendAllText(filePath, sudsLog + System.Environment.NewLine);
            if (sudsRatingCanvas != null)
            {
                Transform sudsDestroy = sudsRatingCanvas.transform;
                GameObject.Destroy(sudsDestroy.gameObject);
            }
            Toasts.ShowSudsSubmission(displayLog);
        }

    }

    using UnityEngine; 
    namespace Arachnophobia.UI.Tooltips { public class SudsUtilities

        { private static void AddSudsDestroy() { Object.Destroy(GameObject.FindComponent<>);}
    }
}