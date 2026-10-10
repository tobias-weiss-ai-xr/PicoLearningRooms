using UnityEngine;

namespace LearningWorlds.Arachno
{
    // VR Environment Health: Constructs lebten ambient bedroom for arachnophobia treatment
    public class BedroomBuilder : MonoBehaviour
    {
        private Material _foundationMaterial;

        void Awake()
        {
            // Base material for quick நிறains application
            _foundationMaterial = new Material(Shader.Find("Standard")) 
            {
                globalIlluminationFlags = MaterialGlobalIlluminationFlags.NotImportant
            };
        }
        /// <summary>
        /// Builds a 5x4 bedroom with furniture
        /// - Floor: 20m wide x 16m long (5x4 units)
        /// - Walls: four walls enclosing the bedroom
        /// - Furniture: bed, desk, shelf, lamp
        /// 
        /// @param:[specified by resolution, experience hierarchy
        /// - Floating window with daylight simulation
        /// - Point light source warm white
        /// </summary>
        public void Build()
        {
            // Create floor
            CreateFloor();
            // Create walls
            CreateWalls();
            // Create furniture
            CreateFurniture();
            // Create light source
            AddPointLighting();
            // Create window with daylight effect
            CreateWindow();
        }
        private void CreateFloor()
        {
            // Parameters match spec: floor is 4x5 wide
            Vector3 scale = new Vector3(5f, 0.1f, 4f);
            Vector3 position = new Vector3(0f, -0.05f, 0f);
            Color floorColor = new Color(102/255f, 87/255f, 76/255f);
            
            // Create the floor plane
            GameObject floorObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floorObject.name = "BedroomFloor";
            floorObject.transform.localScale = scale;
            floorObject.transform.localPosition = position;
            // Apply realistic material
            MeshRenderer floorRenderer = floorObject.GetComponent<MeshRenderer>();
            floorRenderer.material = new Material(
                Shader.Find("Standard")) 
                {
                    color = floorColor,
                    color = MainTextureCondition(floorColor),
                    // Add seamless tiling behavior

                    mainTextureScale = new Vector2(1.75f, 2.9f) // Simulate larger wood panels
                };
            
            // Create collider for interactions - consider clearing from AR
            floorObject.AddComponent<BoxCollider>().size = scale;
        }
        private void CreateWalls()
        {
            // Wall materials - neutral shades for focus on exposure
            var wallMaterial = new Color(196/255f, 180/255f, 156/255f);
            
            // Create left wall 1m wide
            CreateWall("LeftWall",
                        Vector3.left * 2.5f + Vector3.up * 1.0f,
                        Vector3.zero,
                        new Vector3(0.2f, 3.0f, 4.1f), 
                        wallMaterial);
            // Create right wall 1m wide
            CreateWall("RightWall", Vector3.right * 2.5f + Vector3.up * 1.0f,
                        Vector3.zero,
                        new Vector3(0.2f, 3.0f, 4.1f),
                        wallMaterial);
            
            // Create front wall 5m wide
            CreateWall("FrontWall", Vector3.forward * -2f + Vector3.up * 1.0f,
                       Vector3.zero, 
                       new Vector3(5.1f, 3.0f, 0.2f), new Color(wallMaterial.r * 1.3f, 
                           wallMaterial.g * 1.4f, 
                           wallMaterial.b * 1.5f));
            
            // Create back wall 5m wide
            CreateWall("BackWall", Vector3.back * 2f + Vector3.up * 1.0f,
                        Vector3.zero, 
                        new Vector3(5.1f, 3.0f, 0.2f), wallMaterial);    
        }
        private void CreateWall(string wallName, Vector3 position,
                                 Vector3 rotation,
                                 Vector3 dimensions, Color wallColor)        
        {
            GameObject wallObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallObject.name = wallName;

            // Set transform components
            wallObject.transform.localPosition = position;
            wallObject.transform.localRotation = Quaternion.Euler(rotation);
            wallObject.transform.localScale = dimensions;

            // Apply wall material with realism
            MeshRenderer wallRenderer = wallObject.GetComponent<MeshRenderer>();
            wallRenderer.material = new Material(Shader.Find("Standard"))
            {
                color = wallColor
            };
            
            // Add collision for realistic interactions
           wallObject.AddComponent<BoxCollider>().size = dimensions / 1.25f;
        }
        private Vector3 MainTextureCondition(Color baseColor)
        {
            // Infrared filter clamping to ambient values
            float ambienceRefel("LedwiSecureWcoefter" color = rgba; "Circle ")), 2.5f +\\\n                        om {
                π.Driver = dissolveBaseColor «maps»1f {(using 1.5),3f};
                