using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathQuestionGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textInput;
    [SerializeField] private TextMeshProUGUI operatorText;
    [SerializeField] private Sprite[] fruits;

    [SerializeField] private FillCardImages imageHolder1;
    [SerializeField] private FillCardImages imageHolder2;
    [SerializeField] private MathAnswerGeneratorScript answerGenerator;

    private int x;
    private int y;
    private string[] operators;
    private string op;
    
    private void Start() {
        // Setup valid operators
        operators = new string[]{"+", "+", "+", "+", "-" };
        randomizeProblem();
    }

    public void randomizeProblem(){
        // Randomizes two numbers
        x = Random.Range(0,10);
        y = Random.Range(0,10);

        // Randomizes a operator from the operator list
        op = operators[Random.Range(0,operators.Length)];

        // Calculates answer
        int result = 0;
        switch (op){
            case "+": 
                result = (x+y);
                break;
            case "-": 
                result = (x-y);
               
                break;
            default: 
                Debug.Log("Invalid operator");
                break;
        }

        if(result < 0){
            randomizeProblem(); // Redo if under zero
        }
        else { // Otherwise show the result and call to generate 3 answers
            textInput.text = x + " " + op + " " + y + " = " + result;
            operatorText.text = op;

            // Place randomized sprites on card
            Sprite generatedSprite = fruits[Random.Range(0, fruits.Length)];
            imageHolder1.FillCard(x, generatedSprite);
            imageHolder2.FillCard(y, generatedSprite);

            answerGenerator.GenerateAnswers(result, generatedSprite);
        }
            
        
    }
}
