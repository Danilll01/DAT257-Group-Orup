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
        Shuffle(gamePuzzles); 
        gameGuesses = gamePuzzles.Count / 2;
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

    HashSet<int> RandomRange(int elements, int ceiling){
        HashSet<int> indexSet = new HashSet<int>();

        while(indexSet.Count < elements){
            int rand = Random.Range(0, ceiling);
            indexSet.Add(rand);
        }
        return indexSet;
    }

    void AddGamePuzzles(){
        foreach (int a in RandomRange(btns.Count / 2, puzzles.Length)){
            gamePuzzles.Add(puzzles[a]);
            gamePuzzles.Add(puzzles[a]);
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
           
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
            btns[firstGuessIndex].GetComponent<AudioSource>().Play();

        } else if (!secondGuess){
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name; 

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            btns[secondGuessIndex].GetComponent<AudioSource>().Play();

            countGuesses++;

            StartCoroutine(CheckIfPuzzlesMatch());
        }
    }

    IEnumerator CheckIfPuzzlesMatch(){
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex){
            
            yield return new WaitForSeconds(0.25f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;


            /*  Om korten ska försvinna efter man har valt rätt 
            btns[firstGuessIndex].image.color = new Color(0,0,0,0);
            btns[secondGuessIndex].image.color = new Color(0,0,0,0);
            */

            CheckIfTheGameIsFinished();
        } else {
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(0.5f);

        firstGuess = false;
        secondGuess = false; 

    }

    void CheckIfTheGameIsFinished(){
       
    }

    void Shuffle(List<Sprite> list){
        for (int i = 0; i < list.Count; i++){
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

    }
}
