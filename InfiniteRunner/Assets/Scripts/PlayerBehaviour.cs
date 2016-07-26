using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {


    public float maxSpeed = 5f;
    public float speed = 50;
    [Tooltip("this is applied when not moving above one, speed will increase")]
    public float speedMultiplierWhenNotMoving=0.9f;
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
        MovePlayer();
	}


    void MovePlayer()
    {
        xMovement = CrossPlatformInputManager.GetAxis("Horizontal");
          
        if (xMovement!=0)
        {
            if (Mathf.Abs( rb3d.velocity.x)<maxSpeed)
            {
                Vector3 movementForce = new Vector3(0,0,0);
                movementForce.x = movementForce.x + movementDirection.x;
                movementForce = movementForce * xMovement * speed;

                rb3d.AddForce(movementForce);
                currentXSpeed = rb3d.velocity.x;
            }
            else if (Mathf.Abs( rb3d.velocity.x)>=maxSpeed)
            {
                Vector3 speedLimiter = new Vector3();
                speedLimiter.x = movementDirection.x;
                speedLimiter.x = speedLimiter.x * maxSpeed * xMovement;

                rb3d.velocity = speedLimiter;
                currentXSpeed = rb3d.velocity.x;
            }
        }
        if (xMovement==0)
        {
            Vector3 speedDecrementer = new Vector3();
            speedDecrementer.x = movementDirection.x;
            speedDecrementer.x = currentXSpeed * speedMultiplierWhenNotMoving;
            currentXSpeed = speedDecrementer.x;
            rb3d.velocity = speedDecrementer;
            print(rb3d.velocity);
        }
  
    }
}
