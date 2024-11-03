using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static CameraGrab;

public class CameraGrab : MonoBehaviour
{
    public enum CameraMode
    {
        Conveyor,
        TransitionConveyorToBox,
        TransitionBoxToConveyor,
        Box
    }

    public static CameraMode mode = CameraMode.Conveyor;
    public static bool isGrabbing = false;
    public static CameraGrab s;

    public Vector3 conveyorPos;
    public Vector3 conveyorRot;
    public Vector3 boxPos;
    public Vector3 boxRot;


    public AnimationCurve transitionEase;
    public float transitionDuration = 1f;

    private float transitionStartTime;
    public Camera boxCam;

    public float transitionProgress
    {
        get
        {
            return transitionEase.Evaluate((Time.time - transitionStartTime) / transitionDuration);
        }
    }

    void Start()
    {
        s = this;
        boxCam.transform.position = boxPos;
        boxCam.transform.rotation = Quaternion.Euler(boxRot);
    }

    // Update is called once per frame
    void Update()
    {
        // Pick up items
        if (mode == CameraMode.Conveyor && !isGrabbing && Input.GetMouseButton(0))
        {
            // Raycast toward mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hit = Physics.RaycastAll(ray);

            foreach (var h in hit.Where(h => h.rigidbody != null))
            {
                Debug.Log(h.collider.gameObject.name);
            }

            Debug.Log(hit.First<RaycastHit>().collider.gameObject);

            //Debug.Log(hit.Where<RaycastHit>(h => h.rigidbody.gameObject.GetComponent<Grabbable>() != null).First().rigidbody.gameObject);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            // If first thing hit is grabbable
            Grabbable grabbable = hit.Where(h => h.rigidbody != null && h.rigidbody.gameObject.GetComponent<Grabbable>() != null).First().rigidbody.gameObject.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                // Grab it
                grabbable.Grab();
                isGrabbing = true;
            }
        }

        if (mode == CameraMode.Box && !isGrabbing)
        {
            transitionCam();
        }

        // Just for testing, toggle views

        

        // Transitions

        if (mode == CameraMode.TransitionConveyorToBox)
        {
            transform.position = Vector3.Lerp(conveyorPos, boxPos, transitionProgress);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(conveyorRot, boxRot, transitionProgress));
            if (Time.time - transitionStartTime > transitionDuration)
            {
                mode = CameraMode.Box;
            }
        }

        if (mode == CameraMode.TransitionBoxToConveyor)
        {
            transform.position = Vector3.Lerp(boxPos, conveyorPos, transitionProgress);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(boxRot, conveyorRot, transitionProgress));
            if (Time.time - transitionStartTime > transitionDuration)
            {
                mode = CameraMode.Conveyor;
            }
        }
    }
    public void transitionCam()
    {
         if (mode == CameraMode.Conveyor)
         {
             transitionStartTime = Time.time;
             mode = CameraMode.TransitionConveyorToBox;
         }
         if (mode == CameraMode.Box)
         {
             transitionStartTime = Time.time;
             mode = CameraMode.TransitionBoxToConveyor;
         }
    }
}
