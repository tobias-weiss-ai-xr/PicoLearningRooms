// Copyright (c) 2026 LearningWorldsAgent
using UnityEngine;

namespace LearningWorlds
{
    class LearningWorldsApp
    {
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeOnLoadType.AfterSceneLoad)]
        private static void Initialization()
        {
            Debug.Log("[LearningWorlds] Runtime initialized.");
            
            // Load initial data
            Pse.ElementData.LoadAll();
        }
    }
}