using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundClips;
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
        AudioClip clip = GetClipFromArray(soundClips);
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.SetScheduledEndTime(AudioSettings.dspTime + (timeUntillStop));
        //audioSource.PlayOneShot(clip);
    }

    private AudioClip GetClipFromArray(AudioClip[] clips)
    {
        return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
    }

    void Update() {
        //if (audioSource.time > 0.05f) {
        //    Debug.Log("NU");
        //    audioSource.Stop();
        //}
    }
}
