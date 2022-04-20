using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeatherController : MonoBehaviour
{

    // textfield showing current game mode
    [SerializeField] private TextMeshProUGUI gameMode;
    [SerializeField] private TextMeshProUGUI temporaryTempText;

    // Different "scenes" for different weather
    [SerializeField] private GameObject sunnyObject;
    [SerializeField] private GameObject cloudyObject;
    [SerializeField] private GameObject rainyObject;

    // A type for different weather types
    private enum WeatherTypes {Sun, Cloud, Rain};

    // An enum for wind speed. (Not used now)
    private enum WindSpeed {None, Slow, Fast};

    // Start is called before the first frame update
    void Start()
    {
        // Hide all weather objects when starting the game
        HideAllWeather();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTodayClick ()
    {
        gameMode.text = "Dagens väder";

        SetWeather(WeatherTypes.Sun, WindSpeed.None);

        Debug.Log("Todays weather");
    }

    public void OnTomorrowClick()
    {
        gameMode.text = "Morgondagens väder";
        Debug.Log("Tomorrows weather");
    }

    public void OnRandomClick()
    {
        gameMode.text = "Slumpmässigt väder";

        // Generates a random weather type
        System.Random random = new System.Random();
        Array values = Enum.GetValues(typeof(WeatherTypes));
        WeatherTypes randomWeather = (WeatherTypes)values.GetValue(random.Next(values.Length));

        // Sets weather to the randomly generated weather
        SetWeather(randomWeather, WindSpeed.None);
        Debug.Log("Random weather");
    }
    
    // Switches to the correct weather object based on inputted weather.
    private void SetWeather(WeatherTypes weather, WindSpeed windSpeed)
    {

        HideAllWeather();

        // Displays the diffenent weather objects in a mutual exclusive way
        switch (weather) 
        {

            case WeatherTypes.Sun:
                sunnyObject.SetActive(true);
                temporaryTempText.text = "Temprature: 25°";
                Debug.Log("Sunny");
                break;
            case WeatherTypes.Cloud:
                cloudyObject.SetActive(true);
                temporaryTempText.text = "Temprature: 15°";
                Debug.Log("Cloudy");
                break;
            case WeatherTypes.Rain:
                rainyObject.SetActive(true);
                temporaryTempText.text = "Temprature: 7°";
                Debug.Log("Rainy");
                break;
            default:
                Debug.Log("No work");
                break;
        }
    }

    // Hides all weather objects
    private void HideAllWeather()
    {
        sunnyObject.SetActive(false);
        cloudyObject.SetActive(false);
        rainyObject.SetActive(false);
    }
}
