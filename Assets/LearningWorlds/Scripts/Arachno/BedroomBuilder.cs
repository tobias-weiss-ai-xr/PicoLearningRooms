using UnityEngine;

namespace LearningWorlds.Arachno
{
    /// <summary>
    /// Builds a bedroom environment for the arachnophobia exposure therapy.
    /// Features: 5m x 4m floor plate, walls, furniture, a window, and warm lighting.
    /// </summary>
    public class BedroomBuilder : MonoBehaviour
    {
        public void Build()
        {
            // Create floor (5 unit x 4 unit)
            CreateFloor();
            // Create walls
            CreateWalls();
            // Create furniture
            CreateFurniture();
            // Add lighting
            AddLightingAndWindow();
        }
        private void CreateFloor()
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "BedroomFloor";
            floor.transform.localScale = new Vector3(5f, 0.1f, 4f);
            floor.transform.localPosition = new Vector3(0, -0.05f, 0);
            
            // Dark wood material
            Material floorMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.4f, 0.34f, 0.3f), // Dark wood
                mainTextureScale = new Vector2(5f, 4f),
                enableInstancing = true
            };
            floor.GetComponent<Renderer>().material = floorMaterial;
            
            // Add collider
            floor.AddComponent<BoxCollider>().size = new Vector3(5f, 0.1f, 4f);
        }
        private void CreateWalls()
        {
            float wallHeight = 2.8f;
            
            // Create four walls
            CreateWall(-2.5f, 0f, wallHeight, "LeftWall");
            CreateWall( 2.5f, 0f, wallHeight, "RightWall");
            CreateWall(0f,  -1.9f, wallHeight, "FrontWall");
            CreateWall(0f,  2.1f, wallHeight, "BackWall");
        }
        private void CreateWall(float x, float z, float height, string name)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.localPosition = new Vector3(x, height / 2f, z);
            wall.transform.localScale = FindWallScale(name, height);
            
            // Light beige material
            Material wallMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.77f, 0.7f, 0.65f), // Light beige
                enableInstancing = true
            };
            wall.GetComponent<Renderer>().material = wallMaterial;
            
            // Add collider
            wall.AddComponent<BoxCollider>().size = FindWallScale(name, height);
        }
        private Vector3 FindWallScale(string wallName, float height)
        {
            switch (wallName)
            {
                case "LeftWall": return new Vector3(0.25f, height, 4f);
                case "RightWall": return new Vector3(0.25f, height, 4f);
                case "FrontWall": return new Vector3(5f, height, 0.25f);
                case "BackWall": return new Vector3(5f, height, 0.25f);
                default: return Vector3.one;
            }
        }
        private void CreateFurniture()
        {
            // Bed
            CreateBed();
            // Desk
            CreateDesk();
            // Shelf
            CreateShelf();
            // Lamp
            CreateLamp();
        }
        private void CreateBed()
        {
            GameObject bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bed.name = "Bed";
            bed.transform.localPosition = new Vector3(0, 0.6f, -1.2f);
            bed.transform.localScale = new Vector3(1.6f, 0.5f, 1.8f);
            
            // Dark wood bed material
            Material bedMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.2f, 0.1f, 0.05f), // Dark bed frame
                enableInstancing = true
            };
            bed.GetComponent<Renderer>().material = bedMaterial;
            
            // Bedsheets (a planar cube at bed top)
            GameObject sheets = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sheets.name = "Bedsheets";
            sheets.transform.parent = bed.transform;
            sheets.transform.localPosition = new Vector3(0f, 0.4f, -0.7f);
            sheets.transform.localScale = new Vector3(1.6f, 0.05f, 1.8f);
            
            Material sheetsMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.3f, 0.4f, 0.5f), // Light blue sheets
                enableInstancing = true
            };
            sheets.GetComponent<Renderer>().material = sheetsMaterial;
            
            // Mattress (pillow shape)
            Transform mattress = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            mattress.localScale = new Vector3(1f, 0.3f, 1.3f);
            mattress.parent = sheets.transform;
            mattress.localPosition = new Vector3(0f, -0.1f, -0.6f);
            mattress.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) {
                color = new Color(0.6f, 0.4f, 0.2f)
            };
            
            // Remove unnecessary components
            DestroyImmediate(mattress.GetComponent<Rigidbody>());
            DestroyImmediate(sheets.GetComponent<Rigidbody>());
        }
        private void CreateShelf()
        {
            GameObject shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shelf.name = "Shelf";
            shelf.transform.localPosition = new Vector3(1.2f, 0.8f, 1.2f);
            shelf.transform.localScale = new Vector3(1f, 0.2f, 0.6f);
            
            // Wood shelf material
            Material shelfMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.5f, 0.4f, 0.3f), // Wooden shelf
                enableInstancing = true
            };
            shelf.GetComponent<Renderer>().material = shelfMaterial;
            DestroyImmediate(shelf.GetComponent<Rigidbody>());
        }
        private void CreateDesk()
        {
            GameObject desk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            desk.name = "Desk";
            desk.transform.localPosition = new Vector3(-1.2f, 0.9f, 1.5f);
            desk.transform.localScale = new Vector3(1.1f, 0.15f, 0.8f);
            
            // Wood desk material
            Material deskMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.4f, 0.3f, 0.2f), // Wooden desk
                enableInstancing = true
            };
            desk.GetComponent<Renderer>().material = deskMaterial;
            DestroyImmediate(desk.GetComponent<Rigidbody>());
        }
        private void CreateLamp()
        {
            // Lamp base
            GameObject baseObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseObj.name = "LampBase";
            baseObj.transform.localPosition = new Vector3(1f, 0.5f, -0.8f);
            baseObj.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
            baseObj.transform.Rotate(90, 0, 0);
            
            Material baseMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.2f, 0.2f, 0.2f), // Dark base
                enableInstancing = true
            };
            baseObj.GetComponent<Renderer>().material = baseMaterial;
            
            // Lamp shade
            GameObject shade = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            shade.name = "LampShade";
            shade.transform.parent = baseObj.transform;
            shade.transform.localPosition = new Vector3(0, 0.35f, 0);
            shade.transform.localScale = new Vector3(0.3f, 0.35f, 0.3f);
            shade.transform.Rotate(90, 0, 0);
            
            Material shadeMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.9f, 0.85f, 0.8f), // Light shade
                enableInstancing = true
            };
            shade.GetComponent<Renderer>().material = shadeMaterial;
            
            DestroyImmediate(shade.GetComponent<Rigidbody>());
            DestroyImmediate(baseObj.GetComponent<Rigidbody>());
        }
        private void AddLightingAndWindow()
        {
            // Create warm point light
            AddPointLight();
            
            // Create window
            CreateEmissiveWindow();
        }
        private void AddPointLight()
        {
            GameObject lightObj = new GameObject("PointLight");
            Light light = lightObj.AddComponent<Light>();
            
            // Warm white light
            light.color = new Color(0.8f, 0.77f, 0.7f);
            light.range = 3f;
            light.type = LightType.Point;
            light.intensity = 2f;
            light.transform.localPosition = new Vector3(0f, 2f, 0f);
        }
        private void CreateEmissiveWindow()
        {
            GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
            window.name = "Window";
            window.transform.localScale = new Vector3(1f, 1f, 0.1f);
            window.transform.localPosition = new Vector3(2.2f, 1.8f, -1f);
            window.transform.Rotate(0, 90, 0);
            
            // Semi-transparent glass with skybox
            Material windowMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.7f, 0.75f, 1f), // Blush blue glass
                shader = Shader.Find("Transparent/Diffuse"),
                enableInstancing = true,
                renderQueue = 3001
            };
            window.GetComponent<Renderer>().material = windowMaterial;
            
            // Add button glow
            Light glow = window.AddComponent<Light>();
            glow.color = new Color(0.9f, 0.95f, 1f);
            glow.range = 1.5f;
            glow.intensity = 0.5f;
            glow.type = LightType.Point;
            glow.transform.localPosition = new Vector3(0, -0.49f, -0.5f);
            DestroyImmediate(window.GetComponent<Collider>()); // Window is transparent
        }
    }
}