using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceCamera : MonoBehaviour
{

    static WebCamTexture frontCam;

    // Start is called before the first frame update
    void Start()
    {
        if (frontCam == null)
            frontCam = new WebCamTexture();

        GetComponent<Renderer>().material.mainTexture = frontCam;

        if (!frontCam.isPlaying)
            frontCam.Play();
    }
}
