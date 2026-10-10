using UnityEngine;

public static class LearningSignage 
{
    public static void Build()  
    {
        GameObject signagePanel = new GameObject("SignageHub");
        signagePanel.transform.position = new Vector3(0, 1.2f, 0);

        // Center panel with welcoming text
        CreateTextMesh("LearningHubWelcome", "Willkommen in PicoLearningRoom\nAlles ist Lernen",
                     new Vector3(0, 0, 0),
                     Color.green, 0.2f, 50,
                     TextAnchor.MiddleCenter);
                   
          // Left panel with Learning principles
     CreateTextMesh("LernPrinzip", "Lernprinzipien\nInteraktiv Lernen\nexplorieren, recherchieren\nWissen konstruktiv aufbauen",
                   new Vector3(-1.5f, 0, 0),
                   Color.yellow, 0.16f, 45,
                   TextAnchor.MiddleCenter);                    
                  
          // Right panel with Mut-Training phrases
           CreateTextMesh("MutTrainingDescr", "Mut-Training: Fokusangspo anpassung\nForm전을 🕷 GeländePhobie       ",
                   new Vector3(1.5f, 0, 0),
                   Color.red, 0.16f, 45,
                   TextAnchor.MiddleCenter);
       }
}