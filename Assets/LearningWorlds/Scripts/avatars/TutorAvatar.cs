using UnityEngine;

public class TutorAvatar : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;

    void Awake()
    {
        // Initialize references
        anim = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // Public method to play animation
    public void PlayAnimation(string trigger)
    {
        if (anim != null)
        {
            anim.SetTrigger(trigger);
        }
    }

    // Public method to play audio
    public void PlayAudio(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = false;
            }
        }

        // Stop any current audio
        audioSource.Stop();

        // Play new audio
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No audio clip provided");
        }
        
        // Trigger animation
        PlayAnimation("Talk");
    }
}

