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

    private Color blueColor;


    // Start is called before the first frame update and sets max/min temp
    void Start(){
        maxTemp = 50;
        minTemp = -20;
        TempBar.color = Color.red;
        termometer.color = Color.red;
        // RGB values for a light blue color
        blueColor = new Color(45f / 255f, 146f / 255f, 255f / 255f);
    }


    //Used by WeatherController to update the active temperature
    public void setTemp(float temp){
        updateTemp = temp;
        temperatureText.text = (int)updateTemp + "?C";
        TempBar.fillAmount = (updateTemp - minTemp) / (maxTemp - minTemp);

        // Set the color of the termometer depending on temperature
        if (temp < 0)
        {
            updateColor(blueColor);
        }
        else
        {
            updateColor(Color.red);
        }
    }

    // Update to given color
    private void updateColor(Color color)
    {
        TempBar.color = color;
        termometer.color = color;

    }
}