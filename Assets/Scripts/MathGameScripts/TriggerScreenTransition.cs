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


    // L�t nuvarande knapp aktivera colliderns s� att den kan kalla p� r�tt metod
    // I samma veva, disabla s� man inte kan g�ra annat val samt s�tt location s� karakt�ren springer
    // utanf�r sk�rmen.
    // Gl�m ej en transition till allt mellan



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
