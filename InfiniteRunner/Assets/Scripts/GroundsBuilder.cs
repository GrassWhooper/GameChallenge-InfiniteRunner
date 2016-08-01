using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundsBuilder : MonoBehaviour {
    public static GroundsBuilder groundBuilder;
    public GameObject landA;
    public int pooledAmount;
    public float groundYDifferenceFromZero = 1.5f;

    List<GameObject> SegmentA;
    GameObject player;
    Vector3 nextGroundPos;
    float zDistStartToMid = 0f;

    float secs=10f;




    
    void Awake()
    {
        groundBuilder = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start ()
    {
        SegmentA = new List<GameObject>();
        SegmentA = PoolsCreator.poolCreator.CreatPoolSpace(landA, pooledAmount);

        SegmentA = PoolsCreator.poolCreator.CreateActualPool(SegmentA, Vector3.zero,"All Lands",true);
        PoolsCreator.poolCreator.DeActivatePool(SegmentA);

        nextGroundPos = new Vector3();

        PlaceInitialGround();
        //StartCoroutine(PlaceGroundA());
    }

 	// Update is called once per frame
	void Update ()
    {

    }


    public void GetDistForMidSegment(Vector3 dist)
    {
        zDistStartToMid = dist.z;
        
    }


    public void ActivateNextGround()
    {
        GameObject activateThis =PoolsCreator.poolCreator.GetInActiveObjectInPool(SegmentA, false, null);

        nextGroundPos.z = nextGroundPos.z + zDistStartToMid * 2;

        activateThis.transform.position = nextGroundPos;
        activateThis.SetActive(true);
    }

    void PlaceInitialGround()
    {
        GameObject ActivateThis = PoolsCreator.poolCreator.GetInActiveObjectInPool(SegmentA, false, null);

        Vector3 InitalGroundPos = player.transform.position;
        InitalGroundPos.y = InitalGroundPos.y-InitalGroundPos.y - groundYDifferenceFromZero;
        InitalGroundPos.z = 0;


        nextGroundPos = InitalGroundPos;


        ActivateThis.transform.position = InitalGroundPos;
        ActivateThis.SetActive(true);

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
