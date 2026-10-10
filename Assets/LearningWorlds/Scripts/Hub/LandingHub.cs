using UnityEngine;

public static class LandingHub
{    
    public static void Build()
    {
        GameObject hubFloor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hubFloor.name = "LearningWorldHub_Floor";
        hubFloor.transform.localScale = new Vector3(15f, 0.1f, 15f);
        hubFloor.GetComponent<Renderer>().material.color = Color.gray;

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
        
        // Connect plugin "Setup" to system:
        Builders[World.Hub] = Build;

        // Optional infinite pulse flagging
        Shakescene.Pulse(3.0f);
    }
}