using UnityEngine;
using LearningWorlds;

namespace Arachnophobia
{
    public class BedroomBuilder : Builder
    {
        public override GameObject Build()
        {
            // Create a 5x4 floor plate
            GameObject floor = CreateFloor(5f, 4f);
            
            // Create walls
            GameObject[] walls = CreateWalls();
            
            // Create furniture: bed, shelf, desk, and lamp
            GameObject bed = CreateBed();
            GameObject shelf = CreateShelf();
            GameObject desk = CreateDesk();
            GameObject lamp = CreateLamp();
            
            // Create a window with emissive properties
            CreateWindow();
            
            // Assembly the parent object for the bedroom
            GameObject bedroom = new GameObject("Bedroom");
            
            // Organize everything under the bedroom
            floor.transform.SetParent(bedroom.transform);
            foreach (GameObject wall in walls)
            {
                wall.transform.SetParent(bedroom.transform);
            }
            bed.transform.SetParent(bedroom.transform);
            shelf.transform.SetParent(bedroom.transform);
            desk.transform.SetParent(bedroom.transform);
            lamp.transform.SetParent(bedroom.transform);
            
            return bedroom;
        }
        
        private GameObject CreateFloor(float length = 5f, float width = 4f)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.localScale = new Vector3(length, 0.1f, width);
            floor.transform.position = new Vector3(0, 0f, 0f);
            floor.GetComponent<Renderer>().material = CreateSimpleMaterial("Floor", Color.gray);
            return floor;
        }
        
        private GameObject[] CreateWalls()
        {
            const float wallHeight = 3f;
            GameObject[] walls = new GameObject[4];
            
            // Create first wall (left wall)
            walls[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls[0].name = "Left Wall";
            walls[0].transform.position = new Vector3(-1f, wallHeight / 2f, 0f);
            walls[0].transform.localScale = new Vector3(0.1f, wallHeight, 5f);
            walls[0].GetComponent<Renderer>().material = CreateSimpleMaterial("Wall", Color.white);
            
            // Create second wall (front wall)
            walls[1] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls[1].name = "Front Wall";
            walls[1].transform.position = new Vector3(0f, wallHeight / 2f, -2f);
            walls[1].transform.localScale = new Vector3(4.5f, wallHeight, 0.2f);
            walls[1].GetComponent<Renderer>().material = CreateSimpleMaterial("Wall", Color.white);
            
            // Create third wall (right wall)
            walls[2] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls[2].name = "Right Wall";
            walls[2].transform.position = new Vector3(1f, wallHeight / 2f, 0f);
            walls[2].transform.localScale = new Vector3(0.1f, wallHeight, 5f);
            walls[2].GetComponent<Renderer>().material = CreateSimpleMaterial("Wall", Color.white);
            
            // Create fourth wall (back wall)
            walls[3] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls[3].name = "Back Wall";
            walls[3].transform.position = new Vector3(0f, wallHeight / 2f, 2f);
            walls[3].transform.localScale = new Vector3(4.5f, wallHeight, 0.2f);
            walls[3].GetComponent<Renderer>().material = CreateSimpleMaterial("Wall", Color.white);
            
            return walls;
        }
        
        private GameObject CreateBed()
        {
            GameObject bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bed.name = "Bed";
            bed.transform.position = new Vector3(0f, 1f, -1.5f);
            bed.transform.localScale = new Vector3(2f, 0.3f, 1f);
            bed.GetComponent<Renderer>().material = CreateSimpleMaterial("Bed", Color.gray);
            return bed;
        }
        
        private GameObject CreateShelf()
        {
            GameObject shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shelf.name = "Shelf";
            shelf.transform.position = new Vector3(-1.5f, 1.5f, 1f);
            shelf.transform.localScale = new Vector3(0.2f, 1f, 1f);
            shelf.GetComponent<Renderer>().material = CreateSimpleMaterial("Shelf", Color.brown);
            return shelf;
        }
        
        private GameObject CreateDesk()
        {
            GameObject desk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            desk.name = "Desk";
            desk.transform.position = new Vector3(1.5f, 0.8f, 1f);
            desk.transform.localScale = new Vector3(1f, 0.2f, 1f);
            desk.GetComponent<Renderer>().material = CreateSimpleMaterial("Desk", Color.brown);
            return desk;
        }
        
        private GameObject CreateLamp()
        {
            GameObject lampBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            lampBase.GetComponent<CylinderCollider>().radius = 0.1f;
            lampBase.GetComponent<CylinderCollider>().height = 0.05f;
            lampBase.name = "Lamp Base";
            lampBase.transform.position = new Vector3(1.5f, 0.3f, 0.5f);
            
            GameObject lampShade = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            lampShade.GetComponent<CylinderCollider>().radius = 0.12f;
            lampShade.GetComponent<CylinderCollider>().height = 0.2f;
            lampShade.name = "Lamp Shade";
            lampShade.transform.position = new Vector3(1.5f, 0.52f, 0.5f);
            
            GameObject parentLamp = new GameObject("Lamp");
            lampBase.transform.SetParent(parentLamp.transform);
            lampShade.transform.SetParent(parentLamp.transform);
            
            // Add light source for the lamp
            GameObject lampLight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            lampLight.name = "Light Source";
            lampLight.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            lampLight.transform.parent = lampShade.transform;
            lampLight.transform.localPosition = new Vector3(0f, 0.12f, 0f);
            
            Light light = lampLight.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.9f, 0.8f); // Warm white
            light.range = 5f;
            light.intensity = 2f;
            light.shadows = LightShadows.Soft;
            
            // Mark light as a child of the lamp
            lampLight.transform.SetParent(parentLamp.transform);
            return parentLamp;
        }
        
        private void CreateWindow()
        {
            GameObject window = GameObject.CreatePrimitive(PrimitiveType.Quad);
            window.name = "Window";
            window.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            window.transform.localScale = new Vector3(3f, 1f, 1f);
            window.transform.position = new Vector3(0f, 2f, 2.1f);
            
            Renderer renderer = window.GetComponent<Renderer>();
            renderer.material = CreateSimpleMaterial("Window", Color.blue);
            
            // Add emissive effect to simulate sunlight
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", new Color(0.1f, 0.2f, 0.4f, 1f));
            
            // Remove collider as it's part of the environment
            GameObject.Destroy(window.GetComponent<Collider>());
        }
        
        private Material CreateSimpleMaterial(string name, Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.name = name;
            mat.color = color;
            return mat;
        }
    }
}