using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    //[SerializeField] private GameObject car;
    //private Vector3 camDistance = new Vector3(0, 5, -10);
    //private Transform offset;

    //// Start is called before the first frame updatexs
    //void Start()
    //{

    //    //gameObject.transform.position = car.transform.position + camDistance;
    //    offset = car.transform;
    //    offset.position += new Vector3(0, 0, 10);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    gameObject.transform.position = car.transform.position + camDistance;


    //    //instead of looking directly at the car
    //    //look slightly above the car

    //    gameObject.transform.LookAt(offset);

    //}

    public Transform target;

    public float approachPosRate = 0.1f;
    public float approachRotRate = 0.05f;

    void FixedUpdate()
    {
        if (target != null)
        {
            float fixedDeltaTimeRate = (Time.fixedDeltaTime / 0.02f);
            transform.position = Vector3.Lerp(transform.position, target.position, approachPosRate * fixedDeltaTimeRate);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, approachRotRate * fixedDeltaTimeRate);
        }
    }
}
