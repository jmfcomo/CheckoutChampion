using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    private float speed;


    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (transform.position.x < 4.2)
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
