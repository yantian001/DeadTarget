using UnityEngine;
using System.Collections;
using System;

public class BGM : MonoBehaviour
{


    public AudioSource clip;
    private static BGM _instance;

    public static BGM Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BGM>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("BGMContainer");
                    _instance = obj.AddComponent<BGM>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LeanTween.addListener((int)Events.STOPBGM, OnStopBGM);
            LeanTween.addListener((int)Events.STARTBGM, OnStartBGM);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    private void OnStartBGM(LTEvent obj)
    {
        //throw new NotImplementedException();
        PlaySound(true);
    }

    private void OnStopBGM(LTEvent obj)
    {
        //throw new NotImplementedException();
        PlaySound(false);
    }

    public void Start()
    {
        if (clip == null)
        {
            clip = GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="b"></param>
    public void PlaySound(bool b)
    {
        if (clip == null)
            return;
        if (b)
        {
            if (!clip.isPlaying)
            {
                clip.Play();
            }
        }
        else
        {
            if (clip.isPlaying)
                clip.Stop();
        }
    }
}
