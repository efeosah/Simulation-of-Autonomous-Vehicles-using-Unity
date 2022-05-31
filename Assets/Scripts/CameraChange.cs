using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    // Start is called before the first frame update


    public Camera ThirdPersonCamera;
    public Camera mainCamera;
    



    // Update is called once per frame



    void Start(){
    	mainCamera.enabled = true;
    	ThirdPersonCamera.enabled = false;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            mainCamera.enabled = !mainCamera.enabled;
            ThirdPersonCamera.enabled = !ThirdPersonCamera.enabled;
        }
    }
}
