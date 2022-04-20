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
    [SerializeField] private GameObject snowyObject;

    public Text currentWeatherText;
	public Text currentTemperatureText;

    public WeatherData weatherData;

    // A type for different weather types
    private enum WeatherTypes {Sun, Cloud, Rain, Snow};

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

        // Returns the following:
	    // Clear, Rain, ThunderStorm, Drizzle, Snow, Clouds
        string currentWeather = weatherData.GetWeather(false);
        float currentTemp = weatherData.GetTemp(false);

        CheckWeather(currentWeather,currentTemp);
        Debug.Log("Todays weather");
        
    }

    public void OnTomorrowClick()
    {
        gameMode.text = "Morgondagens väder";

        string tomorrowWeather = weatherData.GetWeather(true);
        float tomorrowTemp = weatherData.GetTemp(true);
      
        CheckWeather(tomorrowWeather,tomorrowTemp);
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


    // Sets the weather depending on what we got from API
    private void CheckWeather(string weather, float temp){

        if(weather == "error" || temp == -1f){
            OnRandomClick();
        }
        else{
            switch (weather){
            case "Clear":
                SetWeather(WeatherTypes.Sun, WindSpeed.None);
                break;
            case "Rain": case "ThunderStorm": case "Drizzle":
                SetWeather(WeatherTypes.Rain, WindSpeed.None);
                break;
            case "Snow":
                SetWeather(WeatherTypes.Snow, WindSpeed.None);
                break;
            case "Clouds":
                SetWeather(WeatherTypes.Cloud, WindSpeed.None);
                break;
            
            default:
                SetWeather(WeatherTypes.Cloud, WindSpeed.None);
                break;
            }

            currentWeatherText.text = "WeatherAPI: " + weather;
            currentTemperatureText.text = "TemperatureAPI: " + temp;
        }
        
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
                temporaryTempText.text = "Temperature: 25°";
                Debug.Log("Sunny");
                break;
            case WeatherTypes.Cloud:
                cloudyObject.SetActive(true);
                temporaryTempText.text = "Temperature: 15°";
                Debug.Log("Cloudy");
                break;
            case WeatherTypes.Rain:
                rainyObject.SetActive(true);
                temporaryTempText.text = "Temperature: 7°";
                Debug.Log("Rainy");
                break;
            case WeatherTypes.Snow:
                snowyObject.SetActive(true);
                temporaryTempText.text = "Temperature: -5°";
                Debug.Log("Snowy");
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
        snowyObject.SetActive(false);
    }
}
