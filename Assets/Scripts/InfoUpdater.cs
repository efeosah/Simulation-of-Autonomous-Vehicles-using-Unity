using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InfoUpdater : MonoBehaviour
{

    [SerializeField] string curMode;
    [SerializeField] Car car;
    [SerializeField] GUIHandler guihandler;

    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindObjectOfType<Car>();

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
        float steering = car.GetSteering();
        string mode = curMode;



        //write infor into text object 
        gameObject.GetComponentInChildren<Text>().text =

            "Steering: " + steering.ToString() + "\n" +
            "Mode:    " + mode + "\n";

    }
}
