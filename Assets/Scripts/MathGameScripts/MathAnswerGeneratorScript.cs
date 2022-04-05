using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathAnswerGeneratorScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI answer1txt;
    [SerializeField] private TextMeshProUGUI answer2txt;
    [SerializeField] private TextMeshProUGUI answer3txt;

    [SerializeField] private Sprite[] apples;
    [SerializeField] private Image imgHolder1;
    [SerializeField] private Image imgHolder2;
    [SerializeField] private Image imgHolder3;

    [SerializeField] private int rangeFromCorrectAnswer = 5;

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
                break;
            case 1:
                answer1txt.text = "= " + firstWrong;
                answer2txt.text = "= " + rightAnswer;
                answer3txt.text = "= " + secondWrong;
                break;
            default:
                answer1txt.text = "= " + firstWrong;
                answer2txt.text = "= " + secondWrong;
                answer3txt.text = "= " + rightAnswer;
                break;
        }
    }

    // Generates a new random number that is non negative and not the same as "rightAnswer" 
    private int generateRandomNumber(int rightAnswer) {
        int bonus;
        do {
            // Randomize a number between -5 to 5
            bonus = Random.Range(0 - rangeFromCorrectAnswer, 1 + rangeFromCorrectAnswer);
        } while (rightAnswer + bonus < 0 || bonus == 0); // Ensures that the new generated answer is non negative and is not the same as the answer

        // Returns the new wrong answer
        return rightAnswer + bonus; 
    }
}
