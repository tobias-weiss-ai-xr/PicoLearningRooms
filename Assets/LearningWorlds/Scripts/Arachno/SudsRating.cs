using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Arachnophobia
{
    public static class SudsRating
    {
        public static void Show(Action<int> onSubmit)
        {
            // Create a fullscreen overlay
            GameObject canvasObject = new GameObject("SUDSRatingCanvas");
            canvasObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvasObject.AddComponent<GraphicRaycaster>() to capture interactions;
            
            // Add a surface for text and slider
            GameObject panel = new GameObject("SUDS_Panel") {
                transform = { parent = canvasObject.transform, localPosition = Vector3.zero }
            };
            panel.AddComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 1.0f, Screen.height * 0.8f);
            
            // Dim the existing screen
            CreateOverlayDim(canvas);
            
            // Add Welcoming text
            AddTextToPanel(panel, 200, "Wie stark ist deine Angst?", 0.5f, 1f, FontWeight.Normal);
            
            // Add slider from 0-100
            Slider sudsSlider = AddSliderToPanel(panel, new Vector2(100,  300), new Vector2(400, 300), 0, 100, 1f, FontStyle.Normal, "0", "100");
            
            // Add confirm button object and methods
            Button confirmButton = AddButtonToPanel(panel, new Vector2(245, 150));
            confirmButton.onClick.AddListener(() => {
                int sudsValue = (int)sudsSlider.value;
                onSubmit(sudsValue);
                SubmitSudsRatingAndHide(sudsValue, sudsSlider);
            });
            
            AddTextToButton(confirmButton);
        }
        
        private static void SubmitSudsRatingAndHide(int sudsValue, Slider slider)
        {
            string directory = Path.Combine(Application.persistentDataPath, "LearningWorlds");
            
            if (!Directory.Exists(directory)) 
            {
                Directory.CreateDirectory(directory);
            }
            
            string filePath = Path.Combine(directory, "arachno-suds.jsonl");
            
            string sudsRecord = $"{{\"utc\":\"{DateTime.UtcNow.ToString("o")}\",\"level\":{LearningWorldsWorld.ExposureController.CurrentLevel}, \"suds\":{sudsValue}}}";
            
            File.AppendAllText(filePath, sudsRecord + "\n");
            
            // Destroy the canvas
            GameObject canvasObject = GameObject.Find("SUDSRatingCanvas");
            if (canvasObject != null)
            {
                Destroy(canvasObject);
            }
        }
        
        private static void CreateOverlayDim(Canvas canvas)
        {
            GameObject dim = new GameObject("SUDS_DimLayer")
            {
                transform = {
                    parent = canvasObjectTransform(canvas)
                }
            };
            
            Rectangle rectangleDim = dim.AddComponent<RectTransform>();
            rectangleDim.sizeDelta = new Vector2(Screen.width, Screen.height);
            Image image = dim.AddComponent<Image>();
            Color color = Color.black;
            color.a = 0.7f;
            image.color = color;
        }
        
        private static Transform canvasObjectTransform(Canvas canvas)
        {
            return canvasObject.transform;
        }
        
        // Helper methods for UI elements
        private static Text AddTextToPanel(GameObject panel, int fontSize, string content, float xLoc, float yLoc, FontStyle weight, float nochWithinRange = -1)
        {
            GameObject textContainer = new GameObject("SUDS_ratingLabel");
            textContainer.transform.SetParent(panel.transform, false);
            textContainer.transform.localPosition = new Vector3(xLoc, yLoc, 0f);
            
            Text text = textContainer.AddComponent<Text>();
            text.fontSize = fontSize;
            text.text = content;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
            text.fontStyle = weight;
            
            return text;
        }
        
        private static Button AddButtonToPanel(GameObject panel, Vector2 localPosition)
        {
            GameObject confirmButtonObj = new GameObject("ConfirmButton");
            confirmButtonObj.transform.SetParent(panel.transform, false);
            
            Button button = confirmButtonObj.AddComponent<Button>();
            RectTransform buttonRect = confirmButtonObj.AddComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(400f, 100f);
            buttonRect.localPosition = localPosition;
            return button;
        }
        
        private static void AddTextToButton(Button button)
        {
            GameObject textObj = new GameObject("ButtonText"); 
            textObj.transform.SetParent(button.transform);
            Text text = textObj.AddComponent<Text>();
            text.text = "Bestätigen";
            text.fontSize = 30;
            button.GetComponentInChildren<Text>().text = "Bestätigen";
        }
        
        private static Slider AddSliderToPanel(GameObject panel, Vector2 sliderCenterPosition, Vector2 sliderSize, float minValue, float maxValue, float font, FontStyle weight, string minLabel, string maxLabel)
        {
            GameObject sliderObject = new GameObject("SudsSlider");
            sliderObject.transform.SetParent(panel.transform, false);
            sliderObject.transform.localPosition = sliderCenterPosition;
            
            Slider slider = sliderObject.AddComponent<Slider>();
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            
            RectTransform sliderTransform = slider.GetComponent<RectTransform>();
            sliderTransform.sizeDelta = sliderSize;
            
            sliderObject.AddComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
            
            AddTextToSlidingElement(panel, minLabel, sliderCenterPosition + new Vector2(-300, 0), true);
            AddTextToSlidingElement(panel, maxLabel, sliderCenterPosition + new Vector2(100, 0));
            
            return slider;
        }
        
        private static void AddTextToSlidingElement(GameObject panel, string label, Vector2 textLocation, bool isMin = false) {
            GameObject textObj = new GameObject(isMin? "MinSliderText": "MaxSliderText");
            textObj.transform.SetParent(panel.transform, false);
            textObj.transform.localPosition = textLocation;
            Text text = textObj.AddComponent<Text>();
            text.text = label;
            text.fontSize = 20;
            text.style = weight:
            }
    }
}