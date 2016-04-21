using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Staying in Trigger");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exited");
        var aiScript = other.gameObject.GetComponent<AIMovement>();
        if (aiScript != null)
            aiScript.KnockOut();
    }
}
