using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeapon : MonoBehaviour {

    Camera cam;
    public float maxDistanceOfRay = 50f,FireRate =0.250f ,shotsDisappearAfter = 0.100f;
    LineRenderer lineRenderer;
    float tillNextShot = 0.500f;
    Transform shootingSpot;
    WaitForSeconds shotsDisappear;

    // Use this for initialization
    void Start ()
    {
        cam = FindObjectOfType<Camera>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        shootingSpot = lineRenderer.gameObject.transform;
        shotsDisappear = new WaitForSeconds(shotsDisappearAfter);
    }
	
	// Update is called once per frame
	void Update ()
    {
        GrabHitTarget();
        RotationOfGun();
    }

    void RotationOfGun()
    {
        Plane playerSecondPlane = new Plane(new Vector3(1, 1, 1), transform.position);
        Ray myRay = cam.ScreenPointToRay(Input.mousePosition);
        float secHit = 0;
        if (playerSecondPlane.Raycast(myRay, out secHit))
        {
            Vector3 targetHit = myRay.GetPoint(secHit);
            Quaternion targetRot = Quaternion.LookRotation(targetHit - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }

    GameObject GrabHitTarget()
    {
        RaycastHit whatWasHit;
        Ray ray = new Ray(transform.position, transform.forward);
        GameObject hitObject;
        Debug.DrawRay(transform.position, transform.forward * maxDistanceOfRay);
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && Time.time>tillNextShot)
        {
            tillNextShot = Time.time + FireRate;
            Physics.Raycast(ray, out whatWasHit, maxDistanceOfRay);
            if (whatWasHit.collider)
            {
                hitObject = whatWasHit.collider.gameObject;
                print("i have hit: " + hitObject.name);
            }
            if (whatWasHit.point != new Vector3(0,0,0))
            {
                StartCoroutine(ShootingEffects(whatWasHit.point));
            }
            else
            {
                StartCoroutine(ShootingEffects(shootingSpot.rotation*
                    new Vector3(shootingSpot.position.x,shootingSpot.position.y,shootingSpot.position.z+50f)));
            }
        }
        return null;
    }
    IEnumerator ShootingEffects(Vector3 whereLaserEnds)
    {
        Vector3[] positionsOfLaser = new Vector3[2];
        positionsOfLaser[0] = (shootingSpot.position);
        positionsOfLaser[1] = (whereLaserEnds);
        lineRenderer.SetPositions(positionsOfLaser);
        yield return shotsDisappear;
        positionsOfLaser[0] = new Vector3(-1 ,- 1, -1);
        positionsOfLaser[1] = new Vector3(-1 ,- 1, -1);
        lineRenderer.SetPositions(positionsOfLaser);
    }
}
