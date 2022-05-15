using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MainSoundPlayer : MonoBehaviour
{

    [SerializeField] private AudioClip[] numbers;
    [SerializeField] private AudioClip plus;
    [SerializeField] private AudioClip minus;
    [SerializeField] private AudioClip equals;
    private int[] mathNumbers;
    private string mathOperator;
    private Dictionary<int, int> mathAnswers = new Dictionary<int,int>();

    private bool isPlaying = false;

    private AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void fillFromNewExercise(int[] mathNums, string mathOp) {
        mathNumbers = mathNums;
        mathOperator = mathOp;
    }

    public void fillFromNewAnswer(int cardNumber, int answer) {
        mathAnswers[cardNumber] = answer;
    }

    public void playExerciseSound() {
        if (!isPlaying) {
            isPlaying = true;
            StartCoroutine(playExercise());
        }
        
    }

    public void playAnswerSound(int cardNumber) {
        if (!isPlaying) {
            isPlaying = true;
            StartCoroutine(playAnswer(cardNumber));
        }

    }

    private IEnumerator playExercise() {
        audioSource.PlayOneShot(numbers[mathNumbers[0]]);
        yield return new WaitForSeconds(numbers[mathNumbers[0]].length);
        AudioClip clip = getOperatorSound();
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        audioSource.PlayOneShot(numbers[mathNumbers[1]]);
        yield return new WaitForSeconds(numbers[mathNumbers[1]].length);
        audioSource.PlayOneShot(equals);

        yield return new WaitForSeconds(equals.length);
        isPlaying = false;
    }

    private AudioClip getOperatorSound() {
        return mathOperator switch {
            "+" => plus,
            "-" => minus,
            _ => null,
        };
    }

    private IEnumerator playAnswer(int cardNumber) {
        audioSource.PlayOneShot(numbers[mathAnswers[cardNumber]]);


        yield return new WaitForSeconds(equals.length);
        isPlaying = false;
    }
}
