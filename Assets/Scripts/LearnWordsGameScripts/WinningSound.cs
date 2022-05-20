using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // Play the correct sound when the object is enabled
    void OnEnable() {
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            source.Play();
        }
    }
}
