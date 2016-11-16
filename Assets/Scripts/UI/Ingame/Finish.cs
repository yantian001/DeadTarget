using UnityEngine;
using System.Collections;
using System;

public class Finish : MonoBehaviour
{

    public RectTransform parentRect;
    public RectTransform victoryRect;
    public RectTransform failRect;
    public RectTransform comboRect;
    public RectTransform headShotRect;
    public RectTransform taskRect;
    public RectTransform totalRect;

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
    }


    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.GAMEFINISH, OnGameFinish);
    }


    // Use this for initialization
    void Start()
    {
        if (!parentRect)
        {
            parentRect = GetComponent<RectTransform>();
        }
        //标题
        if (!victoryRect)
        {
            victoryRect = CommonUtils.GetChild(parentRect, "Background/victory/success");
        }
        victoryRect.gameObject.SetActive(false);
        if (!failRect)
        {
            failRect = CommonUtils.GetChild(parentRect, "Background/victory/fail");
        }
        failRect.gameObject.SetActive(false);

        //tongji
        if (!comboRect)
        {
            comboRect = CommonUtils.GetChild(parentRect, "Background/ComboReward");
        }
        comboRect.anchoredPosition += new Vector2(Screen.width * 1.5f, 0);
        //tongji
        if (!headShotRect)
        {
            headShotRect = CommonUtils.GetChild(parentRect, "Background/HeadshotReward");
        }
        headShotRect.anchoredPosition += new Vector2(Screen.width * 1.5f, 0);

        //tongji
        if (!taskRect)
        {
            taskRect = CommonUtils.GetChild(parentRect, "Background/Task");
        }
        taskRect.anchoredPosition += new Vector2(Screen.width * 1.5f, 0);

        //tongji
        if (!totalRect)
        {
            totalRect = CommonUtils.GetChild(parentRect, "Background/Total");
        }
        totalRect.anchoredPosition += new Vector2(Screen.width * 1.5f, 0);
        //parentRect.anchoredPosition = new Vector2(0, 0);
    }

    private void OnGameFinish(LTEvent obj)
    {
        //throw new NotImplementedException();
        GameRecords record = obj.data as GameRecords;
        int taskreward =0;
        if (record.FinishType == GameFinishType.Completed)
        {
            taskreward = GameValue.taskData.Info.Reward;
        }
        int total = record.MaxCombos * 10 + record.HeadShotCount * 10 + taskreward;
        //保存金币
        LeanTween.dispatchEvent((int)Events.MONEYUSED, -total);
        if (record != null)
        {
            failRect.gameObject.SetActive(!(record.FinishType == GameFinishType.Completed));
            victoryRect.gameObject.SetActive((record.FinishType == GameFinishType.Completed));
            CommonUtils.SetChildText(comboRect, "val", record.MaxCombos.ToString());
            CommonUtils.SetChildText(headShotRect, "val", record.HeadShotCount.ToString());
            CommonUtils.SetChildText(taskRect, "val", taskreward.ToString());
            CommonUtils.SetChildText(totalRect, "val", total.ToString());
        }

        parentRect.anchoredPosition = new Vector2(0, 0);
        float moveDistance = Screen.width * 1.5f;
        float moveTime = 0.2f;
        LeanTween.moveX(comboRect, comboRect.anchoredPosition.x - moveDistance, moveTime).setOnComplete(() =>
        {
            LeanTween.moveX(headShotRect, headShotRect.anchoredPosition.x - moveDistance, moveTime).setOnComplete(() =>
            {
                LeanTween.moveX(taskRect, taskRect.anchoredPosition.x - moveDistance, moveTime).setOnComplete(() =>
                {
                    LeanTween.moveX(totalRect, totalRect.anchoredPosition.x - moveDistance, moveTime).setOnComplete(() =>
                    {
                    });
                });
            });
        });
    }

}
