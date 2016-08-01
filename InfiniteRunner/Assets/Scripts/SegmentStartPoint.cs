using UnityEngine;
using System.Collections;

public class SegmentStartPoint : MonoBehaviour {

    Vector3 startPos;

    void OnTriggerExit(Collider collider)
    {

    }

    void OnEnable()
    {
        startPos = new Vector3();
        startPos = transform.position;
        SendMessageUpwards("RecieveStartingPos", startPos);
    }

	// Use this for initialization
	void Start ()
    {
        if (GetComponent<BoxCollider>().enabled)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
