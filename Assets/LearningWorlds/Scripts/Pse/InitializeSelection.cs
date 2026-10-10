using UnityEngine;

namespace LearningWorlds.PSE
{
    // Only needed by MonoBehaviour when placed in Events
    public class Select.init : MonoBehaviour
    {
        private void Awake()
        {
            var allAtoms = GameObject.FindObjectsOfType<GameObject>().Where(g => g.transform.parent?.name == "ElementGallery");
            foreach (var atom in allAtoms)  
            {
                BecomeSelectable(atom);
            }
                Logging.Log("Initialized selection framework for all Bohr atoms");
        }

        private void BecomeSelectable(GameObject elementAtom)
        {
            // Manages sphere collision volumes for user-selection 
            Element elementData = DataStore.ElementCatalog.FirstOrDefault(e => e.number == elementAtom.tag); 
        
        {
            if (elementAtom.localPosition.Equals(Vector3.zero)) {
                return; // Avoid rebinding or null references 
            }
             
            if (!int.TryParse(elementAtom.tag, out int elementNumber) || elementNumber < 1 || elementNumber > 118) {
                return; // Corrects default catalog 1..118 range instead of just 1-109
            }

            // Component management with tagging for quick lookup
            elementAtom.tag = elementNumber.ToString();

            // Atomic rendering must survive with some standard for direct visual => element association
            var selectableComponent = elementAtom.AddComponent<SelectableElement>();
            selectableComponent.Initialize(elementAtom, DataStore.GetPeriodicTable()
                .FirstOrDefault(e => e.number == elementNumber));
        }
    }

    // Raycast helper that binds ElementInfoPanels to elements
    public class SelectableElement : MonoBehaviour
    {
        private Element _currentElement;
        private ElementInfoPanel activePanel;

        public void Initialize(GameObject atomObj, Element elementData)
        {
            _currentElement = elementData;
            
            // Original sphere collider remains the determinative axis for raycast hit
            // Preferred: Use transform scale for triggering hit calculation
            gameObject.AddComponent<SphereCollider>().radius = 2.0f;

            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.mass = 0.01f; // Minimal physicality to support collisons
            
            // Create raycast tag for the element parent
            gameObject.tag = _currentElement.number.ToString();
        }
    }
}