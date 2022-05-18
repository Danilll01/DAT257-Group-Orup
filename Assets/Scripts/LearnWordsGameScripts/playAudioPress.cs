using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Function button will call with the corresponding tect field
    public void playAudioFromName(TextMeshProUGUI nameText)
    {
        // Removes white space from front and end of text
        string name = nameText.text.Trim();

        // If the name contains a space, it is a letter
        // We only want the first letter so we split it
        if (name.Contains(" "))
        {
            string[] nameSections = name.Split(" ");
            name = nameSections[0];
            
        }

        // Look through all the audioClips to find one
        // with a matching name
        AudioClip selectedClip = null;
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name.Trim() == name)
            {
                selectedClip = clip;
            }
        }

        // If we found a matching clip, play it
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
