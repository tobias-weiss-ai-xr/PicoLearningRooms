using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaiaTutorManager : MonoBehaviour
{
    private static SaiaTutorManager _instance;
    public static SaiaTutorManager Instance { get { return _instance; } }

    private SaiaTutorConfig _tutorConfig;
    private Dictionary<string, SaiaTutor> _activeTutors = new Dictionary<string, SaiaTutor>();
    private SaiaTutor _currentTutor;

    // Reference to current microphone usage
    private AudioClip _lastSentAudio;
    private bool _isRecording = false;

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
            Debug.LogError("[SaiaTutorManager] Tutor name is missing");
            return null;
        }

        // Find or create scene-specific TutorAvatar prefab path
        string avatarPath = "Prefabs/LWW Tutorial Avatars/";
        GameObject avatarPrefab = Resources.Load<GameObject>(avatarPath + tutorConfig.name);

        if (avatarPrefab == null)
        {
            Debug.LogError("[SaiaTutorManager] Tutor avatar prefab not found: " + avatarPath + tutorConfig.name);
            return null;
        }

        // Instantiate TutorAvatar with specified config
        GameObject avatarInstance = Instantiate(avatarPrefab, 
            tutorConfig.spawnPosition, 
            tutorConfig.spawnRotation);

        if (avatarInstance.GetComponent<TutorAvatar>() == null)
        {
            Debug.LogError("[SaiaTutorManager] TutorAvatar component not found");
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
        avatarInstance.SetActive(false); // Deactivate by default

        // Register in active tutors
        _activeTutors[world] = saiaTutor;

        Debug.Log("[SaiaTutorManager] Spawned tutor: " + tutorConfig.name + " for " + world);
        return saiaTutor;
    }

    // Spawn all tutors from config
    public void SpawnAllTutors()
    {
        if (_tutorConfig == null || _tutorConfig.tutors == null)
        {
            Debug.LogError("[SaiaTutorManager] Tutor config not initialized");
            return;
        }

        foreach (var tc in _tutorConfig.tutors)
        {
            SpawnTutor(tc, tc.world);
        }
    }

    // Activate the tutor for specific world only
    public void ActivateTutorForWorld(string world)
    {
        // Deactivate current tutor first
        if (_currentTutor != null)
        {
            _currentTutor.gameObject.SetActive(false);
        }

        SaiaTutor tutor;
        if (_activeTutors.TryGetValue(world, out tutor))
        {
            tutor.gameObject.SetActive(true);
            _currentTutor = tutor;
            Debug.Log("[SaiaTutorManager] Activated tutor for " + world);
        }
        else
        {
            Debug.LogWarning("[SaiaTutorManager] No tutor configured for: " + world);
        }
    }

    // Get tutor by world for SocraticManager
    public SaiaTutor GetTutorForWorld(string world)
    {
        SaiaTutor tutor;
        if (_activeTutors.TryGetValue(world, out tutor))
        {
            return tutor;
        }
        return null;
    }

    // STT - Speech to Text methods
    public void StartRecording()
    {
        if (_isRecording) return;
        _isRecording = true;

        string device = Microphone.devices.Length > 0 ? Microphone.devices[0] : null;
        Debug.Log("[SaiaTutorManager] Recording device: " + device);
        if (!string.IsNullOrEmpty(device))
        {
            Microphone.End(null); // Clean up any previous recording
            _lastSentAudio = Microphone.Start(device, true, 5, 44100);
            Debug.Log("[SaiaTutorManager] Recording started");
        }
        else
        {
            Debug.LogError("[SaiaTutorManager] No microphone devices available");
            _isRecording = false;
        }
    }

    public void StopRecording()
    {
        if (!_isRecording) return;
        _isRecording = false;
        
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            Debug.Log("[SaiaTutorManager] Recording stopped");
        }
    }

    public void SendToSTT(byte[] audioData, string world)
    {
        // TODO: Implement HTTP POST to Saia/Whisper API for transcription
        // Example URL: http://localhost:8080/v1/audio/transcriptions
        Debug.Log("[SaiaTutorManager] Sending audio to Whisper API for " + world);
        // In real implementation, would make POST request here
    }

    // TTS - Text to Speech
    public AudioClip SendToTTS(string text, string voiceId, string world)
    {
        // TODO: Implement HTTP POST to Saia/TTS API for text-to-speech
        // Example URL: /v1/text2speech
        Debug.Log("[SaiaTutorManager] Sending text to TTS API for " + world + " with voice: " + voiceId);
        return null; // Return AudioClip after implementing real API call
    }

    // Access config for a tutor
    public SaiaTutorConfig.TutorConfig GetTutorConfig(string world, string tutorName)
    {
        if (_tutorConfig == null || _tutorConfig.tutors == null)
        {
            Debug.LogError("[SaiaTutorManager] Tutor config not initialized");
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
