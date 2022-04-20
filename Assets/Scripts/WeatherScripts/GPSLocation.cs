using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// GPSLocation can fetch and display location data of a hand held devide. The device must have enabled location service.
// If access is enabled by user and connection is succesful, it will update the text fields with the latest fetched location data.
// Latitude and longitude values are stored in the attributes latitudeValue and longitudeValue. 
// OBS! M�ste l�gga till Location Usage Description i Player Settings

public class GPSLocation : MonoBehaviour
{
    // Holds latest latitude value of device
    public float latitudeValue;
    // Holds latest longitude value of device
    public float longitudeValue;

    // Textfield showing status of GPS Service
    public TextMeshProUGUI GPSStatus;
    // Textfield showing current latitude value 
    public TextMeshProUGUI latitudeTextField;
    // Textfield showing current longitude value
    public TextMeshProUGUI longitudeTextField;

    public WeatherData weatherData;

    // Start is called before the first frame update
    void Start()
    {
        // Values for testing weather accuracy when on desktop
    
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            latitudeValue = 57.686433f;
            longitudeValue = 11.966388f;

            weatherData.Begin();
        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fetches location data if possible and updates the associated attributes with the new values
    public void OnUpdateLocationClick()
    {
        StartCoroutine(StartGPSLocation());
    }

    // Clears all text fields and stops calling UpdateGPSData
    public void ClearLocationData()
    {
        CancelInvoke("UpdateGPSData"); // Cancel GPS update
        GPSStatus.text = "";
        latitudeTextField.text = "";
        longitudeTextField.text = "";
        
    }

    // Tries to fetch location data
    IEnumerator StartGPSLocation()
    {
        // Check if user has enabled location service for this application
        if (!Input.location.isEnabledByUser)
        {
            GPSStatus.text = "Applikationen har ej �tkomst till platsdata";
            yield break;
        }

        // Start service
        Input.location.Start();
        
        // Wait for max 20 seconds for service to initialize
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1); // Wait one second
            maxWait--;                          
        }

        // The service did not initialize within 20 seconds, break
        if (maxWait < 1)
        {
            GPSStatus.text = "Service connection tog f�r l�ng tid";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            // Access failed
            GPSStatus.text = "Kunde ej hitta enhetens plats";
            yield break;

        } else
        {
            // Access granted
            GPSStatus.text = "S�ker plats...";

            // Continiously collect location data
            InvokeRepeating("UpdateGPSData", 0.5f, 1f); 
        }


    }

    // Saves the latest latitude and longitude data from service in latitudeValue and longitudeValue and updates the associated text fields
    // Stops service after data has been collected
    private void UpdateGPSData ()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Access granted to GPS values and it has been initialized
            GPSStatus.text = "Visar v�der f�r platsen";

            // Get values from service 
            latitudeValue = Input.location.lastData.latitude;
            longitudeValue = Input.location.lastData.longitude;

            latitudeTextField.text = "Latitude: " + latitudeValue.ToString();
            longitudeTextField.text = "Longitude: " + longitudeValue.ToString();

            weatherData.Begin();

        } else
        {
            // Service has stopped 
        }

        Input.location.Stop();
    }

    
}
