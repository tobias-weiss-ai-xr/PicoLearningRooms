using UnityEngine;
using System.Collections.Generic;
using Convai.Wrapper;

public class SocraticTutorManager : MonoBehaviour
{
    public enum World
    {
        Hub,
        PseGallery,
        ElementRoom,
        Arachnophobia
    }

    [System.Serializable]
    public class TutorConfig
    {
        public World world;
        public string characterId;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
    }

    private static SocraticTutorManager _instance;
    public static SocraticTutorManager Instance { get { return _instance; } }

    public ConvaiNPC convaiNPCPrefab;
    private Dictionary<World, ConvaiNPC> activeTutors = new Dictionary<World, ConvaiNPC>
    {
        { World.Hub, null},
        { World.PseGallery, null},
        { World.ElementRoom, null},
        { World.Arachnophobia, null }
    };

    private ConvaiNPC currentTutor;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnTutor(TutorConfig tutorConfig)
    {
        if (currentTutor != null)
        {
            Object.Destroy(currentTutor.gameObject);
        }

        if (string.IsNullOrEmpty(tutorConfig.characterId) || convaiNPCPrefab == null)
        {
            Debug.LogError("Character ID or Prefab not set!")
            return;
        }

        ConvaiNPC tutor = Object.Instantiate(convaiNPCPrefab, tutorConfig.spawnPosition, Quaternion.Euler(tutorConfig.spawnRotation));
        tutor.characterId = tutorConfig.characterId;

        activeTutors[tutorConfig.world] = tutor;
        currentTutor = tutor;
    }

    public void ActivateTutorForWorld(World world)
    {
        if (!activeTutors.ContainsKey(world))
        {
            Debug.LogError("No tutors for world: " + world);
            return;
        }

        if (activeTutors[world] == null)
        {
            Debug.LogError("Tutor not assigned for world: " + world);
            return;
        }

        if (currentTutor != null) currentTutor.gameObject.SetActive(false);
        currentTutor = activeTutors[world];
        currentTutor.gameObject.SetActive(true);
    }
    }

    public void ToggleTutorVisibility()
    {
        if (currentTutor != null)
        {
            currentTutor.gameObject.SetActive(!currentTutor.gameObject.activeSelf);
        }
    }

    public void SendContextMessage(string context)
    {
        if (currentTutor != null)
        {
            currentTutor.SendContextMessage(context);
        }
    }
}