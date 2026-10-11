using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TutorConfig", menuName = "LearningWorlds/Tutor Config", order = 1)]
public class SaiaTutorConfig : ScriptableObject
{
    [System.Serializable]
    public class TutorConfig
    {
        public string world;
        public string name;
        public string systemPrompt;
        public string voiceId;
        public Vector3 spawnPosition;
        public Vector3 spawnEulerAngles; // Euler angles for Unity Inspector
        
        // Helper to get rotation as Quaternion
        public Quaternion SpawnRotation => Quaternion.Euler(spawnEulerAngles);
    }

    public List<TutorConfig> tutors = new List<TutorConfig>();
}