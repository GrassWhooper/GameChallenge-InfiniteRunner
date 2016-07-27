using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

    [Tooltip(" Force to be put into {X} Direction when moving ")]
    public float speed = 30;

    [Tooltip("maximum speed on the {X} Direction")]
    public float maxSpeed = 5f;
    
    [Tooltip("Auto Move Force put into the {Z} Direction")]
    public float autoMoveSpeed = 30;

    [Tooltip("Maximum Speed in the {Z} Direction")]
    public float maxAutoMoveSpeed=80f;

    [Tooltip("this is applied when not moving above one, speed will increase")]
    public float speedMultiplierWhenNotMoving = 0.9f;


    float yMovement;

    float xMovement;
    Rigidbody rb3d;
    Vector3 movementDirection;
    float currentXSpeed;



    // Use this for initialization
    void Start ()
    {
        rb3d = GetComponent<Rigidbody>();
        movementDirection = new Vector3(1, 1, 1);

    }
	
	// Update is called once per frame
	void Update ()
    {
        xPlayerMovement();
        yPlayerMovement();
        print(rb3d.velocity);
    }

    void AcceleratePlayerOnZ( float yMovement , Vector3 Acceleration)
    {



    }

    void yPlayerMovement()
    {
        yMovement = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(1, 1, 1);
        Vector3 movementForce = new Vector3(0, 0, 0);
        if (Mathf.Abs(rb3d.velocity.z) < (maxAutoMoveSpeed))
        {
            movementForce.z = movementForce.z + movementDirection.z;
            movementForce = movementForce * autoMoveSpeed;
            if (yMovement != 0 )
            {
                AcceleratePlayerOnZ(yMovement,movementForce );
            }
            rb3d.AddForce(movementForce);
        }
        else if (Mathf.Abs( rb3d.velocity.z )>= maxAutoMoveSpeed)
        {
            Vector3 speedLimiter = new Vector3(0, 0, 0);
            speedLimiter.z = speedLimiter.z + movementDirection.z;
            speedLimiter = speedLimiter * maxAutoMoveSpeed;
            speedLimiter.x = rb3d.velocity.x;
            if (yMovement != 0)
            {
                AcceleratePlayerOnZ(yMovement,speedLimiter);
            }
            rb3d.velocity = speedLimiter;
        }
    }

    void xPlayerMovement()
    {
        xMovement = CrossPlatformInputManager.GetAxis("Horizontal");
          
        if (xMovement!=0)
        {
            if (Mathf.Abs(rb3d.velocity.x)<maxSpeed)
            {
                Vector3 movementForce = new Vector3(0,0,0);
                movementForce.x = movementForce.x + movementDirection.x;
                movementForce = movementForce * xMovement * speed;

                rb3d.AddForce(movementForce);
                currentXSpeed = rb3d.velocity.x;
            }
            else if (Mathf.Abs(rb3d.velocity.x)>=maxSpeed)
            {
                Vector3 speedLimiter = new Vector3();
                speedLimiter.x = movementDirection.x;
                speedLimiter.x = speedLimiter.x * maxSpeed * xMovement;

                speedLimiter.z = rb3d.velocity.z;

                rb3d.velocity = speedLimiter;

                currentXSpeed = rb3d.velocity.x;
            }
        }
        if (xMovement==0)
        {
            Vector3 speedDecrementer = new Vector3();
            speedDecrementer.x = movementDirection.x;
            speedDecrementer.x = currentXSpeed * speedMultiplierWhenNotMoving;

            speedDecrementer.z = rb3d.velocity.z;

            currentXSpeed = speedDecrementer.x;
            rb3d.velocity = speedDecrementer;

        }
  
    }
}
