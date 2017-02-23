using UnityEngine;
using Windows.Kinect;

public class BodyManager : MonoBehaviour
{

    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;

    public Body[] GetData()
    {
        return _Data;
    }

    // Use this for initialization
    void Start()
    {
        //Kinect for Windows supports one sensor, which is called the default sensor. The KinectSensor Class has static members to help configure the Kinect sensor and access sensor data.
        //We have to check wether a Kinect sensor is connected to the PC
        _Sensor = KinectSensor.GetDefault(); //Gets the default sensor.

        if (_Sensor != null)
        {
            /*
               Once the sensor is open, we can use that instance to gain access to the individual streams. 
               Each stream has his frame type. In our case we will use Body frame. It contains all the computed real-time tracking information about
               people that are in view of the sensor. The computed info includes skeletal joints and orientations, hand states, and more for up to 6 people at a time.
               The frame gives you access to the data of the stream, for this we need to configure the stream reader type.
           */
            _Reader = _Sensor.BodyFrameSource.OpenReader();

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();//Opens the KinectSensor for use
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
            To extract this tracking data from the BodyFrame, allocate a vector of 6 body pointers and pass it to BodyFrame.GetAndRefreshBodyData Method. 
            Each body in the array represents tracking information for each of the 6 possible bodies that can be tracked simultaneously. 
            Each body that represents an actual live user in view of the sensor will be marked as tracked.
        */
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame(); //Gets the most recent body index frame
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_Data);
                //refresh the stream of data from the Reader
                frame.Dispose();
                frame = null;
            }
        }
    }
    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}
