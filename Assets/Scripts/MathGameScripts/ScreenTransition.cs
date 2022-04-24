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

    public void Transition(Action inBetweenTransition) {
        if (!isActive) {
            isActive = true;
            StartCoroutine(makeScreenTransition(inBetweenTransition));
        }
    }

    private IEnumerator makeScreenTransition(Action inBetweenTransition) {
        float timer = 0;
        float maxTransition = fadeTime / 2;

        while (timer <= maxTransition) {
            transitionScreen.alpha = Mathf.Lerp(0, 1, timer / maxTransition);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        while (timer <= waitTime) {
            timer += Time.deltaTime;
            yield return null;
        }

        inBetweenTransition();
        timer = 0;

        while (timer <= maxTransition) {
            transitionScreen.alpha = Mathf.Lerp(1, 0, timer / maxTransition);
            timer += Time.deltaTime;
            yield return null;
        }

        isActive = false;
    }
}
