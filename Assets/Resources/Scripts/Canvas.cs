using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public GameObject TCPListener;
    public Button connect;
    public GameObject inputField;
    public GameObject Screen;
    public Text display;
    string address;
    public bool isOff;
    public bool isConn;
    string command;


    // Start is called before the first frame update
    void Start()
    {
        isOff = false;
        isConn = true;

    }

    void Update()
    {
        
    }
    public void onClick()
    {
        address = inputField.GetComponent<InputField>().text;
        Debug.Log(address);
        Screen.gameObject.SetActive(true);
        Screen.GetComponent<StreamTexture>().setAddress(address);

        if (isConn)
        {
            TCPListener.GetComponent<RaspberryCon>().closeSocket();
            display.text = "on";
        }
        else
        {
            TCPListener.GetComponent<RaspberryCon>().setupSocket();
            display.text = "off";
        }
        isConn = !isConn;
        Debug.Log("Button click");
    }

    public void toggle()
    {
        command = "on";
        if (isOff)
            command = "off";
      
        TCPListener.GetComponent<RaspberryCon>().writeSocket(command);
        isOff = !isOff;
    }
    public string getAddress()
    {
        return address;
    }
    

}
