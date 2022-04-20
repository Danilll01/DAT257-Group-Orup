using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeatherData : MonoBehaviour {
	private float timer;
	public float minutesBetweenUpdate;
	private WeatherInfo Info;
	public string API_key;
	private float latitude;
	private float longitude;
	private bool locationInitialized;
	public Text currentWeatherText;
	public Text currentTemperatureText;
	public GPSLocation getLocation;

	public void Begin() {
		latitude = getLocation.latitudeValue;
		longitude = getLocation.longitudeValue;
		locationInitialized = true;
		timer = 0;
	}
	void Update() {
		if (locationInitialized) {
			if (timer <= 0) {
				StartCoroutine (GetWeatherInfo ());
				timer = minutesBetweenUpdate * 60;
			} else {
				timer -= Time.deltaTime;
			}
		}
	}
	private IEnumerator GetWeatherInfo()
	{
		var www = new UnityWebRequest("https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=" + API_key + "&units=metric")
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			//error
			yield break;
		}

		Info = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
		currentWeatherText.text = "Weather: " + Info.weather[0].main;
		currentTemperatureText.text = "TemperatureAPI: " + Info.main.temp;
	}
}
[Serializable]
public class WeatherInfo
{
	public Coord coord;
	public List<Weather> weather;
	public Main main;
	public int visibility;
	public Wind wind;
	public Clouds clouds;
	public int dt;
	public Sys sys;
	public int timezone;
	public int id;
	public string name;
	public int cod;
}

[Serializable]
public class Sys
{
	public int type;
	public int id;
	public string country;
	public int sunrise;
	public int sunset;
}

[Serializable]
public class Clouds
{
	public int all;
}

[Serializable]
public class Wind
{
	public float speed;
	public int deg;
	public float gust;
}

[Serializable]
public class Coord
{
	public int lon;
	public int lat;
}

[Serializable]
public class Main
{
	public float temp;
	public float feels_like;
	public float temp_min;
	public float temp_max;
	public int pressure;
	public int humidity;
}

[Serializable]
public class Weather
{
	public int id;
	public string main;
	public string description;
	public string icon;
}
