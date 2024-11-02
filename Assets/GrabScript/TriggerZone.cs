using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Grabbable grabbable = collider.gameObject.GetComponent<Grabbable>();
        if (grabbable != null && grabbable.isGrabbed)
        {
            CameraGrab.s.transitionCam();
        }
    }

    
    void OnTriggerExit(Collider other)
    {
        Grabbable grabbable = other.gameObject.GetComponent<Grabbable>();
        if (grabbable != null && grabbable.isGrabbed)
        {
            CameraGrab.s.transitionCam();
        }
    }
    
}
