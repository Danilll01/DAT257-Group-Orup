using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    // Field for choosing panel
    [SerializeField]
    private Transform puzzleField;

    // Field for choosing button prefab
    [SerializeField]
    private GameObject btn; 

    // Adds buttons to panel and gives each button an index as a strings
    void Awake(){

        // Loops 12 times
        for (int i = 0; i < 12; i++){
            // For each loop creates a button (card)
            GameObject button = Instantiate(btn);

            // Names each button the index 
            button.name = "" + i;

            // Makes button child of panel
            button.transform.SetParent(puzzleField, false);
        }
    }
}
