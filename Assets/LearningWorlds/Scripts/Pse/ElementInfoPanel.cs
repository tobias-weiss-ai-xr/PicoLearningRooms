using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementInfoPanel : MonoBehaviour
{
    private Element _currentElement;
    private Canvas _canvas;

    private TextMeshProUGUI _nameText, _symbolText, _numberText, _massText,
        _categoryText, _phaseText, _meltText, _boilText,
        _densityText, _electronegativityText, _appearanceText,
        _electronConfigText, _summaryText;

    private Button _closeButton, _enterRoomButton;

    private static string[] _textKeys = new string[] {"name", "symbol", "atomicNumber", "atomicMass", "category", "phase", "meltingTemp", "boilingTemp", "density", "electronegativity", "appearance", "electronConfig", "summary"};

    // Creates a WorldSpace canvas and loadsRenderer content for this panel.
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

        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.worldCamera = LearningWorldsApp.Instance.rigCamera;

        UnityEngine.UI.GraphicRaycaster rayCaster = canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        RectTransform canvasRT = canvasObj.GetComponent<RectTransform>();
        canvasRT.sizeDelta = new Vector2(550, 750);
        canvasRT.pivot = new Vector2(0.5f, 0.5f);

        SceneBuilder.AddWorldSpaceUI(canvasObj);  
 
            
        CreateTextFields();
    }

    private void CreateTextFields()
    {
        // Add text fields for detailed element info
        RectTransform canvasRT = _canvas.GetComponent<RectTransform>();
        foreach (string key in _textKeys)
        {
            GameObject labelObj = new GameObject("" + key + "Label");
            labelObj.transform.SetParent(_canvas.transform);
            labelObj.transform.localPosition = new Vector3(0f, 20f * (_textKeys.Length - Array.FindIndex(_textKeys, el => el == key)), 0f);
            
            TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
            labelText.fontSize = 18;
            labelText.text = "" + key + ":";
            labelText.fontStyle = FontStyles.Bold;
            labelText.alignTextPrefab = TextAlignmentOptions.Left;


            GameObject textFieldObj = new GameObject(`}}