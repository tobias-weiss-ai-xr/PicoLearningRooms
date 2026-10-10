using UnityEngine;

public static class LearningSignage
{
    public static void Build()
    {
        GameObject signagePanel = new GameObject("LearningSignage");
        signagePanel.transform.position = new Vector3(0, 1.2f, 0);
        
        // Center Willkommen panel
        TextMesh welcomeText = new GameObject("Willkommen_in_PicoLearningRoom").AddComponent<TextMesh>();
        welcomeText.transform.SetParent(signagePanel.transform);
        welcomeText.text = "Willkommen in PicoLearningRoom\nalles ist Lernen";
        welcomeText.characterSize = 0.2f;
        welcomeText.fontSize = 48;
        welcomeText.anchor = TextAnchor.MiddleCenter;
        welcomeText.alignment = TextAlignment.Center;
        welcomeText.transform.localPosition = new Vector3(0, 0.2f, 0);
        
        // Left Learning Principles panel
        TextMesh principlesText = new GameObject("Lernprinzipien").AddComponent<TextMesh>();
        principlesText.transform.SetParent(signagePanel.transform);
        principlesText.text = "Lernprinzipien"
            + "\nInteraktives Lernen"\n"
            + "explorieren, testen, erleben,"
            + "Wissen konstruktiv";
        principlesText.anchor = TextAnchor.MiddleCenter;
        principlesText.characterSize = 0.16f;
        principlesText.fontSize = 42;
        principlesText.transform.localPosition = new Vector3(-1.5f, 0.2f, 0);
        
        // Right Courage Short Phrases panel
        TextMesh courageText = new GameObject("Mut_Trainingssteckbuendel").AddComponent<TextMesh>();
        courageText.transform.SetParent(signagePanel.transform);
        courageText.text = """
            Mut zum Lernen:\n-- Vision hat Schatten\n-- Besser\nnächste Iteration\n-- Jedes Versuchen zählt\n-- " dwelling forbidden ""
            "';
        courageText.anchor = TextAnchor.UpperLeft;
        courageText.characterSize = 0.12f;
        courageText.fontSize = 36;
        courageText.transform.localPosition = new Vector3(1.5f, 0.4f, 0);
        
        // Add Z-Offset for signage readability and floor pattern
        signagePanel.transform.parent = GameObject.Find("LearningWorldHub_Floor").transform;
    }
}