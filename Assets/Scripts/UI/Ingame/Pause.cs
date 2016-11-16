using UnityEngine;
using System.Collections;
using System;

public class Pause : MonoBehaviour
{

    Vector2 tempPosition;
    public RectTransform rt;
    // Use this for initialization
    void Start()
    {
        if (rt == null)
        {
            rt = GetComponent<RectTransform>();
        }
        tempPosition = rt.anchoredPosition;

    }

    public void Awake()
    {
        LeanTween.addListener((int)Events.GAMEPAUSE, OnGamePause);
        LeanTween.addListener((int)Events.GAMECONTINUE, OnGameContinue);
    }

    private void OnGameContinue(LTEvent obj)
    {
        //throw new NotImplementedException();
        rt.anchoredPosition = tempPosition;
    }

    private void OnGamePause(LTEvent obj)
    {
        //throw new NotImplementedException();
        rt.anchoredPosition = Vector2.zero;
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.GAMEPAUSE, OnGamePause);
        LeanTween.removeListener((int)Events.GAMECONTINUE, OnGameContinue);
    }
    
}
