using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField] string text;
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Manager.instance.ShowDialog(text);
        }
    }
}
