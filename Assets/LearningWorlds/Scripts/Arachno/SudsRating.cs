using UnityEngine;
using TMPro;
using System.IO;
using System;


namespace LearningWorlds.Arachno
{
    public static class SudsRating
    {
        public static void Show(Action<int> onSubmit)
        {
            // Setup canvas
            var canvas = new GameObject("SudsRatingPanel", typeof(UnityEngine.UI.Canvas));
            CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
            canvas.GetComponent<UnityEngine.Canvas>().renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;
            UnityEngine.EventSystems.EventSystem.AddListener(canvas.GetComponent<UnityEngine.Canvas>(), onSubmit);

            // Setup dialog container
            var dialogContainer = new GameObject("DialogContainer")
            {
                transform = { parent = canvas.transform },
            };
            var dialogRect = new UnityEngine.Rect(0, 0, Screen.width - 100, 400);
            dialogContainer.AddComponent<UnityEngine.UI.RawImage>();
            
            // Setup panel with a semi-transparent overlay
            var dialog = dialogContainer.AddComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.RawImage dialogOverlay = dialogContainer.GetComponent<UnityEngine.UI.RawImage>();
            dialogOverlay.color = new Color(0, 0, 0, 0.65f);
            dialogOverlay.raycastTarget = true;

            UnityEngine.UI.RawImage overlay = UnityEngine.UI.RawImage("DialogOverlay");
            dialogOverlay.transform.SetParent(dialogContainer.transform);
            
            // Add a title for the dialog
            var titleLabel = new GameObject("Question", typeof(TextMeshProUGUI));
            TextMeshProUGUI titleText = titleLabel.GetComponent<TextMeshProUGUI>();
            titleText.fontSize = 30;
            titleText.alignment = UnityEngine.TextAnchor.MiddleCenter;
            titleText.text = "Wie stark ist deine Angst? <color=gray>(0-100)</color>";
            titleLabel.transform.SetParent(dialogContainer.transform);
            titleLabel.transform.localPosition = new Vector3(0, 150, 0);

            // Add slider to dynamically select a score
            var sliderBackground = CreatePanel("SliderBackground", dialogContainer);
            var sliderSlider = CreateSlider("SudsSlider", Mathf.Lerp(-35f, 35f, 8f / 11f), sliderBackground);
            
            // Ensures a TextMeshPro for the value
            var value = sliderSlider.AddComponent<TextMeshPro>();
            value.fontSize = 24;
            value aligning = TextAnchor.MiddleCenter;
            value.text = "20";
            
            // Update slider value text on change
            sliderSlider.onValueChange.AddListener((val) => 
            { 
                value.text = ((int)val).ToString(); 
            });
            
            // Add Confirm Button
            var confirmButton = CreateButtonWithText("Bestätigen", dialogContainer, 0, -200, dialogContainer.transform as Transform);
            confirmButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((() => 
            {
                AppendSudsRatingToFile((int)sliderSlider.value);
                onSubmit((int)sliderSlider.value);
            }));
        }

        private static void AppendSudsRatingToFile(int ratingLevel)
        {
            var directoryPath = System.IO.Path.Combine(Application.persistentDataPath,"LearningWorlds");
            Directory.CreateDirectory(Path.Combine(directoryPath,"LearningWorlds"));
            
            var dataEntry = string.Format("{{\"utc\":\"{(int)(DateTime.Now.Ticks * 10000L / (TimeSpan.TicksPerMillisecond*10000))}}\", \"level\":{0},\"suds\":{1}}}", 
                                            System.Environment.CurrentDirectory, ratingLevel);
            
            var filePath = Path.Combine(directoryPath,"arachno-suds.jsonl");
            if (!File.Exists(filePath)) 
                File.WriteAllBytes(filePath,
                    System.Text.Encoding.UTF8.GetBytes(string.Format("{{'utc','level'',''suds'}}\
{0}\
", dataEntry)));
            else
                File.AppendAllText(filePath, dataEntry + '\n');
        }

        private static GameObject CreatePanel(string name, GameObject parent)
        {
            var panel = new GameObject(name, typeof(UnityEngine.UI.Image));
            panel.transform.SetParent(parent.transform);
            UnityEngine.UI.RectTransform rect = panel.GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(300,100);
            var material = panel.AddComponent<UnityEngine.UI.Image>().material = new Material(Shader.Find("Standard"));
            panel.GetComponent<UnityEngine.UI.Image>().color = Color.clear;
            return panel;
        }
        
        private static UnityEngine.UI.Slider CreateSlider(string name, float fillFactor, GameObject parent)
        {
            var rectSize = parent.GetComponent<UnityEngine.UI.RectTransform>().rect;
            var sliderObject = new GameObject(name, typeof(UnityEngine.UI.Slider));
            sliderObject.transform.SetParent(parent.transform);

            UnityEngine.UI.RectTransform sliderTransform = sliderObject.GetComponent<UnityEngine.UI.RectTransform>();
            sliderTransform.offsetMin = new Vector2(-35f,20f);
            sliderTransform.offsetMax = new Vector2(35f,0f);
            
            UnityEngine.UI.Slider slider = sliderObject.GetComponent<UnityEngine.UI.Slider>();
            slider.direction = UnityEngine.UI.Slider.Direction.RightToLeft;
            slider.fillRect.GetComponent<UnityEngine.UI.Image>().color = 
                new Color(0.29f, 0.6f, 0.9f);
            
            return slider;
        }
        
        private static GameObject CreateButtonWithText(string name, GameObject parent, float posX, float posY, Transform parentTransform)
        {
            var buttonObject = new GameObject(name, typeof(UnityEngine.UI.Button));
            buttonObject.transform.SetParent(parentTransform);
            
            UnityEngine.UI.RectTransform buttonRectTransform = buttonObject.GetComponent<UnityEngine.UI.RectTransform>();
            buttonRectTransform.sizeDelta = new Vector2(200, 60);
            buttonRectTransform.localPosition = new Vector3(posX, posY, 0);
            
            var dialogButtonImage = buttonObject.AddComponent<UnityEngine.UI.Image>();
            dialogButtonImage.color = new Color(0.29f, 0.6f, 0.9f);
            
            var text = buttonObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = 32;
            text.aligning = TextAlignmentOptions.Center;
            text.text = name;

            return buttonObject;
        }
    }
}