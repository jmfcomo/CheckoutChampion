using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveToCart : MonoBehaviour
{
    [System.Serializable]
    public class AnimationStep
    {
        public Vector3 pos = Vector3.zero;
        public Vector3 rot = Vector3.zero;
        public float duration = 0;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    }

    public int currentStep = 1;
    public bool playing = false;
    private float animStepStartTime;

    private Action<MoveToCart> endAction = (MoveToCart a) => { };

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
                    //currentStep = 1;
                    endAction(this);
                    return;
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
        currentStep = 1;
        animStepStartTime = Time.time;

        playing = true;

        //Debug.Log(gameObject.name + " movin");
    }

    public void StartAnimation(Action<MoveToCart> func)
    {
        endAction = func;

        StartAnimation();
    }

}
