using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundsBuilder : MonoBehaviour {
    public static GroundsBuilder groundBuilder;
    public GameObject landA;
    public GameObject landB;
    public int pooledAmount;
    public float groundYDifferenceFromZero = 1.5f;
    [Tooltip("which land/Segment do you want to be the inital one")]
    public int firstPieceIndex;
    [Tooltip("Do you want the following segments to be randomized?")]
    public bool RandomizedSegments=true;

    List<List<GameObject>> MasterPool;
    List<GameObject> SegmentA, SegmentB;

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
        SegmentB = new List<GameObject>();
        MasterPool = new List<List<GameObject>>();

        CreateThePools();


        nextGroundPos = new Vector3();

        PlaceInitialGround();
        //StartCoroutine(PlaceGroundA());
    }
    void Update()
    {
        
    }
    void CreateThePools()
    {
        SegmentA = PoolsCreator.poolCreator.CreatPoolSpace(landA, pooledAmount);
        SegmentB = PoolsCreator.poolCreator.CreatPoolSpace(landB, pooledAmount);

        SegmentA = PoolsCreator.poolCreator.CreateActualPool(SegmentA, Vector3.zero, "All Lands");
        SegmentB = PoolsCreator.poolCreator.CreateActualPool(SegmentB, Vector3.zero, "All Lands");

        PoolsCreator.poolCreator.FillMasterPool(MasterPool, SegmentA);
        PoolsCreator.poolCreator.FillMasterPool(MasterPool, SegmentB);

        PoolsCreator.poolCreator.DeActivatePool(SegmentA);
        PoolsCreator.poolCreator.DeActivatePool(SegmentB);

    }

    void PlaceInitialGround()
    {
        List<GameObject> chosenPool = new List<GameObject>();
        int index = 0;
        if (firstPieceIndex == 0)
        {
            index = Random.Range(0, MasterPool.Count);
        }
        else
        {
            index = firstPieceIndex;
        }

        chosenPool = PoolsCreator.poolCreator.GrabChosenPoolFrom(MasterPool, index);

        GameObject ActivateThis = PoolsCreator.poolCreator.GetInActiveObjectInPool(chosenPool, false, null);

        Vector3 InitalGroundPos = player.transform.position;
        InitalGroundPos.y = InitalGroundPos.y - InitalGroundPos.y - groundYDifferenceFromZero;
        InitalGroundPos.z = 0;

        ActivateThis.transform.position = InitalGroundPos;
        ActivateThis.SetActive(true);

        nextGroundPos = InitalGroundPos;
    }

    public void ActivateNextGround()
    {
        List<GameObject> chosenPool = new List<GameObject>();
        int index = 0;

        if (RandomizedSegments == true)
        {
            index = Random.Range(0,MasterPool.Count);
        }
        else
        {
            Debug.LogWarning("HasBeen Set To Default Which is 0 (First Ground)");
        }



        chosenPool = PoolsCreator.poolCreator.GrabChosenPoolFrom(MasterPool, index);
        GameObject activateThis = PoolsCreator.poolCreator.GetInActiveObjectInPool(chosenPool, false, null);

        nextGroundPos.z = nextGroundPos.z + zDistStartToMid * 2;

        activateThis.transform.position = nextGroundPos;
        activateThis.SetActive(true);
        nextGroundPos = activateThis.transform.position;
        print(nextGroundPos.z);
    }



    public void GetDistForMidSegment(Vector3 dist)
    {
        zDistStartToMid = dist.z;
    }


}
