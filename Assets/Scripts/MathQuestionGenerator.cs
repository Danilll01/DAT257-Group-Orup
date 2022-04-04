using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathQuestionGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textInput;
    [SerializeField] private TextMeshProUGUI operatorText;
    [SerializeField] private Sprite[] apples;
    [SerializeField] private Image imageHolder1;
    [SerializeField] private Image imageHolder2;
    private int x;
    private int y;
    private string[] operators;
    private string op;
    
    private void Start() {
        operators = new string[]{"+","-"};
    }

    public void randomizeProblem(){
        x = Random.Range(0,10);
        y = Random.Range(0,10);

        op = operators[Random.Range(0,operators.Length)];
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
            randomizeProblem();
        }
        else{
            textInput.text = x + " " + op + " " + y + " = " + result;
            operatorText.text = op;
            imageHolder1.sprite = apples[x];
            imageHolder2.sprite = apples[y];
        }
            
        
    }
}
