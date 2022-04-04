using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGranade : MonoBehaviour
{
    public GameObject granade;
    public int granadeCount;
    public static ThrowGranade instance;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) && granadeCount > 0)
        {
            Instantiate(granade, transform.position + transform.forward, transform.rotation);
            granadeCount--;
            //Debug.LogError("g");
        }
    }
}
