using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherController : MonoBehaviour
{

    // textfield showing current game mode
    public TextMeshProUGUI gameMode;

    // Different "scenes" for different weather
    public GameObject sunnyObject;
    public GameObject cloudyObject;
    public GameObject rainyObject;

    
    private enum WeatherTypes {Sun, Cloud, Rain};
    private enum WindSpeed {None, Slow, Fast};
    private SpriteRenderer weatherIconSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTodayClick ()
    {
        gameMode.text = "Dagens v�der";
        Debug.Log("Todays weather");
    }

    public void OnTomorrowClick()
    {
        gameMode.text = "Morgondagens v�der";
        Debug.Log("Tomorrows weather");
    }

    public void OnRandomClick()
    {
        gameMode.text = "Slumpm�ssigt v�der";
        Debug.Log("Random weather");
    }
    

    private void SetWeather(WeatherTypes weather, WindSpeed windSpeed)
    {
        switch (weather) 
        {
            case WeatherTypes.Sun:
                Debug.Log("Sunny");
                break;
            default:
                Debug.Log("No work");
                break;
        }
    }
}
