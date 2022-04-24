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


    // Changes what screen is currently active
    private void changeCameraView() {

        // If it is wrong path, switch screen
        if (!mathAnswerGenerator.IsCorrectAnswer(whatCollider)) {
            // Call to run transition with given method
            transitionScreen.Transition(
                () => {
                    pathCam.enabled = true;
                    mainCam.enabled = false;
                    mathAnswerGenerator.ActivateCanvas(false);
                }
            );
            
        } else {
            // Call to run transition with given method
            transitionScreen.Transition(
                () => {
                    Debug.Log("Skickar");
                    mathAnswerGenerator.AnswerPressed(whatCollider);
                    pathSetter.setNewPath(afterTPPoints[1].position);
                    teleportPlayer();
                    pathSetter.canGetNewPos(true);
                    
                }
            );
        }
        
    }

    // Switch to main screen and setup player agent and canvas (Called from other script)
    public void SwitchBackToMainScreen() {
        // Call to run transition with given method
        transitionScreen.Transition(
            () => {
                mainCam.enabled = true;
                pathCam.enabled = false;
                pathSetter.setNewPath(afterTPPoints[0].position);
                pathSetter.canGetNewPos(true);
                mathAnswerGenerator.ActivateCanvas(true);
            }
        );
    }


    // Activates this collider for player to collide on
    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        gameObject.SetActive(false);
        changeCameraView();
        
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
