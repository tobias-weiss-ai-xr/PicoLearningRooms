using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ElementInfoPanel : MonoBehaviour
{
    private Element _currentElement;
    private Canvas _canvas;
    
    private static string[] _textKeys = new string[] {"name", "symbol", "number", "mass", "category", "phase", "meltingTemp", "boilTemp", "density", "electronegativity", "appearance", "electronConfig", "summary"};

    // Creates a WorldSpace canvas and loads renderer content for this panel.
    public void Initialize()
    {
        CreateUIPanel();
    }
    
    private void CreateUIPanel()
    {
        GameObject canvasObj = new GameObject("ElementUIRealm");
        _canvas = canvasObj.AddComponent<Canvas>();
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        // Define WorldSpace canvas
        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.worldCamera = LearningWorldsApp.Instance.rigCamera;
        
        UnityEngine.UI.GraphicRaycaster rayCaster = canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Set canvas dimensions
        RectTransform canvasRT = canvasObj.GetComponent<RectTransform>();
        canvasRT.sizeDelta = new Vector2(550, 750);
        canvasRT.pivot = new Vector2(0.5f, 1.0f);

        SceneBuilder.AddWorldSpaceUI(canvasObj);
        CreateTextFields();
    }
    
    private void CreateTextFields()
    {
        int posCounter = 0;
        
        foreach (var key in _textKeys) {
            string formattedKey = key.Replace('_', ' ');
            
            var labelObj = new GameObject(formattedKey + "Label");
            var textObj = new GameObject(formattedKey + "Value");

            // Setup label
            labelObj.transform.SetParent(_canvas.transform);
            TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
            labelText.text = formattedKey + ":";
            labelText.fontSize = 16;
            labelText.fontStyle = FontStyles.Bold;
            labelObj.transform.localPosition = new Vector3(0, posCounter * -30, 0);

            // Setup value field
            textObj.transform.SetParent(_canvas.transform);
            TextMeshProUGUI valueText = textObj.AddComponent<TextMeshProUGUI>();
            valueText.fontSize = 16;
            textObj.transform.localPosition = new Vector3(0, posCounter * -30 - 20, 0);

            posCounter++;
        } 
        CreateButtons();
    }

    private void CreateButtons()
    {
        // Create a button container at the bottom of the canvas
        GameObject buttonPanel = new GameObject("ButtonPanel"){transform = {parent = _canvas.transform}};

        // "Raum betreten" button
        GameObject enterRoomButtonObj = new GameObject("EnterRoomButtonCreated");
        enterRoomButtonObj.transform.SetParent(buttonPanel.transform);
        Button enterRoomButton = enterRoomButtonObj.AddComponent<Button>();

        TextMeshProUGUI enterRoomText = enterRoomButtonObj.AddComponent<TextMeshProUGUI>();
        enterRoomText.text = "Raum betreten";
        enterRoomText.color = Color.black;
        enterRoomText.textAlign = TextAlignmentOptions.Center;
        enterRoomText.fontSize = 16;

        Image buttonImage = enterRoomButtonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.5f, 0.75f, 0.5f);
        
        enterRoomButton.onClick.AddListener(() => {
            if (_currentElement != null
            )
            {
                LearningWorldsApp.Go(World.ElementRoom, _currentElement.number);
            }
        });

        // Position and size entered room button
        RectTransform enterButtonRT = enterRoomButtonObj.GetComponent<RectTransform>();
        enterButtonRT.sizeDelta = new Vector2(200f, 60f);
        enterButtonRT.anchoredPosition = new Vector2(-150, -300);

        // "Schliessen" button
        GameObject closeButtonObj = new GameObject("CloseButtonCreated");
        closeButtonObj.transform.SetParent(buttonPanel.transform);

        Button closeButton = closeButtonObj.AddComponent<Button>();

        TextMeshProUGUI closeText = closeButtonObj.AddComponent<TextMeshProUGUI>();
        closeText.text = "Schliessen";
        closeText.color = Color.black;
        closeText.fontSize = 16;
        closeText.textAlign = TextAlignmentOptions.Center;

        Image closeButtonImage = closeButtonObj.AddComponent<Image>();
        closeButtonImage.color = new Color(0.75f, 0.5f, 0.5f);

        closeButton.onClick.AddListener(() => {
            GameObject.Destroy(_canvas.gameObject);
        });

        // Position and size close button
        RectTransform closeButtonRT = closeButtonObj.GetComponent<RectTransform>();
        closeButtonRT.sizeDelta = new Vector2(200f, 60f);
        closeButtonRT.anchoredPosition = new Vector2(150, -300);
    }
    
    // Helper method
    public void UpdatePanel(Element e) {
        _currentElement = e;
        TextMeshProUGUI[] allTexts = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        
        var textFields = new Dictionary<string, TextMeshProUGUI>();
        foreach(color = Color.red;
        btnEnterRoom.onClick.AddListener(() => { LearningWorldsApp.Go(World.ElementRoom, selectedElementNumber); });
        SceneBuilder.AddWorldSpaceButton(btnEnterRoom);
        btnEnterRoom.transform.parent = canvasObj.transform;
        RectTransform btnEnterRoomRT = btnEnterRoom.GetComponent<RectTransform>();
        btnEnterRoomRT.anchoredPosition = new Vector2(-150f, -100f);
        
        GameObject buttonCloser = new GameObject("buttonClose");
        buttonCloser.transform.parent = canvasObj.transform;
        Button btnClose = buttonCloser.AddComponent<Button>();
        TextMeshProUGUI textCloser = buttonCloser.AddComponent<TextMeshProUGUI>();

        textCloser.text = "Schliessen";
        textCloser.fontSize = 20;
        textCloser.alignTextPrefab = TextAlignmentOptions.Center;
        textCloser.color = Color.red;
        btnClose.onClick.AddListener(() => { GameObject.Destroy(canvasObj); });
        btnClose.targetGraphic = textCloser.GetComponent<Image>();
        RectTransform closePanelRT = buttonCloser.GetComponent<RectTransform>();
        closePanelRT.anchoredPosition = new Vector2(150f, -100f);

        btnEnterRoomRT.sizeDelta = new Vector2(250f, 50f);
        closePanelRT.sizeDelta = new Vector2(200f, 50f);

        _canvas = canvasObj.GetComponent<Canvas>();
        ElementInfoPanel elementInfoPanel = _canvas.GetComponent<ElementInfoPanel>() ?? _canvas.gameObject.AddComponent<ElementInfoPanel>();
        elementInfoPanel.Initialize(selectedElement);
    }

    private string         _elementSummary = string.Empty;

        public string TruncateSummary(string input, int maxLength = 60)
        {
            if (string.IsNullOrEmpty(input)) return "N/A";
            return input.Length > maxLength ? input.Substring(0, maxLength) + "..." : input;
        }

        private void UpdateCanvasTexts()
    {
        TextMeshProUGUI[] textObjects = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var field in _textKeys)
        {
            var target = GameObject.Find(field + "Value");

            if (target == null || !target.TryGetComponent(out TextMeshProUGUI valueField)) continue;

            FieldInfo property = _currentElement.GetType().GetField(field.Replace('_', ''), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (property == null) continue;
            
            object value = property.GetValue(_currentElement);
            if (value == null || value.ToString() == "") valueField.text = "-";
            else valueField.text = value.ToString();
        }
    }
    
    public void HidePanel()
    {
        if (_canvas != null) GameObject.Destroy(_canvas.gameObject);
    }
}