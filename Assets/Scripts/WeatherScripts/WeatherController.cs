using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherController : MonoBehaviour
{

    // textfield showing current game mode
    public TextMeshProUGUI gameMode;
    

    public void OnTodayClick ()
    {
        gameMode.text = "Todays weather";
        Debug.Log("Todays weather");
    }

    public void OnTomorrowClick()
    {
        gameMode.text = "Tomorrows weather";
        Debug.Log("Tomorrows weather");
    }

    public void OnRandomClick()
    {
        gameMode.text = "Random Weather";
        Debug.Log("Random weather");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
