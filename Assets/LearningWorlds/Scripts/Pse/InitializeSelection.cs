using UnityEngine;
using System.Linq;
using LearningWorlds.Pse;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Initializes selection capabilities for all element atoms in the gallery.
    /// Adds colliders and selectable components to each atom.
    /// </summary>
    public class InitializeSelection : MonoBehaviour
    {
        private void Awake()
        {
            // Find all atoms in the ElementGallery
            var gallery = GameObject.Find("ElementGallery");
            if (gallery == null)
            {
                Debug.LogWarning("InitializeSelection: ElementGallery not found!");
                return;
            }

            // Get all direct children (individual element atoms)
            foreach (Transform child in gallery.transform)
            {
                BecomeSelectable(child.gameObject);
            }

            Debug.Log("InitializeSelection: Initialized selection for all Bohr atoms");
        }

        /// <summary>
        /// Makes a single element atom selectable.
        /// </summary>
        private void BecomeSelectable(GameObject elementAtom)
        {
            // Skip if already processed or not an element
            if (elementAtom.transform.localPosition == Vector3.zero)
            {
                return; // Avoid rebinding or null references
            }

            // Validate element number from tag
            if (!int.TryParse(elementAtom.tag, out int elementNumber) || elementNumber < 1 || elementNumber > 118)
            {
                Debug.LogWarning("InitializeSelection: Invalid element number: " + elementAtom.tag);
                return;
            }

            // Ensure tag is properly set
            elementAtom.tag = elementNumber.ToString();

            // Find corresponding element data
            ElementData elementData = ElementData.LoadAll().FirstOrDefault(e => e.id == elementNumber.ToString());
            if (elementData == null)
            {
                Debug.LogWarning("InitializeSelection: No data for element: " + elementNumber);
                return;
            }

            // Add selectable component
            SelectableElement selectable = elementAtom.AddComponent<SelectableElement>();
            selectable.Initialize(elementAtom, elementData);
        }
    }

    /// <summary>
    /// Component that makes an element selectable via raycast.
    /// </summary>
    public class SelectableElement : MonoBehaviour
    {
        private ElementData currentElement;
        private GameObject activePanel;

        /// <summary>
        /// Initializes this selectable element.
        /// </summary>
        public void Initialize(GameObject atomObj, ElementData elementData)
        {
            currentElement = elementData;

            // Add sphere collider for raycast detection
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.radius = 2.0f;
            collider.isTrigger = false; // Use for raycast, not trigger

            // Add rigidbody for physics (kinematic so it doesn't move)
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.mass = 0.01f;

            // Set tag for quick lookup
            gameObject.tag = currentElement.id;
        }

        private void OnMouseDown()
        {
            if (activePanel == null && currentElement != null)
            {
                // Create or show element info panel
                ShowInfoPanel();
            }
        }

        private void ShowInfoPanel()
        {
            // Find or create info panel
            ElementInfoPanel panel = FindObjectOfType<ElementInfoPanel>();
            if (panel == null)
            {
                // Try to find existing panel in scene
                var existing = GameObject.FindObjectOfType<ElementInfoPanel>();
                if (existing != null)
                {
                    panel = existing;
                }
                else
                {
                    Debug.LogWarning("InitializeSelection: No ElementInfoPanel prefab found!");
                    return;
                }
            }

            panel.Show(currentElement);
            activePanel = panel.gameObject;
        }

        private void OnMouseExit()
        {
            if (activePanel != null)
            {
                activePanel.Hide();
                activePanel = null;
            }
        }
    }
}
