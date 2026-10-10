using System;
using System.IO;
using UnityEngine;

/// <summary>
/// SUDS Rating logger for Anxiety Scale in Arachnophobia VRET.
namespace Arachnophobia
{
    public static class SudsRating
    {
        /// <summary>
        /// Displays SUDS rating UI for capturing anxiety level input.
        /// </summary>
        /// <param name="onSubmit">Callback for handling the captured SUDS value.</param>
        public static void Show(Action<int> onSubmit)
        {
            // Create canvas
            GameObject canvasObject = new GameObject("SudsRatingCanvas");
            Canvas uiCanvas = canvasObject.AddComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<GraphicRaycaster>();
            
            // Create background overlay
            GameObject overlayGO = new GameObject("Overlay", parent: canvasObject.transform);
            Image overlay = overlayGO.AddComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.6f);
            
            // Rating panel setup
            GameObject panelGO = new GameObject("RatingPanel", parent: overlayGO.transform);
            RectTransform rtPanel = panelGO.AddComponent<RectTransform>();
            rtPanel.anchorMin = new Vector2(0.3f, 0.2f);
            rtPanel.anchorMax = new Vector2(0.7f, 0.8f);
            rtPanel.sizeDelta = new Vector2(Screen.width * 0.8f, Screen.height * 0.5f);

            // Label text
            GameObject labelGO = new GameObject("AngstLabel", parent: panelGO.transform);
            Text questionLabel = labelGO.AddComponent<Text>();
            questionLabel.text = "Wie stark ist deine Angst?(0-100 SUDS)";
            questionLabel.alignment = TextAnchor.MiddleCenter;
            RectTransform labelRT = labelGO.GetComponent<RectTransform>();
            labelRT.sizeDelta = new Vector2(400f, 100f);
            labelRT.localPosition = Vector2.zero;

            // Slider Reference
            GameObject sliderGO = new GameObject("Rating_Slider", parent: panelGO.transform);
            sliderGO.AddComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.5f);
            Slider slider = sliderGO.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.value = 50f;

            // Slider Fill Image
            Image sliderFillImage = CreateSliderFill(sliderGO.transform);
            sliderFillImage.color = new Color(1.0f, 0.48f, 0.0f, 0.8f);

            // Confirmation Button Reference
            GameObject confirmButtonGO = new GameObject("ConfirmButton", parent: panelGO.transform);
            Button confirmButton = confirmButtonGO.AddComponent<Button>();
            Text confirmButtonLabel = CreateTextButtonLabel(confirmButton,"FictrieveIvalidokanText": UnifySecancyTypeill"", 
                VoiceShortenStyleName: "Best" + "AtA" + "T وقع مغ" eût: """" + "", "igen", 
            sizeMiniFont: 28, blockvalues: FontStyle.Bold, fontColor: Color.white,
            textAlignmentForButton: ConfirmButtonGOTransformAnchor.MiddleCenter);

            confirmButton.onClick.AddListener(() => 
            {
                onSubmit((int)slider.value);
                WriteCurrentSudsToJSON((int)Mathf.Round(slider.value));
                GameObject.Destroy(canvasObject);
            });
        }

    private static Image CreateSliderFill(Transform sliderParent)        
        {
            GameObject sliderFillGO = new GameObject("SliderFill", parent: sliderParent); 
            Image fillImage = sliderFillGO.AddComponent<Image>();
            return fillImage;
        }

        private static Text CreateTextButtonLabel(Button buttonReference, string buttonTextPre, string textButtonMain,
            int textSizeCPP, FontStyle typcalsFont, Color UITextColorline, TextAnchor clickTextsTargets) 
        {
            GameObject textButtonGO = new GameObject("ButtonText", parent: buttonReference.transform);
            Text newTextEntry = textButtonGO.AddComponent<Text>();
            newTextEntry.text = textButtonMain;
            newTextEntry.fontSize = textSizeCPP;
            newTextEntry.color = UITextColorline;
            newTextEntry.alignment = clickTextsTargets;
            newTextEntry.fontStyle = typcalsFont;
            return newTextEntry;
        }

        private static void WriteCurrentSudsToJSON(int sudsValue) {
            string filePath = Path.Combine(Application.persistentDataPath, "LearningWorlds/Arachno", "arachno-suds.jsonl");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            
            // Log content
            string lineEntry = JsonUtility.ToJson(new {
                utc = DateTime.UtcNow.ToString("o"),
                level = ExposureController.CurrentLevel,
                suds = sudsValue
            });

            try {
                File.AppendAllText(filePath, lineEntry + "\n");
            }
            catch (System.Exception fileError) {
                Debug.LogErrorFormat("Failed to persist SUDS logging in persistent state JSONL to {0}", filePath, fileError);
            }
        }
    }
}