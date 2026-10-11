using UnityEngine;
using LearningWorlds;

/// <summary>
/// Manages Socratic questioning for tutors in different learning worlds.
/// Integrates with SaiaTutorManager to route messages to the correct tutor.
/// </summary>
public class SocraticTutorManager : MonoBehaviour
{
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
    /// </summary>
    /// <param name="world">The learning world (from LearningWorlds.World enum)</param>
    /// <param name="message">The context message to send</param>
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
    /// Gets the tutor name for a specific world (for display purposes).
    /// </summary>
    public string GetTutorNameForWorld(World world)
    {
        switch (world)
        {
            case World.Hub: return "Sokrates-Wegweiser";
            case World.PseGallery: return "Sokrates-Chemiker";
            case World.ElementRoom: return "Sokrates-Forscher";
            case World.Arachnophobia: return "Sokrates-Therapeut";
            default: return "Tutor";
        }
    }

    /// <summary>
    /// Builds a Socratic-style prompt based on the world context.
    /// </summary>
    private string BuildSocraticPrompt(string world, string context)
    {
        string socraticPrefix;
        switch (world)
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
