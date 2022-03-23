using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System;

public class RaspberryCon : MonoBehaviour
{

    public string host;
    public int port;

    internal bool socket_ready = false;
    internal string input_buffer = "";
    public UnityEngine.UI.Text test;
    TcpClient tcp_client;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;


    private void Start()
    {
        test = GameObject.Find("Test").GetComponent<UnityEngine.UI.Text>();
        setupSocket();
    }

    private void Update()
    {
        string received_data = readSocket();
        Debug.Log(" received:" + received_data);
        test.text = received_data;
    }

    private void OnApplicationQuit()
    {
        closeSocket();
    }


    public void setupSocket()
    {
        try
        {
            tcp_client = new TcpClient();

            IPAddress iPAddress = Dns.GetHostEntry(host).AddressList[0];
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8000);
            tcp_client.Connect(iPEndPoint);

            net_stream = tcp_client.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);

            socket_ready = true;

        }
        catch (Exception e)
        {
            Debug.Log("Socket Error " + e);
        }
    }

    public void writeSocket(string line)
    {
        if (!socket_ready)
            return;

        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    public String readSocket()
    {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine().ToString();

        return "";
    }

    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_client.Close();
        socket_ready = false;
    }
}
