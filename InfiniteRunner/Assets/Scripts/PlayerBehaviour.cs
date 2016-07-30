using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour {

    enum movementStatuses {NormalMovement , AcceleratedMovement, SlowedDownMovement, MidAir  };
    enum groundTouchingStatuses {TouchingGround , NotTouchingGround }

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

    [Tooltip("this is the force in which will be added when you use jump keys.")]
    public Vector3 jumpForce = new Vector3(0,450f,0);


    [Tooltip("DistanceCheck Between Player And Ground. Suggested {Half the heigth of the capsule collider + something}")]
    public float raylength = 0;

    [Tooltip("how many times the player can jump mid air")]
    public float midAirJumpNumber = 2f;

    [Tooltip("the number of which the {jumpForce} is multiplied by in each jump in mid air")]
    public float midAirJumpMultiplier=0.5f;

    [Tooltip("the minimum jumpingForce, that can be reached with mid air jumps.")]
    public Vector3 minJumpingForce=new Vector3(0,200,0);

    bool hittingRoadBlock = true;
    bool midAirCalculations = false;
    Vector3 initalJumpForce;
    float jumpsCounter=0;
    bool playerTouchingGround;
    float resetRunningTimer,resetRunningCoolDown;
    CapsuleCollider myCapsule;
    float xMovement;
    Rigidbody rb3d;
    Vector3 BaseVector;
    float currentXSpeed;
    float initalXSpeed, initalXMaxSpeed, initalAutoMovespeed, initalMaxAutoMoveSpeed;
    groundTouchingStatuses groundTouchStatus;
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

        initalJumpForce = jumpForce;
        resetRunningTimer = runningTimer;
        jumpsCounter = 0;
        myCapsule = GetComponent<CapsuleCollider>();


        groundTouchStatus = new groundTouchingStatuses();
        groundTouchStatus = groundTouchingStatuses.TouchingGround;
    }

    void Update()
    {
        CheckForMovementStatus();
        if (playerTouchingGround == true)
        {
            JumpUp();
        }

        if (groundTouchStatus == groundTouchingStatuses.NotTouchingGround)
        {
            DoMidAirJumps();
        }
    }
    
    // Update is called once per frame
    void FixedUpdate ()
    {
        TouchingGroundChecker();
        HittingRoadBlocks();
        PlayerMovement();
        print(rb3d.velocity);
    }

    void HittingRoadBlocks()
    {
        Ray roadBlocksRay = new Ray(transform.position,Vector3.forward);
        RaycastHit whatWasHit = new RaycastHit();
        float maxDistance = 0.0550f;

        Physics.SphereCast(roadBlocksRay, myCapsule.radius + 0.5f, out whatWasHit, maxDistance);

        Vector3 aboveHead = transform.position;
        aboveHead.y = aboveHead.y + ((myCapsule.height / 2) - 0.1f);
        Vector3 underHead = transform.position;
        underHead.y = underHead.y - ((myCapsule.height / 2) - 0.1f);

        Debug.DrawRay(new Vector3(aboveHead.x, aboveHead.y, aboveHead.z + maxDistance), Vector3.forward * myCapsule.radius);
        Debug.DrawRay(new Vector3(underHead.x, underHead.y, underHead.z + maxDistance), Vector3.forward * myCapsule.radius);
        Vector3 v = transform.position;
        v.z = v.z + maxDistance;
        Debug.DrawRay(v, Vector3.forward * myCapsule.radius);

        Physics.CapsuleCast(aboveHead, underHead, myCapsule.radius + 0.5f, transform.forward, out whatWasHit, maxDistance);

        //rb3d.SweepTest(rb3d.velocity.normalized, out whatWasHit, maxDistance);

        if (whatWasHit.collider != null)
        {
            if (whatWasHit.collider.gameObject.tag== "RoadBlocks")
            {
                hittingRoadBlock = true;
            }
        }
        else
        {
            hittingRoadBlock = false;
        }


    }
    
    void DoMidAirJumps()
    {
        if (jumpsCounter> midAirJumpNumber)
        {

        }
        else
        {
            if (midAirCalculations == true)
            {
                midAirCalculations = false;
                jumpForce = jumpForce * midAirJumpMultiplier;
                if (jumpForce.y <= minJumpingForce.y)
                {
                    jumpForce = minJumpingForce;
                }
            }
            JumpUp();
        }
    }

    void TouchingGroundChecker()
    {
        raylength = myCapsule.height / 2 + 0.450f;
        Ray groundCheckingRay = new Ray(transform.position, Vector3.down);
        RaycastHit whatWasHit = new RaycastHit();
        Physics.Raycast(groundCheckingRay, out whatWasHit, raylength);

        //Debug.DrawRay(transform.position, Vector3.down * (raylength));
    
        if (whatWasHit.collider != null)
        {
            if (whatWasHit.collider.tag == "Ground")
            {
                playerTouchingGround = true;
                jumpForce = initalJumpForce;
                jumpsCounter = 0;
                groundTouchStatus = groundTouchingStatuses.TouchingGround;
            }
        }
        else if (whatWasHit.collider == null)
        {
            //Debug.LogWarning("Not Touching Ground");
            playerTouchingGround = false;
            groundTouchStatus = groundTouchingStatuses.NotTouchingGround;
        }

    }
    void JumpUp()
    {
        foreach (KeyCode item in JumpKeys)
        {
            if (Input.GetKeyDown(item))
            {
                rb3d.AddForce(jumpForce);
                groundTouchStatus = groundTouchingStatuses.NotTouchingGround;
                midAirCalculations = true;
                jumpsCounter += 1;
                break;
            }
        }
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
            maxAutoMoveSpeed = slowDownMultiplier * initalMaxAutoMoveSpeed; //will snap to half
            //maxAutoMoveSpeed = initalMaxAutoMoveSpeed; //will snap back to initial automove Z direction speed.
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
            if (hittingRoadBlock==true)
            {
                movementForce.z = movementForce.z *0f;
                Vector3 altSpeed = rb3d.velocity;
                altSpeed.z = altSpeed.z* 0f;
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
            if (hittingRoadBlock == true)
            {
                speedLimiter.z = speedLimiter.z* 0f;
            }
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
