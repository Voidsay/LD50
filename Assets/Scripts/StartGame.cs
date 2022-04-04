using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] string text;
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Manager.instance.ShowDialog(text);
            Manager.instance.GameSequence();
            Destroy(this.gameObject);
            ThrowGranade.instance.granadeCount = 5;
        }
    }
}
