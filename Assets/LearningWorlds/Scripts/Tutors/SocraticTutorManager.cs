using UnityEngine;
using Convai.Scripts;
using System.Collections.Generic;

namespace LearningWorlds.Tutors
{
    /// <summary>
    /// Manages Socratic AI tutors for each learning world.
    /// Integrates with Convai NPCs and adapts their behavior based on the current world.
    /// </summary>
    public class SocraticTutorManager : MonoBehaviour
    {
        [System.Serializable]
        public class TutorConfig
        {
            public World world;
            public string characterId; // Convai Character ID
            public string characterName;
            public Vector3 spawnPosition;
            public Vector3 spawnRotation;
            public bool enabledByDefault = true;
        }

        public enum World
        {
            Hub,
            PseGallery,
            ElementRoom,
            Arachnophobia
        }

        [Header("Tutor Configurations")]
        public List<TutorConfig> tutorConfigs = new List<TutorConfig>();

        [Header("Visuals")]
        public GameObject tutorPrefab; // Convai NPC prefab with Animator

        [Header("Audio")]
        public AudioClip thinkingSound; // "Hmm..." sound for pauses
        public float thinkingSoundInterval = 2f;

        private Dictionary<World, ConvaiNPC> activeTutors = new Dictionary<World, ConvaiNPC>();
        private ConvaiNPC currentTutor;
        private float lastThinkingSoundTime;

        // Singleton pattern
        public static SocraticTutorManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Initialize tutors for each world
            foreach (var config in tutorConfigs)
            {
                SpawnTutor(config);
            }
        }

        /// <summary>
        /// Spawns a tutor for the given configuration.
        /// </summary>
        private void SpawnTutor(TutorConfig config)
        {
            if (tutorPrefab == null)
            {
                Debug.LogError("[SocraticTutorManager] Tutor prefab is not assigned!");
                return;
            }

            GameObject tutorGO = Instantiate(tutorPrefab, config.spawnPosition, Quaternion.Euler(config.spawnRotation));
            tutorGO.name = "Tutor-" + config.world;

            ConvaiNPC convaiNPC = tutorGO.GetComponent<ConvaiNPC>();
            if (convaiNPC == null)
            {
                Debug.LogError("[SocraticTutorManager] ConvaiNPC component missing on " + tutorPrefab.name + "!");
                Destroy(tutorGO);
                return;
            }

            // Configure Convai NPC
            convaiNPC.characterName = config.characterName;
            convaiNPC.characterID = config.characterId;
            tutorGO.SetActive(config.enabledByDefault);

            activeTutors[config.world] = convaiNPC;
            Debug.Log("[SocraticTutorManager] Spawned tutor for " + config.world + ": " + config.characterName);
        }

        /// <summary>
        /// Activates the tutor for the specified world and deactivates others.
        /// </summary>
        public void ActivateTutorForWorld(World world)
        {
            // Deactivate all tutors first
            foreach (var tutor in activeTutors.Values)
            {
                if (tutor != null) tutor.gameObject.SetActive(false);
            }

            // Activate the tutor for the current world
            if (activeTutors.TryGetValue(world, out ConvaiNPC tutor))
            {
                tutor.gameObject.SetActive(true);
                currentTutor = tutor;
                Debug.Log("[SocraticTutorManager] Activated tutor for " + world);
            }
            else
            {
                Debug.LogWarning("[SocraticTutorManager] No tutor configured for " + world + "!");
            }
        }

        /// <summary>
        /// Gets the tutor name for a specific world.
        /// </summary>
        public string GetTutorNameForWorld(World world)
        {
            switch (world)
            {
                case World.PseGallery: return "Sokrates den Alchemisten";
                case World.ElementRoom: return "Sokrates den Forscher";
                case World.Arachnophobia: return "Sokrates den Therapeuten";
                case World.Hub: return "Sokrates den Wegweiser";
                default: return "einen Tutoren";
            }
        }

        /// <summary>
        /// Toggles the current tutor's visibility.
        /// </summary>
        public void ToggleTutorVisibility()
        {
            if (currentTutor != null)
            {
                currentTutor.gameObject.SetActive(!currentTutor.gameObject.activeSelf);
            }
        }

        /// <summary>
        /// Sends a context-aware message to the tutor based on the current world and user action.
        /// </summary>
        public void SendContextMessage(World world, string context)
        {
            if (!activeTutors.TryGetValue(world, out ConvaiNPC tutor) || !tutor.isCharacterActive)
                return;

            string message = GetContextMessage(world, context);
            Debug.Log("[SocraticTutorManager] Sending to " + world + " tutor: " + message);
            // Note: In a real implementation, you would use Convai's SendTextInput or similar.
        }

        /// <summary>
        /// Generates a context-aware message for the tutor.
        /// </summary>
        private string GetContextMessage(World world, string context)
        {
            switch (world)
            {
                case World.PseGallery:
                    if (context.Contains("PeriodicTable"))
                        return "Erklaere dem Nutzer, warum die Elemente im Periodensystem so angeordnet sind.";
                    else if (context.Contains("Metall"))
                        return "Frage den Nutzer, was Metalle von Nichtmetallen unterscheidet.";
                    break;
                case World.ElementRoom:
                    if (context.Contains("Diamond"))
                        return "Stell dir vor, du bist ein Kohlenstoff-Atom. Warum bist du in Diamant anders verbunden als in Graphit?";
                    else if (context.Contains("BCC"))
                        return "Warum haben einige Metalle ein kubisch-raumzentriertes Gitter?";
                    break;
                case World.Arachnophobia:
                    if (context.Contains("Spider"))
                        return "Frage den Nutzer, was er ueber die Spinne denkt - und ob das rational ist.";
                    else if (context.Contains("SUDS"))
                        return "Hilf dem Nutzer, seine Angststufe einzuordnen. Was koennte die Angst reduzieren?";
                    break;
                case World.Hub:
                    if (context.Contains("Quiz"))
                        return "Frage den Nutzer, welche Lernstrategie ihm beim Quiz geholfen hat.";
                    break;
            }
            return "Was denkst du ueber das, was du gerade siehst?";
        }

        /// <summary>
        /// Plays a thinking sound at intervals to simulate the tutor pondering.
        /// </summary>
        private void Update()
        {
            if (currentTutor != null && currentTutor.IsCharacterTalking)
            {
                if (Time.time - lastThinkingSoundTime > thinkingSoundInterval && thinkingSound != null)
                {
                    AudioSource.PlayClipAtPoint(thinkingSound, currentTutor.transform.position);
                    lastThinkingSoundTime = Time.time;
                }
            }
        }
    }
}
