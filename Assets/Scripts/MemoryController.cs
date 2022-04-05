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
        OrderSounds();
        AddSound();


        gameGuesses = gamePuzzles.Count / 2;
    }

    void GetButtons(){
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        AudioSource audioSource;
        for (int i = 0; i < objects.Length; i++){

            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
            audioSource = objects[i].AddComponent<AudioSource>();
            


        }
    }
    void AddSound()
    {
        for (int i = 0; i < btns.Count; i++)
        {
            Debug.Log("H");
            btns[i].GetComponent<AudioSource>().clip = sounds[i];
            
        }

    }


    void OrderSounds() {
        AudioClip[] soundsOrdered = new AudioClip[gamePuzzles.Count];
        for (int i = 0; i < gamePuzzles.Count; i++) {
            Sprite sprite = gamePuzzles[i];
            foreach (AudioClip clip in sounds) {
                if (clip.name == sprite.name) {
                    soundsOrdered[i] = clip;
                }
            }
            if (soundsOrdered[i] == null) {
                Debug.Log(sprite.name + ": Fanns ej som ljud");
            }
        }
        sounds = soundsOrdered;
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
    IEnumerator SoundStop(int index)
    {
        btns[index].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        btns[index].GetComponent<AudioSource>().Stop();

    }
    public void PickAPuzzle(){
        if (!firstGuess){
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
           
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
            StartCoroutine(SoundStop(firstGuessIndex));
            

        }
        else if (!secondGuess){
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name; 

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            StartCoroutine(SoundStop(secondGuessIndex));

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
