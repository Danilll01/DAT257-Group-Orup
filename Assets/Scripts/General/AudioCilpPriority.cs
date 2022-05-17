using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCilpPriority : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;

    [SerializeField] private float deafenPercent = 0.5f;

    private AudioSource sourceToPlay;

    void Start()
    {
        // Get source clip
        sourceToPlay = GetComponent<AudioSource>();
    }

    public void PlayPrioritySound()
    {
        // Prevents duplicate calls
        StopAllCoroutines();

        StartCoroutine(PlaySoundAndDeafenBgMusic());
    }

    private IEnumerator PlaySoundAndDeafenBgMusic()
    {
        // If background music exists
        if (backgroundMusic != null)
        {
            // Store the original volume
            float originalVolume = backgroundMusic.volume;

            // Decrease volume by specified amount of percent
            backgroundMusic.volume = originalVolume * (1 - deafenPercent);

            // Play the helper voice
            sourceToPlay.Play();

            // Wait for the helper voice to be done playing
            yield return new WaitForSeconds(sourceToPlay.clip.length);

            // Reset background music to original volume
            backgroundMusic.volume = originalVolume;

        } else
        {
            // If there was no background music provided just play the helper voice clip
            sourceToPlay.Play();
        }

        yield return null;
    }
}
