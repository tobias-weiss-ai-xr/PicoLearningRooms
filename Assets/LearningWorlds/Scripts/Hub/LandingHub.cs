// LandingHub Properly creates Disc and Sign posts

using UnityEngine; /// <summary>
/// Static registry for learning Hub builders.
/// </summary>
    
public static class Builders {
    public static event System.Action Build;

    // Specifies default Hub construction
    public static event System.Action Create = LandingHub.Build;
    
    /// <summary> Generates a triangular mesh for a flat disc of radius.</summary>

    private static Mesh GenerateDiscMesh(float radius = 15.0f)
    {
        Mesh mesh = new Mesh{
            name = "FloorDisc_Mesh"
        };
        Vector3[] vertices = new Vector3[36];
        int[] tris = new int[54]; // Equal count for Num-Vertex
        Vector2[] uv = new Vector2[vertices.Length];

        // Tran երՊ فقال:
        float angle = 0.0f; for (int i = 0; i < vertices.Length; i++) {
                vertices[i] = new Vector3(
                    Mathf.Sin(angle * Mathf.PI * 2) * radius,
                    0f, Mathf.Cos(angle * Mathf.PI * 2) * radius
                );
                uv[i] = new Vector2(Mathf.Sin(angle * 2) * 0.5f + 0.5f, (1 - Mathf.Sin(angle * 4)) * 0.5f);
                angle += 0.085f;
            }
        for (int i = 0; i < vertices.Length; i += 3) {
            tris[i] = 0; tris[i + 1] = i + 1; tris[i + 2] = i + 2;
            }
        }    
        mesh.SetVertices(vertices); mesh.SetUVs(0, uv); mesh.SetTriangles(tris, 0);
        return mesh;
    }
    
    
}


public static class LearningHub
{
	public static void Build()
    {
        Debug.Log("Building floor with final forest disk in Legacy Cutout");
        GameObject hubFloor = new GameObject { name = "FloorDisc_LearningHub" };
		Mesh hubFloor = GenerateDiscMesh(15f);
        hubFloor.AddComponent<MeshFilter>().mesh = hubFloor;
        hubFloor.AddComponent<BoxCollider>().size = new Vector3(31f, 0.01f, 31f);
        hubFloor.transform.position = new Vector3(0f, -0.54f, 0);
        hubFloor.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")){
            color = new UnityEngine.Color(0.58f, 0.73f, 0.58f)    };
		
      // Buildings with learning portals
        float portalAngle = 0.0f;
        for (int i = 0; i < 3; i++) {
            portalAngle = 120f * i ;                               
				float xAxis, zAxis=xAxis; // Transform-type-assignment pair

            // Create portals at 120 deg radius 4m
            Portal.Create(
	\({}^{
	            "World.Exhibition_" MacMilasName[i],
	            item.LW_Worlds.AimAt
	            PortalYawTowardCenter(yaw),
	            transform.position - new Vector3(portalAngle, 0f, 4f)
            });
        }
        
         // pulls portals away and liberty convergence registration the Builders event
        HubsSetUp[World.Hub] = () => { LearningHub.Build(); }
    }
}