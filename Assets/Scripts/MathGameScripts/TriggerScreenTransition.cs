using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TriggerScreenTransition : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private int whatCollider;
    [SerializeField] private MathAnswerGeneratorScript mathAnswerGenerator;
    [SerializeField] private AIDestinationSetter pathSetter;


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Fixa så den sätter rätt location för rätt svar när player springer utanför skärmen
    // Glöm ej en transition till allt mellan
    // Se till att man inte kan gå utanför i gången till en annan skärm



    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        gameObject.SetActive(false);
        pathSetter.canGetNewPos(true);
        setNewGoToPosition();
        mathAnswerGenerator.AnswerPressed(whatCollider);        
    }

    private void setNewGoToPosition() {

        Vector2 runTo = Vector2.zero;

        switch (whatCollider) {
            case 1:
                runTo = mainCam.ScreenToWorldPoint(new Vector3(-10, mainCam.pixelHeight / 2));
                break;
            case 2:
                runTo = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight + 10));
                break;
            case 3:
                runTo = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth + 10, mainCam.pixelHeight / 2));
                break;
            case 4:
                runTo = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth / 2, -10));
                break;
            default:
                Debug.Log("This should never happen! If you see this report it!!");
                break;
        }
        Debug.Log(runTo);
        Debug.Log(new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight + 10));
        pathSetter.setNewPath(runTo);
    }
}
