using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour {

    Transform player;
    Vector3 offSet;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offSet = player.transform.position - transform.position;
        //print(offSet.z);
	}

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - offSet;
    }
}
