using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frige : MonoBehaviour
{
    public float coolingpower;
    public HingeJoint hinge;
    public bool activated;
    public AudioSource source;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !activated)
        {
            Manager.instance.cooling += coolingpower;
            JointSpring hingeSpring = hinge.spring;
            hingeSpring.spring = 10;
            hingeSpring.damper = 3;
            hingeSpring.targetPosition = 70;
            hinge.spring = hingeSpring;
            hinge.useSpring = true;
            source.Play();
            activated = true;
        }
    }
}
