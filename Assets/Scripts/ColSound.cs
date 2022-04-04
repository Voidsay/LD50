using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColSound : MonoBehaviour
{
    public AudioSource source;
    public float minvel;

    void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > minvel)
        {
            source.Play();
        }
    }
}
