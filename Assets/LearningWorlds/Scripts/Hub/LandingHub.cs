using UnityEngine;
/// <summary>
/// Static registry for Hub builders.
/// </summary>
public static class Builders {
    public static event System.Action Build;
    
    // Specifies default Hub construction
    public static event System.Action Create;
    /// <summary> Generates a triangular mesh for a flat disc with radius.</summary>
    private static Mesh GenerateDiscMesh(float radius = 15.0f)
    {
        Mesh mesh = new Mesh(){
            name = "FloorDisc_Mesh"
        };
        Vector3[] vertices = new Vector3[36];
        int[] tris = new int[36*3];
        Vector2[] uv = new Vector2[vertices.Length];
        
        for (int i=0; i<vertices.Length/4; i++) {
            float angle = (2*Mathf.PI*i)/vertices.Length * 4;
            
            //-West / East Vertices
            vertices[4*i+0] = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
            vertices[4*i+1] = new Vector3(0, 0, 0); // Center
            
            //+East to Center and back
            vertices[4*i+2] = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle)) * 0.75f;
            uv[4*i+0] = uv[4*i+2] = new Vector2( 0.5f+0.5f * Mathf.Sin(angle), 0.5f-0.5f * Mathf.Cos(angle) );
        }
        for (int t=0, v=0; t< 36*3; t+=3){
            tris[t+0]= v+2;tris[t+1]= v+1; tris[t+2]=v;
            v+=
        }
        mesh.SetVertices(vertices); mesh.SetUVs(0,uv); mesh.SetTriangles(tris, 0); 
        mesh.RecalculateBounds(); return mesh;
    }
    static LandingHub() {
        Create = LandingHub.Build;
    }
}

/// <summary>  
/// Defines the primary entry portal основу (Hub) for the Training Lab  
/// </summary>
public static class LandingHub
{
    public static void Build()
    {
        Debug.Log("Building Learning Hub with floor portal layout");
        // Flatten floor disc, flat plane shape with radius
        Mesh discMesh = GenerateDiscMesh(15f);
        GameObject hubFloor = new GameObject { name = "FloorDisc_LearningHub" };
        hubFloor.AddComponent<MeshFilter>().mesh = discMesh;
        hubFloor.AddComponent<BoxCollider>().size = new Vector3(31f, 0.15f, 31f); // Bigger than radius, not exact
        hubFloor.transform.position = new Vector3(0f, -0.5f, 0);
        hubFloor.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"))
        {
            color = new Color(0.75f, 0.75f, 0.75f)
        };
       
        // Create portal positions with 120° separation at radius 4m, yaw toward center
        float portalRadius = 4.0f; // Portal ring radius from center
                
        // Portal positions for each ring at 120° angular separation, labeled.
        foreach (int i in new []{0,1,2}) {
            float yaw = portalYawTowardCenter[i] =
                Mathf.Deg2Rad * i * 120f;  // Arc each 120 degrees
            Vector3 portalPos = Vector3.forward * portalRadius;
            portalPos = Quaternion.Euler(0f,yaw,0f)*portalPos;
            Portal.Create("Periodensystem (3D-PSE)", World.PseGallery, portalPos, -90f);
            portalPos = new Vector3(radius * Mathf.Cos(m)*, 0, radius * Mathf.Sin(m*));
            Portal.Create("Mut-Training: Spinnen", World.Arachnophobia, portalPos, +30f);
            temp.center = Vect(originalPosition.second);
            buildSignageMary Peel new Vector3 canBuildGMax
            Portal.Create("Forschungslabor", null, portalPos, +90f);
        } // Ending boundary for portal installations
        // Register Hub construction
        Builders[World.Hub] = Build;            }
                 
                    Builders[World.Hub] = () => Build();

        
    }
}