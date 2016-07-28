using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour {

    enum movementStatuses {NormalMovement , AcceleratedMovement, SlowedDownMovement  };
    
    movementStatuses playerStatus;

    [Tooltip(" Force to be put into {X} Direction when moving ")]
    public float xSpeed = 30;

    [Tooltip("maximum speed on the {X} Direction")]
    public float xMaxSpeed = 5f;
    
    [Tooltip("Auto Move Force put into the {Z} Direction")]
    public float autoMoveSpeed = 30;

    [Tooltip("Maximum Speed in the {Z} Direction")]
    public float maxAutoMoveSpeed=60f;

    [Tooltip("this is applied when not moving above one, speed will increase")]
    public float speedMultiplierWhenNotMoving = 0.9f;

    [Tooltip("How Much To Multiply everything when accelerating ")]
    public float accelerationMultiplier = 2f;

    [Tooltip("How Much To Multiply everything when slowing down{Suggested Between 0.001 and 0.999} ")]
    public float slowDownMultiplier = 0.5f;

    [Tooltip("how long the player can keep accelerating his speed")]
    public float runningTimer = 4f;
    

    float resetRunningTimer,resetRunningCoolDown;
    float xMovement;
    Rigidbody rb3d;
    Vector3 BaseVector;
    float currentXSpeed;
    float initalXSpeed, initalXMaxSpeed, initalAutoMovespeed, initalMaxAutoMoveSpeed;
    [Tooltip("Select Keys For Each Action")]
    public List<KeyCode> JumpKeys , AccelerateKeys, SlowDownKeys;

    // Use this for initialization
    void Start ()
    {
        rb3d = GetComponent<Rigidbody>();
        BaseVector = new Vector3(1, 1, 1);
        playerStatus = new movementStatuses();
        playerStatus = movementStatuses.NormalMovement;


        initalXSpeed = xSpeed;
        initalXMaxSpeed = xMaxSpeed;
        initalAutoMovespeed = autoMoveSpeed;
        initalMaxAutoMoveSpeed = maxAutoMoveSpeed;

        
        resetRunningTimer = runningTimer;
    }

    void Update()
    {
        CheckForMovementStatus();
        print("running timer +++++" + runningTimer);
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        PlayerMovement();
    }

    void ReSetMovementStatusValues()
    {
        xSpeed = initalXSpeed;
        xMaxSpeed = initalXMaxSpeed;
        autoMoveSpeed = initalAutoMovespeed;
        maxAutoMoveSpeed = initalMaxAutoMoveSpeed;
    }
    void SetMovementStatusValues()
    {
        if (playerStatus == movementStatuses.NormalMovement)
        {
            ReSetMovementStatusValues();
        }
        else if (playerStatus == movementStatuses.AcceleratedMovement )
        {
            xSpeed = accelerationMultiplier * initalXSpeed;
            xMaxSpeed = accelerationMultiplier * initalXMaxSpeed;
            autoMoveSpeed = accelerationMultiplier * initalAutoMovespeed;
            maxAutoMoveSpeed = accelerationMultiplier * initalMaxAutoMoveSpeed;
        }
        else if (playerStatus==movementStatuses.SlowedDownMovement)
        {
            xSpeed = initalXSpeed;
            xMaxSpeed = slowDownMultiplier * initalXMaxSpeed;
            autoMoveSpeed = slowDownMultiplier * initalAutoMovespeed;
            //maxAutoMoveSpeed = slowDownMultiplier * initalMaxAutoMoveSpeed; //will snap to half
            maxAutoMoveSpeed = initalMaxAutoMoveSpeed; //will snap back to initial automove Z direction speed.
        }

    }

    void CheckForMovementStatus()
    {
        bool Accelerating= false;
        bool SlowingDown = false;

        foreach (KeyCode item in AccelerateKeys)
        {
            if (Input.GetKey(item))
            {
                playerStatus = movementStatuses.AcceleratedMovement;
                Accelerating = true;

                runningTimer -= Time.deltaTime;

                if (runningTimer <= 0f)
                {
                    Accelerating = false;
                }

                break;
            }
            else
            {
                Accelerating = false;
            }
        }
        foreach (KeyCode item in SlowDownKeys)
        {
            if (Input.GetKey(item))
            {
                playerStatus = movementStatuses.SlowedDownMovement;
                SlowingDown = true;

                if (runningTimer < resetRunningTimer)
                {
                    runningTimer += 2*Time.deltaTime;
                }

                break;
            }
            else
            {
                SlowingDown = false;
            }
        }
        if (!Accelerating && !SlowingDown)
        {
            playerStatus = movementStatuses.NormalMovement;
            if (runningTimer<resetRunningTimer)
            {
                runningTimer += Time.deltaTime;
            }
        }
    }

    void PlayerMovement()
    {
        xMovement = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector3 movementForce = new Vector3(0, 0, 0);
        Vector3 speedLimiter = new Vector3(0, 0, 0);
        Vector3 speedDecrementer = new Vector3();

        SetMovementStatusValues();

        if (Mathf.Abs(rb3d.velocity.z) < (maxAutoMoveSpeed))
        {
            movementForce.z = BaseVector.z;
            movementForce = movementForce * autoMoveSpeed;

            rb3d.AddForce(movementForce);
        }
        else if (Mathf.Abs(rb3d.velocity.z) >= maxAutoMoveSpeed)
        {
            speedLimiter.z = BaseVector.z;
            speedLimiter = speedLimiter * maxAutoMoveSpeed;

            speedLimiter.x = rb3d.velocity.x;

            rb3d.velocity = speedLimiter;
        }
        movementForce = new Vector3(0, 0, 0); 
        // {X} Movement Starts Here
        if (xMovement != 0)
        {
            if (Mathf.Abs(rb3d.velocity.x) < xMaxSpeed)
            {
                movementForce.x = BaseVector.x;
                movementForce = movementForce * xMovement * xSpeed;

                rb3d.AddForce(movementForce);
                currentXSpeed = rb3d.velocity.x;
            }
            else if (Mathf.Abs(rb3d.velocity.x) >= xMaxSpeed)
            {
                speedLimiter.x = BaseVector.x;
                speedLimiter.x = speedLimiter.x * xMaxSpeed * xMovement;

                speedLimiter.z = rb3d.velocity.z;

                rb3d.velocity = speedLimiter;

                currentXSpeed = rb3d.velocity.x;
            }
           
        }
        if (xMovement == 0)
        {
            speedDecrementer.x = BaseVector.x;
            speedDecrementer.x = currentXSpeed * speedMultiplierWhenNotMoving;

            speedDecrementer.z = rb3d.velocity.z;

            currentXSpeed = speedDecrementer.x;
            rb3d.velocity = speedDecrementer;

        }
    }
}
