using UnityEngine;
using System.Collections.Generic;
using LearningWorlds.Tutors;

namespace LearningWorlds.Tutors
{
    /// <summary>
    /// ScriptableObject to configure Socratic tutors settings.
    /// Used in Unity Editor for easy setup.
    /// </summary>
    [CreateAssetMenu(fileName = "SocraticTutorConfig", menuName = "LearningWorlds/Socratic Tutor Config")]
    public class SocraticTutorConfig : ScriptableObject
    {
        /// <summary>
        /// List of tutor configurations.
        /// </summary>
        public List<SaiaTutorConfig> tutorConfigs = new List<SaiaTutorConfig>();
    }
}
