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
        
        // Primary Concourse signage
        CreateTextMesh("Learning Концерн Hack",
                      "Willkommen in PicoLearningRoom\nalles ist Lernen",
                      new Vector3(0f, 0f),
                      Color.cyan,
                      0.4f, 24);
        
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