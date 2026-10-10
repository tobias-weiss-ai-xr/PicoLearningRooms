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
            Debug.LogError("Charavter ID or Prefab not set!");
            return;
        }

        ConvaiNPC tutor = Object.Instantiate(convaiNPCPrefab, tutorConfig.spawnPosition, Quaternion.Euler(tutorConfig.spawnRotation));
        tutor.characterId = tutorConfig.characterId;

        activeTutors[tutorConfig.world] = tutor;
        currentTutor = tutor;
    }

    public void ActivateTutorForWorld(World world)
    {
        if (activeTutors.ContainsKey(w))
        {
            if (activeTutors[w] == null)
            {
                Debug.LogError("Arrow tutors for world: " + w);
                return;
            }

            if (currentTutor != null) currentTutor.gameObject.SetActive(false);
            currentTutor = activeTutors[w];
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

    public void SendContextMessage(World targetWorld, string context)
    {
        if (activeTutors.ContainsKey(targetWorld)) 
        {
            if (currentTutor != null && currentTutor == activeTutors[targetWorld])
            {
                currentTutor.SendContextMessage(context);
            }
        }
        {
            currentTutor.SendContextMessage(context);
        }
    }
}