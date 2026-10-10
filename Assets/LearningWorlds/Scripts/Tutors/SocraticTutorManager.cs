using UnityEngine;

public class SocraticTutorManager : MonoBehaviour
{
    public enum World
    {
        Hub,
        PseGallery,
        ElementRoom,
        Arachnophobia
    }

    public static SocraticTutorManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SendContextMessage(World world, string message)
    {
        string worldName = world.ToString();

        // Get the TutorAvatar for this world
        // This is a placeholder - in a complete implementation, we'd:
        // 1. Get TutorConfig for this world
        // 2. Get Tutor instance
        // We delegate this to SaiaTutorManager for better decoupling

        // Process the message - first log for debugging
        Debug.Log($"[World:{worldName}] Msg: {message}");

        string builtMessage = BuildSocraticPrompt(worldName, message);
        
        // Get and send to appropriate tutor
        SaiaTutor tutorManager = SaiaTutorManager.Instance.GetTutorForWorld(worldName);
        if (tutorManager != null) // Placeholder method - update this in SaiaTutorManager.cs
        {
            tutorManager.SendToLLM(builtMessage);
        }
        else
        {
            Debug.LogError($"Cannot send context message - no tutor registered for {worldName}");
        }
    }

    private string BuildSocraticPrompt(string world, string context)
    {
        string socraticPrefix;
        switch(world)
        {
            case "Hub":
                socraticPrefix = "In the Learning Hub, consider:";
                break;
            case "PseGallery":
                socraticPrefix = "In the Periodic Table - chemists specialize:";
                break;
            case "ElementRoom":
                socraticPrefix = "In the Element Lab - scientist's approach:";
                break;
            case "Arachnophobia":
                socraticPrefix = "In the Arachnophobia Challenge - therapeutic psychology:";
                break;
            default:
                return context;// Default to context message
        }

        return $"{socraticPrefix} {context}";
    }
    
    public SaiaTutor GetTutorForWorld(string worldName)
    {
        Debug.LogError("Using placeholder from SocraticTutorManager.GetTutorForWorld - should be implemented in SaiaTutorManager");
        return null;
    }
}