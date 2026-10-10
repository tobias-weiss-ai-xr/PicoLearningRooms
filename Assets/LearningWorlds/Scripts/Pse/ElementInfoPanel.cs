using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LearningWorlds.PSE
{
    public class ElementInfoPanel : MonoBehaviour
    {
        private Element _currentElement;
        private Canvas _canvas;
        private GameObject infoPanelRoot;
        public const float MAX_RAYCAST_DISTANCE = 20f;
        private LearningWorldsApp _app;
        
        private void Awake()
        {
            _app = LearningWorldsApp.Instance;
        }
        
        
private bool _canClosePanel = true;

public void ShowPanel(Element element)
        {
            if (_currentElement == element) return;

            _currentElement = element;
            CreateAndPositionUIPanelAtSelectionPoint();
            UpdatePanelUI();
        }
        
        public void ClosePanel()
        {
            if (infoPanelRoot) GameObject.Destroy(infoPanelRoot);
        }
        
        // Raycast detection for element click
        // Update for correct raycast trigger detection
private Collider _triggerCollider;

public void DetectSelection()
{
    Ray ray = _app.rigCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
    RaycastHit hit;
    _triggerCollider = null;

    if (Physics.Raycast(ray, out hit, MAX_RAYCAST_DISTANCE))
    {
        Collider collider = hit.collider;
        _triggerCollider = collider;
        string elementTag = collider.transform.parent?.tag;

        if (int.TryParse(elementTag, out int elementNumber))
        {
            if (_currentElement?.number != elementNumber)
            {
                Element selectedElement = DataStore.ElementCatalog.FirstOrDefault(x => x.number == elementNumber);
                if (selectedElement != null)
                {
                    ShowPanel(selectedElement);
                }
            }
        }
    }
    else
    {
        if (infoPanelRoot && _canClosePanel)
        {
            ClosePanel();
        }
    }
}

        private void CreateAndPositionUIPanelAtSelectionPoint()
        {
            if (infoPanelRoot)
                GameObject.Destroy(infoPanelRoot);
                
            infoPanelRoot = new GameObject("ElementInfoRoot");
            Canvas canvas = infoPanelRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _app.rigCamera;
            _canvas = canvas;

            // Create panels to hold all UI elements
            GameObject panel = CreateElementPanel();
            panel.transform.SetParent(infoPanelRoot.transform);
            
            // Set initial position in front of last selected element
            panel.transform.position = _app.rigCamera.transform.position + _app.rigCamera.transform.forward * 3.5f;
            
            SceneBuilder.AddWorldSpaceUI(infoPanelRoot);
        }

        private TextMeshProUGUI CreateUILabel(string text, RectTransform parent, params Vector2 position)
        {
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(parent);

            TextMeshProUGUI label = labelObj.AddComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = 16;
            label.fontStyle = FontStyles.Bold;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Left;
            label.autoSizeTextContainer = true;

            RectTransform rt = labelObj.GetComponent<RectTransform>();
            rt.anchoredPosition = position;

            return label;
        }
        
        private GameObject CreateElementPanel()
        {
            GameObject panelObj = new GameObject("ElementInfoPanel");
            panelObj.transform.SetParent(_canvas.transform);

            RectTransform panelRT = panelObj.AddComponent<RectTransform>();
            panelRT.sizeDelta = new Vector2(700, 800);
            panelRT.localRotation = Quaternion.Euler(0, 180, 0); // Align forward
            
            CreateInfoTexts(panelRT);
            CreateActionButtons(panelRT);

            return panelObj;
        }
        
        private void CreateInfoTexts(RectTransform parentRT)
        {
            int positionY = 0;

            var infoEntries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Name", _currentElement.name),
                new KeyValuePair<string, string>("Symbol", _currentElement.symbol),
                new KeyValuePair<string, string>("Number", _currentElement.number.ToString()),
                new KeyValuePair<string, string>("Atomic Mass", RegexMatchForMultiChar(_currentElement.atomic_mass,"^([0-9.]+)")),
                new KeyValuePair<string, string>("Category", GetCategory()),
                new KeyValuePair<string, string>("Phase", _currentElement.phase),
                new KeyValuePair<string, string>("Melting Point [K]", RegexMatchForMultiChar(_currentElement.meltingTemp,"^([0-9.]+)")),
                new KeyValuePair<string, string>("Boiling Point [K]", RegexMatchForMultiChar(_currentElement.boilingTemp,"^([0-9.]+)")),
                new KeyValuePair<string, string>("Density [g/l]", RegexMatchForMultiChar(_currentElement.density,"^([0-9.]+)")),
                new KeyValuePair<string, string>("Electronegativity", RegexMatchForMultiChar(_currentElement.electronegativity,"^([0-9.]+)")),
                new KeyValuePair<string, string>("Appearance", _currentElement.appearance),
                new KeyValuePair<string, string>("Configuration", _currentElement.electronConfig),
                new KeyValuePair<string, string>("Summary", Truncate(_currentElement.summary, 60))
            };
                
            // Format labels and values with proper alignment
            foreach (var entry in infoEntries)
            {
                CreateUILabel(entry.Key + ":", parentRT, new Vector2(-300, positionY));
                CreateUILabel(entry.Value, parentRT, new Vector2(-5, positionY));
                positionY -= 40;
            }
        }
        
        private void CreateActionButtons(RectTransform parentRT)
        {
            // "Enter Element Room" button
            GameObject enterButton = CreateUIButton("EnterElementRoom", "Raum betreten", new Vector2(0, -150),
                220, () => LearningWorldsApp.Go(World.ElementRoom, _currentElement.number), new Color(0.31f, 0.65f, 0.3f));
            enterButton.transform.SetParent(parentRT);
            
            // "Close Panel" button
            GameObject closeButton = CreateUIButton("ClosePanel", "Schliessen", new Vector2(0, -250),
                220, () => ClosePanel(), new Color(0.83f, 0.24f, 0.24f));
            closeButton.transform.SetParent(parentRT);
        }

        private GameObject CreateUIButton(string name, string text, Vector2 position, float width,
            UnityEngine.Events.UnityAction onClick, Color color)
        {
            GameObject buttonObj = new GameObject(name);
            Button button = buttonObj.AddComponent<Button>();
            Image image = buttonObj.AddComponent<Image>();
            TextMeshProUGUI textLabel = buttonObj.AddComponent<TextMeshProUGUI>();

            RectTransform rt = buttonObj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(width, 50);
            rt.anchoredPosition = position;

            textLabel.text = text;
            textLabel.fontSize = 18;
            textLabel.color = Color.black;
            textLabel.alignment = TextAlignmentOptions.Center;

            image.color = color;
            image.raycastTarget = true;

            button.onClick.AddListener(onClick);

            return buttonObj;
        }
        
        // Update code to refactor access to element data
        private void UpdatePanelUI()
        {
            // Clean up any existing UI
            foreach (Transform child in infoPanelRoot?.transform)
            {
                if (child != null && child.name != "ElementInfoPanel" && child.name != "Label")
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            
            // Rebuild UI with updated element info
            CreateInfoTexts(infoPanelRoot.GetComponentInChildren<RectTransform>(true));
            CreateActionButtons(infoPanelRoot.GetComponentInChildren<RectTransform>(true));
        }

        private string GetCategory()
        {
            if (DataStore.ElementCategories.TryGetValue(_currentElement.category, out string category)) 
                return category;
            return _currentElement.category;
        }

        private string RegexMatchForMultiChar(string field, string pattern)
        {
            var match = System.Text.RegularExpressions.Regex.Match(field, pattern);
            return match.Success ? match.Groups[1].Value : "N/A";
        }
        
        // Text truncation for UI
        private string Truncate(string input, int maxLength = 60)
        {
            if (string.IsNullOrEmpty(input)) return "N/A";
            return input.Length <= maxLength ? input : input.Substring(0, maxLength) + "...";
        }
    }
}