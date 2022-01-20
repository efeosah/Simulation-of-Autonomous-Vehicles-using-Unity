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
    CarController car;

    //gameobjects to reference panels
    public GameObject infoPanel;
    public GameObject PIDContoller; //TODO
    public GameObject Logger;
    public GameObject NetworkSteering; //TODO
    public GameObject menuPanel;
    public GameObject stopPanel;
    public GameObject PIDControls; //TODO


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


    private void Awake()
    {
        //Find the canvas that holds all our panels
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        menuPanel = canvas.transform.Find("MenuPanel").gameObject;
        infoPanel = canvas.transform.Find("InfoPanel").gameObject;
        stopPanel = canvas.transform.Find("StopPanel").gameObject;


        car = GameObject.FindObjectOfType<CarController>();
        rb = GameObject.FindObjectOfType<RoadBuilder>();
        pm = GameObject.FindObjectOfType<PathManager>();

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
        float steering = 0;
        string mode = curMode;
        float speed;
        float throttle;


        //write infor into text object 
        infoPanel.GetComponentInChildren<Text>().text =

            "Steering: " + steering.ToString() + "\n" +
            "Speed:   " + "\n" +
            "Throttle:   " + "\n" +
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

        car.transform.position = new Vector3(0, 0, 0);
        car.transform.rotation = Quaternion.identity;
    }

    public void OnManualTrain()
    {
        //set manual train only

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
        car.ToggleManualDrive();
        car.ToggleModeSelect();
        curMode = "Manual Driving";

    }

    public void OnPIDTrain()
    {
        //set PID train only

        curMode = "PID Training";
    }

    public void OnPIDDrive()
    {
        //set PID drive only
        curMode = "PID Driving";
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

        car.ToggleModeSelect();
        curMode = "None";
    }

    public void OnExit()
    {
        //End game here
        Application.Quit();
    }
}
