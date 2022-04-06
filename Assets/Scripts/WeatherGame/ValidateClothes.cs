using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateClothes : MonoBehaviour
{
    [SerializeField] private GameObject[] snapPoints;
    
    public void checkClothing(){
        int clothesCounter = 0;
        DragAndDrop.weather tempWeather = DragAndDrop.weather.sunny;
        bool validClothing = true;


        foreach(GameObject snapPoint in snapPoints){
            if(snapPoint.transform.childCount > 0){
                
                DragAndDrop script = snapPoint.GetComponentInChildren<DragAndDrop>();
                if(clothesCounter == 0){
                    tempWeather = script.chosenWeather;
                }
                else{
                    if(script.chosenWeather != tempWeather){
              
                        validClothing = false;
                    }
                }
                clothesCounter++;
                
            
            }
        }

        if(validClothing){
            Debug.Log("Valid");
        }
        else{
            Debug.Log("Invalid");
        }
        
    }
}
