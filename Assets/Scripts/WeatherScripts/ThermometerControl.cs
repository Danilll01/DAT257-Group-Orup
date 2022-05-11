using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ThermometerControl : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private Image TempBar;
    [SerializeField] private Image termometer;

    private float updateTemp;
    private float maxTemp;
    private float minTemp;


    // Start is called before the first frame update and sets max/min temp
    void Start(){
        maxTemp = 100;
        minTemp = -20;
        TempBar.color = Color.red;
        termometer.color = Color.red;
    }


    //Used by WeatherController to update the active temperature
    public void setTemp(float temp){
        updateTemp = temp;
        temperatureText.text = (int)updateTemp + " *C;";
        TempBar.fillAmount = (updateTemp - minTemp) / (maxTemp - minTemp);

        if (temp < 0)
        {
            updateColor(Color.blue);
        }
        else
        {
            updateColor(Color.red);
        }
    }

    private void updateColor(Color color)
    {
        TempBar.color = color;
        termometer.color = color;

    }
}