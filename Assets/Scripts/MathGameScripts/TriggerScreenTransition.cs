using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScreenTransition : MonoBehaviour
{

    [SerializeField] private int whatCollider;
    [SerializeField] private MathAnswerGeneratorScript mathAnswerGenerator;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Låt nuvarande knapp aktivera colliderns så att den kan kalla på rätt metod
    // I samma veva, disabla så man inte kan göra annat val samt sätt location så karaktären springer
    // utanför skärmen.
    // Glöm ej en transition till allt mellan



    public void activateCollider() {
        gameObject.SetActive(true);
    }

    // This will be called when the players enter the collider
    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("HEJ!!!!!!");
        gameObject.SetActive(false);
        mathAnswerGenerator.AnswerPressed(whatCollider);        
    }
}
