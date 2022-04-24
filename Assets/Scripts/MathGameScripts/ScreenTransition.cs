using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{

    [SerializeField] private float fadeTime;
    [SerializeField] private float waitTime;

    private CanvasGroup transitionScreen;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start() 
    {

        transitionScreen = GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update() {}

    // Activates the screen transition
    public void Transition(Action inBetweenTransition) {
        if (!isActive) {
            isActive = true;
            StartCoroutine(makeScreenTransition(inBetweenTransition));
        }
    }

    // Runs the transition and runs the given method when the screen is black
    private IEnumerator makeScreenTransition(Action inBetweenTransition) {
        float timer = 0;
        float maxTransition = fadeTime / 2;

        transitionScreen.blocksRaycasts = true;

        // Fade in black screen
        while (timer <= maxTransition) {
            transitionScreen.alpha = Mathf.Lerp(0, 1, timer / maxTransition);
            timer += Time.deltaTime;
            yield return null;
        }

        // Wait on black screen
        timer = 0;
        while (timer <= waitTime) {
            timer += Time.deltaTime;
            yield return null;
        }

        // This is the given method implemented with lambda that runs here
        inBetweenTransition();
        timer = 0;

        // Fade back to game screen
        while (timer <= maxTransition) {
            transitionScreen.alpha = Mathf.Lerp(1, 0, timer / maxTransition);
            timer += Time.deltaTime;
            yield return null;
        }

        transitionScreen.blocksRaycasts = false;

        isActive = false;
    }
}
