using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundsBuilder : MonoBehaviour {

    public GameObject landA;
    public int pooledAmount;


    PoolsCreator poolCreator;
    List<GameObject> SegmentA;

    float secs=10f;

    void Awake()
    {
        poolCreator = GetComponent<PoolsCreator>();
    }

    // Use this for initialization
    void Start ()
    {
        SegmentA = new List<GameObject>();
        SegmentA = poolCreator.CreatPoolSpace(landA, pooledAmount);

        SegmentA = poolCreator.CreateActualPool(SegmentA, Vector3.zero,"All Lands",true);
        //StartCoroutine(PlaceGroundA());
    }

 	// Update is called once per frame
	void Update ()
    {

    }

    IEnumerator PlaceGroundA()
    {
        {
            yield return new WaitForSeconds(secs);
            print("im doing something");
            foreach (GameObject item in SegmentA)
            {
                item.SetActive(false);
            }
        }
    }
}
