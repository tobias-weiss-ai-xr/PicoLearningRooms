using UnityEngine;

namespace LearningWorlds.Arachnophobia
{
    /// <summary>
    /// Simple light animation for spider objects.
    /// Pulses the spider's material color for visibility in dark environments.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class LightAnimationSelector : MonoBehaviour
    {
        [Header("Animation Settings")]
        public float pulseSpeed = 2f;
        public float minIntensity = 0.5f;
        public float maxIntensity = 1.5f;
        public Color pulseColor = Color.cyan;

        private Renderer objectRenderer;
        private Color originalColor;
        private float timeOffset;

        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalColor = objectRenderer.material.color;
            }
            timeOffset = Random.Range(0f, 2f);
        }

        private void Update()
        {
            if (objectRenderer != null)
            {
                // Calculate pulse intensity
                float pulse = (Mathf.Sin((Time.time + timeOffset) * pulseSpeed) + 1f) / 2f;
                float intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);

                // Apply pulsed color
                Color finalColor = pulseColor * intensity * originalColor.a;
                objectRenderer.material.color = finalColor;
            }
        }

        private void OnDisable()
        {
            // Restore original color when disabled
            if (objectRenderer != null)
            {
                objectRenderer.material.color = originalColor;
            }
        }
    }
}
