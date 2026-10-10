using UnityEngine;

public static class LearningSignage 
{
    public static void Build()  
    {
        GameObject signagePanel = new GameObject("SignageHub");
        signagePanel.transform.position = new Vector3(0, 1.2f, 0);

        // Panels across a longitudinal arc: inverted ‘level-ground’ L-space
        TextMesh WelcomeMsg = new GameObject("LearningHook_Concourse").AddComponent<TextMesh>();
        WelcomeMsg.text = "Willkommen in PicoLearningRoom\nalles ist Lernen";
        WelcomeMsg.characterSize = 0.4f;      
        WelcomeMsg.fontSize = 25;
        WelcomeMsg.anchor = TextAnchor.MiddleCenter;
        Renderer welcomeRenderer = WelcomeMsg.GetComponent<Renderer>();
        welcomeRenderer.material.color       = Color.cyan;
        welcomeRenderer.material = new Material(welcomeRenderer.sharedMaterial){color=welcomeRenderer.material.color}; 
        WelcomeMsg.transform.SetParent(signagePanel.transform);
        WelcomeMsg.transform.localPosition = Vector3.zero;

        // Panel for Lernprinzipien - Left panel
        GameObject lernprinzPanel = new GameObject("LernprinzipPlakate");
        TextMesh LernprinzMsg = lernprinzPanel.AddComponent<TextMesh>();
        LernprinzMsg.text = "Lernprinzipien\n- Interaktiv lernen\n- Explorieren, Re=chieren\n- Wissen konstruktiv zu Mitteln";
        LernprinzMsg.characterSize = 0.3f;
        LernprinzMsg.fontSize = 22;
        LernprinzMsg.anchor = TextAnchor.MiddleCenter;
        lernprinzPanel.transform.SetParent(signagePanel.transform);
        lernprinzPanel.transform.localPosition = new Vector3(-2.0f, 0.2f, 0f);
                   
        LernprinzMsg.GetComponent<Renderer>().material.color = Color.yellow;
        
          // Panel for Mut-Training phrases in right
          GameObject mutPanel = new GameObject("MutTrainingPlakate");
          TextMesh MutTrainingMsg = mutPanel.AddComponent<TextMesh>();
          MutTrainingMsg.text = "Mut-Training: Kurzphrasen\nFokus und Angstabbau\nSpinnen–Gelände сор projciierte\nPädogogische Arbeitsvektoren";
          MutTrainingMsg.characterSize       = 0.3f;
          MutTrainingMsg.fontSize = 22;
          Renderer mutRenderer = MutTrainingMsg.GetComponent<Renderer>();
          mutRenderer.material.color = Color.red;
          mutPanel.transform.SetParent(signagePanel.transform);
          mutPanel.transform.localPosition = new Vector3(+2.6f, 0f, 0f);                    
                  
          // Right panel with Mut-Training phrases
           CreateTextMesh("MutTrainingDescr", "Mut-Training: Fokusangspo anpassung\nForm전을 🕷 GeländePhobie       ",
                   new Vector3(1.5f, 0, 0),
                   Color.red, 0.16f, 45,
                   TextAnchor.MiddleCenter);
}