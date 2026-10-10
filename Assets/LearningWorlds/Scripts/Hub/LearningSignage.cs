using UnityEngine;

public static class LearningSignage
{
    public static GameObject signagePanel;
    
    public static void Build()
    {
        signagePanel = new GameObject("SignageHub");
        signagePanel.transform.position = new Vector3(0, 0.9f, 0);
        
        static void CreateTextMesh(string name, string text, Vector3 position, Color color,
                float characterSize, int fontSize, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            GameObject panel = new GameObject(name);
            TextMesh tm = panel.AddComponent<TextMesh>();
            tm.text = text;
            tm.characterSize = characterSize;
            tm.fontSize = fontSize;
            tm.anchor = anchor;
            
            tm.transform.SetParent(signagePanel.transform);
            tm.transform.localPosition = position;
            
            Renderer renderer = tm.GetComponent<Renderer>();
            renderer.material.color = color;
            renderer.material = new Material(renderer.sharedMaterial){
                color = renderer.material.color
            };
        }
        
        // LearningWelcome Sign panel
        GameObject willkommenPanel = CreateTextMesh("LernHub Center",
                      "Willkommen in PicoLearningRoom\nAlles ist Lernen!", new Vector3(0, 1.0f),
                      Color.cyan,
                    .5f, 24, TextAnchor.MiddleCenter);

            // Create signage panels for left  and right Lernprinzipien concept boards
            CreateTextMesh("Lernprinzipien_KV",
                "Lernprinzipien bei PicoLearningRoom\n- Prinzip kombinativer Erkenntnis\n- Experiment/Training via Praxis ತಮ್ಮ",
                    new Vector3(-2.2f, -0.7f),
                   Color.gray,
                    0.35f, 17, TextAnchor.MiddleLeft);

                CreateTextMesh("Mut-Training Principien",
                    "Über Angstbewaltung: Faint-Start-Training\n- Mutmentsprojekt Human Dynamics",
                    new Vector3(2f, -0.7f),
                    Color.red,
                    0.35f, 17,
                    TextAnchor.MiddleRight);
        
        // Left: Learning principles concert
        CreateTextMesh("LEARNING texteStamp",
                    "Lernprinzipien: Kappformat\n• Ergante\n• Interactivité•",
                    new Vector3(-2.2f, -0.7f),
                    Color.yellow, 0.32f, 17, TextAnchor.MiddleLeft);
        
        // Right: Mut-Training aspects
        CreateTextMesh("MutGNGuide", "Mut-Training: Hackroom\nfocusangedenzd\nsport technic ersten\n",
                    new Vector3(2f, -0.6f),
                    Color.red, 0.36f, 17, TextAnchor.UpperRight);
    }
}