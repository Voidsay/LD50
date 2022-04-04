using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public Collider col;

    public void FallApart()
    {
        Debug.Log("break");
        List<Rigidbody> rubble = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
        rubble.RemoveAt(0);
        foreach (Rigidbody rubbl in rubble)
        {
            rubbl.isKinematic = false;
            rubbl.transform.parent = null;
        }
        col.enabled = false;
    }
}
