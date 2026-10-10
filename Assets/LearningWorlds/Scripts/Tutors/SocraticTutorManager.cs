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

    /// <summary>
    /// Sends a context-aware message to the tutor for the specified world.
    /// Uses SaiaTutorManager to route to the correct tutor.
    /// </summary>
    public void SendContextMessage(World world, string message)
    {
        string worldName = world.ToString();
        Debug.Log("[SocraticTutorManager] Context message for " + worldName + ": " + message);

        string builtMessage = BuildSocraticPrompt(worldName, message);

        // Get tutor from SaiaTutorManager
        SaiaTutor tutor = SaiaTutorManager.Instance.GetTutorForWorld(worldName);
        if (tutor != null)
        {
            tutor.SendToLLM(builtMessage);
        }
        else
        {
            Debug.LogError("[SocraticTutorManager] Cannot send context message - no tutor registered for " + worldName);
        }
    }

    /// <summary>
    /// Builds a Socratic-style prompt based on the world context.
    /// </summary>
    private string BuildSocraticPrompt(string world, string context)
    {
        string socraticPrefix;
        switch(world)
        {
            case "Hub":
                socraticPrefix = "In the Learning Hub, consider:";
                break;
            case "PseGallery":
                socraticPrefix = "In the Periodic Table, chemists specialize:";
                break;
            case "ElementRoom":
                socraticPrefix = "In the Element Lab, scientist's approach:";
                break;
            case "Arachnophobia":
                socraticPrefix = "In the Arachnophobia Challenge, therapeutic psychology:";
                break;
            default:
                return context; // Default to context message
        }

        return socraticPrefix + " " + context;
    }
}
