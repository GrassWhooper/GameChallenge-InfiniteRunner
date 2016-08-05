using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeapon : MonoBehaviour {

    Vector3 mousePos, myPos, VgunRot;
    Camera cam;
    public float maxDistanceOfRay = 50f;

    // Use this for initialization
    void Start ()
    {
        VgunRot = transform.rotation.eulerAngles;
        myPos = transform.position;
        cam = FindObjectOfType<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GrabHitTarget();
    }

    GameObject GrabHitTarget()
    {
        RaycastHit whatWasHit;
        Ray ray = new Ray(transform.position, transform.forward);
        GameObject hitObject;
        Debug.DrawRay(transform.position, transform.forward * maxDistanceOfRay);
        if (Input.GetButtonDown("Fire1"))
        {
            Physics.Raycast(ray, out whatWasHit, maxDistanceOfRay);
            if (whatWasHit.collider)
            {
                hitObject = whatWasHit.collider.gameObject;
                print("i have hit: " + hitObject.name);
            }
        }
        return null;
    }
}
