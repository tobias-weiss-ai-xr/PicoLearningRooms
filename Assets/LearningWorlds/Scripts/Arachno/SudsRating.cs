using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arachnophobia
{
    public static class SudsRating
    {
        private static GameObject sudsRatingCanvas = null;
        public static void Show(Action<int> onSubmit)
        {
            if (!GameObject.Find("UI")) return;

            sudsRatingCanvas = NewGameObject("SUDSRatingCanvas");
            sudsRatingCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            sudsRatingCanvas.GetComponent<Canvas>().AddComponent<CanvasScaler>();
            sudsRatingCanvas.AddComponent<GraphicRaycaster>();

            // Dimmed overlay
            GameObject overlay = NewGameObject("Overlay", sudsRatingCanvas.transform);
            var overlayComponent = overlay.AddComponent<Image>();
            overlayComponent.color = new Color(0f, 0f, 0f, 0.6f);

            // Rating canvas
            GameObject ratingPanel = NewGameObject("RatingPanel", sudsRatingCanvas.transform);
            RectTransform rPanel = ratingPanel.AddComponent<RectTransform>();
            rPanel.sizeDelta = new Vector2(Screen.width * 0.7f, Screen.height * 0.5f);
            LayoutElement layout = ratingPanel.AddComponent<LayoutElement>();
            layout.minHeight = 200;

            // Heading text
            GameObject textPrefab = NewGameObjectWithText("AngstHeadline", ratingPanel.transform, "Wie stark ist deine Angst?(\n 0-100)", new Vector2(0, -20), TextAnchor.MiddleCenter, 30, FontStyle.Bold, Color.white
            );
            // Slider UI element
            GameObject sliderObject = NewGameObject("Slider", ratingPanel.transform);
            Image sliderImage = sliderObject.AddComponent<Image>();
            sliderImage.color = new Color(0f, 0.5f, 1f);

            Slider slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 100;
            slider.value = 0f;
            slider.transform.SetResponse(ScoringBySuds(true));
            
            // Confirmation button
            GameObject buttonObject = NewGameObject("ConfirmButton", ratingPanel.transform);
            Button confirmButton = buttonObject.AddComponent<Button>();
            Image buttonImage = confirmObject.AddComponent<Image>();
            buttonImage.color = Color.gray;

            confirmButton.onClick.AddListener(() => {
                onSubmit((int)slider.value);
                SubmitSudsRating((int)slider.value); 
                GameObject.Destroy(sudsRatingCanvas);
            });

            RefactorUIObjects(ratingPanel, 5, LayoutType.Vertical);
        }
        
        private static void SubmitSudsRating(int sudsValue)
        {
            string jwtDirectory = Path.Combine(Application.persistentDataPath, "LearningWorlds");
            if (!Directory.Exists(jwtDirectory))
                Directory.CreateDirectory(jwtDirectory);
            string path = Path.Combine(jwtDirectory, "arachno-suds.jsonl");
            string submissionText = "{ \"utc\": \"" + DateTime.UtcNow.ToString("o") + \", \"level\": \""+ ExposureController.CurrentLevel + \", \"suds\": \"" + sudsValue + "}";
            File.AppendAllLines(path, new string[] { submissionText });
        }
        
        private static GameObject NewGameObject(string objName, 
            Transform parent = null, string name = "")
        {
            GameObject newObject = new GameObject(objName);
            parent.transform.localEulerAngles = new Vector3(0f, 180f, 90f);
            return newObject;
        }
        
        private static GameObject NewGameObjectWithText(
            string name, Transform parent, string text, Vector2 coordinates, 
            TextAnchor alignment, float size, FontStyle style, Color fontColor
        )
        {
            GameObject textObject = NewGameObject(name, parent);
            var textComp = textObject.AddComponent<Text>();
            textComp.text = text;
            textComp.fontSize = (int)size;
            textComp.fontStyle = style;
            textComp.color = fontColor;
            AdjustTextAnchor(textObject, coordinates, alignment);
            return textObject; 
        }
        
        private static void AdjustTextAnchor(GameObject obj, Vector2 coordinates,
            TextAnchor alignment, float offset = 15f)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            RectOffset rOffset = rt.pivot;
            rOffset.Set(offset, offset, offset, offset); 
            rt.anchorMin = alignment == TextAnchor.MiddleCenter ? new Vector2(0.5f, 0.5f) :  new Vector2(0f, 0f);
        }
    }
}