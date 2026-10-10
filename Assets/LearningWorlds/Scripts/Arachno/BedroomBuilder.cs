using UnityEngine;

public class BedroomBuilder : MonoBehaviour
{
    public void Build()
    {
        // Floor: 5x4 plate
        Vector3 floorScale = new Vector3(5f, 0.1f, 4f);
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.parent = transform;
        floor.transform.localPosition = Vector3.zero;
        floor.transform.localScale = floorScale;
        floor.name = "Floor";
        DestroyImmediate(floor.GetComponent<BoxCollider>());
        floor.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.gray };

        // Walls
        float wallHeight = 3f;
        float wallThickness = 0.2f;
        Vector3 wallOffset = new Vector3(0, wallHeight / 2, 0);

        // Back Wall (5 units wide)
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.transform.parent = transform;
        backWall.transform.localPosition = new Vector3(0, wallHeight / 2, -2f);
        backWall.transform.localScale = new Vector3(5f, wallHeight, wallThickness);
        backWall.name = "BackWall";
        DestroyImmediate(backWall.GetComponent<BoxCollider>());
        backWall.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.gray};

        // Front Wall (5 units wide)
        GameObject frontWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frontWall.transform.parent = transform;
        frontWall.transform.localPosition = new Vector3(0, wallHeight / 2, 2f);
        frontWall.transform.localScale = new Vector3(5f, wallHeight, wallThickness);
        frontWall.name = "FrontWall";
        DestroyImmediate(frontWall.GetComponent<BoxCollider>());
        frontWall.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.gray };

        // Left Wall (4 units tall)
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftWall.transform.parent = transform;
        leftWall.transform.localPosition = new Vector3(-2.5f, wallHeight / 2, 0f);
        leftWall.transform.localScale = new Vector3(wallThickness, wallHeight, 4f);
        leftWall.name = "LeftWall";
        DestroyImmediate(leftWall.GetComponent<BoxCollider>());
        leftWall.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.gray };

        // Right Wall (4 units tall)
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightWall.transform.parent = transform;
        rightWall.transform.localPosition = new Vector3(2.5f, wallHeight / 2, 0f);
        rightWall.transform.localScale = new Vector3(wallThickness, wallHeight, 4f);
        rightWall.name = "RightWall";
        DestroyImmediate(rightWall.GetComponent<BoxCollider>());
        rightWall.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.gray };

        // Bed
        GameObject bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bed.transform.parent = transform;
        bed.transform.localPosition = new Vector3(-1.75f, 0.5f, -0.5f);
        bed.transform.localScale = new Vector3(2.5f, 0.25f, 1.5f);
        bed.name = "Bed";
        DestroyImmediate(bed.GetComponent<BoxCollider>());
        bed.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.brown };

        // Shelf
        GameObject shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
        shelf.transform.parent = transform;
        shelf.transform.localPosition = new Vector3(1.5f, 1.5f, -1f);
        shelf.transform.localScale = new Vector3(1f, 0.25f, 0.5f);
        shelf.name = "Shelf";
        DestroyImmediate(shelf.GetComponent<BoxCollider>());
        shelf.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.lightGray };

        // Desk
        GameObject desk = GameObject.CreatePrimitive(PrimitiveType.Cube);
        desk.transform.parent = transform;
        desk.transform.localPosition = new Vector3(1f, 0.5f, 1f);
        desk.transform.localScale = new Vector3(1.5f, 0.25f, 1f);
        desk.name = "Desk";
        DestroyImmediate(desk.GetComponent<BoxCollider>());
        desk.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.gray };

        // Lamp
        GameObject lampBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lampBase.transform.parent = transform;
        lampBase.transform.localPosition = new Vector3(1f, 0f, 1f);
        lampBase.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
        lampBase.name = "LampBase";
        DestroyImmediate(lampBase.GetComponent<CapsuleCollider>());
        lampBase.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.black };

        // Point Light
        Light lampLight = Instantiate(new GameObject().AddComponent<Light>());
        lampLight.transform.parent = transform;
        lampLight.transform.localPosition = new Vector3(1f, 1.5f, 1f);
        lampLight.intensity = 5;
        lampLight.range = 3;
        lampLight.color = new Color(0.95f, 0.6f, 0.25f); // Specifically Warmer White
        lampLight.type = LightType.Point;

        // Window Emissive Plane
        GameObject windowFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        windowFrame.transform.parent = transform;
        windowFrame.transform.localPosition = new Vector3(1.75f, 2f, 0);
        windowFrame.transform.localScale = new Vector3(0.2f, 1.5f, 1f);
        windowFrame.name = "WindowFrame";
        DestroyImmediate(windowFrame.GetComponent<BoxCollider>());
        windowFrame.AddComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.white };

        // Emissive Window Plane
        Transform windowEmissive = new GameObject("WindowGlass").transform;
        windowEmissive.parent = transform;
        windowEmissive.localPosition = new Vector3(2f, 2f, 0);
        windowEmissive.localScale = new Vector3(3f, 1f, 1f);
        windowEmissive.gameObject.AddComponent<Renderer>().material = new Material(Shader.Find("Standard"))
        {
            color = Color.white,
            enableKeyword = "_EMISSION",
            globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack
        };
        windowEmissive.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.blue * 0.5f);
    }
}