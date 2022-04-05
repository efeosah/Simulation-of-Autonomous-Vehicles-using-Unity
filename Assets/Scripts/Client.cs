using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using System;
using System.Security.AccessControl;

public class Client : MonoBehaviour
{
	//public Car carController;
	public CameraSensor FrontFacingCamera;
	private SocketIOComponent _socket;
	private Car _carController;

    // Use this for initialization
    void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
		_socket.On("manual", onManual);
		//_carController = carController.GetComponent<Car>();
		//_carController = transform.root.gameObject.GetComponent<Car>();
		_carController = GameObject.FindObjectOfType<Car>();






	}

	// Update is called once per frame
	void Update()
	{
	}

	public void Connect()
	{
		if (_socket)
		{
			_socket.Connect();
		}
	}

	public void Disconnect()
	{
		
		_socket.Close();
       
	}

    private void OnDisable()
    {
        if (_carController)
        {
			_carController.RequestThrottle(0.0f);
			_carController.RequestSteering(0.0f);
			_carController.RequestHandBrake(1.0f);
			_carController.RequestFootBrake(1.0f);
		}
	}

    void OnOpen(SocketIOEvent obj)
	{
		Debug.Log("Connection Open");
		EmitTelemetry(obj);
	}

	// 
	void onManual(SocketIOEvent obj)
	{
		Debug.Log("Manual");
		EmitTelemetry(obj);

	}

	void OnSteer(SocketIOEvent obj)
	{
		//Debug.Log(">>>");
		JSONObject jsonObject = obj.data;
		//    print(float.Parse(jsonObject.GetField("steering_angle").str));


		float steering = float.Parse(jsonObject.GetField("steering_angle").str);

		float throttle = float.Parse(jsonObject.GetField("throttle").str);

		steering = Mathf.Clamp(steering, -1.0f, 1.0f);
		throttle = Mathf.Clamp(throttle, -1.0f, 1.0f);

		steering *= _carController.GetMaxSteering();



		transform.root.gameObject.GetComponent<Car>().RequestSteering(steering);
		transform.root.gameObject.GetComponent<Car>().RequestThrottle(throttle);
		EmitTelemetry(obj);


	}

	void EmitTelemetry(SocketIOEvent obj)
	{

		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			print("Attempting to Send...");

			// send only if it's not being manually driven
			if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)))
			{
				_socket.Emit("telemetry", new JSONObject());
			}
			else
			{
				// Collect Data from the Car
				Dictionary<string, string> data = new Dictionary<string, string>();
				data["steering_angle"] = (_carController.GetSteering() / _carController.GetMaxSteering()).ToString("N4");
				//Debug.Log(_carController.GetSteering().ToString("N4"));
				data["throttle"] = _carController.GetThrottle().ToString("N4");
				data["speed"] = _carController.GetVelocity().magnitude.ToString("N4");
				
				data["image"] = Convert.ToBase64String(FrontFacingCamera.GetImageBytes());
				_socket.Emit("telemetry", new JSONObject(data));
			}
		});

	}
}