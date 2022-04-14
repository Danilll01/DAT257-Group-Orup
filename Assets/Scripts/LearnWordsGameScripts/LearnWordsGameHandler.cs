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


    // Start is called before the first frame update
    void Start()
    {
        randomCardAmount = imgController.getCardAmount();
    }

    public void generateNewCards() {
        List<System.Tuple<Sprite, string>> randList = new List<System.Tuple<Sprite, string>>();

        List<Sprite> tempImages = new List<Sprite>(images);
        List<string> tempMatchingWord = new List<string>(matchingWord);

        for (int i = 0; i < randomCardAmount; i++) {
            int index = Random.Range(0, tempImages.Count);
            randList.Add(System.Tuple.Create(tempImages[index], tempMatchingWord[index]));
            tempImages.RemoveAt(index);
            tempMatchingWord.RemoveAt(index);
        }

        imgController.placeInformationInCard(randList);
    }

    // Update is called once per frame
    void Update()
    {
        if (imgController.getSelectedImgCardInfo() == letterController.getSelectedLetterCardInfo()) {
            imgController.haveMatched();
            letterController.haveMatched();
        }
    }
}
