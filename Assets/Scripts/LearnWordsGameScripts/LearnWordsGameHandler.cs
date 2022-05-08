using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearnWordsGameHandler : MonoBehaviour
{

    [SerializeField] private ImageController imgController;
    [SerializeField] private LetterController letterController;

    // The images and matching words must be in the same order. Otherwise it will not work correctly
    [SerializeField] private List<Sprite> images;
    [SerializeField] private List<string> matchingWord;

    private List<GameObject> lines;
    private int randomCardAmount;
    //private int rightAnswerCounter = 0;
    private Dictionary<GameObject, GameObject> selectedAnswers = new Dictionary<GameObject, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        randomCardAmount = imgController.getCardAmount(); // The amount of cards to randomize sprites and words to 
        lines = new List<GameObject>();
        generateNewCards();
        
    }


    // Generate new information to later be placed on the image cards
    public void generateNewCards() {
        // Removes lines
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
        selectedAnswers.Clear();

        // Creates the structure we use to send data with
        List<System.Tuple<Sprite, string>> randList = new List<System.Tuple<Sprite, string>>(); 

        // temp lists to help with randomizing the order of new card data (without duplicates)
        List<Sprite> tempImages = new List<Sprite>(images);
        List<string> tempMatchingWord = new List<string>(matchingWord);

        // Randomize card data for all cards
        for (int i = 0; i < randomCardAmount; i++) {
            int index = Random.Range(0, tempImages.Count); // Randomize a index in the lists
            randList.Add(System.Tuple.Create(tempImages[index], tempMatchingWord[index])); // Place data from the index into the sending structure
            
            // Removes items to avoid duplicates
            tempImages.RemoveAt(index);
            tempMatchingWord.RemoveAt(index);

            // Creates a line to use later
            createLines();
        }

        imgController.placeInformationInCard(randList);
        letterController.placeInformationInCard(randList);
    }

    // Calls to create a line outside the screen
    private void createLines() {
        drawLine(Camera.main.ScreenToWorldPoint(Vector3.left), Camera.main.ScreenToWorldPoint(Vector3.left));
    }

    // Creates a line between the given start/stop vectors
    private void drawLine(Vector3 start, Vector3 end) {
        //For creating line renderer object
        LineRenderer lineRenderer = new LineRenderer();
        GameObject line = new GameObject("Line");
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = GetComponent<MeshRenderer>().material;

        //For drawing line in the world space, provide the x,y,z values
        lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0)); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0)); //x,y and z position of the end point of the line

        lines.Add(line);
    }

    // Redraw the lines based on current selected answers information
    private void reDrawLines() {
        int i = 0;

        // Draw lines for the selected answers
        foreach (KeyValuePair<GameObject, GameObject> fromTo in selectedAnswers) {
            LineRenderer line = lines[i].GetComponent<LineRenderer>();
            Vector3 startPos = Camera.main.ScreenToWorldPoint(fromTo.Key.transform.position);
            Vector3 endPos = Camera.main.ScreenToWorldPoint(fromTo.Value.transform.position);

            line.SetPosition(0, new Vector3(startPos.x, startPos.y, 0));
            line.SetPosition(1, new Vector3(endPos.x, endPos.y, 0));
            i++;
        }


        // Reset the position of unused lines
        Vector3 standartPos = Camera.main.ScreenToWorldPoint(Vector3.left);
        for (int j = i; j < lines.Count; j++) {
            LineRenderer line = lines[j].GetComponent<LineRenderer>();
            line.SetPosition(0, new Vector3(standartPos.x, standartPos.y, 0));
            line.SetPosition(1, new Vector3(standartPos.x, standartPos.y, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Gets selected data from image/letter controllers
        GameObject imgCard = imgController.getSelectedImgCardInfo();
        GameObject letterCard = letterController.getSelectedLetterCardInfo();

        // If two are selected add them to the dictionary (This also allows the player to remap answers)
        if (imgCard != null && letterCard != null) {
            selectedAnswers[imgCard] = letterCard;

            // Make them have normal color again
            imgCard.GetComponent<Image>().color = new Color(1, 1, 1);
            letterCard.GetComponent<Image>().color = new Color(1, 1, 1);

            reDrawLines(); // Uppdate the lines based on new selected answers

            imgController.haveMatched();
            letterController.haveMatched();
        }
    }

    // Check the selected answers
    public void CheckIfCorrect() {

        int totalRight = 0;
        List<GameObject> remove = new List<GameObject>();

        // Counts how many right / wrong answers the player has made
        foreach (KeyValuePair<GameObject, GameObject> fromTo in selectedAnswers) {

            if (fromTo.Key.GetComponentInChildren<Text>().text == fromTo.Value.GetComponentInChildren<TextMeshProUGUI>().text) {
                totalRight++;

                // Make the right answers green
                fromTo.Key.GetComponent<Image>().color = new Color(100f / 255f, 1, 100f / 255f);
                fromTo.Value.GetComponent<Image>().color = new Color(100f / 255f, 1, 100f / 255f);

            } else {
                remove.Add(fromTo.Key);
            }
        }

        // Removes all wrong guesses from the dictionary
        foreach (GameObject rem in remove) {
            selectedAnswers.Remove(rem);
        }

        // Generates new cards / re-draws lines if there where some wrong answers
        Debug.Log("Rätt: " + totalRight);
        if (totalRight == randomCardAmount) {
            generateNewCards();
        } else {
            reDrawLines();
        }

    }
}
