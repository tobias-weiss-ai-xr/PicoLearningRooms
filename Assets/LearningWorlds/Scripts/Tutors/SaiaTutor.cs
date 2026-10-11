using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

namespace LearningWorlds.Tutors
{
    /// <summary>
    /// Handles LLM communication for a Saia-based AI tutor.
    /// Communicates with local Saia STT/TTS/LLM servers.
    /// </summary>
    public class SaiaTutor : MonoBehaviour
    {
        private SaiaTutorConfig.TutorConfig _tutorConfig;
        private string _lastResponse;

        // LLM API Configuration
        private string llmApiUrl = "http://localhost:11434/api/chat"; // Ollama
        private string sttApiUrl = "http://localhost:8080/v1/audio/transcriptions"; // Whisper
        private string ttsApiUrl = "http://localhost:8081/tts"; // Coqui

        /// <summary>
        /// Sets up the tutor with its configuration.
        /// </summary>
        public void Setup(SaiaTutorConfig.TutorConfig config)
        {
            _tutorConfig = config;
        }

        /// <summary>
        /// Sends user text to the LLM and processes the response.
        /// </summary>
        public void SendToLLM(string userText)
        {
            if (_tutorConfig == null)
            {
                Debug.LogError("[SaiaTutor] TutorConfig not set! Call Setup() first.");
                return;
            }
            StartCoroutine(SendLLMRequest(userText));
        }

        /// <summary>
        /// Sends a request to the LLM API.
        /// </summary>
        private IEnumerator SendLLMRequest(string userText)
        {
            string prompt = _tutorConfig.systemPrompt + "\n\nUser: " + userText;

            WWWForm form = new WWWForm();
            form.AddField("model", "mistral");
            form.AddField("prompt", prompt);
            form.AddField("stream", "false");

            using (UnityWebRequest request = UnityWebRequest.Post(llmApiUrl, form))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    Debug.Log("[SaiaTutor] LLM Response: " + responseJson);

                    _lastResponse = responseJson;
                    PlayAudioForResponse(_lastResponse);
                }
                else
                {
                    Debug.LogError("[SaiaTutor] LLM Request Failed: " + request.error);
                    _lastResponse = "Failed to get response from server";
                }
            }
        }

        /// <summary>
        /// Processes the response text and triggers audio playback.
        /// </summary>
        private void PlayAudioForResponse(string responseText)
        {
            if (!string.IsNullOrEmpty(responseText))
            {
                SaiaTutorManager.Instance.SendToTTS(_lastResponse, _tutorConfig.voiceId, _tutorConfig.world);
            }
        }

        /// <summary>
        /// Plays an audio clip with animation.
        /// </summary>
        public void PlayAudio(AudioClip clip)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Talk");
            }

            TutorAvatar tutorAvatar = GetComponent<TutorAvatar>();
            if (tutorAvatar != null)
            {
                tutorAvatar.PlayAudio(clip);
            }
        }
    }
}
