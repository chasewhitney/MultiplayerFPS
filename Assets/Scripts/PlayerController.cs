using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    //[SerializeField]
    //private float jumpVelocity = 5f;

    //[SerializeField]
    //private float fallMultiplier = 2.5f;

    //[SerializeField]
    //private float lowJumpMultiplier = 2f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring settings:")]
  
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    // Component caching
    private Animator animator;
    private PlayerMotor motor;
    private ConfigurableJoint joint;

   

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    void Update()
    {

        // Calculate movement velocity as 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        // Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        // Apply movement
        motor.Move(_velocity);

        // Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        
        // Apply rotation
        motor.Rotate(_rotation);

        // Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        // Apply camera rotation
        motor.RotateCamera(_cameraRotationX);


        // Calculate the thruster force based on player input
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce; // Vector3 (0,1,0)
            SetJointSettings(0f);
        } else
        {
            SetJointSettings(jointSpring);
        }

        // Apply the thruster force
        motor.ApplyThrusterForce(_thrusterForce);

        

        //// Jumping
        //if (Input.GetButtonDown("Jump"))
        //{
        //  rb.velocity = Vector3.up * jumpVelocity;
        //}

        //if (rb.velocity.y < 0)
        //{
        //    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        //} else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        //{
        //    rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        //}
        //// End Jumping
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { // setting using joint.yDrive.maximumForce syntax doesn't work
          
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
