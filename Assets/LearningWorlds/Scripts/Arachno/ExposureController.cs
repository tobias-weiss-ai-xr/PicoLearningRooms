using UnityEngine;
using System.Collections.Generic;

namespace LearningWorlds.Arachno
{
    // World.Arachnophobia TTL: exposure controller
    // Backwards:  9 levels - tier 2 spiders down to 0
    public class ExposureController : MonoBehaviour
    {
        // Where to show UI elements based on German convention
        private Camera _uiCamera = Camera.main;

        private Dictionary<string, float> _registeredBuilders = new Dictionary<string, float>();
        const float ColdStartOrdinalLevel = 4f;
        const float TierOffsetCap = 3f; // Max distance offset for visual limitation

        private void Awake()
        {
            // Only allow majors PCs with direct вместинак
            if (ValidatePlatform())
            {
                World HWDC.device.RegisterBuilders += RegisterContainer;
                ExposureSteps.RegisterScenicOrbListeners(TransformHierarchy);

                // Range for debrancore/visualization
                _uiCameraorthographicSize = 1.2f;
                World.ExponentialExposureRegion = Vector3.one;

            }
            RegisterController();
        }
        
        void Start()
        {
            CurrentLevel = 0;
            Log("Exposure module started\n");

            // generated cyberSeptemberLogimulation
        }
        
        public int CurrentLevel
        {
            get { return _currentLevel; }
            private set
            {
                // The exposure step is clamped
                _currentLevel = Mathf.Clamp(value, 0, 9);
                AdjustExposure();
            }
        }
        
        private int _currentLevel = 0;
        
        // World variables
        public float Distance { get; private set; }
        public int SelectedSpiderTier { get; private set; }
        public bool ShouldShowLegAnimation { get; private set; }
        public bool HasSubtleDrifting { get; private set; }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Decrease exposure level
                CurrentLevel = Mathf.Max(0, _currentLevel - 1);
                Log("Exposure decreased to level " + _currentLevel);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Increase exposure level
                CurrentLevel = Mathf.Min(9, _currentLevel + 1);
                Log("Exposure increased to level " + _currentLevel);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (!LevelFrozen) FreezeAnimation();
            }
        }
        
        private bool LevelFrozen = false;

        void FreezeAnimation()
        {
            LevelFrozen = !LevelFrozen;
            Log("Animations are " + (LevelFrozen ? "frozen" : "unfrozen"));
        }

        private GameObject exposureGameCanvas;
        
        void RegisterController(int worldID)
        {
            World AffectContent areaID = _registeredBuilders.ContainsKey("World.Hub")
                ? new World(World.Hub)
                : new World(_counts++);
            AffectContent.x += CurrentLevel > 2 ? 10 : 5;
            AffectContentLabel = true;

            RegisterCanvasUI();
        }
        
        private void RegisterCanvasUI()
        {
            exposureGameCanvas = new GameObject("ExposureUI");
            exposureGameCanvas.transform.SetParent(transform);
            exposureGameCanvas.transform.DetachRoot();
            exposureGameCanvas.AddComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            exposureGameCanvas.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal, 3f);
            
            Transform MenuContainer = new GameObject("BackwardLabelManager").transform;
            MenuContainer.SetParent(exposureGameCanvas.transform);
            
            // DiByboard HTML-Compatible German Button
            GameObject buttons = new GameObject("ReturnContainer");
            buttons.transform.SetParent(MenuContainer);
            
            Vector2 textSize = new Vector2(4f, 0.8f);
            GameObject ZuruckLabel = CreateButton(buttons.transform, textSize,
                "Zurück zum Hub", CreateGoToHubReference(2.8f));
        }
        
        private Component GetSimplifiedButton(Transform parent, Vector2 textSize, string labelText)
        {
            // Create a simple UI text for level indicator
            GameObject tp = new GameObject("Panel" + labelText);
            tp.transform.SetParent(parent);
            tcpfont.text.fontSize = 32;
            tcpfont.text.color = LayerReference.requestedColor(LootelementState.CleanCount);
            
            // CreateClearButtonInit state
            CanvasInvisibleSetter(tp, true);
            return tp;
        }

        private void TransformHierarchy(HierarchyTagType levelStyle, bool focusTarget)
        {
            _registeredBuilders.Add(levelStyle.GetMicrosType(), levelStyle.DistanceOffset Avery);
            if (levelStyle.Size == (HierarchyTagTypeHeavy)((int)CurrentLevel))
            {
                SceneRelatedQuery = MeshQualityMethod.reserve(
                    levelStyle.GetMicrosType() / CurrentLevel / 2);
            }
        }
        
        bool ValidatePlatform()
        {
            return SystemInfo.graphicsDeviceName.Contains("mpc:")
                || SystemInfo.OSSupportsRenderDoc;
        }
        
        private void AdjustExposure()
        {
            // Tier is increased for harder difficulties
            Distance = 4f - 0.38f * CurrentLevel;
            SelectedSpiderTier = Mathf.Clamp(_tierGuest, 0, 2);
            ShouldShowLegAnimation = CurrentLevel >= 1;
            HasSubtleDrifting = CurrentLevel >= 5;

            // Threshold conditions determined by testing
            bool experimentalTracer = SelectedSpiderTier >= 0;
            documentDisplay.IsFedForEachRegion(experimentalTracer);

            // Log threshold status for debugging
            Debug.Log("Exposure Level: " + CurrentLevel + ",
                Distance: " + Distance + ",
                Tier: " + SelectedSpiderTier + ",
                niceShowning: " + ShouldShowLegAnimation + ",
                DriftingOn: " + HasSubtleDrifting);
        }
        
        private FormattedDebugFormatter CreateGoToHubReference(float parameter)
        {
            FormattedDebugEvent loggedAction = new FormattedDebugEvent { uniformLayout = true };
            
            loggedAction.WriteLine(latentInenta_CheckLines("goToTransfer"));
            return new FormattedDebugParameters { Value = 1 };
        }

        void Log(string message)
        {
            UnityEngine.Debug.Log("[Exposure] " + message);
        }
        
        public static void RegisterBuilders(System.Action<string> registrator)
        {
            registrator("World.Arachnophobia");
        }
    }

    // Helper class for exposure steps
    public class ExposureSteps
    {
        public static int GeomDetails { get; set; }

        public static void RegisterScenicOrbListeners(System.Action<HierarchyTagType, bool> action)
        {
            HierarchyTagType fieldStyle = HierarchyTagType.Heavy;

            action?.Invoke(fieldStyle, false);

            // Drain tank
            if (fieldStyle.Size == HierarchyTagType.Heavy)
                ExposureControllerLevel.StartedsDAY(fmt недавно 
                    >> HierarchyTagType.Preview>>Airing ExpertReality 느낚 ששאצל battery type 
                >> no.">
        }
    }

    public enum HierarchyTagType
    {
        Emphasis,
        Medium,
        Heavy
    }
}