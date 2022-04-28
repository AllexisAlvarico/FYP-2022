using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject TCPListener;
    string command;
    private Vector2 m_leftInput;
    private Vector2 m_rightInput;

    // Update is called once per frame
    void Update()
    {
        if(TCPListener.GetComponent<RaspberryCon>().conn)
        {
            m_leftInput.x = Input.GetAxisRaw("CameraHorizontal");
            m_leftInput.y = Input.GetAxisRaw("CameraVertical");

            m_rightInput.x = Input.GetAxisRaw("FrontWheel");
            m_rightInput.y = Input.GetAxisRaw("BackWheel");


            if (m_leftInput.x < 0)
            {
                command = "LeftCam";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("Camera Horizontal: " + m_leftInput.x);
                // add the command here to move the camera left
            }
            else if (m_leftInput.x > 0)
            {
                command = "RightCam";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("Camera Horizontal: " + m_leftInput.x);
                // add the command here to move the camera right
            }

            if (m_leftInput.y < 0)
            {
                command = "DownCam";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("Camera Vertical: " + m_leftInput.y);
                // add the command here to move the camera down
            }
            else if (m_leftInput.y > 0)
            {
                command = "UpCam";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("Camera Vertical: " + m_leftInput.y);
                // add the command here to move the camera up
            }


            if (m_rightInput.x < 0)
            {
                command = "LeftTurn";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("FrontWheel Horizontal: " + m_rightInput.x);
                // add the command here to move the FrontWheel left
            }
            else if (m_rightInput.x > 0)
            {
                command = "RightTurn";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("FrontWheel Horizontal: " + m_rightInput.x);
                // add the command here to move the FrontWheel right
            }

            if (m_rightInput.y < 0)
            {
                command = "Foward";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("BackWheel Vertical: " + m_rightInput.y);
                // add the command here to move the RC Backwards
            }
            else if (m_rightInput.y > 0)
            {
                command = "Backwards";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("BackWheel Vertical: " + m_rightInput.y);
                // add the command here to move the RC Forward
            }


            if (Input.GetAxisRaw("RightTrigger") != 0)
            {
                command = "Fire";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("RightTrigger: " + Input.GetAxisRaw("RightTrigger"));
                // add the fire command
            }
            if (Input.GetAxisRaw("LeftTrigger") != 0)
            {
                command = "Aim";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("LeftTrigger: " + Input.GetAxisRaw("LeftTrigger"));
                // add the aim command here
            }

            if (Input.GetButton("LeftBumper"))
            {
                command = "Slow";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("LeftBumper is pressed");
                // add the decrease speed command
            }
            if (Input.GetButton("RightBumper"))
            {
                command = "Fast";
                TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
                Debug.Log("RightBumper is pressed");
                // add the increase speed command here
            }
        }
        else
        {
            Debug.LogError("No connection to raspberry");
        }
    }
}
