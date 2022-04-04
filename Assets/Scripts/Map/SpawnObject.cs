using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject[] obj;
    [Range(0, 1)]
    [SerializeField] float chance;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value < chance)
        {
            Instantiate(obj[Random.Range(0, obj.Length)], transform.position, transform.rotation);
        }
    }

}
