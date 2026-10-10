using UnityEngine;using Contavai;using System.Collections.Generic;

/// <summary>
/// Singleton manager for Socratic Tutors in the learning worlds.
/// Uses Convai-based AI tutors for each defined World.
/// </summary>
public class SocraticTutorManager : MonoBehaviour
{
    // Singleton instance
    private static SocraticTutorManager _instance;
    public static SocraticTutorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("SocraticTutorManager");
                _instance = obj.AddComponent<SocraticTutorManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public enum World
    {
        Hub,
        PseGallery,
        ElementRoom,
        Arachnophobia
    }

    public class TutorConfig
    {
        public World world;
        public string characterId;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
    }

    private Dictionary<World, ConvaiNPC> tutors = new Dictionary<World, ConvaiNPC>();

    private TutorConfig[] tutorConfigs;
    private ConvaiNPC currentTutor;

    public ConvaiNPC[] TutorPrefabs;

    private void Start()
    {
        // Initialize tutors if configurations are available
        SocraticTutorConfig config = Resources.Load<SocraticTutorConfig>("SocraticTutorConfig");
        if (config?.Configs != null)
        {
            tutorConfigs = config.Configs.ToArray();
            InitializeTutors();
        }
    }

    /// <summary>
    /// Initialize tutors from their configurations
    /// </summary>
    private void InitializeTutors()
    {
        foreach (TutorConfig config in tutorConfigs)
        {
            SpawnTutor(config);
        }
    }

    /// <summary>
    /// Spawn a tutor at specified configuration
    /// </summary>
    public void SpawnTutor(TutorConfig config)
    {
        if (TutorPrefabs.Length == 0)
        {
            Debug.LogWarning("Tutor Prefabs are not defined in the inspector.");
            return;
        }

        ConvaiNPC tutorPrefab = TutorPrefabs[0]; // Assuming we use single prefab with different character configs
        GameObject tutorObject = Instantiate(tutorPrefab.gameObject, config.spawnPosition, Quaternion.Euler(config.spawnRotation));
        ConvaiNPC tutor = tutorObject.GetComponent<ConvaiNPC>();

        if (tutor == null)
        {
            Debug.LogError("Tutor prefab does not have a ConvaiNPC component!");
            Destroy(tutorObject);
            return;
        }

        tutor.CharacterReference = config.characterId;
        tutor.enabled = false; // Start deactivated
        tutors[config.world] = tutor;
    }

    /// <summary>
    /// Enable the tutor for a specific world and disable others
 /// </summary>    public void ActivateTutorForWorld(World world)
    {
        foreach (World w in System.Enum.GetValues(typeof(World)))
        {
            tutors[w].enabled = (world == w);
            if (w == world)
            {
                currentTutor = tutors[w];
            }
        }
    }

    /// <summary>
    /// Toggle visibility of the current tutor
 /// </summary>
    public void ToggleTutorVisibility()
    {
        if (currentTutor != null)
        {
            currentTutor.enabled = !currentTutor.enabled;
        }
    }

    /// <summary>
    /// Send a context message to a tutor assigned to a specific world
    /// </summary>
    public void SendContextMessage(World world, string context)
    {
        if (tutors.TryGetValue(world, out ConvaiNPC tutor))
        {
            tutor.SendMessage("Prompt", context);
        }
    }
}