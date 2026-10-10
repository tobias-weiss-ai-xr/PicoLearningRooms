using UnityEngine;

namespace LearningWorlds.Arachno
{
    public class BedroomBuilder : MonoBehaviour
    {
        public void Build()
        {
            // Create a flooring plane
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "BedroomFloor";
            floor.transform.localScale = new Vector3(5, 1, 4);
            floor.transform.position = new Vector3(0, 0, 0);
            var floorRenderer = floor.GetComponent<Renderer>();
            floorRenderer.material = new Material(Shader.Find("Standard")) { color = new Color(150 / 255f, 100 / 255f, 50 / 255f) };

            // Create walls (left, right, front, back)
            CreateWall("LeftWall", new Vector3(-2.5f, 0.5f, -2.5f), Vector3.forward, new Vector3(1, 2, 0.2f));
            CreateWall("RightWall", new Vector3(2.5f, 0.5f, -2.5f), Vector3.back, new Vector3(1, 2, 0.2f));
            CreateWall("FrontWall", new Vector3(0, 0.5f, 3), Vector3.forward, new Vector3(5, 2, 0.2f));
            CreateWall("BackWall", new Vector3(0, 0.5f, -3), Vector3.back, new Vector3(5, 2, 0.2f));

            // Create furniture
            CreateBed();
            CreateShelf();
            CreateDesk();
            CreateLamp();
            CreateWindow();
        }

        private void CreateWall(string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = position;
            wall.transform.rotation = Quaternion.Euler(rotation);
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = new Color(200 / 255f, 200 / 255f, 200 / 255f) };
        }

        private void CreateBed()
        {
            var bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bed.name = "Bed";
            bed.layer = LayerMask.NameToLayer("Default");
            bed.transform.position = new Vector3(1.5f, 0.5f, -1.5f);
            bed.transform.localScale = new Vector3(2, 0.4f, 1.5f);
            var bedRenderer = bed.GetComponent<Renderer>();
            bedRenderer.material = new Material(Shader.Find("Standard")) { color = new Color(80 / 255f, 50 / 255f, 30 / 255f) };

            // Add a headboard (extra planar wall-like shape)
            var headboard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            headboard.name = "BedHeadboard";
            headboard.transform.position = new Vector3(2.75f, 1, -1);
            headboard.transform.rotation = Quaternion.Euler(0, 90, 0);
            headboard.transform.localScale = new Vector3(0.2f, 1.6f, 1.5f);
            headboard.GetComponent<Renderer>().material = bedRenderer.material;
        }

        private void CreateShelf()
        {
            var shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shelf.name = "Shelf";
            shelf.transform.position = new Vector3(-1.5f, 1, -0.5f);
            shelf.transform.localScale = new Vector3(1.5f, 0.2f, 0.6f);
            shelf.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = new Color(180 / 255f, 130 / 255f, 90 / 255f) };
        }

        private void CreateDesk()
        {
            var desk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            desk.name = "Desk";
            desk.transform.position = new Vector3(-1f, 0.5f, 1f);
            desk.transform.localScale = new Vector3(1, 0.4f, 1f);
            desk.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = new Color(100 / 255f, 70 / 255f, 40 / 255f) };

            // Add a chair
            var chair = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            chair.name = "DeskChair";
            chair.transform.position = new Vector3(-1f, 0.2f, 1.5f);
            chair.transform.localScale = new Vector3(0.3f, 0.6f, 0.3f);
            chair.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = new Color(60 / 255f, 40 / 255f, 20 / 255f) };
        }

        private void CreateLamp()
        {
            var lampPost = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            lampPost.name = "LampPost";
            lampPost.transform.position = new Vector3(1f, 0.8f, 1f);
            lampPost.transform.localScale = new Vector3(0.2f, 1f, 0.2f);
            lampPost.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.black };

            var lightEmitter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            lightEmitter.name = "LampEmissive";
            lightEmitter.transform.position = new Vector3(1f, 2f, 1f);
            lightEmitter.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            var lightMaterial = new Material(Shader.Find("Standard")) { color = Color.white };  
            lightMaterial.SetFloat("_EmissionColor", Color.white rząd 20);
            lightEmitter.GetComponent<Renderer>().material = lightMaterial;

            var light = lightEmitter.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = Color.white;
            light.range = 3f;
            light.intensity = 2f;
        }

        private void CreateWindow()
        {
            var windowPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            windowPlane.name = "Window";
            windowPlane.transform.position = new Vector3(2.5f, 0.9f, -2f);
            windowPlane.transform.rotation = Quaternion.Euler(90, 0, 0);
            windowPlane.transform.localScale = new Vector3(1.5f, 0.2f, 1.5f);
            var windowMaterial = new Material(Shader.Find("Standard")) { color = new Color(200 / 255f, 220 / 255f, 240 / 255f), color = Color.white };
            windowMaterial.SetColor("_EmissionColor", Color.cyan * (0.1f + 0.2f * Mathf.Sin(Time.time * 2f)));
            windowPlane.GetComponent<Renderer>().material = windowMaterial;

            // Add a window frame (repeated outlines)
            for (int i = 0; i < 6; i++)
            {
                var frameSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                frameSegment.name = "WindowFrameSegment";
                frameSegment.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = new Color(0.4f, 0.4f, 0.4f) };
                frameSegment.transform.parent = windowPlane.transform;
                // Add segment position logic to frame window edges
            }
        }
    }
}