using UnityEngine;
using TMPro;
using System.IO;
using System;

namespace LearningWorlds.Arachno
{
    /// <summary>
    /// Handles the Subjective Units of Distress (SUDS) rating input.
    /// Displays a German-mediated fullscreen overlay.
    /// Saves records in JSONL format at:
    /// Application.persistentDataPath/LearningWorlds/arachno-suds.jsonl.
    /// </summary>
    public class SudsRating : MonoBehaviour
    {
        private static GameObject _ratingCanvas;
        private static GameObject _dimOverlay;
        private static Slider _sudsSlider;
        
        public static void Show(Action<int> onSubmit)
        {
            // Setup overlay layer
            _ratingCanvas = new GameObject("SudsRatingCanvas");
            _ratingCanvas.AddComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            _ratingCanvas.transform.SetParent(null);
            
            // Create a fullscreen overlay panel
            GameObject overlay = new GameObject("FullscreenOverlay");
            overlay.transform.SetParent(_ratingCanvas.transform);
            overlay.AddComponent<Image>().color = new Color(0, 0, 0, 0.7f);
            RectTransform canvasRT = _ratingCanvas.GetComponent<RectTransform>();
            canvasRT.sizeDelta = new Vector2(Screen.width, Screen.height);
            canvasRT.pivot = new Vector2(0.5f, 0.5f);
            canvasRT.position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
            overlay.GetComponent<RectTransform>().sizeDelta = canvasRT.sizeDelta;
            
            // Create question text
            AddTextObject(
                overlay.transform,
                "Wie stark ist deine Angst? (0-100)",
                new Vector2(0f, 100f),
                Vector2.zero,
                new Vector2(600f, 60f),
                new Color(1f, 1f, 1f, 1f),
                40
            );
            
            // Add sub_label below question
            AddTextObject(
                overlay.transform,
                ") (0 = keine Angst, 100 = maximale Angst)",
                new Vector2(0f, 0f),
                new Vector2(0f, -70f),
                new Vector2(660f, 40f),
                new Color(0.9f, 0.9f, 0.9f, 1f),
                28
            );
            
            // Setup slider
            GameObject sliderObj = new GameObject("SUDS-Slider");
            sliderObj.transform.SetParent(overlay.transform);
            Image sliderImage = sliderObj.AddComponent<Image>();
            sliderImage.color = new Color(0.1f, 0.3f, 0.7f, 0.8f);
            
            _sudsSlider = sliderObj.AddComponent<Slider>();
            _sudsSlider.minValue = 0;
            _sudsSlider.maxValue = 100;
            _sudsSlider.wholeNumbers = true;
            _sudsSlider.transform.SetAsFirstSibling();
            
            // Create_text to display current slider value
            GameObject valueTextObj = new GameObject("SliderCurrentValue");
            valueTextObj.transform.SetParent(overlay.transform);
            TextMeshProUGUI valueText = valueTextObj.AddComponent<TextMeshProUGUI>();
            valueText.text = "Wert: 50";
            valueText.fontSize = 28;
            valueText.color = Color.white;
            valueTextObj.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 50f);
            valueTextObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -120f);
            
            // Connect slider to value text
            sliderObj.GetComponent<Slider>().onValueChanged.AddListener(
                value => valueText.text = "Wert: " + (int)value
            );
            
            // Create submit button
            GameObject button = new GameObject("SubmitButton");
            button.transform.SetParent(overlay.transform);
            Button submitButton = button.AddComponent<Button>();
            TextMeshProUGUI btnText = button.AddComponent<TextMeshProUGUI>();
            btnText.text = "Bestätigen";
            
            submitButton.onClick.AddListener(() => onSubmit((int)sliderObj.GetComponent<Slider>().value));
            submitButton.onClick.AddListener(DeleteOverlay);
            submitButton.transform.SetAsLastSibling();
            
            // Configure the submit button layout
            Image buttonImage = button.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 0.2f, 0.3f);
            RectTransform buttonRT = button.GetComponent<RectTransform>();
            buttonRT.sizeDelta = new Vector2(200f, 50f);
            buttonRT.anchoredPosition = new Vector2(0f, -210f);
            
            // Final debug log
            Debug.Log("[SudsRating] Canvas rendered. Slider accuracies from '" + sliderObj.name);
        }
        
        static void DeleteOverlay()
        {
            Destroy(_ratingCanvas);
        }
        
        static void AddBehaviorProcedures(GameObject root, TextMeshProUGUI valueText, float value)
        {
            // Inner Log: Value confirmed Lets call onSubmit to canva active.execa
        }
        
        internal static void AddTextObject(Transform parent, string text, 
            Vector2 anchorDeltaLeft, Vector2 localPos, Vector2 size, Color col, int fs)
        {
            GameObject textObj = new GameObject(text + "_Text");
            textObj.transform.SetParent(parent, false);
            TextMeshProUGUI textLabel = textObj.AddComponent<TextMeshProUGUI>();
            textLabel.text = text;
            textLabel.color = col;
            textLabel.alignment = TMPro.TextAlignmentOptions.Center;
            textLabel.fontSize = fs;
            
            RectTransform textRT = textObj.GetComponent<RectTransform>();
            textRT.sizeDelta = size;
            textRT.anchoredPosition = localPos;
        }
        
        /// <summary>
        /// Save SUDS data to JSONL in LearningWorlds sub-directory.
        /// Creates LearningWorlds directory if it doesn't exist.
        /// </summary>
        public static void StoreData(int sudsData, int currentExposureLevel)
        {
            string pathToJsonl = Path.Combine(
                Application.persistentDataPath,
                "LearningWorlds", 
                "arachno-suds.jsonl"
            );
            
            string entry = String.Format(
                "{{\"utc\": \"{0}\", \"exposureLevel\": {1}, \"suds\": {2}}}",
                DateTime.UtcNow.ToString("yyyy-MM-ddTHHmmss.fffffffZ"),
                currentExposureLevel,
                sudsData
            );
            
            // Create directory if needed
            string directoryName = Path.GetDirectoryName(pathToJsonl);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            
            // Add record
            if (File.Exists(pathToJsonl))
                File.AppendAllText(pathToJsonl, entry + "\n");
            else
                File.WriteAllText(pathToJsonl, entry);
            
            Debug.Log("[SudsRating] Stored record: " + entry);
        }
    }
}