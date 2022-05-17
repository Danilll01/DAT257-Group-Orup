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
    [SerializeField] private GameObject rightAnswerCam;
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
                    // Switch to the path camera
                    pathCam.enabled = true;
                    mainCam.enabled = false;
                    transitionScreen.GetComponents<AudioSource>()[1].Play(); // Play sound for wrong answer
                    mathAnswerGenerator.AnswerPressedWrong(whatCollider); // Make selected answer red
                }
            );
            
        } else {
            // Call to run transition with given method for the right answer

            // Makes transition to the black display screen
            transitionScreen.Transition(
                () => {

                    // Swtich to the right camera
                    rightAnswerCam.GetComponent<Camera>().enabled = true;
                    mainCam.enabled = false;
                    
                    // Play right sound and make the canvas ready to show the right answer
                    transitionScreen.GetComponents<AudioSource>()[0].Play(); 
                    mathAnswerGenerator.MakeCorrectAnswerGreen();

                    // Call method to beging counting to move away from the black right answer screen
                    rightAnswerCam.GetComponent<RightAnswerScreen>().StartGoToNormalScreen(
                        () => {
                            // Method to call when it should go back to the main math screen
                            switchFromRightScreen();
                        }
                    );

                }
            );

        }
        
    }

    // This method is called when the right answer has been shown for the specified time there
    // It transitions back from that screen to the main math screen and sets up for the new question to be generated
    private void switchFromRightScreen() {

        // Make transition back
        transitionScreen.Transition(
            () => {

                // Sets new path for the ai agent to walk towards and teleport the player to walk from the right place
                pathSetter.setNewPath(afterTPPoints[1].position);
                teleportPlayer();

                // Hides canvas untill the new one is generated
                mathAnswerGenerator.HideCanvas();
                
                // Switch camera back to the main math screen
                mainCam.enabled = true;
                rightAnswerCam.GetComponent<Camera>().enabled = false;
                

                // Method to generate new exercise is called when player stopps after a teleportation 
                pathSetter.CallGenerateNewAnswers(
                        () => {
                            mathAnswerGenerator.AnswerPressedRight(whatCollider);
                            pathSetter.canGetNewPos(true);
                        });

            }
        );
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
                mathAnswerGenerator.ActivateCanvas();
            }
        );
    }


    // Activates this collider for player to collide on
    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            gameObject.SetActive(false);
            changeCameraView();
        }
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
