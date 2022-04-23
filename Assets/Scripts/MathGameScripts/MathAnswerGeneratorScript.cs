using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathAnswerGeneratorScript : MonoBehaviour
{
    [SerializeField] private MathQuestionGenerator questionGenerator;

    [SerializeField] private TextMeshProUGUI[] answerTxts;
    
    [SerializeField] private Sprite[] apples;
    [SerializeField] private Image[] imgHolders;

    [SerializeField] private GameObject[] cardHolders;
    [SerializeField] private Camera[] cams;
    [SerializeField] private Transform playerCharachter;

    // [SerializeField] private Transform[] spawnLocations; This can wait for now


    [SerializeField] private int rangeFromCorrectAnswer = 5;
    private int correctAnswerHolder;


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // Generate 3 answers with one correct answer of them
    public void GenerateAnswers(int rightAnswer) {
        // Generate first random wrong number
        int firstWrong = generateRandomNumber(rightAnswer);
        int secondWrong;
        do {
            // Generate second wrong number but ensures it is not the same as the first
            secondWrong = generateRandomNumber(rightAnswer);
        } while (secondWrong == firstWrong);

        

        // Calls fillAnswer function with a different position for the right answer
        switch (Random.Range(0, 3)) {
            case 0:

                int[] answers = { rightAnswer, firstWrong, secondWrong };
                fillAnswers(answers, rightAnswer);

                break;
            case 1:

                int[] answers2 = { firstWrong, rightAnswer, secondWrong };
                fillAnswers(answers2, rightAnswer);

                break;
            default:

                int[] answers3 = { firstWrong, secondWrong, rightAnswer };
                fillAnswers(answers3, rightAnswer);

                break;
        }
    }

    // Fills active cardHolders with the given answers
    private void fillAnswers(int[] answers, int correctAnswer) {
        
        int answerCounter = 0; // Help count what answer to set

        for (int i = 0; i < cardHolders.Length; i++) {
            if (cardHolders[i].activeSelf && answerCounter < 3) { // Skips the inactive card
                int currentWriting = answers[answerCounter];
                answerTxts[i].text = "= " + currentWriting;
                imgHolders[i].sprite = apples[currentWriting];
                answerCounter++;

                if (currentWriting == correctAnswer) {
                    correctAnswerHolder = i + 1;
                }
            }
        }
    }

    // Generates a new random number that is non negative and not the same as "rightAnswer" 
    private int generateRandomNumber(int rightAnswer) {
        int bonus;
        do {
            // Randomize a number between -5 to 5
            bonus = Random.Range(0 - rangeFromCorrectAnswer, 1 + rangeFromCorrectAnswer);
        } while (rightAnswer + bonus < 0 || bonus == 0 || rightAnswer + bonus > 18); // Ensures that the new generated answer is non negative and is not the same as the answer

        // Returns the new wrong answer
        return rightAnswer + bonus; 
    }

    // Method to call when a answer is pressed
    public void AnswerPressed(int answerNumber) {
        if (answerNumber == correctAnswerHolder) { // Checks if the button pressed contains the correct answer
            // If so, make all card holders active, blue and interactable again
            foreach (GameObject card in cardHolders) {
                card.GetComponent<Image>().color = new Color(0, 166f / 255f, 1);
                card.GetComponent<Button>().interactable = true;
                card.SetActive(true);
            }

            // Dissables the card straight over from the pressed answer
            changeActiveCard(answerNumber);

            // And randomize a new question
            questionGenerator.randomizeProblem();
        } else {
            // Make the selected card red to show that it is wrong and dissable button
            cardHolders[answerNumber - 1].GetComponent<Image>().color = new Color(1, 40f/255f, 0);
            cardHolders[answerNumber - 1].GetComponent<Button>().interactable = false;
        }
    }

    // Changes what card is active and also teleports player charachter to that location
    private void changeActiveCard(int answerNumber) {
        int disableNumber = (answerNumber + 1) % cardHolders.Length;
        cardHolders[disableNumber].SetActive(false);

        Vector2 runFromVector = Vector2.zero; // Temp value before switch

        switch (disableNumber + 1) {
            case 1:
                runFromVector = cams[0].ScreenToWorldPoint(new Vector3(-10, cams[0].pixelHeight / 2));
                break;
            case 2:
                runFromVector = cams[0].ScreenToWorldPoint(new Vector3(cams[0].pixelWidth / 2, cams[0].pixelHeight + 10));
                break;
            case 3:
                runFromVector = cams[0].ScreenToWorldPoint(new Vector3(cams[0].pixelWidth + 10, cams[0].pixelHeight / 2));
                break;
            case 4:
                runFromVector = cams[0].ScreenToWorldPoint(new Vector3(cams[0].pixelWidth / 2, -10));
                break;
            default: Debug.Log("This should never happen! If you see this report it!!");
                break;
        }

        playerCharachter.position = runFromVector;
    } 
}
