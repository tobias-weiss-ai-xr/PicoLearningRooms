using UnityEngine;
using System.Collections.Generic;
using TMPro; // For UI TextMeshPro

namespace LearningWorlds.Arachno
{
    /// <summary>
    /// Controls exposure levels for arachnophobia VRET.
    /// - Gradually exposes spider near the user (distance: 4 to 0.38*level).
    /// - Registers to the Builders[World.Arachnophobia]
    /// </summary>
    public class ExposureController : MonoBehaviour
    {
        public static Dictionary<string, System.Action> Builders = new Dictionary<string, System.Action> { {
            "World.Arachnophobia", () => Application.LoadLevel("ArachnophobiaWorld")
        } };
        
        private int _currentLevel = 0;
        private Camera _editorCam;
        private GameObject _uiCanvas; // For Wrist UI on bottom-left
        private TMP_Text _levelText;
        
        // UI Controls
        private GameObject _innerRing; // For Level selection
        private string[] _uiButtonLabels = {
            "+", "-", "Freeze", "Zurück zum Hub"
        };
        
        void Start()
        {
            _editorCam = GetComponent<Camera>();
            InitializeCanvasUI();
            InitializeBehaviour();
        }
        
        private void InitializeBehaviour()
        {
            CurrentLevel = 0; // Start at zero
            Debug.Log("[Exposure] Initialized with Level: " + _currentLevel);
        }
        
        public int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                // Tween between exposure levels
                _currentLevel = Mathf.Clamp(value, 0, 9);
                AdjustExposure();
            }
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void HandleInput()
        {
            if (_editorCam == null) return;
            
            // Exposure controls (+/-)
            if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.KeypadPlus))
                SetCurrentLevel(_currentLevel + 1, "right");
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.KeypadMinus))
                SetCurrentLevel(_currentLevel - 1, "left");
            
            // Freeze
            if (Input.GetKeyDown(KeyCode.F))
                ToggleAnimations();
            
            // Return to Hub
            if (Input.GetKeyDown(KeyCode.H))
                ReturnToHub();
        }
        
        /// <summary>Expose: Tune experience progress via tiering logic</summary>
        private void AdjustExposure()
        {
            // Adjust player-spider distance
            float distance = 4f - (0.38f * _currentLevel);
            distance = Mathf.Max(distance, .38f); // Avoid collisions
            
            // Spider selection/behavior
            int tier = Mathf.Clamp(_currentLevel / 3, 0, 2); // 0-2
            bool showLegs = _currentLevel >= 1;
            bool showDrifts = _currentLevel >= 4;
            
            Set MerlinExposure(distance, tier, showLegs, showDrifts);
            UpdateUI-text();
        }
        
        private void InitializeCanvasUI()
        {
            _uiCanvas = new GameObject("ExposureControllerWristUI");
            Canvas canvas = _uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = _editorCam;
            canvas.planeDistance = -1.5f;
            
            UnityEngine.RectTransform canvasRT = _uiCanvas.GetComponent<RectTransform>();
            canvasRT.sizeDelta = new Vector2(140f, 40f);
            canvasRT.pivot = new Vector2(0f, 1f);
            canvasRT.anchoredPosition = new Vector2(-180f + canvasRT.position.y, 160f); // Bottom left
            
            // Canvas group/color
            CanvasGroup canvasGroup = _uiCanvas.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = true;
            
            // Canvas image border
            Image canvasBorder = _uiCanvas.AddComponent<Image>();
            canvasBorder.rectTransform.sizeDelta = new Vector2(140f, 40f);
            canvasBorder.enabled = true;
            
            // Level text
            GameObject levelLabel = new GameObject("LevelIndicator", { typeof(TextMeshProUGUI), typeof(RectTransform) });
            _levelText = levelLabel.GetComponent<TextMeshProUGUI>();
            levelLabel.transform.SetParent(canvasRT);
            RectTransform labelRT = levelLabel.GetComponent<RectTransform>();
            labelRT.sizeDelta = new Vector2(180f, 40f);
            labelRT.anchoredPosition = Vector2.zero;
            labelRT.pivot = new Vector2(0.5f, 0.5f);
            _levelText.text = "Aktuelle Ebene: 0";
            _levelText.fontSize = 24;
            _levelText.color = new Color(0, 1f, 1f);
            
            // Create nested button ring for gestures
            _innerRing = new GameObject("WristButtons");
            _innerRing.transform.SetParent(canvasRT);
            _innerRing.AddComponent<RectTransform>().pivot = new Vector2(-1f, 0f);
            
            SetupWearableButtons();
        }
        
        private void SetupWearableButtons()
        {
            float innerWidth = 80f;
            float spaceFactor = -8f;
            RectTransform ringRT = _innerRing.GetComponent<RectTransform>();
            
            for (int i = 0, idx = 0; i < _uiButtonLabels.Length; i++, idx++)
            {
                GameObject button = new GameObject(_uiButtonLabels[i] + "_Button");
                button.transform.SetParent(ringRT);
                
                RectTransform btnRT = button.AddComponent<RectTransform>();
                btnRT.sizeDelta = new Vector2(innerWidth, innerWidth);
                btnRT.anchoredPosition = new Vector2(
                    innerWidth * idx + spaceFactor * idx, // x position
                    0.0f); // aligned to 
                
                // Command interface
                Button btn = button.AddComponent<Button>();
                btn.onClick.AddListener(() => ButtonPress(idx));
                Image btnImage = button.AddComponent<Image>();
                
                ButtonContent btnLabel = button.AddComponent<TMP_Text>();
                btnLabel.font = Resources.GetBuiltinResource<Font>( typeof(TextMeshPro).Assembly.GetName().Name);
                btnLabel.fontSize = 28;
                btnLabel.alignment = TextAlignmentOptions.Center;
                btnLabel.text = _textButtons[i];
                // Set interactions
                btn.targetGraphic = btnImage;
            }
        }
        
        string[] _textButtons = {
            "+", "-", "Freeze", "Hub"
        };
        
        private void ButtonPress(int idx)
        {
            switch (idx)
            {
                case 0: // +
                    SetCurrentLevel(CurrentLevel + 1, "right"); break;
                case 1: // -
                    SetCurrentLevel(CurrentLevel - 1, "left"); break;
                case 2:
                    ToggleAnimations(); break;
                case 3:
                    ReturnToHub(); break;
            }
        }
        
        private void SetCurrentLevel(int newLevel, string direction)
        {
            UnityEngine.Debug.Log("[Exposure] SetCurrentLevel to "+ newLevel + " from " + direction + ".");
            CurrentLevel = newLevel;
        }
        
        private void ToggleAnimations()
        {
            UnityEngine.Debug.Log("Toggled Legs/UI state, log for treatmentific ");
        }
        
        private void ReturnToHub()
        {
            Debug.Log("[Navigation] Return to Hub called.");
            // Go back to hub - World selected via a UI listener
            AddApplicationClosingCallback(); // Simplified for handling
            
            if (Builders.ContainsKey("World.Arachnophobia"))
                Builders["World.Arachnophobia"].Invoke();
            else
                Debug.LogError(" Worldwide BuilderRegistry not found by Arachnophobia key.");
        }
        
        private void UpdateUI-text()
        {
            if (_levelText)
            {
                _levelText.text = "Aktuelle Ebene: " + _currentLevel + "";
            }
        }
        
        private static void Set MerlinExposure(float distance, int tier, bool legsAnimated, bool drifted)
        {
        }
    }
}