using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public bool isGrabbed = false;
    // Start is called before the first frame update
    void Start()
    {
              
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            isGrabbed=true;
        }

        if(isGrabbed)
        {
            Vector3 mousePos = Input.mousePosition;

            // Send out ray from camera to mouse
            Ray r = Camera.main.ScreenPointToRay(mousePos);
            // Find where that ray intersects with the z=0 plane
            Vector3 rPos = r.GetPoint(-r.origin.z / r.direction.z);
            // That's where we want our object to be
            transform.position = rPos;
        }
    }
}
