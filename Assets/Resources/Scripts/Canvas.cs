using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public GameObject TCPListener;
    public Button connect;
    public Text display;
    public bool isOff;
    public bool isConn;


    // Start is called before the first frame update
    void Start()
    {
        isOff = false;
        isConn = true;
    }
    public void onClick()
    {
        if (isConn)
            TCPListener.GetComponent<RaspberryCon>().closeSocket();
        else
            TCPListener.GetComponent<RaspberryCon>().setupSocket();
        isConn = !isConn;
        Debug.Log("Button click");
    }

    public void toggle()
    {
        string command = "on";
        if (isOff) command = "off";
        TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
        isOff = !isOff;
    }

}
