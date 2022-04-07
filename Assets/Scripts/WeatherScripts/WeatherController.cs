using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeatherController : MonoBehaviour
{

    // textfield showing current game mode
    public TextMeshProUGUI gameMode;

    // Different "scenes" for different weather
    public GameObject sunnyObject;
    public GameObject cloudyObject;
    public GameObject rainyObject;

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
        // Displays the diffenent weather objects in a mutual exclusive way
        switch (weather) 
        {
            case WeatherTypes.Sun:
                HideAllWeather();
                sunnyObject.SetActive(true);
                Debug.Log("Sunny");
                break;
            case WeatherTypes.Cloud:
                HideAllWeather();
                cloudyObject.SetActive(true);
                Debug.Log("Cloudy");
                break;
            case WeatherTypes.Rain:
                HideAllWeather();
                rainyObject.SetActive(true);
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
