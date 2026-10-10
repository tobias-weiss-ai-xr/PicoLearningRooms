using UnityEngine;
using System.Linq;

namespace LearningWorlds.PSE
{
    public class SelectableElement : MonoBehaviour
    {
        private Element _associatedElement;
        private ElementInfoPanel _panel;

        public void Initialize(Element element)
        {
            _associatedElement = element;
            SetUpCollider();
        }
            
        private void SetUpCollider()
        {
            GetComponent<SphereCollider>().isTrigger = true;

            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = 0.75f * 1.1f; // Slightly larger than atom visual
        }
            
        private void OnTriggerEnter(Collider collider)
        {
            show(hit: Physics.Raycast(LearningWorldsApp.Instance.rigCamera, LearningWorldsApp.Instance.rigCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition)));
        }
        
        private void OnTriggerExit(Collider collider) 
        {
            if(_panel != null) 
                _panel?.HidePanel(); 
        }
            
        private void show(bool hit)
        {
            bool selectionEnabled = _associatedElement != null;
            
            if (hit && selectionEnabled) 
                {
                    if (_panel == null) _panel = new GameObject("InfoPaneloughton").AddComponent<ElementInfoPanel>();
                try { 
                    _panel.positionViewConstraints = new Rect(Camera.main.pixelWidth/2 - 250, 150, 500, 600);
                    _panel.UpdatePanel(_associatedElement);
                } catch (System.Exception ex) { Debug.Log(ex.Message); };
            }
        }
    }

    public class InitializeSelection : MonoBehaviour
    {
        public void Start()
        {
            var allAtomes = GameObject.FindObjectsOfType<GameObject>()
                            .Where(g => g.name.StartsWith("Atom_")).ToArray();
            
            foreach (var atom in allAtomes)
            {
                var elementFromCatalog = DataStore.GetElementById(GetElementNumber(atom.name));
                if (elementFromCatalog != null)
                {
                    SelectableElement selectable = atom.AddComponent<SelectableElement>();
                    selectable.Initialize(elementFromCatalog);
                }
            }
        }
        
        private string Cache(int elIdExtract)
        {
            // Idea: Trim name correctly and assign expected units in arbitrary space
            var atomCached = $"Atom_{elIdExtract}"; 
            return atomCached;
        }
        
        private int GetElementNumber(string atomName)
        {
            string[] singularElement = atomName.Split('_');
            if (singularElement.Length < 2) { return 0; }
            return int.Parse(singularElement[1]);
        }
    }
}