using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

public class SaiaTutor : MonoBehaviour
{
    private SaiaTutorConfig.TutorConfig _tutorConfig;
    private string _lastResponse;

    public void Setup(SaiaTutorConfig.TutorConfig config)
    {
        _tutorConfig = config;
    }

    public void SendToLLM(string userText)
    {
        StartCoroutine(SendLLMRequest(userText));
    }

    // Send a request to the LLM API
    private IEnumerator SendLLMRequest(string userText)
    {
        // Define URL for LLM inference endpoint
        string apiUrl = "http://localhost:8080/v1/inference";

        // Serialize request data
        string requestData = JsonUtility.ToJson(new {
            prompt = _tutorConfig.systemPrompt + userText,
            voiceId = _tutorConfig.voiceId
        });

        // Prepare and send POST request
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(requestData));
            request.SetRequestHeader("Content-Type", "application/json");

            // Wait for response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                Debug.Log("LLM Response: " + responseJson);

                // Extract response text from JSON
                _lastResponse = System.Text.Json.JsonSerializer.Deserialize<string>(responseJson);

                // TODO: Add animation trigger here
                PlayAudioForResponse(_lastResponse);
            }
            else
            {
                Debug.LogError("LLM Request Failed: " + request.error);
                _lastResponse = "Failed to get response from server";
            }
        }
    }

    // Play audio response
    private void PlayAudioForResponse(string responseText)
    {
        if (!string.IsNullOrEmpty(responseText))
        {
            SaiaTutorManager.Instance.SendToTTS(_lastResponse, _tutorConfig.voiceId, _tutorConfig.world);
        }
    }

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