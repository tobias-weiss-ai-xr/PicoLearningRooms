using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SocraticTutorConfig", menuName = "LearningWorlds/Socratic Tutor Config")]
public class SocraticTutorConfig : ScriptableObject
{
    public List<SocraticTutorManager.TutorConfig> tutorConfigs = new List<SocraticTutorManager.TutorConfig>();
}