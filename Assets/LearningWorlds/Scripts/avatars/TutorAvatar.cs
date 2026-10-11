using UnityEngine;

namespace LearningWorlds.Tutors
{
    /// <summary>
    /// Controls avatar animations and audio playback for Saia tutors.
    /// </summary>
    public class TutorAvatar : MonoBehaviour
    {
        private Animator avatarAnimator;
        private AudioSource audioSource;

        private void Awake()
        {
            // Initialize references
            avatarAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = false;
            }
        }

        /// <summary>
        /// Plays an animation via Animator trigger.
        /// </summary>
        /// <param name="triggerName">The trigger parameter name to set.</param>
        public void PlayAnimation(string triggerName)
        {
            if (avatarAnimator != null)
            {
                avatarAnimator.SetTrigger(triggerName);
            }
            else
            {
                Debug.LogWarning("TutorAvatar: No Animator component found!");
            }
        }

        /// <summary>
        /// Plays an audio clip with optional animation trigger.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="triggerAnimation">If true, triggers the "Talk" animation.</param>
        public void PlayAudio(AudioClip clip, bool triggerAnimation = true)
        {
            if (audioSource == null)
            {
                Debug.LogError("TutorAvatar: No AudioSource component!");
                return;
            }

            // Stop any current audio
            audioSource.Stop();

            // Play new audio
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();

                // Trigger animation if requested
                if (triggerAnimation)
                {
                    PlayAnimation("Talk");
                }
            }
            else
            {
                Debug.LogError("TutorAvatar: No audio clip provided!");
            }
        }

        /// <summary>
        /// Stops any currently playing audio.
        /// </summary>
        public void StopAudio()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// Resets the avatar to idle state.
        /// </summary>
        public void ResetToIdle()
        {
            PlayAnimation("Idle");
            StopAudio();
        }
    }
}
