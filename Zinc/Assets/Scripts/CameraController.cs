using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public bool lockCursor;
    public float yaw;
    public float pitch;
    public float mouseSensitivity = 10;
    public float currentZoom = 2f;
    public float rotationSmoothTime = .12f;
    public float zoomSpeed = 4f;
    public Vector2 zoomMinMax = new Vector2(2f, 15f);
    public Vector2 pitchMinMax = new Vector2(-40f, 85);
    public Transform target;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;



    private void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void LateUpdate () {
        yaw += Input.GetAxis("Mouse X")* mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y")* mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, zoomMinMax.x,zoomMinMax.y);
        

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * currentZoom;
	}
}
