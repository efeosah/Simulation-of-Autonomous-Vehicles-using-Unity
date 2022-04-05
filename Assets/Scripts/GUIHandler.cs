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
    //public GameObject infoPanel;
    public GameObject PIDController; //TODO
    public GameObject Logger;
    public GameObject menuPanel;
    public GameObject stopPanel;
    public GameObject ManualController; //TODO
    public GameObject Controllers;
    public GameObject Client;


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
    CarSpawner cs;

    //client object
    private Client client;


    private void Awake()
    {
        //Find the canvas that holds all our panels
        //Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        //menuPanel = canvas.transform.Find("MenuPanel").gameObject;
        //infoPanel = canvas.transform.Find("InfoPanel").gameObject;
        //stopPanel = canvas.transform.Find("StopPanel").gameObject;


        

        //try
        //{

        //    //NetworkSteering = GameObject.Find("Client");
        //    //client = NetworkSteering.GetComponent<Client>();

        //    //NetworkSteering.SetActive(false);
            
        //}
        //catch (Exception e)
        //{
        //    print("Could'nt find client script, Error: "+ e);
        //}

        
        

    }

    // Start is called before the first frame update
    void Start()
    {

        car = GameObject.FindObjectOfType<Car>();
        rb = GameObject.FindObjectOfType<RoadBuilder>();
        pm = GameObject.FindObjectOfType<PathManager>();
        cs = GameObject.FindObjectOfType<CarSpawner>();



        if (car)
        {
            Controllers = car.gameObject.transform.GetChild(0).gameObject;
            if (Controllers)
            {
                ManualController = Controllers.transform.GetChild(0).gameObject;
                PIDController = Controllers.transform.GetChild(1).gameObject;
                Logger = Controllers.transform.GetChild(2).gameObject;
                Client = Controllers.transform.GetChild(3).gameObject;
            }

            if (ManualController)
            {
                ManualController.SetActive(false);
            }

            if (PIDController)
            {
                PIDController.SetActive(false);
            }

            if (Logger)
            {
                Logger.SetActive(false);
            }

            if (Client)
            {
                client = Client.GetComponent<Client>();
                Client.SetActive(false);
            }

            //if (!infoPanel)
            //{
            //    print("Info panel not available");
            //}

        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
        //UpdateRightPanel();
        if(curMode == "None")
        {
            car.RequestThrottle(0.0f);
            car.RequestSteering(0.0f);
            car.RequestHandBrake(1.0f);
            car.RequestFootBrake(1.0f);
        }
    }

    public string GetCurMode()
    {
        return curMode;
    }
   

    //spawn/regen new track button
    public void OnSpawnNewTrack()
    {
        rb.DestroyRoad();
        pm.InitCarPath();

        //reset overhead camera
        if (GlobalState.overheadCamera)
        {
            GameObject OHCamGo = cs.cameras[0];
            OverHeadCamera overheadCamera = OHCamGo.GetComponent<OverHeadCamera>();
            overheadCamera.Init();
        }



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

        //NetworkSteering.SetActive(true);
        //NetworkSteering.GetComponent<Client>().Connect();
        Client.SetActive(true);
        client.Connect();
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
            client.Disconnect();

            car.RequestThrottle(0.0f);
            car.RequestSteering(0.0f);
            car.RequestHandBrake(1.0f);
            car.RequestFootBrake(1.0f);


            Client.SetActive(false);
        }


        //car.ToggleModeSelect();

        curMode = "None";
    }

    public void OnExit()
    {
        //End game here
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void onRestart()
    {
        car.transform.position = car.startPos;
        car.transform.rotation = car.startRot;
    }
}
