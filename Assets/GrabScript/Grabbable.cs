using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    const float MAX_RELEASE_VEL = 1f;

    public bool isGrabbed = false;
    // Value when it is at rest on the conveyor belt. Prevents it from being pushed through
    // NO LONGER IN USE
    /*
    public float startY;
    public float maxY;
    */

    // Clamp mouse positions to keep grabbed item from clipping through conveyor belt or camera in box view
    const float MOUSE_Y_MIN = 500f;
    const float MOUSE_Y_MAX = 725f;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(Input.mousePosition);

        if (isGrabbed)
        {
            // Do we drop?
            if (!Input.GetMouseButton(0))
            {
                isGrabbed = false;
                CameraGrab.isGrabbing = false;
                rb.useGravity = true;
                rb.isKinematic = false;
                // Keeps you from accidentally flinging things
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, MAX_RELEASE_VEL);
            }
            else
            {
                if (CameraGrab.mode == CameraGrab.CameraMode.Conveyor)
                {
                    Vector3 mousePos = Input.mousePosition;

                    // Make sure it does not clip through 
                    mousePos.y = Mathf.Clamp(mousePos.y, MOUSE_Y_MIN, MOUSE_Y_MAX);

                    // Send out ray from camera to mouse
                    Ray r = Camera.main.ScreenPointToRay(mousePos);

                    // Find where that ray intersects with the z=0 plane
                    Vector3 rPos = r.GetPoint(-r.origin.z / r.direction.z);

                    // That's where we want our object to be

                    // No longer using an individual min/max y system:
                    
                    /*
                    rPos.y = Mathf.Clamp(rPos.y, startY, maxY);
                    */
                    rb.MovePosition(rPos);
                }

                if (CameraGrab.mode == CameraGrab.CameraMode.Box)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.transform.position.y - transform.position.y;

                    Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);

                    // Just make extra sure the y position doesn't move
                    newPos.y = transform.position.y;


                    // TODO: Transparent image

                    /*
                    Color c = gameObject.GetComponent<Renderer>().material.color;
                    c.a = 0.5f;
                    //gameObject.GetComponent<Renderer>().material.SetColor(, c);
                    gameObject.GetComponent<Renderer>().material.ToFadeMode();

                    */


                    rb.MovePosition(newPos);
                }

                if (CameraGrab.mode == CameraGrab.CameraMode.TransitionConveyorToBox)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.transform.position.y - transform.position.y;

                    Vector3 newPos = CameraGrab.s.boxCam.ScreenToWorldPoint(mousePos);

                    // Just make extra sure the y position doesn't move
                    newPos.y = transform.position.y;


                    // TODO: Transparent image
                    Vector3 flatZ = transform.position;
                    flatZ.z = 0;

                    rb.MovePosition(Vector3.Lerp(flatZ, newPos, CameraGrab.s.transitionProgress));
                }
                
            }
        }
    }

    public void Grab()
    {
        isGrabbed = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        Debug.Log("got grabbed");
    }
}
