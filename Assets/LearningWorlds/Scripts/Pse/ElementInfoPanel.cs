using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;

namespace LearningWorlds.PSE
{
    public class ElementInfoPanel : MonoBehaviour
    {
        private Element _currentElement;
        private Canvas _canvas;
        private static readonly string[] _textKeys = {"name", "symbol", "number", "atomic_mass", "category", "phase", "meltingTemp", "boilingTemp", "density", "electronegativity", "appearance", "electronConfig", "summary"};

        public void Initialize(Element currentElement)
        {
            _currentElement = currentElement;
            CreateUIPanel();
            UpdatePanel(_currentElement);
        }

        private void CreateUIPanel()
        {
            GameObject canvasObj = new GameObject("ElementInfoCanvas");
            _canvas = canvasObj.AddComponent<Canvas>();
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Define WorldSpace canvas
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = LearningWorldsApp.Instance.rigCamera;

            UnityEngine.UI.GraphicRaycaster rayCaster = canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            // Set canvas dimensions and position it in front of the selected element
            RectTransform canvasRT = canvasObj.GetComponent<RectTransform>();
            canvasRT.sizeDelta = new Vector2(550, 750);
            canvasRT.pivot = new Vector2(0.5f, 1.0f);
            canvasObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5f;

            SceneBuilder.AddWorldSpaceUI(canvasObj);
            CreateTextFields();
            CreateButtons();
        }

        private void CreateTextFields()
        {
            int posCounter = 0;
            foreach (var key in _textKeys)
            {
                string formattedKey = key == "atomic_mass" ? "atomicMass" : key.Replace('_', ' ');

                var labelObj = new GameObject(formattedKey + "Label");
                var textObj = new GameObject(formattedKey + "Value");

                // Position both fields vertically
                labelObj.transform.SetParent(_canvas.transform);
                TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
                labelText.text = formattedKey + ":";
                labelText.alignment = TextAlignmentOptions.MidRight;
                labelText.fontSize = 16;
                labelText.fontStyle = FontStyles.Bold;
                labelObj.transform.localPosition = new Vector3(-200, posCounter * -30, 0);

                // Value field
                textObj.transform.SetParent(_canvas.transform);
                TextMeshProUGUI valueText = textObj.AddComponent<TextMeshProUGUI>();
                valueText.fontSize = 16;
                textObj.transform.localPosition = new Vector3(200, posCounter * -30, 0);

                posCounter++;
            }
        }

        private GameObject CreateUIButton(Transform parent, string name, string text, Vector2 anchoredPosition, Color backgroundColor)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent);
            
            Button button = buttonObj.AddComponent<Button>();
            TextMeshProUGUI textLabel = buttonObj.AddComponent<TextMeshProUGUI>();

            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = backgroundColor;

            textLabel.text = text;
            textLabel.color = Color.black;
            textLabel.textAlign = TextAlignmentOptions.Center;
            textLabel.fontSize = 16;

            RectTransform buttonRT = buttonObj.GetComponent<RectTransform>();
            buttonRT.sizeDelta = new Vector2(200f, 50f);
            buttonRT.anchoredPosition = anchoredPosition;
            
            return buttonObj;
        }

        private void CreateButtons()
        {
            GameObject buttonPanel = new GameObject("ButtonPanel");
            buttonPanel.transform.SetParent(_canvas.transform);

            // Enter room button
            GameObject btnEnterRoom = CreateUIButton(buttonPanel.transform, "EnterRoomButton", "Raum betreten", new Vector2(-150, -330), new Color(0.5f, 0.8f, 0.5f));
            Button btnEnterRoomButton = btnEnterRoom.GetComponent<Button>();
            
            // Go to room functionality
            int selectedElementNumber = _currentElement.number;
            btnEnterRoomButton.onClick.AddListener(() => { LearningWorldsApp.Go(World.ElementRoom, selectedElementNumber); });
            
            // Close button
            GameObject btnClose = CreateUIButton(buttonPanel.transform, "CloseButton", "Schliessen", new Vector2(150, -330), new Color(0.8f, 0.5f, 0.5f));
            Button btnCloseButton = btnClose.GetComponent<Button>();
            btnCloseButton.onClick.AddListener(
                () => {
                    GameObject.Destroy(_canvas.gameObject);
                });
        }

        private void UpdatePanel(Element e)
        {
            _currentElement = e;
            UpdateCanvasTexts();
        }

        private void UpdateCanvasTexts()
        {
            TextMeshProUGUI[] textFields = _canvas.GetComponentsInChildren<TextMeshProUGUI>().Where(tf => tf.name.StartsWith("Value")).ToArray();
            if (textFields.Length != _textKeys.Length) return;

            foreach (TextMeshProUGUI txt in textFields)
            {
                string fieldName = txt.name.Replace("Value", "");
                FieldInfo property = _currentElement.GetType().GetField(fieldName == "atomicMass"? "atomic_mass" : fieldName.Replace(' ', '_'), BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    object value = property.GetValue(_currentElement);
                    string text = value?.ToString() ?? "N/A";
                    
                    txt.text = Truncate(text);
                }
            }
        }
        
        public void HidePanel()
        {
            if (_canvas) GameObject.Destroy(_canvas.gameObject);
        }
        
        private string Truncate(string input, int maxLength = 60)
        {
            if (string.IsNullOrEmpty(input)) { return "N/A"; }
            return input.Length > maxLength ? input.Substring(0, maxLength) + "..." : input;
        }
    }
}