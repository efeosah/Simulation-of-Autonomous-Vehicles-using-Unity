using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// PID Controller written by Tawn Kramer
/// 
/// </summary>


public class PIDController : MonoBehaviour
{

    public GameObject carObj;
    //public ICar car;
    public CarController car;

    public PathManager pm;

    float errA, errB;
    float Kp = GlobalState.kp;
    float Kd = GlobalState.kd;
    float Ki = GlobalState.ki;

    //Ks is the proportion of the current vel that
    //we use to sample ahead of the vehicles actual position.
    public float Kv = 0.0f;

    //Ks is the proportion of the current err that
    //we use to change throtlle.
    public float Kt = 1.0f;

    float diffErr = 0f;
    public float prevErr = 0f;
    public float steeringReq = 0.0f;
    public float throttleVal = 0.5f;
    public float totalError = 0f;
    public float absTotalError = 0f;
    public float totalAcc = 0f;
    public float totalOscilation = 0f;
    public float AccelErrFactor = 0.1f;
    public float OscilErrFactor = 10f;

    public delegate void OnEndOfPathCB();
    public OnEndOfPathCB endOfPathCB;

    public bool isDriving = true;
    public bool brakeOnEnd = false;
    public bool looping = true;

    public float maxSpeed = 5.0f;
    public int iActiveSpan = 0;
	public int lookAhead = 1;

    public Text pid_steering;

    void Awake()
    {
        car = carObj.GetComponent<CarController>();
        pm = GameObject.FindObjectOfType<PathManager>();

        if (pm == null)
            Debug.LogWarning("couldn't get PathManager reference");

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        //GameObject go = CarSpawner.getChildGameObject(canvas.gameObject, "PIDSteering");
        //if (go != null)
        //    pid_steering = go.GetComponent<Text>();

    }

    private void OnEnable()
    {
        StartDriving();
    }

    private void OnDisable()
    {
        StopDriving();
    }

    public void StartDriving()
    {
        if (pm == null || !pm.isActiveAndEnabled || pm.carPath == null)
            return;

        isDriving = true;
        steeringReq = 0f;
        prevErr = 0f;
        totalError = 0f;
        totalAcc = 0f;
        totalOscilation = 0f;
        absTotalError = 0f;

        iActiveSpan = pm.carPath.GetClosestSpanIndex(carObj.transform.position);
        //car.Resume();
    }

    public void StopDriving()
    {
        isDriving = false;
        //car.RequestThrottle(0.0f);
        //car.RequestHandBrake(1.0f);
        //car.RequestFootBrake(1.0f);
        //car.RequestSteering(0.0f);
        //car.Move(0, 0, 0, 1.0f);
        //car.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDriving) { return; }

        PathNode n = pm.carPath.GetNode(iActiveSpan);

        float velMag = car.GetVelocity().magnitude;
        Vector3 samplePos = car.GetTransform().position + (car.GetTransform().forward * velMag * Kv);

        float err = 0.0f;
        bool cte_ret = pm.carPath.GetCrossTrackErr(samplePos, ref iActiveSpan, ref err, lookAhead);

        diffErr = (err - prevErr) / Time.deltaTime;
        steeringReq = -(Kp * err) - (Kd * diffErr) - (Ki * totalError);
        // Debug.Log(steeringReq);
        steeringReq = Mathf.Clamp(steeringReq, -car.GetMaxSteering(), car.GetMaxSteering());

        //car.RequestSteering(steeringReq);

        // need to refactor this
        if (car.GetVelocity().magnitude < maxSpeed)
        {
            //car.RequestThrottle(throttleVal);
        }


        car.Move(steeringReq, throttleVal, throttleVal, 0);



        if (pid_steering != null)
            pid_steering.text = string.Format("PID: {0}", steeringReq);

        totalError += err;
        prevErr = err;

        if (cte_ret) // check wether we lapped
        {
            if (looping)
            {
                var foundObjects = FindObjectsOfType<Logger>();

                foreach (var logger in foundObjects)
                    logger.curFrameCount++;
            }
            else if (brakeOnEnd)
            {
                car.RequestFootBrake(1.0f);

                if (car.GetAccel().magnitude < 0.0001f)
                {
                    isDriving = false;

                    if (endOfPathCB != null)
                        endOfPathCB.Invoke();
                }
            }
            else
            {
                isDriving = false;

                if (endOfPathCB != null)
                    endOfPathCB.Invoke();
            }
        }
    }
}
