using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ImageController : MonoBehaviour
{

    private GameObject selectedImgCard = null;
    [SerializeField] private GameObject[] cards;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    // Returns the amount of cards that exist
    public int getCardAmount() { return cards.Length; }

    // Places given information in each card
    public void placeInformationInCard(List<Tuple<Sprite, string>> cardInfoList) {

        for (int i = 0; i < cards.Length; i++) {
            cards[i].GetComponent<Image>().color = new Color(1, 1, 1); // Make normal color

            cards[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = cardInfoList[i].Item1; // Set new image
            cards[i].GetComponentInChildren<TextMeshProUGUI>().text = cardInfoList[i].Item1.name; // Set text to word of image
            cards[i].GetComponentInChildren<Text>().text = cardInfoList[i].Item2;  // Set new "hidden" text
        }

    }


    // When a letter is clicked this method is called. The parameter is the clicked object
    public void ImageClicked(GameObject imageObject) {

        // Only select and do things if it's not already matched
        if (imageObject.GetComponent<Image>().color != new Color(1, 180f / 255f, 0)) {

            // Make old card blue
            if (selectedImgCard != null) {
                selectedImgCard.GetComponent<Image>().color = new Color(1, 1, 1);
            }


            // If it's the same we have, unselect it
            if (imageObject.Equals(selectedImgCard)) {
                selectedImgCard = null;
            } else {
                selectedImgCard = imageObject;
                selectedImgCard.GetComponent<Image>().color = new Color(0, 1, 100f / 255f);
            }
        }
    }

    // Returns the selected card to the main game controller
    public GameObject getSelectedImgCardInfo() {
        return selectedImgCard; 
    }

    // Make sure the selected card now have the right color
    public void haveMatched() {
       // selectedImgCard.GetComponent<Image>().color = new Color(1, 180f / 255f, 0); // Makes card orange
        selectedImgCard = null;
    }
}
