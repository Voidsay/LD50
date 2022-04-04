using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject effect;
    public Vector3 force;

    void Start()
    {
        transform.GetComponent<Rigidbody>().AddRelativeForce(force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        var bb = col.gameObject.GetComponent<Break>();
        if (bb != null)
        {
            bb.FallApart();
        }
        Instantiate(effect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
