using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] walkClips;
    [SerializeField] private AudioClip[] jumpClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Step is an event from the Animation itself, everytime the animation fires "Step" , a Clip gets played
    public void Step()
    {
        AudioClip clip = GetClipFromArray(walkClips);
        audioSource.PlayOneShot(clip);
    }

    // Calls from jump animation trigger
    public void Jump() {
        audioSource.Stop();
        AudioClip clip = GetClipFromArray(jumpClips);
        audioSource.PlayOneShot(clip);
    }

    // Gets random sound clip from the given array
    private AudioClip GetClipFromArray(AudioClip[] clips)
    {
        return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
    }
}
