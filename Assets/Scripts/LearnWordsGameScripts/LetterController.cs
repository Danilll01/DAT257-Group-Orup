using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LetterController : MonoBehaviour
{

    private GameObject selectedLetter = null;
    [SerializeField] private GameObject[] cards;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // Returns the amount of cards that exist
    public int getCardAmount() { return cards.Length; }

    // When a letter is clicked this method is called. The parameter is the clicked object
    public void LetterClicked(GameObject letterObject) {
        if (letterObject.GetComponent<Image>().color != new Color(100f / 255f, 1, 100f / 255f)) {
            // Make old card normal
            if (selectedLetter != null) {
                selectedLetter.GetComponent<Image>().color = new Color(1, 1, 1);
            }


            // If it's the same we have, unselect it
            if (letterObject.Equals(selectedLetter)) {
                selectedLetter = null;
            } else {
                selectedLetter = letterObject;
                selectedLetter.GetComponent<Image>().color = new Color(1, 1, 0); // Makes card yellow
            }
        }
    }

    // Place the correct information on the crads
    public void placeInformationInCard(List<Tuple<Sprite, string>> cardInfoList)
    {
        List<GameObject> tempCards = new List<GameObject>();
        foreach (GameObject card in cards)
        {
            tempCards.Add(card);
        }
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<Image>().color = new Color(1, 1, 1); // Make normal color

            int index = UnityEngine.Random.Range(0, tempCards.Count); // Ranomize a index in the lists
            tempCards[index].GetComponentInChildren<TextMeshProUGUI>().text = cardInfoList[i].Item2;  // Set matching letter
            tempCards.RemoveAt(index); // Remove from temp list to randomize on remaining cards
            
        }
    }

    // Returns the selected card to the main game controller
    public GameObject getSelectedLetterCardInfo() {
        return selectedLetter; 
    }

    // Make sure the selected card now have the right color
    public void haveMatched() {
        selectedLetter = null;
    }
}
