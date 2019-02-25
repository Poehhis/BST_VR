/* Parts of this code are copied from:
 * ArduinoConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class ArduinoBridge : MonoBehaviour {

    private SerialPort stream;
    public bool msg = false;
    public bool grab = false;
    public bool grabbed = false;
    private float mass;
    BallTriggeringScript bts;

    //opening of the serialport to arduino
    public void Open()
    {
        // Opens the serial port COM4 with baud rate 9600
        stream = new SerialPort("COM4", 9600);
        stream.ReadTimeout = 50;
        stream.Open();
    }

    //Use this function to read from Arduino
    public string ReadFromArduino(int timeout = 0)
    {
        stream.ReadTimeout = timeout;
        try
        {
            return stream.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }

    //This function is for a response waiting time from arduino
    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield break; // Terminates the Coroutine
            }
            else
                yield return null; // Wait for next frame

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    // Use this for initialization
    void Start () {
        //Opening port to arduino
        Open();
        //initializing ball mass
        bts = gameObject.GetComponent<BallTriggeringScript>();
    }
	
	// Update is called once per frame
	void Update () {
        mass = bts.ballMass;

        //if there is message to be sent then send mass of the ball to arduino via "ECHO arg" command 
        if (msg == true) MessageTest(mass);
        
        //if scene is changed close the stream
        if (bts.sceneChange == true) stream.Close();
	}

    //Sends message, waits for reply and sets message state to false
    void MessageTest(float ballmass)
    {
        //Sending arduino a message "ECHO arg"
        if (Mathf.Approximately(ballmass, 1.0f)) WriteToArduino("ECHO 1");
        if (Mathf.Approximately(ballmass, 1.25f)) WriteToArduino("ECHO 2");
        if (Mathf.Approximately(ballmass, 1.5f)) WriteToArduino("ECHO 3");

        //Use this to wait for response from arduino
        StartCoroutine
        (
            AsynchronousReadFromArduino
            ((string s) => Debug.Log(s),        // Callback
                () => Debug.LogError("Error!"), // Error callback
                10000f                          // Timeout (milliseconds)
            )
        );
        
        //reset message state
        msg = false;
    }

    //Sends message waits for reply and confirms reply by "false"
   /* void GrabMessage()
    {
        //Debug.Log("Mass of the ball: " + ballmass);
        //Sending arduino a message "ECHO X"
        WriteToArduino("PING");

        //wait for reply and grab object
        StartCoroutine
        (
            AsynchronousReadFromArduino
            ((string s) => Debug.Log(s),        // Callback
                () => Debug.LogError("Error!"), // Error callback
                10000f                          // Timeout (milliseconds)
            )
        );
        grab = false; 
    }*/

    //Use this function to write to Arduino
    public void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }
}
