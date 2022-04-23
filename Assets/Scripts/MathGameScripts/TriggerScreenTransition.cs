using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TriggerScreenTransition : MonoBehaviour
{

    [SerializeField] private int whatCollider;
    [SerializeField] private MathAnswerGeneratorScript mathAnswerGenerator;
    [SerializeField] private AIDestinationSetter pathSetter;


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Fixa så den sätter rätt location för rätt svar när player springer utanför skärmen
    // Gör så den byter tillbaka från fel svar + gör player animation
    // Gör så att den bara ger tillbaka kontrollen efter transition
    // Glöm ej en transition till allt mellan
    // Se till att man inte kan gå utanför i gången till en annan skärm



    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        gameObject.SetActive(false);
        pathSetter.canGetNewPos(true); // This maybe will change 
        mathAnswerGenerator.AnswerPressed(whatCollider);        
    }

  
}
