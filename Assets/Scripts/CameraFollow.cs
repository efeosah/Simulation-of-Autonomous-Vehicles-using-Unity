using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;
   public float smoothSpeed = 0.125f;
   public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedSpeed = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
        transform.position = smoothedSpeed;

        transform.LookAt(target);
    }
}
