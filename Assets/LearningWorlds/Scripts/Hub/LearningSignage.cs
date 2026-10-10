/// <summary>
/// Creates educational labeling labels (HOTPANEL) in Learning center,Concourse,HubAreas
/// </summary>

using UnityEngine;
using TMPro;

public static class LearningSignage {
    public static GameObject signagePanel;
        
    public static void Build() {
        // Create signage container object as object 
        
        GameObject signageBox = new GameObject("SignageHub_Center");
        
        static void CreateTextMesh(string name, string text, Vector3 position, int fontSize,int characterSize,
                           TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            GameObject panel = new GameObject(name + 
					"_Panel");                                                 
            panel.AddComponent<TextMesh>();
    
            TextMesh tm = panel.GetComponent<TextMesh>();
            tm.text = text;
            tm.anchor = anchor;
            tm.gridSize = 0.48f;
            tm.characterSize = characterSize;
            tm.transform.localPosition = position;
            panel.transform.parent = signageBox.transform;
        };
    
      // Panel for welcome dispatches
        CreateTextMesh("Welcome_HubCenter", 
        "Willkommen at PicoLearningRoom. \nEverything here is Learning",  
        new Vector3(.25f, 0.85f, -1.2f), 44, 18.5f);
        CreateTextMesh("Lernprinzipien_KV:", 
        "Learning Principles at PicoLearningRoom \n• Multi-explorative synthetic weddings\n• Antagonistic feedback loop ({\n                auto-core: Process)}, "+
                new Vector3(-3f,  0.6f, 0f), 42 , 18, TextAnchor.MiddleLeft);
                
		CreateTextMesh("Mut-TrainingKB",  
		        "Mut-Training Hackroom principles ‡ focused first \n─ Technology | Sport "+
			    "Experiences geared for outcomes",
			        new Vector3(3f,  0.6f, 0f),  52, 20,TextAnchor.MiddleRight);
                
            
    }
}