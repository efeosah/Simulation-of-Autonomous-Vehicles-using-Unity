using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    [SerializeField] private GameObject car;
    private Vector3 camDistance = new Vector3(0, 13, -10);

    // Start is called before the first frame update
    void Start()
    {
        
        //gameObject.transform.position = car.transform.position + camDistance; 
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = car.transform.position + camDistance;


        //instead of looking directly at the car
        //look slightly above the car
        //Transform offset = car.transform;
        //offset.position = new Vector3(car.transform.position.x, 3, car.transform.position.z);
        gameObject.transform.LookAt(car.transform);

    }
}
