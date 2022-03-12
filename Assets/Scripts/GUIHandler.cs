using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GUI handler script to handle GUI events
/// </summary>

public class GUIHandler : MonoBehaviour
{
    //car controller game object
    Car car;

    //gameobjects to reference panels
    public GameObject infoPanel;
    public GameObject PIDController; //TODO
    public GameObject Logger;
    public GameObject NetworkSteering;
    public GameObject menuPanel;
    public GameObject stopPanel;
    public GameObject ManualController; //TODO
    public GameObject Controllers;


    //boolean flags for modes
    //bool trainingMode;
    //bool PIDMode;
    //bool regenTrack;
    //bool arrowControlMode;

    //maybe just make it a string
    string curMode = "None";

    //roadbuilder object to manage track
    RoadBuilder rb;
    PathManager pm;

    //client object
    public Server client;


    private void Awake()
    {
        //Find the canvas that holds all our panels
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        menuPanel = canvas.transform.Find("MenuPanel").gameObject;
        infoPanel = canvas.transform.Find("InfoPanel").gameObject;
        stopPanel = canvas.transform.Find("StopPanel").gameObject;


        car = GameObject.FindObjectOfType<Car>();
        rb = GameObject.FindObjectOfType<RoadBuilder>();
        pm = GameObject.FindObjectOfType<PathManager>();

        Controllers = car.gameObject.transform.GetChild(0).gameObject;
        if(Controllers != null)
        {
            ManualController = Controllers.transform.GetChild(0).gameObject;
            PIDController = Controllers.transform.GetChild(1).gameObject;
            Logger = Controllers.transform.GetChild(2).gameObject;


            if (ManualController != null)
            {
                ManualController.SetActive(false);
            }

            if(PIDController != null)
            {
                PIDController.SetActive(false);
            }

            if(Logger != null)
            {
                Logger.SetActive(false);
            }
        }

        //client = GameObject.FindObjectOfType<Server>();
        if (client != null)
        {
            //Debug.Log("Not Empty");

            NetworkSteering = client.gameObject;
            if (NetworkSteering == null)
            {
                //NetworkSteering.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Empty");
        }
        //if (NetworkSteering == null)
        //{
        //    Debug.Log("Empty");
        //}

    }

    // Start is called before the first frame update
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfoPanel();
        //UpdateRightPanel();
    }

   
    //Update info panel top left
    private void UpdateInfoPanel()
    {
        //variables/info to show on left panel
        float steering = car.GetSteering();
        string mode = curMode;


        //write infor into text object 
        infoPanel.GetComponentInChildren<Text>().text =

            "Steering: " + steering.ToString() + "\n" +
            "Mode:    " + mode + "\n";

    }

    //spawn/regen new track button
    public void OnSpawnNewTrack()
    {
        rb.DestroyRoad();
        pm.InitCarPath();

        //reset car transform
        //reset to beginnign of track
        //GameObject StartPos

        car.transform.position = car.startPos;
        car.transform.rotation = car.startRot;
    }

    public void OnManualTrain()
    {
        if (stopPanel != null)
        {
            stopPanel.SetActive(true);
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        //set manual train only
        ManualController.SetActive(true);
        Logger.SetActive(true);
        curMode = "Manual Training";

    }

    public void OnManualDrive()
    {
        //set manual drive only
        if(stopPanel != null)
        {
            stopPanel.SetActive(true);
        }
        if(menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        //car.ToggleManualDrive();
        //car.ToggleModeSelect();
        ManualController.SetActive(true);
        curMode = "Manual Driving";

    }

    public void OnPIDTrain()
    {
        //set PID train only
        if (stopPanel != null)
        {
            stopPanel.SetActive(true);
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        PIDController.SetActive(true);
        Logger.SetActive(true);
        curMode = "PID Training";
    }

    public void OnPIDDrive()
    {
        if (stopPanel != null)
        {
            stopPanel.SetActive(true);
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        //set PID drive only
        PIDController.SetActive(true);
        curMode = "PID Driving";
    }

    public void onNNSteering()
    {
        if (stopPanel != null)
        {
            stopPanel.SetActive(true);
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        //set PID drive only
        //NetworkSteering.SetActive(true);
        //client.Connect();

        curMode = "NN Steering";
    }

    public void OnStop()
    {
        if(stopPanel != null)
        {
            stopPanel.SetActive(false);
        }

        if(menuPanel != null)
        {
            menuPanel.SetActive(true);
        }

        if(curMode == "Manual Driving")
        {
            ManualController.SetActive(false);
        }

        if (curMode == "PID Driving")
        {
            PIDController.SetActive(false);
        }

        if (curMode == "PID Training")
        {
            PIDController.SetActive(false);
            Logger.SetActive(false);
        }

        if (curMode == "Manual Training")
        {
            ManualController.SetActive(false);
            Logger.SetActive(false);
        }

        if (curMode == "NN Steering")
        {
            NetworkSteering.SetActive(false);
        }


        //car.ToggleModeSelect();

        curMode = "None";
    }

    public void OnExit()
    {
        //End game here
        Application.Quit();
    }
}
