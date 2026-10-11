using UnityEngine;

/// <summary>
/// Creates pedestals for element displays with feedback visuals.
/// </summary>
public static class PedestalCreator
{
    private static Texture2D correctTexture;
    private static Texture2D incorrectTexture;

    /// <summary>
    /// Creates a pedestal platform for an element or object.
    /// </summary>
    /// <param name="parent">Parent transform.</param>
    /// <param name="name">Name for the pedestal.</param>
    /// <returns>The created pedestal GameObject.</returns>
    public static GameObject CreatePedestal(Transform parent, string name)
    {
        GameObject pedestalBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pedestalBase.name = name + "_Pedestal";
        pedestalBase.transform.SetParent(parent);

        pedestalBase.transform.localPosition = new Vector3(0f, -0.5f, 0f);
        pedestalBase.transform.localScale = new Vector3(1.8f, 0.4f, 1.8f);

        Material standMaterial = new Material(Shader.Find("Standard"));
        standMaterial.color = new Color(0.2f, 0.55f, 0.18f); // Dark green
        pedestalBase.GetComponent<Renderer>().material = standMaterial;

        // Add trigger collider for interaction
        BoxCollider interactTrigger = pedestalBase.AddComponent<BoxCollider>();
        interactTrigger.center = Vector3.zero;
        interactTrigger.size = new Vector3(1.5f, 1.5f, 1.5f);
        interactTrigger.isTrigger = true;

        return pedestalBase;
    }

    /// <summary>
    /// Adds a feedback panel to indicate correct/incorrect answers.
    /// </summary>
    /// <param name="pedestalObject">The pedestal to add feedback to.</param>
    /// <param name="isCorrect">Whether the answer was correct.</param>
    /// <param name="message">Optional message to display.</param>
    public static void AddFeedbackPanel(GameObject pedestalObject, bool isCorrect, string message = "")
    {
        GameObject feedbackPanel = new GameObject("FeedbackPanel");
        feedbackPanel.transform.SetParent(pedestalObject.transform);
        feedbackPanel.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        feedbackPanel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        // Create a plane for the feedback display
        GameObject panelPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        panelPlane.transform.SetParent(feedbackPanel.transform);
        panelPlane.transform.localScale = new Vector3(0.8f, 0.5f, 1f);
        panelPlane.transform.localPosition = Vector3.zero;

        // Set material color based on correctness
        Color panelColor = isCorrect ? new Color(0f, 1f, 0f, 0.8f) : new Color(1f, 0f, 0f, 0.8f);
        Material panelMaterial = new Material(Shader.Find("Standard"));
        panelMaterial.color = panelColor;
        panelMaterial.SetFloat("_Mode", 3); // Transparent
        panelMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        panelMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        panelPlane.GetComponent<Renderer>().material = panelMaterial;

        UnityEngine.Object.DestroyImmediate(panelPlane.GetComponent<BoxCollider>());

        // Add pedestreal feedback component
        PedestalFeedback feedback = pedestalObject.AddComponent<PedestalFeedback>();
        if (!string.IsNullOrEmpty(message))
        {
            if (isCorrect)
                feedback.CorrectFeedback(message);
            else
                feedback.IncorrectFeedback(message);
        }
    }
}

/// <summary>
/// Handles feedback visuals for pedestals.
/// Uses standard Unity TextMesh instead of TextMeshPro to avoid dependency.
/// </summary>
public class PedestalFeedback : MonoBehaviour
{
    private float fadeDuration = 1.2f;
    private TextMesh feedbackText;

    /// <summary>
    /// Shows correct feedback message.
    /// </summary>
    public void CorrectFeedback(string message)
    {
        ShowFeedback(message, Color.green);
    }

    /// <summary>
    /// Shows incorrect feedback message.
    /// </summary>
    public void IncorrectFeedback(string message)
    {
        ShowFeedback(message, Color.red);
    }

    /// <summary>
    /// Displays feedback text above the pedestal.
    /// </summary>
    private void ShowFeedback(string message, Color color)
    {
        // Clean up any existing text
        if (feedbackText != null)
        {
            Destroy(feedbackText.gameObject);
        }

        // Create text object
        GameObject textObj = new GameObject("FeedbackText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0f, 1.0f, 0f);

        feedbackText = textObj.AddComponent<TextMesh>();
        feedbackText.text = message;
        feedbackText.characterSize = 0.1f;
        feedbackText.color = color;
        feedbackText.alignment = TextAlignment.Center;
        feedbackText.anchor = TextAnchor.MiddleCenter;

        // Start fade out
        StartCoroutine(Dissolve(color, fadeDuration, feedbackText));
    }

    /// <summary>
    /// Coroutine to fade out feedback text.
    /// </summary>
    private System.Collections.IEnumerator Dissolve(Color startColor, float timeFadeIn, TextMesh textMesh)
    {
        Color fadeTo = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsed = 0f;

        while (elapsed < timeFadeIn)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / timeFadeIn;
            textMesh.color = Color.Lerp(startColor, fadeTo, normalizedTime);
            yield return null;
        }

        textMesh.color = fadeTo;
        Destroy(textMesh.gameObject);
    }
}
