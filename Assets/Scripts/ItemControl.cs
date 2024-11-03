using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class ItemControl : MonoBehaviour
{
    private float speed = 0.01f;
    private Grabbable grabbable;

    private void Start()
    {
        grabbable = GetComponent<Grabbable>();
    }

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (!grabbable.isGrabbed && transform.position.x < 4.2)
        {
            Vector3 pos = transform.position;
            pos.x += speed;
            transform.position = pos;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
