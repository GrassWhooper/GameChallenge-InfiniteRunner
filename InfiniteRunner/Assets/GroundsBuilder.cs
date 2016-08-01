using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundsBuilder : MonoBehaviour {

    public GameObject landA;
    public int pooledAmount;
    public float groundYDifferenceFromZero = 1.5f;

    PoolsCreator poolCreator;
    List<GameObject> SegmentA;
    GameObject player;

    float secs=10f;

    void Awake()
    {
        poolCreator = GetComponent<PoolsCreator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start ()
    {
        SegmentA = new List<GameObject>();
        SegmentA = poolCreator.CreatPoolSpace(landA, pooledAmount);

        SegmentA = poolCreator.CreateActualPool(SegmentA, Vector3.zero,"All Lands",true);
        poolCreator.DeActivatePool(SegmentA);

        PlaceInitialGround();
        //StartCoroutine(PlaceGroundA());
    }

 	// Update is called once per frame
	void Update ()
    {

    }

    void PlaceInitialGround()
    {
        Vector3 InitalPlayerPos = player.transform.position;
        int IndexOfFirstLand = 0;
        InitalPlayerPos.y = InitalPlayerPos.y-InitalPlayerPos.y - groundYDifferenceFromZero;
        for (int i = 0; i < SegmentA.Count;)
        {
            IndexOfFirstLand = i;
            SegmentA[i].transform.position = InitalPlayerPos;
            SegmentA[i].SetActive(true);
            break;
        }
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
