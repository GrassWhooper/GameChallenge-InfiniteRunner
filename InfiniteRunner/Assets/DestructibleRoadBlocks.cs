using UnityEngine;
using System.Collections;

public class DestructibleRoadBlocks : MonoBehaviour {

    public float health=3;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if ( health<=0)
        {
            gameObject.SetActive(false);
        }
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
