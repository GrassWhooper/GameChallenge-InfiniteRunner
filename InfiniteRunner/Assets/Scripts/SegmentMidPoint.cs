using UnityEngine;
using System.Collections;

public class SegmentMidPoint : MonoBehaviour {
    float timeCounter=0f;

    Vector3 middleOfSegment = new Vector3();

    void OnTriggerEnter(Collider collider)
    {
        //print("Delta time Counter : " + timeCounter);
        if (collider.tag=="Player")
        {
            GroundsBuilder.groundBuilder.ActivateNextGround();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    
    void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = true;
        middleOfSegment = transform.position;
        SendMessageUpwards("RecieveMidPos", middleOfSegment);
    }

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
	    
	}
    void FixedUpdate()
    {
        timeCounter = timeCounter + Time.deltaTime;
    }
}
