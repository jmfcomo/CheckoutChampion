using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerControl : MonoBehaviour
{
    public GameObject spawnPoint;
    private int totalFramesWalking = 350;
    private int frame = 0;
    public float speed = 0.02f;
    private Animator animator;
    private TMP_Text dialogueText;
    public string dialogueString = "Hello world";

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = spawnPoint.transform.position;
        animator = GetComponent<Animator>();
        animator.SetTrigger("TriStartWalk");

        dialogueText = transform.Find("Dialogue").GetComponent<TMP_Text>();

        dialogueText.enabled = true;

        dialogueText.text = dialogueString;
        dialogueText.alpha = 0f;


        Invoke("fadeIn", 5f);
        Invoke("fadeOut", 10f);
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

    public void fadeIn()
    {
        StartCoroutine(fadeOverTime(0, 1, 1));
    }

    public void fadeOut()
    {
        StartCoroutine(fadeOverTime(1, 0, 1));
    }
    
    public IEnumerator fadeOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            dialogueText.alpha = Mathf.Lerp(fromVal, toVal, counter / duration);
            //Debug.Log("Val: " + dialogueText.alpha);
            yield return null;
        }
    }

    public void RetriggerAnimation()
    {
        frame = 0;
        animator.SetTrigger("TriStartWalk");
    }
}
