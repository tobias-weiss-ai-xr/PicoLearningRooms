using UnityEngine;
using UnityEngine.UI;
using LearningWorlds.Pse;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Displays detailed information about a chemical element in a world-space UI panel.
    /// </summary>
    public class ElementInfoPanel : MonoBehaviour
    {
        private ElementData currentElement;
        private Canvas uiCanvas;
        private GameObject panelRoot;
        private LearningWorldsApp appReference;

        private const float MaxRaycastDistance = 20f;

        private void Awake()
        {
            appReference = LearningWorldsApp.Instance;
        }

        /// <summary>
        /// Shows the info panel for a specific element.
        /// </summary>
        public void Show(ElementData element)
        {
            if (currentElement?.id == element.id)
                return;

            currentElement = element;
            CreateAndPositionPanel();
            UpdatePanelContent();
        }

        /// <summary>
        /// Hides the info panel.
        /// </summary>
        public void Hide()
        {
            if (panelRoot != null)
            {
                Destroy(panelRoot);
                panelRoot = null;
            }
        }

        /// <summary>
        /// Detects element selection via raycast.
        /// </summary>
        public void DetectSelection()
        {
            if (appReference?.rigCamera == null)
                return;

            Ray ray = appReference.rigCamera.ScreenPointToRay(
                new Vector2(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, MaxRaycastDistance))
            {
                string elementTag = hit.collider.transform.parent?.tag;

                if (!string.IsNullOrEmpty(elementTag))
                {
                    // Try to find matching element data
                    ElementData element = ElementData.LoadAll().Find(e => e.id == elementTag);
                    if (element != null)
                    {
                        Show(element);
                    }
                }
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// Creates and positions the UI panel.
        /// </summary>
        private void CreateAndPositionPanel()
        {
            Hide(); // Hide existing panel if any

            // Create root object with canvas
            panelRoot = new GameObject("ElementInfoPanel_Root");
            uiCanvas = panelRoot.AddComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.WorldSpace;

            if (appReference?.rigCamera != null)
            {
                uiCanvas.worldCamera = appReference.rigCamera;
            }

            // Create the main panel
            GameObject panel = CreatePanel();
            panel.transform.SetParent(panelRoot.transform);

            // Position in front of camera
            if (appReference?.rigCamera != null)
            {
                panel.transform.position = appReference.rigCamera.transform.position +
                    appReference.rigCamera.transform.forward * 3.5f;
            }
        }

        /// <summary>
        /// Creates the main panel GameObject.
        /// </summary>
        private GameObject CreatePanel()
        {
            GameObject panelObj = new GameObject("ElementPanel");

            RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 500);

            // Add background image
            Image background = panelObj.AddComponent<Image>();
            background.color = new Color(0.15f, 0.15f, 0.15f, 0.95f);

            // Create content
            CreateTitle(panelObj);
            CreateInfoText(panelObj);
            CreateActionButtons(panelObj);

            return panelObj;
        }

        /// <summary>
        /// Creates the title text.
        /// </summary>
        private void CreateTitle(GameObject parent)
        {
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(parent.transform);

            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = currentElement.nameEn ?? currentElement.id;
            titleText.fontSize = 24;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.sizeDelta = new Vector2(0, 60);
            titleRect.anchoredPosition = new Vector2(0, -10);
        }

        /// <summary>
        /// Creates the information text labels.
        /// </summary>
        private void CreateInfoText(GameObject parent)
        {
            float yPos = -40f;

            AddTextRow(parent, "Symbol", currentElement.id, ref yPos);
            AddTextRow(parent, "Name", currentElement.nameEn ?? "N/A", ref yPos);
            AddTextRow(parent, "Category", GetCategoryDescription(currentElement), ref yPos);
            
            if (currentElement.properties != null)
            {
                AddTextRow(parent, "Melting Point", 
                    currentElement.properties.meltingPoint + " K", ref yPos);
            }
            
            // Add description
            if (!string.IsNullOrEmpty(currentElement.descriptionEn))
            {
                AddDescriptionText(parent, currentElement.descriptionEn, yPos - 30f);
            }
        }

        /// <summary>
        /// Adds a row with label and value.
        /// </summary>
        private void AddTextRow(GameObject parent, string label, string value, ref float yPos)
        {
            GameObject rowObj = new GameObject("Row_" + label);
            rowObj.transform.SetParent(parent.transform);

            // Label
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(rowObj.transform);
            Text labelText = labelObj.AddComponent<Text>();
            labelText.text = label + ":";
            labelText.fontSize = 16;
            labelText.color = Color.cyan;
            labelText.alignment = TextAnchor.MiddleRight;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0.5f);
            labelRect.anchorMax = new Vector2(0, 0.5f);
            labelRect.sizeDelta = new Vector2(150, 25);
            labelRect.anchoredPosition = new Vector2(0, 0);

            // Value
            GameObject valueObj = new GameObject("Value");
            valueObj.transform.SetParent(rowObj.transform);
            Text valueText = valueObj.AddComponent<Text>();
            valueText.text = value;
            valueText.fontSize = 16;
            valueText.color = Color.white;
            valueText.alignment = TextAnchor.MiddleLeft;
            valueText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            RectTransform valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0, 0.5f);
            valueRect.anchorMax = new Vector2(0, 0.5f);
            valueRect.sizeDelta = new Vector2(240, 25);
            valueRect.anchoredPosition = new Vector2(160, 0);

            // Row positioning
            RectTransform rowRect = rowObj.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.5f, 1);
            rowRect.anchorMax = new Vector2(0.5f, 1);
            rowRect.sizeDelta = new Vector2(380, 25);
            rowRect.anchoredPosition = new Vector2(0, yPos);

            yPos -= 30f;
        }

        /// <summary>
        /// Adds a longer description text.
        /// </summary>
        private void AddDescriptionText(GameObject parent, string text, float yPos)
        {
            GameObject descObj = new GameObject("Description");
            descObj.transform.SetParent(parent.transform);

            Text descText = descObj.AddComponent<Text>();
            descText.text = text;
            descText.fontSize = 14;
            descText.color = Color.gray;
            descText.alignment = TextAnchor.UpperLeft;
            descText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            RectTransform descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.05f, 1);
            descRect.anchorMax = new Vector2(0.95f, 1);
            descRect.sizeDelta = new Vector2(0, 100);
            descRect.anchoredPosition = new Vector2(0, yPos);
        }

        /// <summary>
        /// Creates action buttons.
        /// </summary>
        private void CreateActionButtons(GameObject parent)
        {
            // Enter Element Room button
            CreateButton(parent, "Enter Room", new Vector2(0, -200), new Vector2(180, 40),
                Color.green, () => { 
                    ElementRoom.SelectedElement = currentElement;
                    LearningWorldsApp.Go(World.ElementRoom); 
                });

            // Close button
            CreateButton(parent, "Close", new Vector2(0, -250), new Vector2(180, 40),
                Color.red, Hide);
        }

        /// <summary>
        /// Creates a button.
        /// </summary>
        private void CreateButton(GameObject parent, string text, Vector2 position, 
            Vector2 size, Color color, UnityEngine.Events.UnityAction onClick)
        {
            GameObject buttonObj = new GameObject("Button_" + text);
            buttonObj.transform.SetParent(parent.transform);

            Button button = buttonObj.AddComponent<Button>();
            Image image = buttonObj.AddComponent<Image>();

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);
            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = text;
            textComponent.fontSize = 16;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = new Vector2(-20, -10);

            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 1);
            buttonRect.anchorMax = new Vector2(0.5f, 1);
            buttonRect.sizeDelta = size;
            buttonRect.anchoredPosition = position;

            image.color = color;
            button.onClick.AddListener(onClick);
        }

        /// <summary>
        /// Updates panel content with current element data.
        /// </summary>
        private void UpdatePanelContent()
        {
            // Implementation not needed as we recreate the panel
        }

        /// <summary>
        /// Gets a human-readable category description.
        /// </summary>
        private string GetCategoryDescription(ElementData element)
        {
            if (element.properties != null && !string.IsNullOrEmpty(element.descriptionEn))
            {
                return "Element"; // Simplified for now
            }
            return "Chemical Element";
        }
    }
}
