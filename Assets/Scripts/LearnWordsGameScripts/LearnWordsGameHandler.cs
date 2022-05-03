using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnWordsGameHandler : MonoBehaviour
{

    [SerializeField] private ImageController imgController;
    [SerializeField] private LetterController letterController;

    // The images and matching words must be in the same order. Otherwise it will not work correctly
    [SerializeField] private List<Sprite> images;
    [SerializeField] private List<string> matchingWord;

    private int randomCardAmount;
    private int rightAnswerCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        randomCardAmount = imgController.getCardAmount(); // The amount of cards to randomize sprites and words to 
    }


    // Generate new information to later be placed on the image cards
    public void generateNewCards() {
        List<System.Tuple<Sprite, string>> randList = new List<System.Tuple<Sprite, string>>(); // Creates the structure we use to send data with

        // temp lists to help with randomizing the order of new card data (without duplicates)
        List<Sprite> tempImages = new List<Sprite>(images);
        List<string> tempMatchingWord = new List<string>(matchingWord);

        // Randomize card data for all cards
        for (int i = 0; i < randomCardAmount; i++) {
            int index = Random.Range(0, tempImages.Count); // Ranomize a index in the lists
            randList.Add(System.Tuple.Create(tempImages[index], tempMatchingWord[index])); // Place data from the index into the sending structure
            
            // Removes items to avoid duplicates
            tempImages.RemoveAt(index);
            tempMatchingWord.RemoveAt(index);
        }

        imgController.placeInformationInCard(randList);
        letterController.placeInformationInCard(randList);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the currently selected cards match from each side / card base
        if (imgController.getSelectedImgCardInfo() == letterController.getSelectedLetterCardInfo()) {
            imgController.haveMatched();
            letterController.haveMatched();
            checkIfAllAnswers();
        }
    }

    // Checks if all cards have been answered correctly and if so, calls to genereate new ones
    private void checkIfAllAnswers() {
        rightAnswerCounter++;
        
        if (rightAnswerCounter == randomCardAmount) {
            rightAnswerCounter = 0;
            generateNewCards();
        } 
    }
}
