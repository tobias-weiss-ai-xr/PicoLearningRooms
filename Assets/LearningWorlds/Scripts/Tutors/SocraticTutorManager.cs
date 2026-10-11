using UnityEngine;
using LearningWorlds;
using LearningWorlds.Tutors;

namespace LearningWorlds.Tutors
{
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
        /// Sends a context message using world name directly.
        /// </summary>
        public void SendContextMessage(string worldName, string message)
        {
            Debug.Log("[SocraticTutorManager] Context message for " + worldName + ": " + message);

            string builtMessage = BuildSocraticPrompt(worldName, message);

            SaiaTutor tutor = SaiaTutorManager.Instance.GetTutorForWorld(worldName);
            if (tutor != null)
            {
                tutor.SendToLLM(builtMessage);
            }
        }

        /// <summary>
        /// Builds a Socratic-style prompt based on world context.
        /// </summary>
        private string BuildSocraticPrompt(string worldName, string context)
        {
            // Socratic questioning patterns based on world
            switch (worldName)
            {
                case "PseGallery":
                    return "As a chemistry teacher, guide the student through the periodic table. Ask: " + context + " What patterns do you notice? Why are elements grouped this way?";

                case "ElementRoom":
                    return "As a chemistry explorer, discuss: " + context + " What are the atomic properties? How does electron configuration determine behavior?";

                case "Arachnophobia":
                    return "As a therapist, gently ask: " + context + " How do you feel about this? What thoughts come to mind as you observe the spider?";

                case "Hub":
                    return "As a wise guide, inquire: " + context + " Which learning world interests you today? What would you like to explore?";

                default:
                    return "Discuss: " + context + " What can we learn from this?";
            }
        }

        /// <summary>
        /// Gets the appropriate tutor for a specific world.
        /// </summary>
        public SaiaTutor GetTutor(World world)
        {
            string worldName = world.ToString();
            return SaiaTutorManager.Instance.GetTutorForWorld(worldName);
        }

        /// <summary>
        /// Gets the appropriate tutor for a world by name.
        /// </summary>
        public SaiaTutor GetTutor(string worldName)
        {
            return SaiaTutorManager.Instance.GetTutorForWorld(worldName);
        }
    }
}
