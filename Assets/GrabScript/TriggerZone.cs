using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("");
        if(collider.gameObject.GetComponent<Grabbable>() != null)
        {
            CameraGrab.s.transitionCam();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Grabbable>() != null)
        {
            CameraGrab.s.transitionCam();
        }
    }
}
