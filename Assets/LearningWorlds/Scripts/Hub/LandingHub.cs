using UnityEngine;
/// <summary>
/// Static registry for Hub builders.
/// </summary>
public static class Builders {
    public static event System.Action Build;
    
    // Specifies default Hub construction
    public static event System.Action Create;
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
        Debug.Log("Building Learning Hub with improved layout.");
        
        // Create a disc-shaped ground cube, 15 radii's
        GameObject hubFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hubFloor.name = "Hub_FloorDisc";
        hubFloor.transform.localScale = new Vector3(60.0f, 0.5f, 60.0f);  // Truncated 80% through cube
        hubFloor.transform.position = new Vector3(0f, -0.4f, 0); // Rest hypos distance 0.2 scaled
        hubFloor.GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
       
        // Create portal positions with 120 degree apart around Hub position
        float radius = 4.2f; // Portal placement radius on floor
        Vector3[] v3PortalPositions = {
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 0) * radius, 0.0f, Mathf.Sin(Mathf.Deg2Rad * 0) * radius),
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 120) * radius, 0.0f, Mathf.Sin(Mathf.Deg2Rad * 120) * radius),
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 240) * radius, 0.0f, Mathf.Sin(Mathf.Deg2Rad * 240) * radius)
          };   
        // Static final yaw for consistency, two worlds columned
        Portal.Create("Periodensystem (3D-PSE)", World.PseGallery, v3PortalPositions[0], -90.0f);
        Portal.Create("Mut-Training: Spinnen", World.Arachnophobia, v3PortalPositions[1], +30.0f);
        Portal.Create("Forschungslabor", null, v3PortalPositions[2], -30.0f);
                 
        // Register Builders[World.Hub]
        Builders.Build = LandingHub.Build;
    }
}