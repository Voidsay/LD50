using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public bool invX, invY;
    public MusicManager music;
    public GameObject StartScreen;
    public AudioMixer mixer;

    public float sensX
    {
        get
        {
            return invX ? -_sensX : _sensX;
        }
        set
        {
            _sensX = value;
        }
    }
    public float sensY
    {
        get
        {
            return invY ? _sensY : -_sensY;
        }
        set
        {
            _sensY = value;
        }
    }

    public void SetSnes(float val)
    {
        sensY = val;
        sensX = val;
    }

    public void InvX(bool val)
    {
        invX = val;
    }
    public void InvY(bool val)
    {
        invY = val;
    }

    public void Volume(float val)
    {
        mixer.SetFloat("Volume", val);
    }

    public void ToggleObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    private float _sensX = 1, _sensY = 1;
    public Text textbox;

    // Start is called before the first frame update
    void Awake()
    {
        if (Manager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Manager.instance = this;
        }
        Time.timeScale = 0;
    }

    public void ShowDialog(string text)
    {
        StartCoroutine(Show(text));
    }

    IEnumerator Show(string text)
    {
        textbox.gameObject.SetActive(true);
        textbox.text = text;
        yield return new WaitForSeconds(2);
        textbox.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        music.level = 1;
        Time.timeScale = 1;
        StartScreen.SetActive(false);
    }

    public float starttime, endtime;
    public float[] phasetime;
    public void GameSequence()
    {
        starttime = Time.time;
        StartCoroutine(Phases());
    }

    public Image heatlevel;
    public float cooling;
    void Update()
    {
        if (starttime > 0)
        {
            heatlevel.color = new Color(heatlevel.color.r, heatlevel.color.g, heatlevel.color.b, (Time.time - starttime - cooling) / phasetime[phasetime.Length - 1]);
        }
    }

    public GameObject ScoreScreen;
    public Text ScoreText;
    IEnumerator Phases()
    {
        for (int i = 2; i < phasetime.Length; i++)
        {
            yield return new WaitUntil(() => Time.time - starttime - cooling >= phasetime[i]);
            music.level = i - 1;
            Debug.Log(i - 1);
        }
        music.source.loop = false;
        yield return new WaitUntil(() => !music.source.isPlaying);
        endtime = Time.time - starttime;
        Debug.Log("End: endtime" + endtime);
        music.source.clip = music.music[music.music.Length - 1];
        music.source.Play();
        Time.timeScale = 0;
        ScoreScreen.SetActive(true);
        ScoreText.text = endtime.ToString("0#.0");
        Cursor.lockState = CursorLockMode.None;
    }
}
