using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// GPSLocation tries to find device location.
// If access is enabled by user and connection is succesful, it will continously update location data.
// Latitude and longitude values are stored in the attributes latitudeValue and longitudeValue. 

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

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(StartGPSLocation());

    }

    // Update is called once per frame
    void Update()
    {

    }

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

    private void UpdateGPSData ()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Access granted to GPS values and it has been initialized
            GPSStatus.text = "S�ker plats...";

            // Get values from service 
            latitudeValue = Input.location.lastData.latitude;
            longitudeValue = Input.location.lastData.longitude;

            latitudeTextField.text = latitudeTextField.ToString();
            longitudeTextField.text = longitudeValue.ToString();

        } else
        {
            // Service has stopped 
            GPSStatus.text = "Slutar s�ka plats";
        }
    }

    
}
