using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TriggerScreenTransition : MonoBehaviour
{

    [SerializeField] private int whatCollider;
    [SerializeField] private MathAnswerGeneratorScript mathAnswerGenerator;
    [SerializeField] private AIDestinationSetter pathSetter;
              
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera pathCam;
    [SerializeField] private Transform playerCharachter;
    [SerializeField] private Transform[] afterTPPoints;  // [0] is same side, [1] is opposite


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Fixa så den sätter rätt location för rätt svar när player springer utanför skärmen
    // Gör player animation i fel svar
    // Gör så att den bara ger tillbaka kontrollen efter transition
    // Glöm ej en transition till allt mellan
    // Se till att man inte kan gå utanför i gången till en annan skärm


    // Changes what screen is currently active
    private void changeCameraView() {

        // If it is wrong path, switch screen
        if (!mathAnswerGenerator.IsCorrectAnswer(whatCollider)) {
            // Here Screen transition goes!!!!!!!!!!!!!!
            pathCam.enabled = true;
            mainCam.enabled = false;
        } else {
            // Here Screen transition goes!!!!!!!!!!
            pathSetter.setNewPath(afterTPPoints[1].position);
            teleportPlayer();
            pathSetter.canGetNewPos(true);
        }
        
    }

    public void SwitchBackToMainScreen() {
        // Here Screen transition goes!!!!!!!!!!!!
        mainCam.enabled = true;
        pathCam.enabled = false;
        pathSetter.setNewPath(afterTPPoints[0].position);
        pathSetter.canGetNewPos(true);
        mathAnswerGenerator.ActivateCanvas();
    }

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

        Debug.Log("PLayer Teleported!!!!");
        Debug.Log(playerCharachter.position);
        //playerCharachter.position = runFromVector; // Sets new position of player

        pathSetter.teleportAgent(runFromVector);
        Debug.Log(playerCharachter.position);
    }

}
