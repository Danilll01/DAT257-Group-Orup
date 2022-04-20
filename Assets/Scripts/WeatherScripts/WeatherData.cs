using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeatherData : MonoBehaviour {
	private float timer;
	public float minutesBetweenUpdate;
	[SerializeField]
	private WeatherInfo Info;
	public string API_key;
	private float latitude;
	private float longitude;
	private bool locationInitialized;

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

	public string GetWeather(bool getTomorrow){
		if(getTomorrow){
			return Info.list[8].weather[0].main;
		}
		else{
			return Info.list[0].weather[0].main;
		}
		
	}

	public float GetTemp(bool getTomorrow){
		if(getTomorrow){
			return Info.list[8].main.temp;
		}
		else{
			return Info.list[0].main.temp;
		}
	}

	
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

		Info = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
		
	}
}
[Serializable]
public class WeatherInfo
{
	//public Coord coord;
	//public List<Weather> weather;
	//public Main main;
	//public int visibility;
	//public Wind wind;
	//public Clouds clouds;
	//public int dt;
	//public Sys sys;
	//public int timezone;
	//public int id;
	//public string name;
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
	/*
	public int type;
	public int id;
	public string country;
	public int sunrise;
	public int sunset;
	*/
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
