using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterController : MonoBehaviour
{

    private GameObject selectedLetter = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When a letter is clicked this method is called. The parameter is the clicked object
    public void LetterClicked(GameObject letterObject) {

        // Make old card blue
        if (selectedLetter != null) {
            selectedLetter.GetComponent<Image>().color = new Color(0, 166f / 255f, 1);
        }
        

        // If it's the same we have, unselect it
        if (letterObject.Equals(selectedLetter)) {
            selectedLetter = null;
        } else {
            selectedLetter = letterObject;
            selectedLetter.GetComponent<Image>().color = new Color(0, 1, 100f / 255f);
        }
    }

    // Returns the selected card info to the main game controller
    public string getSelectedLetterCardInfo() {
        return selectedLetter != null ? selectedLetter.GetComponentInChildren<TextMeshProUGUI>().text : "NULL_LETTER_OBJECT" ;
    }

    // Make sure the selected card now have the right color
    public void haveMatched() {
        selectedLetter.GetComponent<Image>().color = new Color(0, 166f / 255f, 1);
        selectedLetter = null;
    }
}
