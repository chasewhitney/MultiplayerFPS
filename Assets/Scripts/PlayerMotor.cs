using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Gets movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // Gets movement vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    // Get a force vector for thruster
    public void ApplyThrusterForce(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }


    // Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        
    }

    // Perform movement based on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            // moves rb to position of rb + the velocity vector
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        }
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

    }

    // Perform rotation
    void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {
            // moves rb to position of rb + the velocity vector
            rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
            if (cam != null)
            {
                // Set rotation and clamp it
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

                // Apply rotation to transform of the camera
                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            }
        }
    }

    
}
