using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

namespace LearningWorlds.Arachno
{
    public static class SudsRating
    {
        private static Canvas _uiCanvas;
        private static Slider _sudsSlider;
        private static Button _submitButton;
        private static Text _instructionText;
        private static string _filePath;

        public static void Show(Action<int> onSubmit)
        {
            // Create persistent directory if it doesn't exist
            EnsurePathExists();

            // Create canvas
            CreateUICanvas();

            // Create UI elements
            CreateInstructionText();
            CreateSlider(onSubmit);
            CreateSubmitButton(onSubmit);
        }

        private static void CreateUICanvas()
        {
            // Create full-screen canvas for overlay
            GameObject canvasObj = new GameObject("SudsRatingCanvas");
            _uiCanvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Make it a full screen dim
            _uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Create a full screen half-transparent background
            GameObject backgroundObj = new GameObject("BackgroundPanel");
            backgroundObj.transform.SetParent(_uiCanvas.transform);
            
            Image background = backgroundObj.AddComponent<Image>();
            background.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform bgTransform = backgroundObj.GetComponent<RectTransform>();
            bgTransform.anchorMin = Vector2.zero;
            bgTransform.anchorMax = Vector2.one;
            
            // Create a panel for the main content
            GameObject contentPanel = new GameObject("SudsContentPanel");
            contentPanel.transform.SetParent(_uiCanvas.transform);
            contentPanel.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            
            RectTransform contentTransform = contentPanel.GetComponent<RectTransform>();
            contentTransform.sizeDelta = new Vector2(400, 250);
            contentTransform.anchorMin = new Vector2(0.6f, 0.6f); // Centered-ish
            contentTransform.anchorMax = new Vector2(0.4f, 0.4f); // Reverse anchor for centered position
            contentTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private static void CreateInstructionText()
        {
            // Create instruction text
            GameObject textObj = new GameObject("InstructionText");
            textObj.transform.SetParent(_uiCanvas.transform);
            _instructionText = textObj.AddComponent<Text>();

            // Text configuration
            _instructionText.fontSize = 24;
            _instructionText.color = Color.white;
            _instructionText.alignment = TextAnchor.MiddleCenter;
            _instructionText.text = "Wie stark ist deine Angst?\n(0 = keine Angst, 100 = maximale Angst)";
            
            RectTransform textTransform = textObj.GetComponent<RectTransform>();
            textTransform.sizeDelta = new Vector2(320, 80);
            textTransform.position = new Vector3(0, 75, 0); // Position relative to canvas
        }

        private static void CreateSlider(Action<int> onSubmit)
        {
            // Create a slider object
            GameObject sliderObj = new GameObject("SudsSlider");
            sliderObj.transform.SetParent(_uiCanvas.transform);
            RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.5f, 0.55f);
            sliderRect.anchorMax = new Vector2(0.5f, 0.5f);
            sliderRect.pivot = new Vector2(0.5f, 0.5f);
            sliderRect.sizeDelta = new Vector2(300, 25);

            // Create slider component
            _sudsSlider = sliderObj.AddComponent<Slider>();
            _sudsSlider.minValue = 0;
            _sudsSlider.maxValue = 100;
            _sudsSlider.value = 50; // Default value
            
            // Create slider fill area
            GameObject fillObj = new GameObject("SliderFill");
            fillObj.transform.SetParent(sliderObj.transform);
            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.color = new Color(0.7f, 0.7f, 0.7f);
            
            // Create slider handle
            CreateSliderHandle(sliderObj.transform);

            // Add value text to the slider
            GameObject textObj = new GameObject("ValueText");
            textObj.transform.SetParent(sliderObj.transform);
            Text text = textObj.AddComponent<Text>();
            text.color = Color.white;
            text.fontSize = 20;
            text.alignment = TextAnchor.MiddleCenter;
            text.rectTransform.anchorMin = Vector2.one * 2f;
            text.rectTransform.sizeDelta = new Vector2(50, 30);
            
            // Call value update function on change
            _sudsSlider.onValueChanged.AddListener((value) => text.text = Mathf.Round(value).ToString());
        }

        private static void CreateSubmitButton(Action<int> onSubmit)
        {
            // Create a submit button object
            GameObject buttonObj = new GameObject("SubmitButton");
            buttonObj.transform.SetParent(_uiCanvas.transform);
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(150, 50);
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.position = new Vector2(-25, -85); // Position relative to panel

            // Create button component
            _submitButton = buttonObj.AddComponent<Button>();
            
            // Create text for the button
            GameObject textObj = new GameObject("ButtonText");
            textObj.transform.SetParent(buttonObj.transform);
            Text buttonText = textObj.AddComponent<Text>();
            buttonText.text = "Bestätigen";
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.fontSize = 22;
            buttonText.color = Color.white;
                
            // Make the child text fill the button area
            textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50);

            // Set button color
            Image bgImage = buttonObj.AddComponent<Image>();
            bgImage.color = new Color(0f, 0.6f, 0.9f);

            // Add the click event
            _submitButton.onClick.AddListener(() => {
                int sudsValue = Mathf.RoundToInt(_sudsSlider.value);
                LogSudsValue(sudsValue);
                onSubmit?.Invoke(sudsValue);
                Destroy(_uiCanvas.gameObject);
            });
        }

        private static void CreateSliderHandle(Transform sliderTransform)
        {
            // Create the handle object
            GameObject handleObj = new GameObject("SliderHandle");
            handleObj.transform.SetParent(sliderTransform);
            RectTransform handleRect = handleObj.AddComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(30, 40);

            // Create and set up the image component
            Image handleImage = handleObj.AddComponent<Image>();
            handleImage.color = new Color(0.9f, 0.9f, 0.9f);
            handleImage.material = new Material(Shader.Find("Standard"));
        }

        private static void EnsurePathExists()
        {
            string dirPath = Path.Combine(Application.persistentDataPath, "LearningWorlds");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            _filePath = Path.Combine(dirPath, "arachno-suds.jsonl");
        }

        private static void LogSudsValue(int sudsValue)
        {
            string jsonLine = $"{DateTime.UtcNow:o},{sudsValue}\n";
            File.AppendAllText(_filePath, jsonLine);
        }
    }
}