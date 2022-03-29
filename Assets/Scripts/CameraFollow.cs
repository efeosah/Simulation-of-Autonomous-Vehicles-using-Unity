using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;
  //public float smoothSpeed = 0.125f;
   //public Transform cameraPosition ;
   

   // public Vector3 offset;
   // Quaternion offsetRotation;

 
    // Update is called once per frame
     // public float distance = 10; 
     // public float height = 20;
     // public float heightDamping = 0.25f; 
     // public float rotationDamping=0.15f ;

   // void start(){
   //   offsetRotation = transform.rotation * Quaternion.Inverse(target.transform.rotation);
   //   offset = transform.position - target.transform.position;
   // }
    void LateUpdate()

    { 
        //Vector3 desiredPosition = target.position + offset;
         //Vector3 smoothedSpeed = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
         //transform.position = smoothedSpeed;

         //Vector3 resultingPosition = cameraPosition.position + cameraPosition.forward * distance;
         //transform.position = new Vector3 (resultingPosition.x, transform.position.y, resultingPosition.z);
         //transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, 0.8f);
         //transform.rotation = Quaternion.Slerp(target.transform.rotation, target.transform.rotation * offsetRotation, 0.8f);

         //transform.LookAt(new Vector3 (resultingPosition.x, transform.position.y, resultingPosition.z));
         // Always look at the target
         transform.LookAt(target);
    }



}
