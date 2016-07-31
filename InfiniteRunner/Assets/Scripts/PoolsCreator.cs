using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolsCreator : MonoBehaviour {

    public List<GameObject> CreatPoolSpace(GameObject OfThisObj, int withThisAmount)
    {
        List<GameObject> poolToBeFilled = new List<GameObject>();
        for (int i = 0; i < withThisAmount; i++)
        {
            poolToBeFilled.Add(OfThisObj);
        }
        print("Created A Pool Filled With: " + OfThisObj.name +" with count of: "+ poolToBeFilled.Count);
        return poolToBeFilled;
    }

    public List<GameObject> CreateActualPool(List<GameObject> poolToCreate, Vector3 startingLocation, string Parent, bool WantRefrenceAfterBirth)
    {
        List<GameObject> BornObjectsFromPool = new List<GameObject>();

        foreach (GameObject item in poolToCreate)
        {
            GameObject clonedItem = (GameObject)Instantiate(item, startingLocation, Quaternion.identity);
            if (GameObject.Find(Parent) == false)
            {
                new GameObject(Parent);
            }
            GameObject selectParent = GameObject.Find(Parent);
            clonedItem.transform.parent = selectParent.transform;
            BornObjectsFromPool.Add(clonedItem);
        }

        return BornObjectsFromPool;
    }

    public void CreatePool(List<GameObject>poolToCreate , Vector3 startingLocation , string Parent)
    {
        foreach (GameObject item in poolToCreate)
        {
            GameObject clonedItem = (GameObject)Instantiate(item, startingLocation, Quaternion.identity);
            if (GameObject.Find(Parent) == false)
            {
                new GameObject(Parent);
            }
            GameObject selectParent = GameObject.Find(Parent);
            clonedItem.transform.parent = selectParent.transform;
        }
    }

    public void CreatePool(List<GameObject> poolToCreate)
    {
        foreach (GameObject item in poolToCreate)
        {
            Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

}
