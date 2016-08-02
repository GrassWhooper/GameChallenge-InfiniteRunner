using UnityEngine;
using System.Collections;

public class PlayerJumpBehaviour : MonoBehaviour {
    public static PlayerJumpBehaviour playerJumpBehaviour;

    [Tooltip("this is the force in which will be added when you use jump keys.")]
    public Vector3 jumpForce = new Vector3(0, 450f, 0);

    [Tooltip("how many times the player can jump mid air")]
    public float midAirJumpNumber = 2f;

    [Tooltip("the number of which the {jumpForce} is multiplied by in each jump in mid air")]
    public float midAirJumpMultiplier = 0.5f;

    [Tooltip("the minimum jumpingForce, that can be reached with mid air jumps.")]
    public Vector3 minJumpingForce = new Vector3(0, 200, 0);

    Rigidbody rb3d;

    

    [Header("Do Not Touch These Unless you return them to default when done.")]
    public float jumpsCounter = 0;
    public bool midAirCalculations = false;

    public void JumpCalculations()
    {
        rb3d.AddForce(jumpForce);
        jumpsCounter += 1;
        midAirCalculations = true;
    }

    public void DoMidAirJumps()
    {
        if (jumpsCounter > midAirJumpNumber)
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
            PlayerBehaviour.playerBehaviour.JumpUp();
        }
    }

    void Awake()
    {
        playerJumpBehaviour = this;
    }

	// Use this for initialization
	void Start ()
    {
        rb3d = GetComponent<Rigidbody>();
        jumpsCounter = 0;
        midAirCalculations = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
