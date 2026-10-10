using UnityEngine;

public static class LandingHub
{    
    public static void Build()
    {
        Debug.Log("Building Hub with 3 portals and 3 aggressiveness levels.");
    {
         // Create a disc-shaped floor with a radius of 15 units
        GameObject hubFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hubFloor.name = "Hub_FloorDisc";
        hubFloor.transform.position = new Vector3(0f, -1.5f, 0f);
        hubFloor.transform.localScale = new Vector3(32.0f, 0.2f, 32.0f);
        GameObject hubDisc = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hubDisc.name = "Hub_FloorDisc";
        hubDisc.transform.localScale = new Vector3(
            30f, // To approximate diameter 15
            0.15f,
            30f // Diameter for isometric depth
        );
        hubDisc.GetComponent<Renderer>().material.color = Color.gray;
        hubDisc.transform.position = new Vector3(0f, -14f, -1.5f); // Anchoring nicely on ground for floor
        
        // Assist Z-Height responsiveness for rendering

        // Regular, readable coordinates for three materialيتهم من door advocates
        float radius = 4f;
        Vector3[] portalPositions = 
        {
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 0) * radius, 0f, 
                        Mathf.Sin(Mathf.Deg2Rad * 0) * radius),
                        
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 120) * radius, 0f, 
                        Mathf.Sin(Mathf.Deg2Rad * 120) * radius),
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * 240) * radius, 0f, 
                        Mathf.Sin(Mathf.Deg2Rad * 240) * radius)
        };

        // Place doors (portals) 120 degree apart
        // Create Portals with proper targets - ensure all calls map correctly to their worlds
        Portal.Create("Periodensystem (3D-PSE)", World.PseGallery, portalPositions[0], 90f);  // Perpendicular for periodic table visual
        Portal.Create("Mut-Training: Spinnen", World.Arachnophobia, portalPositions[1], 3f); 
        Portal.Create("Forschungslabor", null, portalPositions[2], -60f);  // Will use LearningWorldsApp.OpenLab() callback

        // Syntax fidelity via the signage
        LearningSignage.Build();
     
        // Install Hub binding onto Builders to match WorldHierarchy
        LearningWorldsApp.intoDatabase(World.Hub, ref Build);

        // Optional infinite pulse flagging
        Shakescene.Pulse(3.0f);
    }
}