using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightAnswerScreen : MonoBehaviour
{
    [SerializeField][Range(1, 60)] float timeUntillNewQuestion = 3;
    private bool isRunning = false;
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    public void StartGoToNormalScreen(Action goBackMethod) {
        if (!isRunning) {
            Debug.Log("1");
            isRunning = true;
            StartCoroutine(GoBackTransition(goBackMethod));
        }
    }

    private IEnumerator GoBackTransition(Action goBackMethod) {
        yield return new WaitForSeconds(timeUntillNewQuestion);
        Debug.Log("2");
        goBackMethod();
        isRunning = false;
    }
}
