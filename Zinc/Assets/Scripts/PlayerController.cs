using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0,1)]
    public float airControlPerc;
    bool running;
    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    float butCooler = .9f;
    float butCount = 0;
    float headRotation;

    public Animator animator;
    public Transform cameraT;
    public Transform head;
    public CharacterController controller;
    Interactable focus;

    public delegate void onCameraModeChange();
    public onCameraModeChange cameraChangedCallback;

	void Start () {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cameraT = FindObjectOfType<Camera>().transform;
    }
	

	void Update () {
        //Input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        // Double tap to run
        if (!Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKeyDown(KeyCode.W)) {
                if (butCooler > 0 && butCount == 1) {
                    running = true;
                }
                else {
                    butCooler = .9f;
                    butCount += 1;
                }
            } 
        } else { running = Input.GetKey(KeyCode.LeftShift);  }
        if (butCooler > 0) { butCooler -=1*Time.deltaTime; } else { butCount = 0;  }

        // Movement
        Move(inputDir, running);
        if (input.magnitude == 0) { running = false; }
        // Jumping 
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("Throwing");
            if (cameraChangedCallback != null) { cameraChangedCallback.Invoke(); }
        }
        

        //Animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        HeadFaceTarget(); // Rotates head to focus


    }

    void Move(Vector2 inputDir, bool running) {

        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded) { velocityY = 0; }


    }

    void Jump() {
        if (controller.isGrounded) {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }

    }

    float GetModifiedSmoothTime(float smoothTime) {
        if (controller.isGrounded) {
            return smoothTime;
        }
        if(airControlPerc == 0) { return float.MaxValue; }
        return smoothTime / airControlPerc;
    }


    void HeadFaceTarget() {

       if (focus != null) {
            // Rotate to face focus object
            Debug.Log("focused on " + focus.name);
        } else {
            head.rotation = transform.rotation;
        }

    }
}
