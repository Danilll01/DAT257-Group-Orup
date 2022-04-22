using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeatherData : MonoBehaviour {
	private float timer;
	[SerializeField]
	private float minutesBetweenUpdate;
	[SerializeField]
	private WeatherInfo Info;
	[SerializeField]
	private string API_key;
	private float latitude;
	private float longitude;
	private bool locationInitialized;

	[SerializeField]
	private GPSLocation getLocation;

	// Called when GPSLocation has found users location
	public void Begin() {
		latitude = getLocation.latitudeValue;
		longitude = getLocation.longitudeValue;
		locationInitialized = true;
		timer = 0;
	}
	void Update() {
		//If location found, start coroutine of API request
		if (locationInitialized) {
			if (timer <= 0) {
				StartCoroutine (GetWeatherInfo ());
				timer = minutesBetweenUpdate * 60;
			} else {
				timer -= Time.deltaTime;
			}
		}
	}

	// Returns the weather
	public string GetWeather(bool getTomorrow){
		if(Info.list.Count > 1){
			if(getTomorrow){
			return Info.list[8].weather[0].main;
		}
			else{
				return Info.list[0].weather[0].main;
			}
		}

		else{
			return "error";
		}
	}

	// Returns the temperature
	public float GetTemp(bool getTomorrow){
		if(Info.list.Count > 1){
			if(getTomorrow){
				return Info.list[8].main.temp;
			}
			else{
				return Info.list[0].main.temp;
			}
		}

		else{
			return -1f;
		}
	}

	// Coroutine for requesting weather data
	private IEnumerator GetWeatherInfo()
	{
		var www = new UnityWebRequest("https://api.openweathermap.org/data/2.5/forecast?lat=" + latitude + "&lon=" + longitude + "&appid=" + API_key + "&units=metric")
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			//error
			yield break;
			
		}

		// Our weather data is stored here
		Info = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
		
	}
}

// Classes for reading the Json file
[Serializable]
public class WeatherInfo
{
	public int cod;
	public float message;
	public int cnt;
	public List<APIList> list;
	public City city;
	public string country;
	public int timezone;
	public int sunrise;
	public int sunset;
}

[Serializable]
public class APIList
{
	public int dt;
	public Main main;
	public List<Weather> weather;
	public Clouds clouds;
	public Wind wind;
	public int visibility;
	public float pop;
	public Sys sys;
	public string dt_txt;
}

[Serializable]
public class Sys
{
	public string pod;
}

[Serializable]
public class City
{
	public int id;
	public string name;
	public Coord coord;
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
	public int lat;
	public int lon;
}

[Serializable]
public class Main
{
	public float temp;
	public float feels_like;
	public float temp_min;
	public float temp_max;
	public int pressure;
	public int sea_level;
	public int grnd_level;
	public int humidity;
	public int temp_kf;
}

[Serializable]
public class Weather
{
	public int id;
	public string main;
	public string description;
	public string icon;
}
