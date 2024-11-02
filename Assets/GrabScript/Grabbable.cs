using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    const float MAX_RELEASE_VEL = 1f;

    public bool isGrabbed = false;
    // Value when it is at rest on the conveyor belt. Prevents it from being pushed through
    public float startY;
    public float maxY;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isGrabbed)
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

                    // Send out ray from camera to mouse
                    Ray r = Camera.main.ScreenPointToRay(mousePos);

                    // Find where that ray intersects with the z=0 plane
                    Vector3 rPos = r.GetPoint(-r.origin.z / r.direction.z);

                    // That's where we want our object to be
                    rPos.y = Mathf.Clamp(rPos.y, startY, maxY);
                    rb.MovePosition(rPos);
                }

                if (CameraGrab.mode == CameraGrab.CameraMode.Box)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.transform.position.y - transform.position.y;

                    Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);

                    Color c = gameObject.GetComponent<Renderer>().material.color;
                    c.a = 0.5f;
                    //gameObject.GetComponent<Renderer>().material.SetColor(, c);
                    gameObject.GetComponent<Renderer>().material.ToFadeMode();




                    rb.MovePosition(newPos);
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
