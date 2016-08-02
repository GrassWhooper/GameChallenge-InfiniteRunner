using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovementBehaviour : MonoBehaviour {
    public static PlayerMovementBehaviour playerMovementBehaviour;

    Rigidbody rb3d;

    [Tooltip(" Force to be put into {X} Direction when moving ")]
    public float xSpeed = 30;

    [Tooltip("maximum speed on the {X} Direction")]
    public float xMaxSpeed = 5f;

    [Tooltip("Auto Move Force put into the {Z} Direction")]
    public float autoMoveSpeed = 10f;

    [Tooltip("Maximum Speed in the {Z} Direction")]
    public float maxAutoMoveSpeed = 20f;
    
    [Tooltip("this is applied when not moving above one, speed will increase")]
    public float speedMultiplierWhenNotMoving = 0.9f;

    [Tooltip("How Much To Multiply everything when accelerating ")]
    public float accelerationMultiplier = 2f;

    [Tooltip("How Much To Multiply everything when slowing down{Suggested Between 0.001 and 0.999} ")]
    public float slowDownMultiplier = 0.5f;


    public enum movementStatuses { NormalMovement, AcceleratedMovement, SlowedDownMovement, MidAir };
    public movementStatuses playerStatus;

    float xMovement, currentXSpeed, initalXSpeed, initalXMaxSpeed, initalAutoMovespeed, initalMaxAutoMoveSpeed;
    Vector3 BaseVector;

    void Awake()
    {
        playerMovementBehaviour = this;
    }

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
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void FixedUpdate()
    {
        PlayerMovement();

    }


    void SetMovementStatusValues()
    {
        if (playerStatus == movementStatuses.NormalMovement)
        {
            ReSetMovementStatusValues();
        }
        else if (playerStatus == movementStatuses.AcceleratedMovement)
        {
            xSpeed = accelerationMultiplier * initalXSpeed;
            xMaxSpeed = accelerationMultiplier * initalXMaxSpeed;
            autoMoveSpeed = accelerationMultiplier * initalAutoMovespeed;
            maxAutoMoveSpeed = accelerationMultiplier * initalMaxAutoMoveSpeed;
        }
        else if (playerStatus == movementStatuses.SlowedDownMovement)
        {
            xSpeed = initalXSpeed;
            xMaxSpeed = slowDownMultiplier * initalXMaxSpeed;
            autoMoveSpeed = slowDownMultiplier * initalAutoMovespeed;
            maxAutoMoveSpeed = slowDownMultiplier * initalMaxAutoMoveSpeed; //will snap to half
            //maxAutoMoveSpeed = initalMaxAutoMoveSpeed; //will snap back to initial automove Z direction speed.
        }

    }
    void ReSetMovementStatusValues()
    {
        xSpeed = initalXSpeed;
        xMaxSpeed = initalXMaxSpeed;
        autoMoveSpeed = initalAutoMovespeed;
        maxAutoMoveSpeed = initalMaxAutoMoveSpeed;
    }

    void PlayerMovement()
    {
        zPlayerMovement();
        xPlayerMovement();
        print(rb3d.velocity);
    }

    void zPlayerMovement()
    {
        xMovement = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector3 movementForce = new Vector3(0, 0, 0);
        Vector3 speedLimiter = new Vector3(0, 0, 0);

        SetMovementStatusValues();

        if (Mathf.Abs(rb3d.velocity.z) < (maxAutoMoveSpeed))
        {
            movementForce.z = BaseVector.z;
            movementForce = movementForce * autoMoveSpeed;
            if (PlayerBehaviour.playerBehaviour.hittingRoadBlock == true)
            {
                movementForce.z = movementForce.z * 0f;
                Vector3 altSpeed = rb3d.velocity;
                altSpeed.z = altSpeed.z * 0f;
                rb3d.velocity = altSpeed;
            }
            rb3d.AddForce(movementForce);
        }

        else if (Mathf.Abs(rb3d.velocity.z) >= maxAutoMoveSpeed)
        {
            speedLimiter.z = BaseVector.z;
            speedLimiter = speedLimiter * maxAutoMoveSpeed;

            speedLimiter.x = rb3d.velocity.x;

            speedLimiter.y = rb3d.velocity.y;
            if (PlayerBehaviour.playerBehaviour.hittingRoadBlock == true)
            {
                speedLimiter.z = speedLimiter.z * 0f;
            }
            rb3d.velocity = speedLimiter;
        }
        movementForce = new Vector3(0, 0, 0);
        // {X} Movement Starts Here
       
    }

    void xPlayerMovement()
    {
        Vector3 speedDecrementer = new Vector3();
        Vector3 movementForce = new Vector3(0, 0, 0);
        Vector3 speedLimiter = new Vector3(0, 0, 0);

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
                speedLimiter.y = rb3d.velocity.y;

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

            speedDecrementer.y = rb3d.velocity.y;

            rb3d.velocity = speedDecrementer;

        }


    }
}
