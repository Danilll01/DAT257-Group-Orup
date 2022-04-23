using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TriggerScreenTransition : MonoBehaviour
{

    [SerializeField] private int whatCollider;
    [SerializeField] private MathAnswerGeneratorScript mathAnswerGenerator;
    [SerializeField] private AIDestinationSetter pathSetter;

    //[SerializeField] private Camera[] cams;                 
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera pathCam;
    [SerializeField] private Transform playerCharachter;


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Fixa s� den s�tter r�tt location f�r r�tt svar n�r player springer utanf�r sk�rmen
    // G�r s� den byter tillbaka fr�n fel svar + g�r player animation
    // G�r s� att den bara ger tillbaka kontrollen efter transition
    // Gl�m ej en transition till allt mellan
    // Se till att man inte kan g� utanf�r i g�ngen till en annan sk�rm


    // Changes what screen is currently active
    private void changeCameraView() {

        // If it is wrong path, switch screen
        if (!mathAnswerGenerator.IsCorrectAnswer(whatCollider)) {
            pathCam.enabled = true;
            mainCam.enabled = false;
        }
        
    }

    public void SwitchBackToMainScreen() {
        mainCam.enabled = true;
        pathCam.enabled = false;
        mathAnswerGenerator.ActivateCanvas();
    }

    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        gameObject.SetActive(false);
        pathSetter.canGetNewPos(true); // This maybe will change 
        changeCameraView();
        mathAnswerGenerator.AnswerPressed(whatCollider);
        
    }

  
}
