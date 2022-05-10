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

    private DragAndDropClothing dragScript;
    private bool walkOut;
    [SerializeField] private float speed = 10f;

    void Start(){
        weatherSet = false;
        dragScript = new DragAndDropClothing();
        walkOut = false;
        
        doorPos.transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3((Camera.main.pixelWidth),0));
        Vector3 doorParent = doorPos.transform.parent.position;
        doorPos.transform.parent.position = new Vector3(doorPos.position.x - 6.6f, -2.47f);
    }

    void Update()
    {
        if (walkOut)
        {
            float step = speed * Time.deltaTime;

            // move sprite towards the target location
            foxPos.position = Vector2.MoveTowards(foxPos.position, doorPos.position, step);

            if (foxPos.position.x == doorPos.position.x)
            {
                doorAnim.SetTrigger("closeDoor");
                walkOut = false;
            }
        }
        
    }

    // Check if the clothes on the snappoints are valid with the current weather
    public void checkClothing(){
        bool validClothing = true;
        string message = "";
        //DragAndDropClothing[] allScripts = null;
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
                disableClothing(allScripts);
                
            }
            else{
                Debug.Log("Invalid: " + message);
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

    private void disableClothing(List<DragAndDropClothing> allScripts)
    {
        dragScript.setEnding();
        doorAnim.SetTrigger("openDoor");
        foreach (var script in allScripts)
        {
            script.ridgidBody.bodyType = RigidbodyType2D.Static;
        }
        foxAnim.SetTrigger("walkOut");
        walkOut = true;
    }







}
