using UnityEngine;
using LearningWorlds;

namespace Arachnophobia
{
    public class ExposureController : MonoBehaviour
    {
        public static int CurrentLevel { get; private set; } = 0;
        public static bool ExposureFrozen = false;
        private SpiderFactory spiderFactory;
        private Vector3 exposurePos = new Vector3(4f, 0.5f, 0f);
        private const int MAX_EXPOSURE_LEVEL = 9;
        
        public int CurrentTier => CurrentLevel < 3 ? 0 : CurrentLevel < 6 ? 1 : 2;
        public float CurrentMotion => 0.3f * CurrentLevel;
        
        void Start()
        {
            spiderFactory = FindObjectOfType<SpiderFactory>();
            
            // Register builder for World enum (SocraticTutorManager.World)
            var world = new World { Name = "Arachnophobia" };
            LearningWorldsApp.Builders[world] = Build;
        }
        
        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
                IncrementExposure();
            else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                DecrementExposure();
            else if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftControl))
                ExposureFrozen = !ExposureFrozen;
            else if (Input.GetKeyDown(KeyCode.Backslash) && Input.GetKey(KeyCode.LeftShift))
                LearningWorldsApp.Go(new World { Name = "Hub" });
        }
        
        public GameObject Build()
        {
            UpdateExposure();
            return GameObject.Find("Bedroom");
        }
        
        void UpdateExposure()
        {
            exposurePos.x = Mathf.Lerp(4f, 0.38f, CurrentLevel / (float)MAX_EXPOSURE_LEVEL);
            
            GameObject spider = spiderFactory.Build(CurrentTier, exposurePos);
            if (spider != null)
            {
                if (CurrentTier == 2) spider.AddComponent<SpiderDrift>();
                spider.AddComponent<LightAnimationSelector>();
            }
        }
        
        void IncrementExposure()
        {
            if (ExposureFrozen || CurrentLevel >= MAX_EXPOSURE_LEVEL) return;
            CurrentLevel++;
            UpdateExposure();
        }
        
        void DecrementExposure()
        {
            if (ExposureFrozen || CurrentLevel <= 0) return;
            CurrentLevel--;
            UpdateExposure();
        }
    }

    public class SpiderDrift : MonoBehaviour
    {
        void Update()
        {
            transform.position += new Vector3(
                Random.Range(-0.003f, 0.003f), 
                0,
                Random.Range(-0.003f, 0.003f)
            );
        }
    }
}
