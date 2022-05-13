using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] groundClips;
    [SerializeField] private AudioClip[] metalClips;
    [SerializeField] private AudioClip[] mudClips;
    [SerializeField] private AudioClip[] grassClips;
    [SerializeField] private AudioClip[] gravelClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Step is an event from the Animation itself, everytime the animation fires "Step" , a Clip gets played
    public void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        int terrainTextureIndex = 1;
        switch (terrainTextureIndex)
        {
            case 0:
                return GetClipFromArray(groundClips);
            case 1:
                return GetClipFromArray(groundClips);
            case 2:
                return GetClipFromArray(metalClips);
            case 3:
                return GetClipFromArray(mudClips);
            case 4:
                return GetClipFromArray(grassClips);
            case 5:
            case 6:
                return GetClipFromArray(gravelClips);
            default:
                return GetClipFromArray(groundClips);

        }
    }

    private AudioClip GetClipFromArray(AudioClip[] clips)
    {
        return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
    }
}
