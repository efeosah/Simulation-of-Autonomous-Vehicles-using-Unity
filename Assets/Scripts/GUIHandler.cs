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
    GameObject leftPanel;
    GameObject rightPanel;


    //boolean flags for modes
    bool trainingMode;
    bool PIDMode;
    bool regenTrack;
    bool arrowControlMode;

    //roadbuilder object to manage track
    RoadBuilder rb;

    // Start is called before the first frame update
    void Start()
    {

        car = gameObject.GetComponentInParent<CarController>();
        leftPanel = gameObject.transform.Find("LeftPanel").gameObject;
        rightPanel = gameObject.transform.Find("RightPanel").gameObject;
        rb = gameObject.GetComponentInParent<RoadBuilder>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLeftPanel();
        UpdateRightPanel();
    }

    private void UpdateRightPanel()
    {
        throw new NotImplementedException();
    }

    private void UpdateLeftPanel()
    {
        //variables/info to show on left panel
        float steering;
        string mode;
        float speed;
        float throttle;
        bool isTraining = trainingMode;

        //write infor into text object 
        leftPanel.GetComponentInChildren<Text>().text = "Steering: " + "\n" +
            "Mode:    " + "\n" +
            "Speed:   " + "\n" +
            "Throttle:   " + "\n" +
            "isTraining:   " + isTraining + "\n";

    }
}
