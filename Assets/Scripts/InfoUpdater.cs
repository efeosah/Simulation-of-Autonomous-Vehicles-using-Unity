using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class InfoUpdater : MonoBehaviour
{

    [SerializeField] string curMode;
    [SerializeField] CarController car;
    [SerializeField] GUIHandler guihandler;

    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindObjectOfType<CarController>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (guihandler)
        {
            curMode = guihandler.GetCurMode();
        }
        UpdateInfoPanel();
    }


    //Update info panel top left
    private void UpdateInfoPanel()
    {
        //variables/info to show on left panel
        float steering = (float)Math.Round(car.CurrentSteerAngle, 2);
        float speed = (float)Math.Round(car.CurrentSpeed, 2);

        if(speed < 0.0f)
        {
            speed = 0.0f;
        }
        float throttle = (float)Math.Round(car.AccelInput, 2);

        string speedType = car.Type;
        string mode = curMode;



        //write infor into text object 
        gameObject.GetComponentInChildren<Text>().text =

            "Steering: " + steering.ToString() + "\n" +
            "Speed: " + speed + speedType + "\n" +
            "Throttle: " + throttle.ToString() + "\n" +
            "Mode: " + mode + "\n";

    }
}
