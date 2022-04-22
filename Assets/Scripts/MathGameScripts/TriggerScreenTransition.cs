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


    //  samt sätt location så karaktären springer utanför skärmen.
    // Glöm ej en transition till allt mellan



    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        gameObject.SetActive(false);
        pathSetter.canGetNewPos(true);
        mathAnswerGenerator.AnswerPressed(whatCollider);        
    }
}
