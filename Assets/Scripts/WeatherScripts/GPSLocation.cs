using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// GPSLocation can fetch location data of a hand held devices. The device must have enabled location service.
// If access is enabled by user and connection is succesful, it will update the text fields with the latest fetched location data.
// Latitude and longitude values are stored in the attributes latitudeValue and longitudeValue. 

public class GPSLocation : MonoBehaviour
{
    // Holds latest latitude value of device
    public float latitudeValue;
    // Holds latest longitude value of device
    public float longitudeValue;

    // Textfield showing status of GPS Service
    [SerializeField]
    private TextMeshProUGUI GPSStatus;
    // Textfield showing current latitude value 
    /*
    [SerializeField]
    private TextMeshProUGUI latitudeTextField;
    // Textfield showing current longitude value
    [SerializeField]
    private TextMeshProUGUI longitudeTextField;
    */

    [SerializeField]
    private WeatherData weatherData;

    // Start is called before the first frame update
    void Start()
    {
        // Values for testing weather accuracy when on desktop
        // Preset coordinates for gothenburg

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
        /*latitudeTextField.text = "";
        longitudeTextField.text = "";*/
        
    }

    // Tries to fetch location data
    IEnumerator StartGPSLocation()
    {
        // Check if user has enabled location service for this application
        if (!Input.location.isEnabledByUser)
        {
            GPSStatus.text = "Applikationen har ej åtkomst till platsdata";
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
            GPSStatus.text = "Service connection tog för lång tid";
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
            GPSStatus.text = "Söker plats...";

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
            GPSStatus.text = "Visar väder för platsen";

            // Get values from service 
            latitudeValue = Input.location.lastData.latitude;
            longitudeValue = Input.location.lastData.longitude;

            /*
            latitudeTextField.text = "Latitude: " + latitudeValue.ToString();
            longitudeTextField.text = "Longitude: " + longitudeValue.ToString();
            */

            weatherData.Begin();

        } else
        {
            // Service has stopped 
        }

        Input.location.Stop();
    }

    
}
