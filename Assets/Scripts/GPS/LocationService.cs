using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class LocationService : MonoBehaviour
{
    public float longitude;
    public float latitude;
    public string debug;

    IEnumerator Start()
    {
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            debug = "Location not enabled on device or app does not have permission to access location";

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            debug = "Timed out";
            yield break;
        }

        int count = 0;

        while(true)
        {
            // If the connection failed this cancels location service use.
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                debug = "Unable to determine device location";
                yield return null;
            }
            else
            {
                // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
                Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;
                debug = "refresh: " + count.ToString();
                count++;
            }
            yield return new WaitForSeconds(0.5f);
        }

        // Stops the location service if there is no need to query location updates continuously.
        //Input.location.Stop();
    }
}
