using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour {
    public static PlayerBehaviour playerBehaviour;
    enum groundTouchingStatuses {TouchingGround , NotTouchingGround }

    public Rigidbody rb3d;
    
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

    public bool hittingRoadBlock = true;

    bool midAirCalculations = false;
    Vector3 initalJumpForce;
    float jumpsCounter=0;

    float resetRunningTimer,resetRunningCoolDown;
    CapsuleCollider myCapsule;
    groundTouchingStatuses groundTouchStatus;
    [Tooltip("Select Keys For Each Action")]
    public List<KeyCode> JumpKeys , AccelerateKeys, SlowDownKeys;

    void Awake()
    {
        playerBehaviour = this;
    }

    // Use this for initialization
    void Start ()
    {
        rb3d = GetComponent<Rigidbody>();

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
        if (groundTouchStatus == groundTouchingStatuses.TouchingGround)
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

        Debug.DrawRay(transform.position, Vector3.down * (raylength));
    
        if (whatWasHit.collider != null)
        {
            if (whatWasHit.collider.tag == "Ground")
            {
                jumpForce = initalJumpForce;
                jumpsCounter = 0;
                groundTouchStatus = groundTouchingStatuses.TouchingGround;
            }
        }
        else if (whatWasHit.collider == null)
        {
            //Debug.LogWarning("Not Touching Ground");
            groundTouchStatus = groundTouchingStatuses.NotTouchingGround;
        }

    }

    void JumpUp()
    {
        foreach (KeyCode item in JumpKeys)
        {
            if (Input.GetKeyDown(item) || CrossPlatformInputManager.GetButtonDown("myJump"))
            {
                rb3d.AddForce(jumpForce);
                groundTouchStatus = groundTouchingStatuses.NotTouchingGround;
                midAirCalculations = true;
                jumpsCounter += 1;
                break;
            }
        }
    }
    void CheckForMovementStatus()
    {
        bool Accelerating= false;
        bool SlowingDown = false;

        foreach (KeyCode item in AccelerateKeys)
        {
            if (Input.GetKey(item) || CrossPlatformInputManager.GetAxis("Vertical")>0)
            {
                PlayerMovementBehaviour.playerMovementBehaviour.playerStatus = PlayerMovementBehaviour.movementStatuses.AcceleratedMovement;
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
            if (Input.GetKey(item) || CrossPlatformInputManager.GetAxis("Vertical") < 0)
            {
                PlayerMovementBehaviour.playerMovementBehaviour.playerStatus 
                    = PlayerMovementBehaviour.movementStatuses.SlowedDownMovement;
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
            PlayerMovementBehaviour.playerMovementBehaviour.playerStatus = PlayerMovementBehaviour.movementStatuses.NormalMovement;
            if (runningTimer<resetRunningTimer)
            {
                runningTimer += Time.deltaTime;
            }
        }
    }
}
