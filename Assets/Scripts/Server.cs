using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using System;
using System.Security.AccessControl;

public class Server : MonoBehaviour
{
	public Car carController;
	public CameraSensor FrontFacingCamera;
	private SocketIOComponent _socket;
	private Car _carController;

	// Use this for initialization
	void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
   //     if (_socket)
   //     {
			//Debug.Log("<<<");
   //     }
		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
		_socket.On("manual", onManual);
		_carController = carController.GetComponent<Car>();
	}

	// Update is called once per frame
	void Update()
	{
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
		carController.RequestSteering(float.Parse(jsonObject.GetField("steering_angle").str));
		carController.RequestThrottle(float.Parse(jsonObject.GetField("throttle").str));
		EmitTelemetry(obj);
		

	}

	void EmitTelemetry(SocketIOEvent obj)
	{
		//Debug.Log("Emit");
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			print("Attempting to Send...");
			// send only if it's not being manually driven
			if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)))
			{
				//print("Manual");
				_socket.Emit("telemetry", new JSONObject());
			}
			else
			{
				// Collect Data from the Car
				Dictionary<string, string> data = new Dictionary<string, string>();
				data["steering_angle"] = _carController.GetSteering().ToString("N4");
				//Debug.Log(_carController.GetSteering().ToString("N4"));
				data["throttle"] = _carController.GetThrottle().ToString("N4");
				data["speed"] = _carController.GetVelocity().magnitude.ToString("N4");
				//Debug.Log("WE got her");
				//print(data.Values.ToString());
				foreach (KeyValuePair<string, string> kvp in data)
				{
					print(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
				}
				data["image"] = Convert.ToBase64String(FrontFacingCamera.GetImageBytes());
                _socket.Emit("telemetry", new JSONObject(data));
			}
		});

		//    UnityMainThreadDispatcher.Instance().Enqueue(() =>
		//    {
		//      	
		//      
		//
		//		// send only if it's not being manually driven
		//		if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
		//			_socket.Emit("telemetry", new JSONObject());
		//		}
		//		else {
		//			// Collect Data from the Car
		//			Dictionary<string, string> data = new Dictionary<string, string>();
		//			data["steering_angle"] = _carController.CurrentSteerAngle.ToString("N4");
		//			data["throttle"] = _carController.AccelInput.ToString("N4");
		//			data["speed"] = _carController.CurrentSpeed.ToString("N4");
		//			data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera));
		//			_socket.Emit("telemetry", new JSONObject(data));
		//		}
		//      
		////      
		//    });
	}
}