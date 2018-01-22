using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    

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

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }


    // Run ever physics iteration
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
                cam.transform.Rotate(-cameraRotation);
            }
        }
    }

}
