using UnityEngine;using System.Collections.Generic;

/// <summary>
/// Tutor configuration for the worlds, stored as a ScriptableObject.
/// </summary>
[CreateAssetMenu(menuName = "LearningWorlds/Socratic Tutor Config")]
public class SocraticTutorConfig : ScriptableObject
{
    [SerializeField]
    public List<TutorConfig> Configs = new List<TutorConfig>();

    [System.Serializable]
    public class TutorConfig
    {
        public SocraticTutorManager.World world;
        public string characterId;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
    }
}