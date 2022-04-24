using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StreamTexture : MonoBehaviour
{
    string streamAddress;
    int chunkSize = 4;

    Texture2D tex;

    const int initWidth = 4;
    const int initHeight = 4;

    bool updateFrame = false;

    StreamProcess stream;

    float deltaTime = 0.0f;
    float StreamDeltaTime = 0.0f;

    public void Start()
    {
        stream = new StreamProcess(chunkSize * 1024);
        stream.FrameReady += OnStreamFrameReady;
        stream.Error += OnStreamError;
        Uri Address = new Uri(streamAddress);
        stream.ParseStream(Address);
        tex = new Texture2D(initWidth, initHeight, TextureFormat.RGBA32, false);
    }
    private void OnStreamFrameReady(object sender, FrameReadyEventArgs e)
    {
        updateFrame = true;
    }

    void OnStreamError(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error received while reading the MJPEG.");
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        if (updateFrame)
        {
            tex.LoadImage(stream.CurrentFrame);
            // Assign texture to renderer's material.
            GetComponent<Renderer>().material.mainTexture = tex;
            updateFrame = false;

            StreamDeltaTime += (deltaTime - StreamDeltaTime) * 0.2f;

            deltaTime = 0.0f;
        }
    }

    void OnDestroy()
    {
        stream.StopStream();
    }

    public void setAddress(string t_adress)
    {
        streamAddress = t_adress;
    }

}
