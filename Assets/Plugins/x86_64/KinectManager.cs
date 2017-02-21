using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class KinectManager : MonoBehaviour
{
    private BodyFrameReader _bodyFrameReader;
    private KinectSensor _sensor;
    private bool IsAvailable;
    private Body[] _bodies = null;

    public Text AvailableText;

    void Start()
    {
        //Kinect for Windows supports one sensor, which is called the default sensor. The KinectSensor Class has static members to help configure the Kinect sensor and access sensor data.
        //We have to check wether a Kinect sensor is connected to the PC

        _sensor = KinectSensor.GetDefault(); //Gets the default sensor.
        if (_sensor != null)
        {
            /*
                Once the sensor is open, we can use that instance to gain access to the individual streams. 
                Each stream has his frame type. In our case we will use Body frame. It contains all the computed real-time tracking information about
                people that are in view of the sensor. The computed info includes skeletal joints and orientations, hand states, and more for up to 6 people at a time.
                The frame gives you access to the data of the stream, for this we need to configure the stream reader type.
            */
            _bodyFrameReader = _sensor.BodyFrameSource.OpenReader();

            if (!_sensor.IsOpen)
            {
                _sensor.Open(); //Opens the KinectSensor for use
            }

            AvailableText.text = "Kinect is ready !";   
        }
        else
        {
            AvailableText.text = "Kinect is unavailable !";
        }
    }

    void Update()
    {
        /*
            To extract this tracking data from the BodyFrame, allocate a vector of 6 body pointers and pass it to BodyFrame.GetAndRefreshBodyData Method. 
            Each body in the array represents tracking information for each of the 6 possible bodies that can be tracked simultaneously. 
            Each body that represents an actual live user in view of the sensor will be marked as tracked.
        */
        if (_bodyFrameReader != null)
        {
            var frame = _bodyFrameReader.AcquireLatestFrame(); //Gets the most recent body index frame
            
            if (frame != null)
            {
                if (_bodies == null)
                {
                    _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
                }
                frame.GetAndRefreshBodyData(_bodies);

                //refresh the stream of data from the Reader
                frame.Dispose();
                frame = null;
            }
            
        }
    }

    void OnApplicationQuit()
    {
        if (_bodyFrameReader != null)
        {
            _bodyFrameReader.Dispose();
            _bodyFrameReader = null;
        }

        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }
            _sensor = null;
        }
    }
}
