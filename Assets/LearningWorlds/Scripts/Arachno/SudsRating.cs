using UnityEngine;
using System.IO;
using System;

public class SudsRating : MonoBehaviour
{
    public static void Show(Action<int> onSubmit)
    {
        Canvas dim = CreateDisplayOver();
        Slider slider = CreateSudsSlider(dim.transform);
        Button confirmButton = CreateConfirmButton(dim.transform, slider, onSubmit);
    }

    private static Canvas CreateDisplayOver()
    {
        Canvas backgroundCanvas = new GameObject("BackgroundCanvas").AddComponent<Canvas>();
        backgroundCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = backgroundCanvas.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        Vector2 referenceResolution = new Vector2(1920, 1080);
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        GraphicRaycaster graphicRaycaster = backgroundCanvas.gameObject.AddComponent<GraphicRaycaster>();

        GameObject bg = new GameObject("DimBackground", typeof(Image));
        bg.transform.SetParent(backgroundCanvas.transform);
        Image bgImage = bg.GetComponent<Image>();
        bgImage.color = new Color(0f, 0f, 0f, 0.7f);
        bg.transform.localScale = new Vector3(5, 5, 1);

        return backgroundCanvas;
    }

    private static Slider CreateSudsSlider(Transform parent)
    {
        GameObject sliderObj = new GameObject("SudsSlider", typeof(Slider), typeof(Image));
        sliderObj.transform.SetParent(parent);

        Slider slider = sliderObj.GetComponent<Slider>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 50;

        RectTransform rectTransform = sliderObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 25);
        rectTransform.localPosition = new Vector3(0f, 0f, 0);
        rectTransform.anchoredPosition = new Vector2(0f, 0f);

        Text text = sliderObj.AddComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.text = "Wie stark ist deine Angst? (0-100)";

        return slider;
    }

    private static Button CreateConfirmButton(Transform parent, Slider slider, Action<int> onSubmit)
    {
        GameObject buttonObj = new GameObject("ConfirmButton", typeof(Button), typeof(Text));
        buttonObj.transform.SetParent(parent);

        Button confirmButton = buttonObj.GetComponent<Button>();
        Text btText = buttonObj.GetComponentInChildren<Text>();
        btText.text = "BestÃ¤tigen";

        ConfirmHandler actionHandler = buttonObj.AddComponent<ConfirmHandler>();
        actionHandler.slider = slider;
        actionHandler.onSubmit = onSubmit;

        RectTransform rectButton = buttonObj.GetComponent<RectTransform>();
        rectButton.sizeDelta = new Vector2(200, 50);
        rectButton.localPosition = new Vector3(0f, -60f, 0);

        return confirmButton;
    }

    private class ConfirmHandler : MonoBehaviour {
        public Slider slider;
        public Action<int> onSubmit;

        public void OnButtonClick() {
            int sudsRating = (int)slider.value;
            SaveSUDsRating(sudsRating);
            onSubmit?.Invoke(sudsRating);
        }

        public void SaveSUDsRating(int suds) {
            string filepath = Path.Combine(Application.persistentDataPath, "LearningWorlds");

            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);

            string sudsFilePath = Path.Combine(filepath, "arachno-suds.jsonl");
            string sudsEntry = $"{{\"utc\":\"{DateTime.UtcNow:O}\",\"level\":{ExposureController.CurrentLevel},\"suds\":{suds}}}";

            File.AppendAllText(sudsFilePath, sudsEntry + "\n");
        }
    }
}