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

    private List<GameObject> lines;
    private int randomCardAmount;
    private int rightAnswerCounter = 0;
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

        // Creates the structure we use to send data with
        List<System.Tuple<Sprite, string>> randList = new List<System.Tuple<Sprite, string>>(); 

        // temp lists to help with randomizing the order of new card data (without duplicates)
        List<Sprite> tempImages = new List<Sprite>(images);
        List<string> tempMatchingWord = new List<string>(matchingWord);

        // Randomize card data for all cards
        for (int i = 0; i < randomCardAmount; i++) {
            int index = Random.Range(0, tempImages.Count); // Ranomize a index in the lists
            randList.Add(System.Tuple.Create(tempImages[index], tempMatchingWord[index])); // Place data from the index into the sending structure
            
            // Removes items to avoid duplicates
            tempImages.RemoveAt(index);
            tempMatchingWord.RemoveAt(index);
        }

        imgController.placeInformationInCard(randList);
        letterController.placeInformationInCard(randList);
    }

    // Update is called once per frame
    void Update()
    {

        GameObject imgCard = imgController.getSelectedImgCardInfo();
        GameObject letterCard = letterController.getSelectedLetterCardInfo();

        if (imgCard != null && letterCard != null) {
            selectedAnswers[imgCard] = letterCard;

            drawLine(imgCard.transform, letterCard.transform);

            imgController.haveMatched();
            letterController.haveMatched();
        }







        // Checks if the currently selected cards match from each side / card base
        //if (imgController.getSelectedImgCardInfo() == letterController.getSelectedLetterCardInfo()) {
        //    drawLine(imgController.getSelectedImgTransform(), letterController.getSelectedLetterTransform());
        //    imgController.haveMatched();
        //    letterController.haveMatched();
        //    checkIfAllAnswers();
        //}
    }

    private void drawLine(Transform start, Transform end)
    {
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

        Vector3 startPos = Camera.main.ScreenToWorldPoint(start.position);
        Vector3 endPos = Camera.main.ScreenToWorldPoint(end.position);

        //For drawing line in the world space, provide the x,y,z values
        lineRenderer.SetPosition(0, new Vector3(startPos.x, startPos.y, 0)); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, new Vector3(endPos.x, endPos.y, 0)); //x,y and z position of the end point of the line

        lines.Add(line);
    }

    // Checks if all cards have been answered correctly and if so, calls to genereate new ones
    private void checkIfAllAnswers() {
        rightAnswerCounter++;
        
        if (rightAnswerCounter == randomCardAmount) {
            rightAnswerCounter = 0;
            generateNewCards();
        } 
    }

    public void CheckIfCorrect() {

        int totalRight = 0;
        List<GameObject> remove = new List<GameObject>();

        foreach (KeyValuePair<GameObject, GameObject> fromTo in selectedAnswers) {
            Debug.Log("Image" + fromTo.Key.GetComponentInChildren<Text>().text);
            Debug.Log("Letter" + fromTo.Value.GetComponentInChildren<TextMeshProUGUI>().text);

            if (fromTo.Key.GetComponentInChildren<Text>().text == fromTo.Value.GetComponentInChildren<TextMeshProUGUI>().text) {
                totalRight++;
            } else {
                remove.Add(fromTo.Key);
                // Also remove line here
            }
        }

        foreach (GameObject rem in remove) {
            selectedAnswers.Remove(rem);
        }

        Debug.Log("Rätt: " + totalRight);
        if (totalRight == randomCardAmount) {
            selectedAnswers = new Dictionary<GameObject, GameObject>();
            generateNewCards();
        }

    }
}
