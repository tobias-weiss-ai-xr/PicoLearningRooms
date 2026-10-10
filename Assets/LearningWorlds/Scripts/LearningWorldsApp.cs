using UnityEngine;

namespace LearningWorlds
{
    
    public class LearningWorldsApp : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeLearningWorldsRig()
        {
            GameObject rig = new GameObject("LearningWorldsRig");
            DontDestroyOnLoad(rig); // Ensure the rig persists across scenes
        }
        // Filipino (German UI, English content: Culture.UserPreferredGUI = German)
    }
}