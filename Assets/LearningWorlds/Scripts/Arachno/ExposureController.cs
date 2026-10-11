using UnityEngine;
using LearningWorlds;

namespace LearningWorlds.Arachnophobia
{
    /// <summary>
    /// Manages exposure therapy levels for arachnophobia treatment.
    /// Controls spider instantiation and difficulty progression.
    /// </summary>
    public class ExposureController : MonoBehaviour
    {
        public static int CurrentLevel { get; private set; } = 0;
        public static bool ExposureFrozen = false;

        private SpiderFactory spiderFactory;
        private Vector3 exposurePos = new Vector3(4f, 0.5f, 0f);
        private const int MaxExposureLevel = 9;

        /// <summary>
        /// Gets the current tier (0, 1, or 2) based on level.
        /// </summary>
        public int CurrentTier => CurrentLevel < 3 ? 0 : CurrentLevel < 6 ? 1 : 2;

        /// <summary>
        /// Gets the current motion intensity based on level.
        /// </summary>
        public float CurrentMotion => 0.3f * CurrentLevel;

        private void Start()
        {
            spiderFactory = FindObjectOfType<SpiderFactory>();
            
            // Register builder with LearningWorldsApp
            BuildingBuilders.Register(World.Arachnophobia, Build);
        }

        private void LateUpdate()
        {
            // Keyboard controls for exposure level (dev/testing)
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
                IncrementExposure();
            else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                DecrementExposure();
            else if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftControl))
                ExposureFrozen = !ExposureFrozen;
            else if (Input.GetKeyDown(KeyCode.Backslash) && Input.GetKey(KeyCode.LeftShift))
                LearningWorldsApp.Go(World.Hub);
        }

        /// <summary>
        /// Builds the arachnophobia exposure world.
        /// </summary>
        /// <returns>The root GameObject of the scene.</returns>
        public static GameObject Build()
        {
            // Create or find the bedroom
            GameObject bedroom = GameObject.Find("Bedroom");
            if (bedroom == null)
            {
                bedroom = new GameObject("Bedroom");
                bedroom.tag = "Bedroom";
            }

            // Setup initial exposure
            ExposureController controller = bedroom.GetComponent<ExposureController>();
            if (controller == null)
            {
                controller = bedroom.AddComponent<ExposureController>();
            }

            controller.UpdateExposure();
            return bedroom;
        }

        /// <summary>
        /// Updates the spider exposure based on current level.
        /// </summary>
        private void UpdateExposure()
        {
            exposurePos.x = Mathf.Lerp(4f, 0.38f, CurrentLevel / (float)MaxExposureLevel);

            if (spiderFactory != null)
            {
                GameObject spider = spiderFactory.Build(CurrentTier, exposurePos);
                if (spider != null)
                {
                    if (CurrentTier == 2)
                        spider.AddComponent<SpiderDrift>();
                    
                    // Try to add light animation if available
                    var lightAnim = spider.AddComponent<LightAnimationSelector>();
                }
            }
        }

        /// <summary>
        /// Increases exposure level.
        /// </summary>
        public void IncrementExposure()
        {
            if (ExposureFrozen || CurrentLevel >= MaxExposureLevel) return;
            CurrentLevel++;
            UpdateExposure();
            Debug.Log("Exposure increased to level: " + CurrentLevel);
        }

        /// <summary>
        /// Decreases exposure level.
        /// </summary>
        public void DecrementExposure()
        {
            if (ExposureFrozen || CurrentLevel <= 0) return;
            CurrentLevel--;
            UpdateExposure();
            Debug.Log("Exposure decreased to level: " + CurrentLevel);
        }
    }

    /// <summary>
    /// Adds subtle random drift movement to spiders at high tiers.
    /// </summary>
    public class SpiderDrift : MonoBehaviour
    {
        private void Update()
        {
            transform.position += new Vector3(
                Random.Range(-0.003f, 0.003f),
                0,
                Random.Range(-0.003f, 0.003f)
            );
        }
    }
}
