using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    
    [SerializeField]
    Camera CamSensor1;
    [SerializeField]
    int FrameCount;

    // Start is called before the first frame update
    void Start()
    {

        //plane = GameObject.Find("Plane");
        //plane = gameObject.transform.parent.transform.GetChild(2).gameObject;
        Debug.Log(1.0f/Time.deltaTime);
       
    }

    // Update is called once per frame
    // FPS = 50
    void Update()
    {
        gameObject.transform.Rotate(90.0f/50.0f, 0.0f/50.0f, 90.0f/50.0f, Space.Self);




    }


    
}
