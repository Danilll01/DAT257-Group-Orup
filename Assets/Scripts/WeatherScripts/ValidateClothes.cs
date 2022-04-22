using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateClothes : MonoBehaviour
{
    [SerializeField] private GameObject[] snapPoints;
    private WeatherController.WeatherTypes weather;
    private bool weatherSet;

    void Start(){
        weatherSet = false;
    }

    // Check if the clothes on the snappoints are valid with the current weather
    public void checkClothing(){
        bool validClothing = true;

        // If the weather is set, meaning we know what weather it is
        if(weatherSet){
            // Loop through all snapPoints
            foreach(GameObject snapPoint in snapPoints){
                if(snapPoint.transform.childCount > 0){
                    // Get the DragAndDrop script on the clothing object
                    DragAndDrop script = snapPoint.GetComponentInChildren<DragAndDrop>();
                    // If the clothings weather does not match current weather
                    // send to script to remove it from character
                    if(script.chosenWeather != weather){
                        script.removeFromSnapPoint();
                        validClothing = false;
                    }
                    
                }
            }

            // If one clothins was not valid
            if(validClothing){
                Debug.Log("Valid");
            }
            else{
                Debug.Log("Invalid");
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

    

}
