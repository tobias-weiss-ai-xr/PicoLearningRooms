using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace LearningWorlds.Arachnophobia
{
    /// <summary>
    /// Displays a Subjective Units of Distress (SUDs) rating scale for therapy sessions.
    /// Measures anxiety level from 0-100.
    /// </summary>
    public class SudsRating : MonoBehaviour
    {
        /// <summary>
        /// Shows the SUDs rating dialog.
        /// </summary>
        /// <param name="onSubmit">Callback when user submits their rating.</param>
        public static void Show(Action<int> onSubmit)
        {
            Canvas canvas = CreateDisplayOverlay();
            Slider slider = CreateSudsSlider(canvas.transform);
            CreateConfirmButton(canvas.transform, slider, onSubmit);
        }

        /// <summary>
        /// Creates the full-screen overlay canvas.
        /// </summary>
        private static Canvas CreateDisplayOverlay()
        {
            GameObject canvasObj = new GameObject("SudsCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            canvasObj.AddComponent<GraphicRaycaster>();

            // Dark semi-transparent background
            GameObject bg = new GameObject("Background");
            bg.transform.SetParent(canvas.transform);
            Image bgImage = bg.AddComponent<Image>();
            bgImage.color = new Color(0f, 0f, 0f, 0.7f);

            RectTransform bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            return canvas;
        }

        /// <summary>
        /// Creates the SUDs slider control.
        /// </summary>
        private static Slider CreateSudsSlider(Transform parent)
        {
            GameObject sliderObj = new GameObject("SudsSlider");
            sliderObj.transform.SetParent(parent);

            Slider slider = sliderObj.AddComponent<Slider>();
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = 0;
            slider.maxValue = 100;
            slider.value = 50;

            // Add fill area and handle
            GameObject fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(sliderObj.transform);
            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.color = Color.cyan;

            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = new Vector2(-20, -10);

            GameObject handleObj = new GameObject("Handle");
            handleObj.transform.SetParent(sliderObj.transform);
            Image handleImage = handleObj.AddComponent<Image>();
            handleImage.color = Color.white;

            RectTransform handleRect = handleObj.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 20);

            slider.fillRect = fillRect;
            slider.handleRect = handleRect;

            // Position and size
            RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.5f, 0.5f);
            sliderRect.anchorMax = new Vector2(0.5f, 0.5f);
            sliderRect.sizeDelta = new Vector2(300, 25);
            sliderRect.anchoredPosition = Vector2.zero;

            // Label
            GameObject textObj = new GameObject("Label");
            textObj.transform.SetParent(parent);
            Text labelText = textObj.AddComponent<Text>();
            labelText.text = "Wie stark ist deine Angst? (0-100)";
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.color = Color.white;
            labelText.fontSize = 24;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(400, 30);
            textRect.anchoredPosition = new Vector2(0f, 50f);

            return slider;
        }

        /// <summary>
        /// Creates the confirm button.
        /// </summary>
        private static Button CreateConfirmButton(Transform parent, Slider slider, Action<int> onSubmit)
        {
            GameObject buttonObj = new GameObject("ConfirmButton");
            buttonObj.transform.SetParent(parent);

            Button button = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = Color.green;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);
            Text text = textObj.AddComponent<Text>();
            text.text = "Bestätigen";
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = new Vector2(-20, -20);

            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(200, 50);
            buttonRect.anchoredPosition = new Vector2(0f, -60f);

            ConfirmHandler handler = buttonObj.AddComponent<ConfirmHandler>();
            handler.slider = slider;
            handler.onSubmit = onSubmit;

            button.onClick.AddListener(handler.OnButtonClick);

            return button;
        }

        /// <summary>
        /// Handles button click and saves rating.
        /// </summary>
        public class ConfirmHandler : MonoBehaviour
        {
            public Slider slider;
            public Action<int> onSubmit;

            public void OnButtonClick()
            {
                int sudsRating = (int)slider.value;
                SaveSudsRating(sudsRating);
                onSubmit?.Invoke(sudsRating);

                // Destroy the canvas after submission
                Destroy(transform.parent.parent.gameObject);
            }

            private void SaveSudsRating(int suds)
            {
                try
                {
                    string dirPath = Path.Combine(Application.persistentDataPath, "LearningWorlds");
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    string filePath = Path.Combine(dirPath, "arachno-suds.jsonl");
                    string entry = $"{{\"utc\":\"{DateTime.UtcNow:O}\",\"level\":{ExposureController.CurrentLevel},\"suds\":{suds}}}";
                    File.AppendAllText(filePath, entry + "\n");
                    Debug.Log("SUDs rating saved: " + suds);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Failed to save SUDs rating: " + ex.Message);
                }
            }
        }
    }
}
