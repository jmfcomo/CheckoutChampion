using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveToCart : MonoBehaviour
{
    [System.Serializable]
    public class AnimationStep
    {
        public Vector3 pos;
        public Vector3 rot;
        public float duration;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    }

    private int currentStep = 1;
    public bool playing = false;
    private float animStepStartTime;

    private Rigidbody rb;

    public List<AnimationStep> steps = new List<AnimationStep>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (Time.time > animStepStartTime + steps[currentStep].duration)
            {
                if (++currentStep == steps.Count)
                {
                    playing = false;
                }
                else
                {
                    animStepStartTime = Time.time;
                }
            }
            
            Vector3 pos = Vector3.Lerp(steps[currentStep - 1].pos, steps[currentStep].pos, steps[currentStep].curve.Evaluate((Time.time - animStepStartTime) / steps[currentStep].duration));
            Quaternion rot = Quaternion.Euler(Vector3.Lerp(steps[currentStep - 1].pos, steps[currentStep].pos, steps[currentStep].curve.Evaluate((Time.time - animStepStartTime) / steps[currentStep].duration)));

            rb.Move(pos, rot);
        }    
    }

    public void StartAnimation()
    {
        playing = true;
        currentStep = 1;
        animStepStartTime = Time.time;
    }

}
