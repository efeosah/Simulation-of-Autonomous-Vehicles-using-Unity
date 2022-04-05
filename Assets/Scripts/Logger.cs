using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;


public class Logger : MonoBehaviour
{
    /// <summary>
    /// Log data from the car/car camera
    /// See Nvidia : https://github.com/llSourcell/How_to_simulate_a_self_driving_car/blob/master/self-driving-car.ipynb
    /// Save data in a *.csv file
    /// Save data in a folder :: /log/*.csv
    /// Create our file save labelling system and output folder
    /// Currently saving image and steering data 
    /// </summary>

    public CameraSensor CamSensor_Centre;
    public CameraSensor CamSensor_Left;
    public CameraSensor CamSensor_Right;

    public int curFrameCount = 0;
    public int maxFrameCount;
    //[SerializeField] GameObject car;
    private Car carControl;
    private float timeSinceLastLog = 0.0f;
 

    private float FPS = 30.0f;

    private StreamWriter sw;

    private string filename = "driving_log.csv";

    class ImageSaveJob
    {
        public string filename;
        public byte[] bytes;
    }

    List<ImageSaveJob> imagesToSave;

    Thread thread;



    [SerializeField] private bool isLog = true;

    //private string filepath = Application.dataPath + "/../log/";

    // Start is called before the first frame update
    void Start()
    {

        
        //Debug.Log(Application.dataPath);
        
    }

    private string GetFilePath()
    {
        //store data in a folder called "log" in the "assets" folder
        return Application.dataPath + "/log/";
    }

    private void Awake()
    {
        //maxFrameCount = 10000;

        carControl = gameObject.GetComponentInParent<Car>();
        

        if(isLog && carControl != null)
        {
            //TODO

            string filePath = GetFilePath() + filename;
            //initialize filewriter
            sw = new StreamWriter(filePath);
        }



        imagesToSave = new List<ImageSaveJob>();

        thread = new Thread(SaverThread);
        thread.Start();
    }

    // Update is called once per frame
    // FPS = 50
    void Update()
    {

        if(!isLog)
        {
            return;
        }

        if (!CamSensor_Centre) { print("cam1"); return; }

        if (!CamSensor_Left) { print("cam2"); return; }

        if (!CamSensor_Right) { print("cam3"); return; }
        
        timeSinceLastLog += Time.deltaTime;
        //gameObject.transform.Rotate(90.0f/50.0f, 0.0f/50.0f, 90.0f/50.0f, Space.Self);
        if (timeSinceLastLog < 1.0f / FPS)
        {
            return;
            //TODO
        }

        timeSinceLastLog -= (1.0f / FPS);

        LogDataToCSV();
        //SaveCamSensor(CamSensor_Centre, CamSensor_Left, CamSensor_Right, "");
        SaveCamSensor(CamSensor_Centre, "CenterCam_");
        SaveCamSensor(CamSensor_Left, "LeftCam_");
        SaveCamSensor(CamSensor_Right, "RightCam_");


        if (curFrameCount >= maxFrameCount)
        {
            Debug.Log("Finish");
            Close();
            gameObject.SetActive(false);
        }

        curFrameCount += 1;


    



    }

    private void LogDataToCSV()
    {
        //data we wish to collect
        float curSteerAngle = carControl.GetSteering() / carControl.GetMaxSteering();
        float curThrottle = carControl.GetThrottle();
        //float curSpeed = carControl.GetVelocity();
        float curBrake = carControl.GetHandBrake();
        string camImage = GetFilePath() + "CenterCam_" + curFrameCount + ".jpg";
        string camImage_Left = GetFilePath() + "LeftCam_" + curFrameCount + ".jpg";
        string camImage_Right = GetFilePath() + "RightCam_" + curFrameCount + ".jpg";

        //Debug.Log("Steering Angle: " + curSteerAngle.ToString() + " Throttle: " + curThrottle.ToString()
        //    + " Brake: " + curBrake.ToString() + " Image Link: " + camImage);
        //if (!(curSteerAngle == 0 || curThrottle == 0 || curBrake == 0))
        //{

        //}
        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", camImage,camImage_Left,camImage_Right, curSteerAngle.ToString(),curThrottle.ToString(), curBrake.ToString(), '0'));
        sw.Flush();
    }

    //Save the camera sensor to an image. Use the suffix to distinguish between cameras.
    void SaveCamSensor(CameraSensor cs1, CameraSensor cs2, CameraSensor cs3, string prefix)
    {


        Texture2D image1 = cs1.GetImage();
        Texture2D image2 = cs2.GetImage();
        Texture2D image3 = cs3.GetImage();

        //image.GetPixelData

        ImageSaveJob ij1 = new ImageSaveJob();
        ImageSaveJob ij2 = new ImageSaveJob();
        ImageSaveJob ij3 = new ImageSaveJob();



        ij1.filename = GetFilePath() + "CenterCam_" + curFrameCount + ".jpg"; //string.Format("{0}_{1,8:D8}.png", prefix, "CenterCam_" + curFrameCount);
        //Debug.Log(ij1.filename);
        ij1.bytes = image1.EncodeToJPG();

        ij1.filename = GetFilePath() + "LeftCam_" + curFrameCount + ".jpg"; //string.Format("{0}_{1,8:D8}.png", prefix, "CenterCam_" + curFrameCount);
        //Debug.Log(ij1.filename);
        ij1.bytes = image2.EncodeToJPG();

        ij1.filename = GetFilePath() + "RightCam_" + curFrameCount + ".jpg"; //string.Format("{0}_{1,8:D8}.png", prefix, "CenterCam_" + curFrameCount);
        //Debug.Log(ij1.filename);
        ij1.bytes = image3.EncodeToJPG();

        lock (this)
        {
            imagesToSave.Add(ij1);
            imagesToSave.Add(ij2);
            imagesToSave.Add(ij3);

        }

    }


    //Save the camera sensor to an image. Use the suffix to distinguish between cameras.
    void SaveCamSensor(CameraSensor cs1, string prefix)
    {


        Texture2D image1 = cs1.GetImage();
        

        //image.GetPixelData

        ImageSaveJob ij1 = new ImageSaveJob();
       


        ij1.filename = GetFilePath() + prefix + curFrameCount + ".jpg"; //string.Format("{0}_{1,8:D8}.png", prefix, "CenterCam_" + curFrameCount);
        //Debug.Log(ij1.filename);
        ij1.bytes = image1.EncodeToJPG();


        lock (this)
        {
            imagesToSave.Add(ij1);
            

        }

    }

    public void SaverThread()
    {
        while (true)
        {
            int count = 0;

            lock (this)
            {
                count = imagesToSave.Count;
            }

            if (count > 0)
            {
                ImageSaveJob ij = imagesToSave[0];

                //Debug.Log("saving: " + ij.filename);

                File.WriteAllBytes(ij.filename, ij.bytes);

                lock (this)
                {
                    imagesToSave.RemoveAt(0);
                }
            }
        }
    }


    public void Togglelogger()
    {
        //if isLog is true; isLog = false
        //if isLog is false; isLog = true;

        isLog = !isLog;
    }

    //helper function to close instances
    private void Close()
    {
        if(sw != null)
        {
            sw.Close();
        }


    }


    
}
