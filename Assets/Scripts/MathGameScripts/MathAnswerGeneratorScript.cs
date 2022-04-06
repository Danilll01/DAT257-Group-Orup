using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathAnswerGeneratorScript : MonoBehaviour
{
    [SerializeField] private MathQuestionGenerator questionGenerator;

    [SerializeField] private TextMeshProUGUI answer1txt;
    [SerializeField] private TextMeshProUGUI answer2txt;
    [SerializeField] private TextMeshProUGUI answer3txt;

    [SerializeField] private Sprite[] apples;
    [SerializeField] private Image imgHolder1;
    [SerializeField] private Image imgHolder2;
    [SerializeField] private Image imgHolder3;

    [SerializeField] private Image[] cardHolders;

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

        // Writes the generated numbers to random answer boxes
        switch (Random.Range(0, 3)) {
            case 0:
                answer1txt.text = "= " + rightAnswer;
                answer2txt.text = "= " + firstWrong;
                answer3txt.text = "= " + secondWrong;
                imgHolder1.sprite = apples[rightAnswer];
                imgHolder2.sprite = apples[firstWrong];
                imgHolder3.sprite = apples[secondWrong];
                correctAnswerHolder = 1;
                break;
            case 1:
                answer1txt.text = "= " + firstWrong;
                answer2txt.text = "= " + rightAnswer;
                answer3txt.text = "= " + secondWrong;
                imgHolder1.sprite = apples[firstWrong];
                imgHolder2.sprite = apples[rightAnswer];
                imgHolder3.sprite = apples[secondWrong];
                correctAnswerHolder = 2;
                break;
            default:
                answer1txt.text = "= " + firstWrong;
                answer2txt.text = "= " + secondWrong;
                answer3txt.text = "= " + rightAnswer;
                imgHolder1.sprite = apples[firstWrong];
                imgHolder2.sprite = apples[secondWrong];
                imgHolder3.sprite = apples[rightAnswer];
                correctAnswerHolder = 3;
                break;
        }
    }

    // Generates a new random number that is non negative and not the same as "rightAnswer" 
    private int generateRandomNumber(int rightAnswer) {
        int bonus;
        do {
            // Randomize a number between -5 to 5
            bonus = Random.Range(0 - rangeFromCorrectAnswer, 1 + rangeFromCorrectAnswer);
        } while (rightAnswer + bonus < 0 || bonus == 0 || rightAnswer > 18); // Ensures that the new generated answer is non negative and is not the same as the answer

        // Returns the new wrong answer
        return rightAnswer + bonus; 
    }

    // Method to call when a answer is pressed
    public void AnswerPressed(int answerNumber) {
        if (answerNumber == correctAnswerHolder) { // Checks if the button pressed contains the correct answer
            // If so, make all card holders blue again
            foreach (Image  img in cardHolders) {
                img.color = new Color(0, 166f / 255f, 1);
            }
            // And randomize a new question
            questionGenerator.randomizeProblem();
        } else {
            // Make the selected card red to show that it is wrong
            cardHolders[answerNumber - 1].color = new Color(1, 40f/255f, 0);
        }
    }
}
