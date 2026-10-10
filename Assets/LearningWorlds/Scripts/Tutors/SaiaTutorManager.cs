using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaiaTutorManager : MonoBehaviour
{
    private static SaiaTutorManager _instance;
    public static SaiaTutorManager Instance { get { return _instance; } }

    private SaiaTutorConfig _tutorConfig;
    private Dictionary<string, SaiaTutor> _activeTutors = new Dictionary<string, SaiaTutor>();

    // Reference to current microphone usage
    private AudioClip _lastSentAudio;

    // Load config
    public void Initialize(SaiaTutorConfig config)
    {
        _tutorConfig = config;
    }

    // Singleton pattern + Don'tDestroyOnLoad
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Initialize a tutor with the specified config
    public SaiaTutor SpawnTutor(SaiaTutorConfig.TutorConfig tutorConfig, string world)
    {
        if (string.IsNullOrEmpty(tutorConfig.name))
        {
            Debug.LogError("Tutor name is missing");
            return null;
        }

        // Find or create scene-specific TutorAvatar prefab path
        string avatarPath = "Prefabs/LWW Tutorial Avatars/";
        GameObject avatarPrefab = Resources.Load<GameObject>(avatarPath + tutorConfig.name);

        if (avatarPrefab == null)
        {
            Debug.LogError($"Tutor avatar prefab not found: {avatarPath + tutorConfig.name}");
            return null;
        }

        // Instantiate TutorAvatar with specified config
        GameObject avatarInstance = Instantiate(avatarPrefab, 
            tutorConfig.spawnPosition, 
            tutorConfig.spawnRotation);

        if (avatarInstance.GetComponent<TutorAvatar>() == null)
        {
            Debug.LogError("TutorAvatar component not found");
            Destroy(avatarInstance);
            return null;
        }

        // Add SaiaTutor component if not present
        SaiaTutor saiaTutor = avatarInstance.GetComponent<SaiaTutor>();
        if (saiaTutor == null)
        {
            saiaTutor = avatarInstance.AddComponent<SaiaTutor>();
        }

        // Initialize tutor config
        saiaTutor.Setup(tutorConfig);

        // Enable based on world
        _activeTutors[tutorConfig.world] = saiaTutor;

        return saiaTutor;
    }

    // Activate the tutor for specific world only
    public void ActivateTutorForWorld(string world)
    {
        SaiaTutor tutor;
        if (_activeTutors.TryGetValue(world, out tutor))
        {
            tutor.gameObject.SetActive(true);
            Debug.Log($"Activated tutor for {world}");
        }
        else
        {
            Debug.LogWarning($"No tutor configured for world: {world}");
        }
    }

    // Get tutor by world for SocraticManager
    public SaiaTutor GetTutorForWorld(string world)
    {
        foreach (var keyValuePair in _activeTutors)
        {
            if (keyValuePair.Key == world && keyValuePair.Value != null)
            {
                return keyValuePair.Value;
            }
        }
        return null;
    }
    {
        SaiaTutor tutor;
        if (_activeTutors.TryGetValue(world, out tutor))
        {
            tutor.gameObject.SetActive(true);
            Debug.Log($"Activated tutor for {world}");
        }
        else
        {
            Debug.LogWarning($"No tutor configured for world: {world}");
        }
    }

    // STT - Speech to Text methods
    public void StartRecording()
    {
#if UNITY_STANDALONE
        Microphone.End(null); // Clean up any previous recording
        string device = Microphone.devices.Length > 0 ? Microphone.devices[0].toString() : "";
        Debug.Log("Recording device: " + device);
        if (!string.IsNullOrEmpty(device))
        {
            Microphone.Start(device, true, 1, 44100);
            Debug.Log("Recording begun");
        }
        else
        {
            Debug.LogError("No microphone devices available");
        }

handler: #SendToSTT helper method to process microphone data.
#endif
    }

    public void SendToSTT(byte[] audioData, string world)
    {
        // TODO: Implement HTTP POST to Saia/Whisper API for transcription
        // Example URL: http://localhost:8080/v1/audio/transcriptions
        Debug.Log($"Sending audio to Whisper API for {world}");
        // In real implementation, would make POST request here
    }

    // TTS & Common Utility methods
    public AudioClip SendToTTS(string text, string voiceId, string world)
    {
        // TODO: Implement HTTP POST to Saia/TTS API for text-to-speech
        // Example URL: /v1/text2speech
        Debug.Log($"Sending text to TTS API for {world} with voice: {voiceId}");
        return null; // Return AudioClip after implementing real API call
    }

    // Access config for a tutor
    public SaiaTutorConfig.TutorConfig GetTutorConfig(string world, string tutorName)
    {
        if (_tutorConfig == null || _tutorConfig.tutors == null)
        {
            Debug.LogError("Tutor config not initialized");
            return null;
        }

        foreach (var tutor in _tutorConfig.tutors)
        {
            if (tutor.world == world && tutor.name == tutorName)
            {
                return tutor;
            }
        }
        return null;
    }
}