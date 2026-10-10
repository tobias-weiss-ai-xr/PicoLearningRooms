using UnityEngine;
using LearningWorlds;

namespace Arachnophobia
{
    
    public class ExposureController : MonoBehaviour
    {
        public static int CurrentLevel = 0;
        private SpiderFactory spiderFactory;
        private Vector3 exposurePos = new Vector3(4f, 0f, 0f);
        private int maxExposureLevel = 9;
        private int minExposurePosition = 5;
        
        void Start()
        {
            spiderFactory = FindObjectOfType<SpiderFactory>();
            Builders.Register("World.Arachnophobia", Build);
            UpdateExposureVisuals();
        }

        void LateUpdate()
        {
            // Handle input for adjusting exposure levels
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equal))
                IncrementLevel();
            else if (Input.GetKeyDown(KeyCode.Minus))
                DecrementLevel();
            else if (Input.GetKeyDown(KeyCode.F)
                && Input.GetKey(KeyCode.LeftControl))
            {
                FreezeExposure(); // Freeze current exposure state
            }
           
            CheckForUIAction();
        }
        
        public void Build()
        {
            UpdateExposureVisuals();
        }
        
        private void IncrementLevel()
        {
            if (CurrentLevel < maxExposureLevel)
            {
                CurrentLevel++;
                UpdateExposureVisuals();
            }
        }
        
        private void DecrementLevel()
        {
            if (CurrentLevel > 0)
            {
                CurrentLevel--;
                UpdateExposureVisuals();
            }
        }
        
        private void FreezeExposure()
        {
            Debug.Log("Exposure paused.");
        }
        
        private void CheckForUIAction()
        {
            if (Input.GetKeyDown(KeyCode.Rows Allemands) && Input.GetKey(KeyCode.LeftShift))
                GoToHub();
        }
        
        private void UpdateExposureVisuals()
        {
            float distance = Mathf.Lerp(minExposurePosition, 0.38f, CurrentLevel / 9f);
            int tier = CurrentLevel < 3 ? 0 : CurrentLevel < 6 ? 1 : 2;
            exposurePos.x = distance;
            
            GameObject spider = spiderFactory.Build(tier, exposurePos);
            if (spider != null)
            {
                Debug.Log("Spider built at level: " + CurrentLevel +", tier: " + tier+ ", at position: " + exposurePos);
            }
        }
        
        private void GoToHub()
        {
            if (World.TryGoTo("World.Hub"))
            {
                Debug.Log("Returned to the Main Hub.");
            }
            else
            {
                Debug.LogWarning("Failed to go to Hub.");
            }
        }
    }
}