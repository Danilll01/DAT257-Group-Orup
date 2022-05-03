using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimator : MonoBehaviour
{

    private float timer = 0f;
    private Animator frogAnimator;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(5, 15);
        frogAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0) {
            frogAnimator.SetTrigger("FrogBlow");
            timer = Random.Range(5, 15);
        }
    }
}
