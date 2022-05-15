using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] walkClips;
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private double timeUntillStop = 0.2;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Step is an event from the Animation itself, everytime the animation fires "Step" , a Clip gets played
    public void Step()
    {
        Debug.Log("HEJ");
        AudioClip clip = GetClipFromArray(walkClips);
        //audioSource.clip = clip;
        //audioSource.Play();
        //audioSource.SetScheduledEndTime(AudioSettings.dspTime + (timeUntillStop));
        audioSource.PlayOneShot(clip);
    }

    public void Jump() {
        Debug.Log("D�");
        audioSource.Stop();
        AudioClip clip = GetClipFromArray(jumpClips);
        //audioSource.clip = clip;
        //audioSource.Play();
        //audioSource.SetScheduledEndTime(AudioSettings.dspTime + (timeUntillStop));
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetClipFromArray(AudioClip[] clips)
    {
        return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
    }
}
