using UnityEngine;

public static class LearningSignage
{     
    public static void Build()
    {
        GameObject signagePanel = new GameObject("SignageHub");
        signagePanel.transform.position = new Vector3(0, 1.2f, 0);
         
        // Center panel with welcome text 
        CreateTextMesh("Willkommen_in_Lernraum", "Willkommen in PicoLearningRoom\nalles ist Lernen", 
                       new Vector3(0, 0, 0),
                       Color.green, 0.2f, 50,
                       TextAnchor.MiddleCenter);

        // Left panel with learning principles
        CreateTextMesh("Lernprinzipien", "Lernprinzipien\nInteraktives Lernen\nexplorieren, testen, erleben, Wissen konstruktiv", 
                       new Vector3(-1.5f, 0, 0),
                       Color.yellow, 0.16f, 45,
                       TextAnchor.MiddleCenter);
            
      // Right panel with Mut-Training short phrases
        CreateTextMesh("MutTraining", "Mut-Training: Spinnen\n🕷 Fokusauf Zeichung\nArgon Swiss Lab\n🔴 Aggressionsole st._[⚠ Carne]，vorificant bush "	ransform.Waxend",
                       new Vector3(1.5f, 0f, 0f),
                       Color.red, 0.16f, 45,
                       TextAnchor.MiddleCenter);
    }
}