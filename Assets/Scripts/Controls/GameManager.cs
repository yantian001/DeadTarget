using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    #region variable
    [HideInInspector]
    public GameStatu statu = GameStatu.Init;
    [HideInInspector]
    public GameRecords record = new GameRecords();
    [HideInInspector]
    public int currentCombo = 0;
    [HideInInspector]
    public bool isCombo = false;

    public AudioClip startAudio;
    public AudioClip successAudio;
    public AudioClip failAudio;
    //连击有效时间
    public float comboInterval = 5.0f;

    private float curRemainComboInterval = 0.0f;
    #endregion

    #region MonoBehaviour
    public void OnEnable()
    {
        LeanTween.addListener((int)Events.TIMEUP, OnTimeUp);
        LeanTween.addListener((int)Events.ENEMYCLEARED, OnEnemyCleared);
        LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
        LeanTween.addListener((int)Events.PLAYERDIE, OnPlayerDie);
        LeanTween.addListener((int)Events.GAMEPAUSE, OnPause);
        LeanTween.addListener((int)Events.PREVIEWSTART, OnPreviewStart);
    }

    private void OnPreviewStart(LTEvent obj)
    {
        // throw new NotImplementedException();
        ChangeGameStatu(GameStatu.InGame);

    }

    public void Start()
    {
        //  ChangeGameStatu(GameStatu.InGame);
        if (startAudio)
        {
            LeanAudio.play(startAudio);
        }
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.TIMEUP, OnTimeUp);
        LeanTween.removeListener((int)Events.ENEMYCLEARED, OnEnemyCleared);
        LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
        LeanTween.removeListener((int)Events.PLAYERDIE, OnPlayerDie);
        LeanTween.removeListener((int)Events.GAMEPAUSE, OnPause);
        LeanTween.removeListener((int)Events.PREVIEWSTART, OnPreviewStart);
    }

    public void Update()
    {
        UpdateCombo();
    }


    #endregion

    #region custom method

    void OnEnemyDie(LTEvent evt)
    {
        if (evt.data != null)
        {
            var edi = evt.data as EnemyDeadInfo;
            if (edi.headShot)
            {
                record.HeadShotCount += 1;
            }
            isCombo = true;
            currentCombo += 1;
            curRemainComboInterval = comboInterval;
            record.MaxCombos = currentCombo;
        }
    }

    void UpdateCombo()
    {
        if (isCombo)
        {
            curRemainComboInterval -= Time.deltaTime;
            if (curRemainComboInterval <= 0.0f)
            {
                isCombo = false;
                currentCombo = 0;
            }

        }
    }

    /// <summary>
    /// 获取当前连击剩余有效时间
    /// </summary>
    /// <returns></returns>
    public float GetComboRemainPrectage()
    {
        return curRemainComboInterval / comboInterval;
    }

    void OnTimeUp(LTEvent evt)
    {
        record.FinishType = GameFinishType.Failed;
        ChangeGameStatu(GameStatu.Failed);
        if (failAudio)
        {
            LeanAudio.play(failAudio);
        }
    }

    void OnEnemyCleared(LTEvent evt)
    {
        record.FinishType = GameFinishType.Completed;
        ChangeGameStatu(GameStatu.Completed);
        if (successAudio)
        {
            LeanAudio.play(successAudio);
        }
    }

    private void OnPlayerDie(LTEvent obj)
    {
        // throw new NotImplementedException();
        record.FinishType = GameFinishType.Failed;
        ChangeGameStatu(GameStatu.Failed);
        if (failAudio)
        {
            LeanAudio.play(failAudio);
        }
    }

    /// <summary>
    /// 更改游戏状态
    /// </summary>
    /// <param name="s"></param>
    void ChangeGameStatu(GameStatu s)
    {
        if (s != statu)
        {
            statu = s;
            GameValue.staus = statu;
            if (statu == GameStatu.Failed || statu == GameStatu.Completed)
            {
                Invoke("DelayDispatchFinish", 2f);
            }
        }
    }

    void DelayDispatchFinish()
    {

        LeanTween.dispatchEvent((int)Events.GAMEFINISH, record);
        //  LeanTween.dispatchEvent((int)Events.GAMEFINISH,);
    }

    void OnPause(LTEvent evt)
    {

        if (statu == GameStatu.InGame)
        {
            ChangeGameStatu(GameStatu.Paused);
        }
        LeanTween.addListener((int)Events.GAMECONTINUE, OnContinue);
        //Time.timeScale = 0;
    }

    void OnContinue(LTEvent evt)
    {
        LeanTween.removeListener((int)Events.GAMECONTINUE, OnContinue);
        ChangeGameStatu(GameStatu.InGame);
        // Time.timeScale = 1;
    }
    #endregion
}
