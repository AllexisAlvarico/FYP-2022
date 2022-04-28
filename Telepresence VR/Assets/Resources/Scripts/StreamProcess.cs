using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Drawing;


// Note if bitmap is giving errors to work you need the place the system.drawing.dll
// into the plugin folder. After this step is done, go to edit > project setting > select
// player > select other setting > go down to configuration > set api compatibillity level
// to .Net 4.x and that should fix the problem.
// you can find the system.drawing.dll in my google drive https://tinyurl.com/2z3zxtyy

public class StreamProcess
{
    public Bitmap bitmap { get; set; }
    // 2 byte header for JPEG images
    private readonly byte[] JpegHeader = new byte[] { 0xff, 0xd8 };

    // pull down 1024 bytes at a time
    private int _chunkSize = 1024 * 4;

    // used to cancel reading the stream
    private bool _streamActive;

    // current encoded JPEG image
    public byte[] CurrentFrame { get; private set; }

    // used to marshal back to UI thread
    private SynchronizationContext _context;
    public byte[] latestFrame = null;
    private bool responseReceived = false;

    // event to get the buffer above handed to you
    public event EventHandler<FrameReadyEventArgs> FrameReady;
    public event EventHandler<ErrorEventArgs> Error;

    public StreamProcess(int chunkSize = 4 * 1024)
    {
        _context = SynchronizationContext.Current;
        _chunkSize = chunkSize;
    }

    public void ParseStream(Uri uri)
    {
        Debug.Log("Parsing Stream " + uri.ToString());
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        // asynchronously get a response
        request.BeginGetResponse(OnGetResponse, request);
    }

    public void StopStream()
    {
        _streamActive = false;
    }

    public static int FindBytes(byte[] buff, byte[] search)
    {
        for (int start = 0; start < buff.Length - search.Length; start++)
        {
            // finds the first character
            if (buff[start] == search[0])
            {
                int next;
                // goes through the rest of the bytes
                for (next = 1; next < search.Length; next++)
                {
                    // if we don't match, exit
                    if (buff[start + next] != search[next])
                        break;
                }
                if (next == search.Length)
                    return start;
            }
        }
        return -1;
    }
    private void OnGetResponse(IAsyncResult asyncResult)
    {
        responseReceived = true;
        Debug.Log("OnGetResponse");
        byte[] imageBuffer = new byte[1024 * 1024];

        Debug.Log("Starting request");
        // get the response
        HttpWebRequest req = (HttpWebRequest)asyncResult.AsyncState;

        try
        {
            Debug.Log("OnGetResponse try entered.");
            HttpWebResponse resp = (HttpWebResponse)req.EndGetResponse(asyncResult);
            Debug.Log("response received");
            // find our magic boundary value
            string contentType = resp.Headers["Content-Type"];
            if (!string.IsNullOrEmpty(contentType) && !contentType.Contains("="))
            {
                throw new Exception("Invalid content-type header.  The camera is likely not returning a proper MJPEG stream.");
            }

            string boundary = resp.Headers["Content-Type"].Split('=')[1].Replace("\"", "");
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(boundary.StartsWith("--") ? boundary : "--" + boundary);

            Stream s = resp.GetResponseStream();
            BinaryReader br = new BinaryReader(s);

            _streamActive = true;
            byte[] buff = br.ReadBytes(_chunkSize);

            while (_streamActive)
            {
                // find the JPEG header
                int imageStart = FindBytes(buff, JpegHeader);// buff.Find(JpegHeader);

                if (imageStart != -1)
                {
                    // copy the start of the JPEG image to the imageBuffer
                    int size = buff.Length - imageStart;
                    Array.Copy(buff, imageStart, imageBuffer, 0, size);

                    while (true)
                    {
                        buff = br.ReadBytes(_chunkSize);

                        // Find the end of the jpeg
                        int imageEnd = FindBytes(buff, boundaryBytes);
                        if (imageEnd != -1)
                        {
                            // copy the remainder of the JPEG to the imageBuffer
                            Array.Copy(buff, 0, imageBuffer, size, imageEnd);
                            size += imageEnd;

                            // Copy the latest frame into `CurrentFrame`
                            byte[] frame = new byte[size];
                            Array.Copy(imageBuffer, 0, frame, 0, size);
                            CurrentFrame = frame;

                            // tell whoever's listening that we have a frame to draw
                            if (FrameReady != null)
                                FrameReady(this, new FrameReadyEventArgs());
                            // copy the leftover data to the start
                            Array.Copy(buff, imageEnd, buff, 0, buff.Length - imageEnd);

                            // fill the remainder of the buffer with new data and start over
                            byte[] temp = br.ReadBytes(imageEnd);

                            Array.Copy(temp, 0, buff, buff.Length - imageEnd, temp.Length);
                            break;
                        }

                        // copy all of the data to the imageBuffer
                        Array.Copy(buff, 0, imageBuffer, size, buff.Length);
                        size += buff.Length;

                        if (!_streamActive)
                        {
                            Debug.Log("CLOSING");
                            resp.Close();
                            break;
                        }
                    }
                }
            }
            resp.Close();
        }
        catch (Exception ex)
        {
            if (Error != null)
                _context.Post(delegate { Error(this, new ErrorEventArgs() { Message = ex.Message }); }, null);

            return;
        }
    }
}


public class FrameReadyEventArgs : EventArgs
{
    // events
}

public sealed class ErrorEventArgs : EventArgs

{
    public string Message { get; set; }
    public int ErrorCode { get; set; }
}
