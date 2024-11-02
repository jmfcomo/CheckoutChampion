using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraGrab : MonoBehaviour
{
    public static bool isGrabbing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrabbing && Input.GetMouseButtonDown(0))
        {
            // Raycast toward mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hit = Physics.RaycastAll(ray);


            // If first thing hit is grabbable
            Grabbable grabbable = hit.First<RaycastHit>().rigidbody.gameObject.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                // Grab it
                grabbable.Grab();
                isGrabbing = true;
            }
        }
    }
}
