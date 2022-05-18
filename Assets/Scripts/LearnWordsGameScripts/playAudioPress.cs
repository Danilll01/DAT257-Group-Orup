using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playAudioPress : MonoBehaviour
{
    [SerializeField ]private AudioClip[] audioClips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playAudioFromName(TextMeshProUGUI nameText)
    {
        string name = nameText.text.Trim();
        if (name.Contains(" "))
        {
            string[] nameSections = name.Split(" ");
            name = nameSections[0];
            
        }

        AudioClip selectedClip = null;
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name.Trim() == name)
            {
                Debug.Log("matched");
                selectedClip = clip;
            }
        }

        if (selectedClip != null)
        {
            audioSource.PlayOneShot(selectedClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
