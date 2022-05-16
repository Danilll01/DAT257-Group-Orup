using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateClothes : MonoBehaviour
{
    [SerializeField] private GameObject[] snapPoints;
    private WeatherController.WeatherTypes weather;
    [SerializeField] private Animator foxAnim;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Transform doorPos;
    [SerializeField] private Transform foxPos;
    private bool weatherSet;

    [SerializeField] private DragAndDropClothing dragScript;
    [SerializeField] private speechBubble bubbleScript;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject startMenuButton;

    private bool walkOut;
    [SerializeField] private float speed = 10f;

    //[SerializeField] private GameObject fox;
    private Vector3 foxOriginPos;


    void Start(){
        weatherSet = false;
        walkOut = false;

        foxOriginPos = foxPos.transform.position;

        // Set the doors position to be at the edge of the camera no matter what device
        doorPos.transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3((Camera.main.pixelWidth),0));
        Vector3 doorParent = doorPos.transform.parent.position;
        // Set a little offset on the door to bring it back in frame
        doorPos.transform.parent.position = new Vector3(doorPos.position.x - 8f,doorPos.position.y + 12.563f);
    }

    void Update()
    {
        // If fox is walking out
        if (walkOut)
        {
            float step = speed * Time.deltaTime;
            startMenuButton.SetActive(false);

            // move fox towards the door location
            foxPos.position = Vector2.MoveTowards(foxPos.position, doorPos.position, step);

            // If the box has reached the door, close the door
            if (Vector2.Distance(foxPos.position, doorPos.position) < 5f)
            {
                doorAnim.SetTrigger("closeDoor");
                foxAnim.SetTrigger("stopWalking");
                
            }

            if (foxPos.position.x == doorPos.position.x)
            {
                endMenu.SetActive(true);
                walkOut = false;
            }
        }

    }

    // Check if the clothes on the snappoints are valid with the current weather
    public void checkClothing(){
        bool validClothing = true;
        string message = "";
        List<DragAndDropClothing> allScripts = new List<DragAndDropClothing>();

        // If the weather is set, meaning we know what weather it is
        if (weatherSet){
            // Loop through all snapPoints
            foreach(GameObject snapPoint in snapPoints){
                if(snapPoint.transform.childCount > 0){
                    // Get the DragAndDrop scripts on the clothing object
                    DragAndDropClothing[] scripts = snapPoint.GetComponentsInChildren<DragAndDropClothing>();
                    allScripts.AddRange(scripts);

                    // If the clothings weather does not match current weather
                    // send to script to remove it from character
                    foreach (DragAndDropClothing script in scripts)
                    {
                        // If the chosen weather is not marked in the editor and the clothing is not of type any
                        // then we remove it from the snapPoint
                        if (!((script.chosenWeather & weather) != WeatherController.WeatherTypes.None))
                        {
                            script.removeFromSnapPoint(false);
                            validClothing = false;
                            message = "Some clothing does not match the weather";
                        }

                    }
                     // If it is not sunny, we need a jacket
                    if (weather != WeatherController.WeatherTypes.Sun && snapPoint.name == "Chest" && snapPoint.transform.childCount < 2)
                    {
                        validClothing = false;
                        message = "Need a jacket in this weather";
                    }
                }

                // Check what snapPoints need clothing depending on weather
                else
                {

                    string name = snapPoint.name;

                    // These points always need one clothing item on them
                    if (name == "Chest" || name == "Legs" || name == "Feet")
                    {
                        validClothing = false;
                        message = "Missing shirt, pants or shoes";
                    }

                    // If it is raining, we need a hat and gloves
                    else if (weather == WeatherController.WeatherTypes.Rain && (name == "Head" || name == "Hand"))
                    {
                        validClothing = false;
                        message = "Missing hat or gloves in the rain";
                    }

                    // If it is snowing, we need a hat, gloves and a scarf
                    else if (weather == WeatherController.WeatherTypes.Snow && (name == "Head" || name == "Hand" || name == "Throat"))
                    {
                        validClothing = false;
                        message = "Missing hat, gloves or scarf in the snow weather";
                    }

                }

            }

            // If one clothing was not valid
            if(validClothing){
                Debug.Log("Valid");
                StartCoroutine(disableClothings(allScripts));
                //disableClothing(allScripts);
                bubbleScript.showBubble("Härligt, nu kan jag gå ut och leka! ", 1);

            }
            else{
                Debug.Log("Invalid: " + message);
                // Activate speech bubble
                bubbleScript.showBubble("Ojdå det blev nog lite tokigt.Försök igen!", 0);
            }
        }
        else{
            Debug.Log("Weather not set");
        }


    }


    // Sets the weather variable to current weather
    public void setWeather(WeatherController.WeatherTypes newWeather){
        weather = newWeather;
        weatherSet = true;
    }

    IEnumerator disableClothings(List<DragAndDropClothing> allScripts)
    {
        // Disable touch when fox is going to walk out
        dragScript.setEnding(true);

        yield return new WaitForSeconds(2.5f);

        // Start door opening animation
        doorAnim.SetTrigger("openDoor");
        // Set rigidbody for clothing objects on character to static
        // this will allow the clothes to follow the character when walking out
        foreach (var script in allScripts)
        {
            script.ridgidBody.bodyType = RigidbodyType2D.Static;
        }
        // Start fox walking animation

        foxAnim.SetTrigger("walkOut");
        walkOut = true;
    }

    private void disableClothing(List<DragAndDropClothing> allScripts)
    {
        // Disable touch when fox is going to walk out
        dragScript.setEnding(true);
        // Start door opening animation
        doorAnim.SetTrigger("openDoor");
        // Set rigidbody for clothing objects on character to static
        // this will allow the clothes to follow the character when walking out
        foreach (var script in allScripts)
        {
            script.ridgidBody.bodyType = RigidbodyType2D.Static;
        }
        // Start fox walking animation
        
        foxAnim.SetTrigger("walkOut");
        walkOut = true;
    }

    public void resetAllClothes()
    {
        // Reset transform and position of fox
        foxPos.transform.position = foxOriginPos;
        foxPos.rotation = Quaternion.identity;

        // Reset the fox walk out  and door animation
        foxAnim.ResetTrigger("stopWalking");
        doorAnim.ResetTrigger("closeDoor");


        foreach (GameObject snapPoint in snapPoints)
        {

                // Gets all scripts of notes
            DragAndDropClothing[] clothes = snapPoint.transform.GetComponentsInChildren<DragAndDropClothing>();
            

                // Tell all notes to unsnap and delete itself
            foreach (DragAndDropClothing clothing in clothes)
            {
                clothing.setEnding(false);
                clothing.ridgidBody.bodyType = RigidbodyType2D.Dynamic;
                clothing.inventoryScript.AddClothingToArray(clothing.gameObject, false);
                clothing.removeFromSnapPoint(false);
                
            }
        }

    }






}
