using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MemoryController : MonoBehaviour {
    // Field for choosing the background image of the cards
    [SerializeField]
    private Sprite bgImage;

    // Array for all the puzzle-pictures, including those not in the current game
    public Sprite[] puzzles;

    // List of the puzzle-pictures in the current game
    public List<Sprite> gamePuzzles = new List<Sprite>();

    [SerializeField]
    private GameObject cardsParent;

    private GameObject[] cards;

    // List with the buttons (cards) 
    public List<Button> btns = new List<Button>();

    // Array with the different sounds
    public AudioClip[] sounds;

    // If the first guess has been done or not
    private bool firstGuess;

    // If the second guess has been done or not
    private bool secondGuess;

    // Counts the number of guesses made
    private int countGuesses;

    // Number of correct guesses. Used for checking if game is won
    private int countCorrectGuesses;

    // Lowest number of guesses possible, number of pairs
    private int gameGuesses;

    // Index for card of the first guess
    private int firstGuessIndex;

    // Index for card of the second guess
    private int secondGuessIndex;

    // Name of the picture for the first guess
    private string firstGuessPuzzle;

    // Name of the picture for the second guess
    private string secondGuessPuzzle;

    // Meny som är inaktiv vid start men som aktiveras när spelet är vunnet
    [SerializeField]
    private GameObject finishMenu;

    [SerializeField]
    private AudioSource rightAnswerSound;

    // Pointer for Audio Source
    private AudioSource victorySound;

    // Runs at start. Loads all pictures and sounds
    void Awake() {
        // If no sprites are provided use fallback method to load sprites
        if (puzzles.Length == 0) puzzles = Resources.LoadAll<Sprite>("Sprites/Animals");

        // If no sounds are provided use fallback method to load sounds
        if (sounds.Length == 0) sounds = Resources.LoadAll<AudioClip>("Animal_sounds/Edited_Sounds");

        // Gets the AudioSource component and attaches to the pointer
        victorySound = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start() {
        InitializeCardsArray();

        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        OrderSounds();
        AddSound();

        AddImageToCards();
        cardsParent.SetActive(true);

        finishMenu.SetActive(false);

        // Number of guesses in the game is equal to half the cards
        gameGuesses = gamePuzzles.Count / 2;
    }

    // Initializes card array
    private void InitializeCardsArray()
    {
        // Get number of snap points
        int nrCards = cardsParent.transform.childCount;
        cards = new GameObject[nrCards];

        // Add all snap points to array
        for (int i = 0; i < nrCards; i++)
        {
            GameObject child = cardsParent.transform.GetChild(i).gameObject;
            cards[i] = child;
        }
    }

    // Gets buttons added by AddButtons-script and adds to btns-list. Attaches background picture and soundcomponent
    void GetButtons() {

        // Finds buttons 
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++) {

            // Adds buttons to button-list
            btns.Add(objects[i].GetComponent<Button>());

            // Adds audio component
            objects[i].AddComponent<AudioSource>();

        }
    }

    // Adds AudioClip to the button's audio component for each button
    void AddSound() {
        for (int i = 0; i < btns.Count; i++) {
            btns[i].GetComponent<AudioSource>().clip = sounds[i];
        }
    }

    // Matches the sound to the correct animal
    void OrderSounds() {
        AudioClip[] soundsOrdered = new AudioClip[gamePuzzles.Count];

        // Loops through each puzzle in the game
        for (int i = 0; i < gamePuzzles.Count; i++) {
            Sprite sprite = gamePuzzles[i];

            // For each puzzle loops through audiofiles
            foreach (AudioClip clip in sounds) {

                // If filenames match, change index of audiofile to index of picture
                if (clip.name.ToLower() == sprite.name.ToLower()) {
                    soundsOrdered[i] = clip;
                }
            }

            if (soundsOrdered[i] == null) {
                Debug.Log(sprite.name + ": Fanns ej som ljud");
            }
        }
        sounds = soundsOrdered;
    }

    // Gives random indexes for choosing puzzle pictures
    HashSet<int> RandomRange(int elements, int ceiling) {

        // Create set of only possible of containing unique values
        HashSet<int> indexSet = new HashSet<int>();

        // Fills set with indexes until it matches number of cards
        while (indexSet.Count < elements) {

            // Gets random number between index 0 and and max number
            int rand = Random.Range(0, ceiling);
            indexSet.Add(rand);
        }
        return indexSet;
    }

    // Adds puzzles to the current game from the available puzzles
    void AddGamePuzzles() {

        // Gets indexes of pictures for half the number of cards since there shoud be two of each
        // Max index value is final index if the puzzles available
        foreach (int a in RandomRange(btns.Count / 2, puzzles.Length)) {
            gamePuzzles.Add(puzzles[a]);
            gamePuzzles.Add(puzzles[a]);
        }
    }

    // Adds listeners for button presses
    void AddListeners() {
        foreach (Button btn in btns) {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void HomeMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain() {
        SceneManager.LoadScene("MemoryGame");
    }

    // Coroutine for deciding how long sounds should play
    IEnumerator SoundStop(int index) {
        btns[index].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        btns[index].GetComponent<AudioSource>().Stop();

    }

    // Checks if first and second guess has been done or not
    // Changes picture from backside to animal
    // Plays sound bu using coroutine
    // Starts coroutine for checking if guesses match
    public void PickAPuzzle() {

        // If first guess hasn't been made 
        if (!firstGuess) {
            firstGuess = true;

            // Sets index for first guess as name of the card chosen
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            // Puzzle of first guess is the filename of the picture on the card
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            // Sets image of the first guess to the image corresponding to the index
            //btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];

            cards[firstGuessIndex].GetComponent<FlipCard>().FlipCardToAnimalState();

            // Plays the audiofile of animal
            StartCoroutine(SoundStop(firstGuessIndex));
        }
        // Same as if-clause but with second
        else if (!secondGuess && (firstGuessIndex != int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name))) {
            secondGuess = true;

            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            //btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            cards[secondGuessIndex].GetComponent<FlipCard>().FlipCardToAnimalState();

            StartCoroutine(SoundStop(secondGuessIndex));

            // Counts up number of guesses made
            countGuesses++;

            // Checks if puzzles match 
            StartCoroutine(CheckIfPuzzlesMatch());
        }
        else {
            StartCoroutine(SoundStop(secondGuessIndex));
        }
    }

    // Checks if puzzles match by checking the filename of picture
    // Makes sure they are different cards through second condition
    // Makes matching cards uninteractable and greyed out
    IEnumerator CheckIfPuzzlesMatch() {
        yield return new WaitForSeconds(1f);

        // Makes sure images are the same, but the cards are different
        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex) {

            yield return new WaitForSeconds(0.25f);
            rightAnswerSound.Play();    
            // Makes correct guesses uninteractable
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            /*  Om korten ska försvinna efter man har valt rätt 
            btns[firstGuessIndex].image.color = new Color(0,0,0,0);
            btns[secondGuessIndex].image.color = new Color(0,0,0,0);
            */

            // Checks if there are cards left to guess
            CheckIfTheGameIsFinished();
        }

        // Sets image back to background image of card if guess was wrong
        else {
            cards[firstGuessIndex].GetComponent<FlipCard>().FlipCardToOriginalState();
            cards[secondGuessIndex].GetComponent<FlipCard>().FlipCardToOriginalState();
        }
        yield return new WaitForSeconds(0.5f);


        // Restarts guessing process
        firstGuess = false;
        secondGuess = false;

    }

    // To be made. Checks if correctGuesses is equal to gameGuesses
    void CheckIfTheGameIsFinished() {

        // Increments number of correct guesses
        countCorrectGuesses++;

        // Activates the menu if game is finished and plays victory sound
        if (countCorrectGuesses == gameGuesses) {
            // Show victory screen
            finishMenu.SetActive(true);

            // Hide all cards
            cardsParent.SetActive(false);

            StartCoroutine(PlayVictorySound());
        }
    }

    // Plays the sound after winning the game
    IEnumerator PlayVictorySound() {
        // Wait one second before playing to not intefere with sound of matching final pair of cards
        yield return new WaitForSeconds(1f);
        victorySound.Play();
    }

    // Shuffles cards
    void Shuffle(List<Sprite> list) {

        // For every sprite in given list
        for (int i = 0; i < list.Count; i++) {

            // Make temporary sprite
            Sprite temp = list[i];

            // Get random index between current and max index
            int randomIndex = Random.Range(i, list.Count);

            // Sets sprite on current index to sprite on the random index
            list[i] = list[randomIndex];

            // Sets sprite on random index to sprite on current index
            list[randomIndex] = temp;
        }

    }

    // Assign an image to every card
    private void AddImageToCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            int imgIndex = cards[i].transform.childCount - 1;

            // Set image on card to corrosponding image in the puzzle 
            cards[i].transform.GetChild(imgIndex).GetComponent<SpriteRenderer>().sprite = gamePuzzles[i];
        }
    }
}
