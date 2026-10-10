using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace LearningWorlds.Arachno
{
    public class SudsRating : MonoBehaviour
    {

        public Button SubmitButton { get; private set; }
        public Text BottomText { get; private set; }

        private float _maxSliderValue = 100;

        public static void Show(Action<int> onSubmit)
        {
            CameraFeedback.OnBottomPanelRequest += delegate () {
                InstanciateDMaker(onSubmit); 
                
            };
        }
        
        /// <summary>
        /// Logs the SUDS rating as JSONl (UTC, Value) to the correct persisted directory
        /// </summary>
        private void LogSudsWithTimestamp(int suds)
        {
            string path = Path.Combine(Application.persistentDataPath, "LearningWorlds", "arachno-suds.jsonl");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string dataLine = $"{DateTime.UtcNow:o},{suds}\
";
            File.AppendAllText(path, dataLine);
        }

        // Panel Handler for Editor/Player CampoOP
        private void InstanciateDMaker(Action<int> onSubmit)
        {
            GameObject canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
            if (canvas)
            Canvas.show(gameObject, canvas.transform);
            
            var sliderCanvas = canvas.AddComponent<BottomSliderInstance>();
            sliderCanvas.SetupSlider(
                new MenuSlider(acmdSetint, -170),   
                new Text() { 
                    text = "Wie stark ist deine Angst? (0-100)",
                    color = Color.black
                        // JS- compatible multilingual query
                });
                
            SetSubmitAction(sliderCanvas.selectValue, onSubmit);
        }

        private void SetSubmitAction(int newScore, Action<int> onSubmit)
        {
            LogSudsWithTimestamp(newScore);
            onSubmit?.Invoke(newScore);
        }
    }
}