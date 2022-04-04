using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] music;
    public AudioSource source;

    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = StartCoroutine(SetMusic(value));
            _level = value;
        }
    }
    public int _level;
    Coroutine routine;

    void Start()
    {
        level = 0;
    }

    IEnumerator SetMusic(int val)
    {
        source.loop = false;
        yield return new WaitUntil(() => !source.isPlaying);
        source.clip = music[val];
        source.Play();
        if (_level < music.Length - 2)
        {
            source.loop = true;
        }
        routine = null;
    }
}
