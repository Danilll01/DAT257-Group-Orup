using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// GPSLocation can fetch location data of a hand held devices. The device must have enabled location service.

public class GPSLocation : MonoBehaviour
{
    // Holds latest latitude value of device, default value set to gothenburg
    private static float latitudeValue = 57.686433f;
    // Holds latest longitude value of device
    private static float longitudeValue = 11.966388f;
    // Holds the current status of the service: Running(can query for locations), Initializing, Stopped or Failed
    public LocationServiceStatus locationServiceStatus;



    // Start is called before the first frame update
    void Start()
    {

        // Updates latitude and longitude attributes if service successfully connects
        UpdateLocationData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Tries to connect to location service and updates location data if connection was successful
    public void UpdateLocationData()
    {
        Debug.Log("Tries to connect to service");
        StartCoroutine(StartGPSLocation());
    }


    // Tries to fetch location data
    IEnumerator StartGPSLocation()
    {


        /*Debug.Log("Wait for remote to connect..."); // <------------- uncomment this code block for using unity remote
        yield return new WaitForSeconds(5);
        Debug.Log("Waited for remote");*/



        // Check if user has enabled location service for this application
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled by user");
            locationServiceStatus = Input.location.status;
            yield break;
        }

        // Start service
        Input.location.Start();

        // Wait for max 20 seconds for service to initialize
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            locationServiceStatus = Input.location.status;
            yield return new WaitForSeconds(1); // Wait one second
            maxWait--;                          
        }

        // The service did not initialize within 20 seconds, break
        if (maxWait < 1)
        {
            Debug.Log("Service took too long to connect");
            locationServiceStatus = Input.location.status;
            yield break;
        }

        // Check if service succesfully connected
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            // Access failed
            locationServiceStatus = Input.location.status;
            Debug.Log("Location Service failed");
            yield break;

        } else
        {
            // Access granted
            locationServiceStatus = Input.location.status;
            Debug.Log("Succesfully connected to location service");
            // Collect location data
            InvokeRepeating("UpdateGPSData", 0.5f, 1f); 
        }


    }

    // Saves the latest latitude and longitude data from service in latitudeValue and longitudeValue
    // Stops service after data has been collected
    private void UpdateGPSData ()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {

            // Get lastest values from service 
            latitudeValue = Input.location.lastData.latitude;
            longitudeValue = Input.location.lastData.longitude;
            Debug.Log("Latitude: " + latitudeValue + " Longitude:" + longitudeValue);

        } else
        {
            // Service has stopped 
            
        }

        // Stop service when latest location data has been fetched
        Input.location.Stop();
        locationServiceStatus = Input.location.status;
    }

    public static float getLatitudeValue()
    {
        return latitudeValue;
    }

    public static float getLongitudeValue()
    {
        return longitudeValue;
    }


    
}
