using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Vector2 m_leftInput;
    private Vector2 m_rightInput;

    // Update is called once per frame
    void Update()
    {

        m_leftInput.x = Input.GetAxisRaw("CameraHorizontal");
        m_leftInput.y = Input.GetAxisRaw("CameraVertical");

        m_rightInput.x = Input.GetAxisRaw("FrontWheel");
        m_rightInput.y = Input.GetAxisRaw("BackWheel");


        if (m_leftInput.x < 0)
        {
            Debug.Log("Camera Horizontal: " + m_leftInput.x);
            // add the command here to move the camera left
        }
        else if(m_leftInput.x > 0)
        {
            Debug.Log("Camera Horizontal: " + m_leftInput.x);
            // add the command here to move the camera right
        }

        if (m_leftInput.y < 0)
        {
            Debug.Log("Camera Vertical: " + m_leftInput.y);
            // add the command here to move the camera down
        }
        else if (m_leftInput.y > 0)
        {
            Debug.Log("Camera Vertical: " + m_leftInput.y);
            // add the command here to move the camera up
        }


        if (m_rightInput.x < 0)
        {
            Debug.Log("FrontWheel Horizontal: " + m_rightInput.x);
            // add the command here to move the FrontWheel left
        }
        else if (m_rightInput.x > 0)
        {
            Debug.Log("FrontWheel Horizontal: " + m_rightInput.x);
            // add the command here to move the FrontWheel right
        }

        if (m_rightInput.y < 0)
        {
            Debug.Log("BackWheel Vertical: " + m_rightInput.y);
            // add the command here to move the RC Backwards
        }
        else if (m_rightInput.y > 0)
        {
            Debug.Log("BackWheel Vertical: " + m_rightInput.y);
            // add the command here to move the RC Forward
        }


        if (Input.GetAxisRaw("RightTrigger") != 0)
        {
            Debug.Log("RightTrigger: " + Input.GetAxisRaw("RightTrigger"));
            // add the fire command
        }
        if (Input.GetAxisRaw("LeftTrigger") != 0)
        {
            Debug.Log("LeftTrigger: " + Input.GetAxisRaw("LeftTrigger"));
            // add the aim command here
        }

        if (Input.GetButton("LeftBumper"))
        {
            Debug.Log("LeftBumper is pressed");
            // add the decrease speed command
        }
        if (Input.GetButton("RightBumper"))
        {
            Debug.Log("RightBumper is pressed");
            // add the increase speed command here
        }

        if(Input.GetButton("Jump"))
        {
            Debug.Log("Jumped!");
        }
    }
}
