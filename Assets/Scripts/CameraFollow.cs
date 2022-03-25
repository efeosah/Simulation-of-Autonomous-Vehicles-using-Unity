using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;
   public float smoothSpeed = 0.125f;
   public Vector3 offset;
   public float max_x;
   public float max_y;
   public float max_z;
   //public float desiredAngle = target.eulerAngles.y;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedSpeed = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
        transform.position = smoothedSpeed;
        //Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.LookAt(target);
    }
}
