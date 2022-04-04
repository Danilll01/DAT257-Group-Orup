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

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // Generate 3 answers with one correct answer of them
    public void GenerateAnswers(int rightAnswer) {
        Debug.Log("Fungerar!!");
    }
}
