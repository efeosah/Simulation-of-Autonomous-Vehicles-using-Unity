using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    bool isTraining;

    private float horizontalIput;
    private float verticalInput;
    [SerializeField]private float currentSteerAngle; 
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    private float throttle;


    //f :: Front
    //l :: Left
    //b :: Back
    //r :: Right
    [SerializeField] private WheelCollider FLWheelCollider;
    [SerializeField] private WheelCollider FRWheelCollider;
    [SerializeField] private WheelCollider BLWheelCollider;
    [SerializeField] private WheelCollider BRWheelCollider;

    [SerializeField] private Transform FLWheelTransform;
    [SerializeField] private Transform FRWheelTransform;
    [SerializeField] private Transform BLWheelTransform;
    [SerializeField] private Transform BRWheelTransform;




    //movement variables


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //fiexd update better for physics calculations
    private void FixedUpdate()
    {

        if(!isTraining)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            HandleWheel();
        }

        //Debug.Log(throttle);

    }

    private void HandleMotor()
    {
        throttle = verticalInput * motorForce;
        FLWheelCollider.motorTorque = throttle;
        FRWheelCollider.motorTorque = throttle;

        currentBreakForce = isBreaking ? breakForce : 0.0f;
        ApplyBreaks();
    }

    private void ApplyBreaks()
    { 
        FLWheelCollider.brakeTorque = currentBreakForce;
        BLWheelCollider.brakeTorque = currentBreakForce;
        FRWheelCollider.brakeTorque = currentBreakForce;
        BRWheelCollider.brakeTorque = currentBreakForce;

    }

    private void HandleWheel()
    {
        UpdateSingleWheel(FLWheelCollider, FLWheelTransform);
        UpdateSingleWheel(FRWheelCollider, FRWheelTransform);
        UpdateSingleWheel(BLWheelCollider, BLWheelTransform);
        UpdateSingleWheel(BRWheelCollider, BRWheelTransform);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {

        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
        
    }

    public float GetCurThrottle()
    {
        return throttle;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalIput;
        FLWheelCollider.steerAngle = currentSteerAngle;
        FRWheelCollider.steerAngle = currentSteerAngle;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void GetInput()
    {
        horizontalIput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    public float GetCurSteeringAngle()
    {
        return currentSteerAngle;
    }
}
