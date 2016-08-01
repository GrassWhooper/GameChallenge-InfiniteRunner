using UnityEngine;
using System.Collections;

public class SegmentEndPoint : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag=="Player")
        {
            //print("im gay");
            SendMessageUpwards("GoToSleep", 4f);
            GetComponent<BoxCollider>().enabled = false;
            //print("sent message upwards to sleep");
        }
    }

    void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = true;

    }
	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
