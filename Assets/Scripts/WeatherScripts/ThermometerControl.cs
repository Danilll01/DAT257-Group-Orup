using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ThermometerControl : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private Image TempBar;

    private float updateTemp;
    private float maxTemp;
    private float minTemp;


    // Start is called before the first frame update and sets max/min temp
    void Start(){
        maxTemp = 40;
        minTemp = -20;
    }

    void Update(){
        temperatureText.text = (int)updateTemp + " *C;";
        TempBar.fillAmount = (updateTemp - minTemp) / (maxTemp - minTemp);
    }

    //Used by WeatherController to update the active temperature
    public void setTemp(float temp){
        updateTemp = temp;
    }
}