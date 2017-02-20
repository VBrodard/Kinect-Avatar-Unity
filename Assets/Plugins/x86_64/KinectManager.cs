using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;


public class KinectManager : MonoBehaviour
{

    private KinectSensor _sensor;
    private bool IsAvailable;

    public Text AvailableText;

    void Start()
    {
        //Kinect for Windows supports one sensor, which is called the default sensor. The KinectSensor Class has static members to help configure the Kinect sensor and access sensor data.
        //Check if a Kinect sensor is connected to the PC

        _sensor = KinectSensor.GetDefault(); //Gets the default sensor.

        if (_sensor != null)
        {
            IsAvailable = _sensor.IsAvailable; //find out whether the sensor is able to get frames

            
            _sensor.Open(); //Opens the KinectSensor for use

            if (_sensor.IsOpen)
            {
                AvailableText.text = "Kinect is ready !";
            }
        }
        else
        {
            AvailableText.text = "Kinect is unavailable !";
        }
    }
}
