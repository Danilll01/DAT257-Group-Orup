using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathAnswerGeneratorScript : MonoBehaviour {
    [SerializeField] private MathQuestionGenerator questionGenerator;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private MainSoundPlayer soundPlayer;

    [SerializeField] private TextMeshProUGUI[] answerTxts;

    [SerializeField] private GameObject[] cardHolders;

    [SerializeField] private Transform[] fillImageCard;


    [SerializeField] private int rangeFromCorrectAnswer = 5;
    [SerializeField] private float fadeInTimer = 1;
    private int correctAnswerHolder;


    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // Generate 3 answers with one correct answer of them
    public void GenerateAnswers(int rightAnswer, Sprite fruitImage) {
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
                fillAnswers(answers, rightAnswer, fruitImage);

                break;
            case 1:

                int[] answers2 = { firstWrong, rightAnswer, secondWrong };
                fillAnswers(answers2, rightAnswer, fruitImage);

                break;
            default:

                int[] answers3 = { firstWrong, secondWrong, rightAnswer };
                fillAnswers(answers3, rightAnswer, fruitImage);

                break;
        }
    }

    // Fills active cardHolders with the given answers
    private void fillAnswers(int[] answers, int correctAnswer, Sprite fruitImage) {

        int answerCounter = 0; // Help count what answer to set

        for (int i = 0; i < cardHolders.Length; i++) {
            if (cardHolders[i].activeSelf && answerCounter < cardHolders.Length - 1) { // Skips the inactive card
                int currentWriting = answers[answerCounter];
                answerTxts[i].text = "= " + currentWriting;

                fillImageCard[i].GetComponent<FillCardImages>().FillCard(currentWriting, fruitImage);
                soundPlayer.fillFromNewAnswer(i + 1, currentWriting); // Makes the sound player have the correct information for this card
                
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
    public void AnswerPressedRight(int answerNumber) {

        // Make all card holders active and interactable again
        foreach (GameObject card in cardHolders) {
            card.GetComponent<Image>().color = new Color(1,1,1); // No color tint 
            card.GetComponent<Button>().interactable = true;
            card.SetActive(true);
        }

        // Changes active cards and teleports player to new location
        changeActiveCard(answerNumber);

        // And randomize a new question
        questionGenerator.randomizeProblem();

        StartCoroutine(makeAnswersFadeIn());
    }

    // Changes what card is active
    private void changeActiveCard(int answerNumber) {
        int disableNumber = (answerNumber + 1) % cardHolders.Length;
        cardHolders[disableNumber].SetActive(false);
    }

    // Hides all wrong answers and highlights the correct answer in green
    public void MakeCorrectAnswerGreen() {
        for (int i = 0; i < cardHolders.Length; i++) {
            if (i == correctAnswerHolder - 1) {
                cardHolders[i].GetComponent<Button>().interactable = false;
                cardHolders[i].GetComponent<Image>().color = new Color(100f / 255f, 1, 100f / 255f); // Makes it green
            } else {
                cardHolders[i].SetActive(false);
            }
        }
    }

    public void AnswerPressedWrong(int answerNumber) {
        // Make the selected card red to show that it is wrong and dissable button
        cardHolders[answerNumber - 1].GetComponent<Image>().color = new Color(1, 40f / 255f, 0);
        cardHolders[answerNumber - 1].GetComponent<Button>().interactable = false;
        canvas.alpha = 0;
        canvas.interactable = false;
    }

    // Activates the canvas object
    public void ActivateCanvas() {
        canvas.alpha = 1;
        canvas.interactable = true;
    }

    // Hides the interactable canvas
    public void HideCanvas() {
        canvas.alpha = 0;
    }

    // Returns if its the right path 
    public bool IsCorrectAnswer(int whatPath) {
        return whatPath == correctAnswerHolder;
    }

    private IEnumerator makeAnswersFadeIn() {

        // Fade in answers
        float timer = 0;
        while (timer <= fadeInTimer) {
            canvas.alpha = Mathf.Lerp(0, 1, timer / fadeInTimer);
            timer += Time.deltaTime;
            yield return null;
        }
        canvas.interactable = true;
        canvas.alpha = 1;
    }
}
