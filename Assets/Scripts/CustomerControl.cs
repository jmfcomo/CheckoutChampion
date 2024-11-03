using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerControl : MonoBehaviour
{
    public GameObject spawnPoint;
    private int totalFramesWalking = 350;
    private int frame = 0;
    public float speed = 0.02f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = spawnPoint.transform.position;
        animator = GetComponent<Animator>();
        animator.SetTrigger("TriStartWalk");
    }

    private void FixedUpdate()
    {
        frame++;

        if (frame <= totalFramesWalking)
        {
            Vector3 pos = transform.position;
            pos.x += speed;
            transform.position = pos;
        }

        if (frame == totalFramesWalking)
        {
            animator.SetTrigger("TriEndWalk");
        }
    }

    public void RetriggerAnimation()
    {
        frame = 0;
        animator.SetTrigger("TriStartWalk");
    }
}
