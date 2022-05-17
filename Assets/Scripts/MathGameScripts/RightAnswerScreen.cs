using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightAnswerScreen : MonoBehaviour
{
    [SerializeField][Range(1, 60)] float timeUntillNewQuestion = 3;
    [SerializeField] MainSoundPlayer soundPlayer;
    [SerializeField] GameObject[] audioButtons;
    private bool isRunning = false;
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Starts the coroutine that counts untill it should switch back to the main screen.
    public void StartGoToNormalScreen(float winningSoundTime, int whatCollider, Action goBackMethod) {
        if (!isRunning) {
            isRunning = true;
            StartCoroutine(GoBackTransition(goBackMethod, winningSoundTime, whatCollider));
        }
    }

    // Count down the time that the player should see the right answer and then runs the given back transition method
    private IEnumerator GoBackTransition(Action goBackMethod, float winningSoundTime, int whatCollider) {
        yield return new WaitForSeconds(winningSoundTime);
        soundPlayer.playWholeExercise(whatCollider);
        yield return new WaitForSeconds(timeUntillNewQuestion); // This wait has to be longer than the fade back, otherwise it does not work
        goBackMethod();
        isRunning = false;
    }

    public void disableAudioButtons() {
        foreach (GameObject button in audioButtons) {
            button.SetActive(false);
        }
    }

    public void activateAudioButtons() {
        foreach (GameObject button in audioButtons) {
            button.SetActive(true);
        }
    }
}
