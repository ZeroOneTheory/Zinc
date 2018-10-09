using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    public Rigidbody objToss;
    public GameObject tossing;
    public Transform target;
    public Camera gameCamera;
    public LayerMask ground;
    public bool debugPath;
    public float maxThrowHeight = 25;
    public float gravity = -18;

    GameObject held;


    private void Start() {

    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            held = Instantiate(tossing, transform.position, transform.rotation);
            objToss = held.GetComponent<Rigidbody>();
            if (objToss != null) {
                objToss.useGravity = false;
            
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            if (objToss != null) {
                Launch();
            }
        }

        if (debugPath) {
            if (objToss != null) {
                DrawPath();
            }

        }
    }


    void Launch() {
        Physics.gravity = Vector3.up * gravity;
        objToss.useGravity = true;
        objToss.velocity = CalcLaunchData().initialVelocity;
    }

    LaunchData CalcLaunchData() {
        float displacementY = target.position.y - objToss.position.y;
        Vector3 displacmentXZ = new Vector3(target.position.x - objToss.position.x, 0, target.position.z - objToss.position.z);
        float time = Mathf.Sqrt(-2 * maxThrowHeight / gravity) + Mathf.Sqrt(2 * (displacementY - maxThrowHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxThrowHeight);
        Vector3 velocityXZ = displacmentXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity),time);
    }

    void DrawPath() {
        LaunchData launchData = CalcLaunchData();
        Vector3 previousDrawPoint = objToss.position;
        int resolution = 30;
        for(int i=0; i<=resolution; i++) {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = objToss.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }



    struct LaunchData {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget) {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
