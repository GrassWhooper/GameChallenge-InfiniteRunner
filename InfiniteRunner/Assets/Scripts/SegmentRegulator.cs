using UnityEngine;
using System.Collections;

public class SegmentRegulator : MonoBehaviour {
    Vector3 startPos,midPos,restultant;

    void GoToSleep(float after)
    {
        StartCoroutine(GetToSleep(after));
    }

    void RecieveStartingPos(Vector3 startPos)
    {
        this.startPos = startPos;
    }

    void RecieveMidPos(Vector3 midPos)
    {
        this.midPos = midPos;
    }

    // Use this for initialization
    void OnEnable ()
    {
        StartCoroutine(GetResultant(startPos, midPos));
	}

    IEnumerator GetToSleep(float after)
    {
        yield return new WaitForSeconds(after);
        gameObject.SetActive(false);
    }

    IEnumerator GetResultant(Vector3 startPos, Vector3 midPos)
    {
        yield return new WaitForSeconds(1);
        if (midPos != startPos)
        {
            restultant = midPos - startPos;
            GroundsBuilder.groundBuilder.GetDistForMidSegment(restultant);
        }
        else
        {
            Debug.LogWarning("SomethingFishy Happened");
        }
        
    }
}
