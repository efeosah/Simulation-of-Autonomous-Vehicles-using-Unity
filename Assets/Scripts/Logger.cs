using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    [SerializeField] Camera CamSensor1;

    [SerializeField] int curFrameCount = 0;
    public int maxFrameCount;
    //[SerializeField] GameObject car;
    private CarController carControl;
    private float timeSinceLastLog = 0.0f;
 

    private float FPS = 30.0f;

    private StreamWriter sw;

    private string filename = "driving_log.csv";
    
 

    [SerializeField] private bool isLog = true;

    //private string filepath = Application.dataPath + "/../log/";

    // Start is called before the first frame update
    void Start()
    {

        
        Debug.Log(Application.dataPath);
        
    }

    private string GetFilePath()
    {
        //store data in a folder called "log" in the "assets" folder
        return Application.dataPath + "/log/";
    }

    private void Awake()
    {
        //maxFrameCount = 10000;

        carControl = gameObject.GetComponentInParent<CarController>();
        

        if(isLog && carControl != null)
        {
            //TODO

            string filePath = GetFilePath() + filename;
            //initialize filewriter
            sw = new StreamWriter(filePath);
        }


        
        


    }

    // Update is called once per frame
    // FPS = 50
    void Update()
    {

        if(!isLog)
        {
            return;
        }
        
        timeSinceLastLog += Time.deltaTime;
        //gameObject.transform.Rotate(90.0f/50.0f, 0.0f/50.0f, 90.0f/50.0f, Space.Self);
        if (timeSinceLastLog < 1.0f / FPS)
        {
            return;
            //TODO
        }

        timeSinceLastLog -= (1.0f / FPS);

        LogDataToCSV();

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
        float curSteerAngle = carControl.GetCurSteeringAngle();
        float curThrottle = carControl.GetCurThrottle();
        string camImage = "CenterCam_" + curFrameCount;

        Debug.Log("Steering Angle: " + curSteerAngle.ToString() + " Throttle: " + curThrottle.ToString());

        sw.WriteLine(string.Format("{0},{1},{2}", camImage, curSteerAngle.ToString(), curThrottle.ToString()));
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
