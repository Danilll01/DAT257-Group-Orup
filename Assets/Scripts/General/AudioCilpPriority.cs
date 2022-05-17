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
        sourceToPlay = GetComponent<AudioSource>();
    }

    public void PlayPrioritySound()
    {
        StopAllCoroutines();
        StartCoroutine(PlaySoundAndDeafenBgMusic());
    }

    private IEnumerator PlaySoundAndDeafenBgMusic()
    {
        if (backgroundMusic != null)
        {
            float originalVolume = backgroundMusic.volume;

            backgroundMusic.volume = originalVolume * (1 - deafenPercent);

            sourceToPlay.Play();

            yield return new WaitForSeconds(sourceToPlay.clip.length);

            backgroundMusic.volume = originalVolume;

        } else
        {
            sourceToPlay.Play();
        }

        yield return null;
    }
}
