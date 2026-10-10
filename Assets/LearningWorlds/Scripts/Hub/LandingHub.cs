using UnityEngine;

public static class LandingHub
{    
    public static void Build()
    {
        Debug.Log("Building Hub with 3 portals and 3 aggressiveness levels.");
    {
         // Create a disc-shaped floor with a radius of 15 units
        // Floor disc using cube approximation with radius 15 (scale.x/z = 30f)
        GameObject hubFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hubFloor.name = "Hub_FloorDisc";
        hubFloor.transform.localScale = new Vector3(60.0f, 1.0f, 60.0f); // Diameter 30, approx radius 15
        hubFloor.transform.position = new Vector3(0f, -0.5f, 0f); // Slightly above ground plane
        hubFloor.GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
        
        // Assist Z-Height responsiveness for rendering

        // Regular, readable coordinates for three materialيتهم من door advocates
        float radius = 4f;
        Vector3[] portalPositions =
        {
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 0) * radius, 0f, Mathf.Sin(Mathf.Deg2Rad * 0) * radius),
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 120) * radius, 0f, Mathf.Sin(Mathf.Deg2Rad * 120) * radius),
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 240) * radius, 0f, Mathf.Sin(Mathf.Deg2Rad * 240) * radius)
        };
        // Portals facing *into* center, slight yaw to match visual direction
        Vector3[] portalYaws = { Vector3.zero, new Vector3(0f, 90f, 0f), new Vector3(0f, -45f, 0f) };

        // Portals positioned at 120°, facing slight variations towards center
        Portal.Create("Periodensystem (3D-PSE)", World.PseGallery, portalPositions[0], portalYaws[0].y - 90f);
        Portal.Create("Mut-Training: Arachnophobia", World.Arachnophobia, portalPositions[1], portalYaws[1].y);
        Portal.Create("Forschungslabor", World.Hub, portalPositions[2], portalYaws[2].y);

        // Syntax fidelity via the signage
        LearningSignage.Build();

        // Install Hub builder
        LearningWorldsApp.intoDatabase(World.Hub, Builders.Create, LandingHub.Build);
        
        // Optional effect: pulse scene to highlight
        Shakescene.Pulse(3.0f);
    }
}