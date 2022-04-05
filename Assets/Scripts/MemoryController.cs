using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryController : MonoBehaviour
{
    
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    public AudioClip[] sounds;


    private bool firstGuess;
    private bool secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex;
    private int secondGuessIndex;

    private string firstGuessPuzzle;
    private string secondGuessPuzzle;


    void Awake(){
        puzzles = Resources.LoadAll<Sprite>("Sprites/Animals");
        sounds = Resources.LoadAll<AudioClip>("Animal_sounds");
    }

    
    // Start is called before the first frame update
    void Start(){
        GetButtons();
        AddListeners();
        AddGamePuzzles();
    }

    void GetButtons(){
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        //GameObject[] audioObjects = new GameObject[20];
        AudioSource audioSource;
        for (int i = 0; i < objects.Length; i++){

            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;

            audioSource = objects[i].AddComponent<AudioSource>();
            audioSource.clip = sounds[i % 10];
            


        }
    }

    void AddGamePuzzles(){
        int counter = btns.Count;
        int index = 0;

        for (int i = 0; i < counter; i++){
            if (index == counter / 2){
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners(){
        foreach (Button btn in btns){
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle(){
        if (!firstGuess){
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
           
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
            btns[firstGuessIndex].GetComponent<AudioSource>().Play();

        } else if (!secondGuess){
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            btns[secondGuessIndex].GetComponent<AudioSource>().Play();

        }

    }
}
