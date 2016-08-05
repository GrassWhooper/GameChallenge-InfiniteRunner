using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundsBuilder : MonoBehaviour {
    public static GroundsBuilder groundBuilder;

    [Tooltip("Place All segments you want to be made in this list")]
    public List<GameObject> allSegments;

    public int pooledAmount;
    public float groundYDifferenceFromZero = 1.5f;
    [Tooltip("which land/Segment do you want to be the inital one")]
    public int firstPieceIndex;
    [Tooltip("Do you want the following segments to be randomized?")]
    public bool RandomizedSegments=true;

    List<List<GameObject>> MasterPool;

    GameObject player;
    Vector3 nextGroundPos;
    float zDistStartToMid = 0f;






    
    void Awake()
    {
        groundBuilder = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start ()
    {

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
        foreach (GameObject item in allSegments)
        {
            List<GameObject> segmentPoolSpace = new List<GameObject>();
            segmentPoolSpace=PoolsCreator.poolCreator.CreatPoolSpace(item, pooledAmount);
            segmentPoolSpace= PoolsCreator.poolCreator.CreateActualPool(segmentPoolSpace, Vector3.zero, "All Lands");
            PoolsCreator.poolCreator.FillMasterPool(MasterPool, segmentPoolSpace);
            PoolsCreator.poolCreator.DeActivatePool(segmentPoolSpace);
        }
    }

    void PlaceInitialGround()
    {
        List<GameObject> chosenPool = new List<GameObject>();
        int index = 0;
        if (firstPieceIndex == 0)
        {
            index = Random.Range(0, MasterPool.Count);
        }
        else if(index>0 && index<=MasterPool.Count)
        {
            index = firstPieceIndex;
        }
        else
        {
            Debug.LogWarning("Index Of Piece Is Out Of Range so piece is set to the first one");
            index = 0;
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
