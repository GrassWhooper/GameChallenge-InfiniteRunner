using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour {
    public static PlayerBehaviour playerBehaviour;
    public enum groundTouchingStatuses {TouchingGround , NotTouchingGround }
    public enum HittingRoadBlock {HittingBlock , NotHittingBlock }
    [Tooltip("how long the player can keep accelerating his speed")]
    public float runningTimer = 4f;
    
    [Tooltip("DistanceCheck Between Player And Ground. Suggested {Half the heigth of the capsule collider + something}")]
    public float raylength = 0;


    Vector3 initalJumpForce;

    float resetRunningTimer,resetRunningCoolDown;
    CapsuleCollider myCapsule;
    [Tooltip("Select Keys For Each Action")]
    public List<KeyCode> JumpKeys , AccelerateKeys, SlowDownKeys;

    [Header("Do Not Touch these")]
    public groundTouchingStatuses groundTouchStatus;
    public HittingRoadBlock hittingRoadBlock;


    void Awake()
    {
        playerBehaviour = this;
    }

    // Use this for initialization
    void Start ()
    {

        initalJumpForce = PlayerJumpBehaviour.playerJumpBehaviour.jumpForce;
        resetRunningTimer = runningTimer;
        myCapsule = GetComponent<CapsuleCollider>();


        groundTouchStatus = new groundTouchingStatuses();
        groundTouchStatus = groundTouchingStatuses.TouchingGround;
        hittingRoadBlock = new HittingRoadBlock();
        hittingRoadBlock = HittingRoadBlock.NotHittingBlock;
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
            PlayerJumpBehaviour.playerJumpBehaviour.DoMidAirJumps();
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
        switch (groundTouchStatus)
        {
            case groundTouchingStatuses.TouchingGround:
                break;
            case groundTouchingStatuses.NotTouchingGround:
                maxDistance = maxDistance * 2;
                break;
            default:
                break;
        }

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
            if (whatWasHit.collider.gameObject.tag == "RoadBlocks")
            {
                hittingRoadBlock = HittingRoadBlock.HittingBlock;
            }
        }
        else
        {
            hittingRoadBlock = HittingRoadBlock.NotHittingBlock;
            
        }

    }
    void TouchingGroundChecker()
    {
        //raylength = myCapsule.height / 2 + 0.450f; //commented out because well, in case we want to change that number. in the inespector
        Ray groundCheckingRay = new Ray(transform.position, Vector3.down);
        RaycastHit whatWasHit = new RaycastHit();
        Physics.Raycast(groundCheckingRay, out whatWasHit, raylength);

        Debug.DrawRay(transform.position, Vector3.down * (raylength));
    
        if (whatWasHit.collider != null)
        {
            if (whatWasHit.collider.tag == "Ground")
            {
                PlayerJumpBehaviour.playerJumpBehaviour.jumpForce = initalJumpForce;
                PlayerJumpBehaviour.playerJumpBehaviour.jumpsCounter = 0;
                groundTouchStatus = groundTouchingStatuses.TouchingGround;
            }
        }
        else if (whatWasHit.collider == null)
        {
            //Debug.LogWarning("Not Touching Ground");
            groundTouchStatus = groundTouchingStatuses.NotTouchingGround;
            
        }
    }
    public void JumpUp()
    {
        foreach (KeyCode item in JumpKeys)
        {
            if (Input.GetKeyDown(item) || CrossPlatformInputManager.GetButtonDown("myJump"))
            {
                PlayerJumpBehaviour.playerJumpBehaviour.JumpCalculations();
                groundTouchStatus = groundTouchingStatuses.NotTouchingGround;
                
                break;
            }
        }
    }
    void CheckForMovementStatus()
    {

        bool Accelerating = false;
        bool SlowingDown = false;
        if (groundTouchStatus == groundTouchingStatuses.TouchingGround)
        {
            foreach (KeyCode item in AccelerateKeys)
            {
                if (Input.GetKey(item) || CrossPlatformInputManager.GetAxis("Vertical") > 0)
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
                        runningTimer += 2 * Time.deltaTime;
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
                if (runningTimer < resetRunningTimer)
                {
                    runningTimer += Time.deltaTime;
                }
            }

        }
        else
        {
            PlayerMovementBehaviour.playerMovementBehaviour.playerStatus = PlayerMovementBehaviour.movementStatuses.MidAir;
        }
    }
    
}
