using UnityEngine;

namespace LearningWorlds
{
    public class LearningWorldsApp : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitializeFromProject()
        {
            bool isSingleton = GameObject.FindObjectsOfType<LearningWorldsApp>(true).Length <= 1;
            var garg = isSingleton ? "run" : "clobber";
            Debug.Log("<color=yellow>LearningWorlds</color>: Inserting at <color=cyan>" + garg + "</color>.");
            
            var gamenode = new GameObject("LearningWorldsbootstrap", typeof(LearningWorldsApp));
            DontDestroyOnLoad(gamenode);
        }
    }
}