using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class TriggerScreenTransition : MonoBehaviour
{

    [SerializeField] private int whatCollider;
    [SerializeField] private MathAnswerGeneratorScript mathAnswerGenerator;
    [SerializeField] private AIDestinationSetter pathSetter;
              
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera pathCam;
    [SerializeField] private Transform playerCharachter;
    [SerializeField] private Transform[] afterTPPoints;  // [0] is same side, [1] is opposite

    [SerializeField] private ScreenTransition transitionScreen;
    


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    

    // Glöm ej en transition till allt mellan
    // Se till att man inte kan gå utanför i gången till en annan skärm


    // Changes what screen is currently active
    private void changeCameraView() {

        // If it is wrong path, switch screen
        if (!mathAnswerGenerator.IsCorrectAnswer(whatCollider)) {
            // Here Screen transition goes!!!!!!!!!!!!!!

            transitionScreen.Transition(
                () => {
                    pathCam.enabled = true;
                    mainCam.enabled = false;
                }
            );
            
        } else {
            // Here Screen transition goes!!!!!!!!!!

            transitionScreen.Transition(
                () => {
                    pathSetter.setNewPath(afterTPPoints[1].position);
                    teleportPlayer();
                    pathSetter.canGetNewPos(true);
                }
            );
        }
        
    }

    // Switch to main screen and setup player agent and canvas (Called from other script)
    public void SwitchBackToMainScreen() {
        // Here Screen transition goes!!!!!!!!!!!!

        transitionScreen.Transition(
            () => {
                mainCam.enabled = true;
                pathCam.enabled = false;
                pathSetter.setNewPath(afterTPPoints[0].position);
                pathSetter.canGetNewPos(true);
                mathAnswerGenerator.ActivateCanvas();
            }
        );
    }

    //private IEnumerator makeScreenTransition(Action inBetweenTransition) {
    //    float timer = 0;
    //    float maxTransition = transitionTime / 2;

    //    while (timer <= maxTransition) {
    //        transitionScreen.alpha = Mathf.Lerp(0, 1, timer / maxTransition);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    Debug.Log("NU gör den det!!!!!");
    //    inBetweenTransition();
    //    timer = 0;

    //    while (timer <= maxTransition) {
    //        transitionScreen.alpha = Mathf.Lerp(1, 0, timer / maxTransition);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //}


    //private IEnumerator Fade(bool fadeOut) {

    //    transitionScreen.alpha = Mathf.Lerp(0, 1, );

    //    float timer = transitionTime / 2;

    //    while

    //    Color c = renderer.material.color;
    //    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f) {
    //        c.a = alpha;
    //        renderer.material.color = c;
    //        yield return null;
    //    }
    //}


    // Activates this collider for player to collide on
    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        gameObject.SetActive(false);
        changeCameraView();
        mathAnswerGenerator.AnswerPressed(whatCollider);
    }


    // This will teleport the player charachter to the other side of the gamescreen
    private void teleportPlayer() {
        Vector2 runFromVector = Vector2.zero; // Temp value before switch

        switch (whatCollider) {
            case 1:
                runFromVector = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth + 50, mainCam.pixelHeight / 2));
                
                break;
            case 2:
                runFromVector = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth / 2, -50));
                break;
            case 3:
                runFromVector = mainCam.ScreenToWorldPoint(new Vector3(-50, mainCam.pixelHeight / 2));
                break;
            case 4:
                runFromVector = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight + 50));
                break;
            default:
                Debug.Log("This should never happen! If you see this report it!!");
                break;
        }

        pathSetter.teleportAgent(runFromVector);
        
    }



}
