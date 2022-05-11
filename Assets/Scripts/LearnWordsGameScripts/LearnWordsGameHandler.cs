using System.Collections;
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

    [SerializeField] private bool checkAnswerDirectly = false;
    [SerializeField] private float timeUntillReset = 1f;

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
    public void reDrawLines() {
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

        makeGuess(imgCard, letterCard);
    }

    public void makeGuess(GameObject imgCard, GameObject letterCard) {
        // If two are selected add them to the dictionary (This also allows the player to remap answers)
        if (imgCard != null && letterCard != null) {
            selectedAnswers[imgCard] = letterCard;

            // Make them have normal color again
            imgCard.GetComponent<Image>().color = new Color(1, 1, 1);
            letterCard.GetComponent<Image>().color = new Color(1, 1, 1);

            reDrawLines(); // Uppdate the lines based on new selected answers

            imgController.haveMatched();
            letterController.haveMatched();

            // Toggle to switch between if the answers should be checked directly / later
            if (checkAnswerDirectly) {
                CheckIfCorrect();
            }
        }
    }

    // Check the selected answers
    public void CheckIfCorrect() {

        int totalRight = 0;
        Dictionary<GameObject, GameObject> remove = new Dictionary<GameObject, GameObject>();

        // Counts how many right / wrong answers the player has made
        foreach (KeyValuePair<GameObject, GameObject> fromTo in selectedAnswers) {

            // If they are right
            if (fromTo.Key.GetComponentInChildren<Text>().text == fromTo.Value.GetComponentInChildren<TextMeshProUGUI>().text) {
                totalRight++;

                // Make the right answers green
                fromTo.Key.GetComponent<Image>().color = new Color(100f / 255f, 1, 100f / 255f);
                fromTo.Value.GetComponent<Image>().color = new Color(100f / 255f, 1, 100f / 255f);

            } else {

                // Make the wrong answers red
                fromTo.Key.GetComponent<Image>().color = new Color(1, 90f / 255f, 90f / 255f);
                fromTo.Value.GetComponent<Image>().color = new Color(1, 90f / 255f, 90f / 255f);
                remove[fromTo.Key] = fromTo.Value; // Add them to be removed later
            }
        }

        // Generates new cards / re-draws lines if there where some wrong answers
        if (totalRight == randomCardAmount) {
            generateNewCards();
        } else {
            StartCoroutine(makeNormalCardAgain(remove));
            reDrawLines();
        }

    }

    // Resets wrong answers later
    private IEnumerator makeNormalCardAgain(Dictionary<GameObject, GameObject> tobeUnDone) {
        yield return new WaitForSeconds(timeUntillReset); // Wait the time
        
        foreach (KeyValuePair<GameObject, GameObject> fromTo in tobeUnDone) {

            // If some of the key value pair is red and they still hold the same connection in dictionary
            if (fromTo.Value == selectedAnswers[fromTo.Key]) {
                selectedAnswers.Remove(fromTo.Key);
            }

            // Uncolor the objects
            unColor(fromTo.Key);
            unColor(fromTo.Value);

        }
        // Draw the new lines
        reDrawLines();
    }

    // If the given game object is red, turn it normal again
    private void unColor(GameObject colorObject) {
        if (colorObject.GetComponent<Image>().color == new Color(1, 90f / 255f, 90f / 255f)) {
            colorObject.GetComponent<Image>().color = new Color(1, 1, 1);
        }
    }

    public LineRenderer GetAvalibleLine(Vector3 nearPos) {
        LineRenderer minDistanceLine = null;
        float minDistance = float.MaxValue;
        Vector3 comparePos = Camera.main.ScreenToWorldPoint(Vector3.left);

        foreach (GameObject line in lines) {

            LineRenderer renderer = line.GetComponent<LineRenderer>();
            Vector3[] pos = new Vector3[2];
            renderer.GetPositions(pos);

            if (pos[0].x == comparePos.x && pos[1].x == comparePos.x) {
                return renderer;
            }

            float tempDistance1 = Vector2.Distance(pos[0], nearPos);
            float tempDistance2 = Vector2.Distance(pos[1], nearPos);
            if (tempDistance1 < minDistance || tempDistance2 < minDistance) {
                minDistanceLine = renderer;
                minDistance = (tempDistance1 < tempDistance2 ? tempDistance1 : tempDistance2);
            }  
        }
        return minDistanceLine;
    }
}
