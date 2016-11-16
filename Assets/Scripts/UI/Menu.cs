using UnityEngine;
using System.Collections;
using GameDataEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{

    //更新关卡

    RectTransform rect;
    RectTransform rr;
    RectTransform rr2;
    private List<string> levels;
    private GDELevelData currentLevel;
    float rrOriginPositionY;


    // Use this for initialization
    void Start()
    {

        rect = GetComponent<RectTransform>();
        rr = CommonUtils.GetChild(rect, "RawImage/RawImage");
        rr2 = CommonUtils.GetChild(rect, "RawImage/ShopPosition");
        rrOriginPositionY = rr.anchoredPosition.y;
        GDEDataManager.Init("gde_data");
        if (!GDEDataManager.GetAllDataKeysBySchema("Level", out levels))
        {
            return;
        }

        updateLevel(0);
        setButtonFunc();
        GameValue.diffdegree = "Ordinary";

        showShopMenu();


    }

    //更新toggle
    void initToggle()
    {

        Vector3 newPos;
        bool isFoundCurrent = false;
        List<Toggle> toggles = new List<Toggle>();

        int taskcnt = currentLevel.TaskList.Count;
        GameValue.taskData = null;


        Toggle task = CommonUtils.GetChildComponent<Toggle>(rect, "middle/Panel/Panel/0");

        ToggleGroup group = CommonUtils.GetChildComponent<ToggleGroup>(rect, "middle/Panel/Panel");


        group.allowSwitchOff = true;

        if (taskcnt == 0)
        {
            task.enabled = false;
            return;
        }

        toggles.Add(task);


        Toggle[] tgs = group.GetComponentsInChildren<Toggle>();


        foreach (Toggle t in tgs)
        {
            if (t.name != "0")
            {
                Destroy(t.gameObject);
            }
            else
            {
                SelectToggle(t, false);
            }
        }

        int i = 0;
       
        for (i = 1; i < taskcnt; i++)
        {
            Toggle clone = (Toggle)Instantiate(task, task.GetComponent<RectTransform>().position, task.GetComponent<RectTransform>().rotation);


            clone.GetComponent<RectTransform>().SetParent(group.GetComponent<RectTransform>());
            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            newPos = clone.GetComponent<RectTransform>().anchoredPosition;

            newPos.y = newPos.y - 160f * i;



            clone.GetComponent<RectTransform>().anchoredPosition = newPos;
            clone.name = "" + (i);

            if (i == taskcnt - 1)
                (CommonUtils.GetChildComponent<RawImage>(clone.GetComponent<RectTransform>(), "Continue")).enabled = false;
            toggles.Add(clone);

        }
        group.GetComponent<RectTransform>().sizeDelta = new Vector2(group.GetComponent<RectTransform>().sizeDelta.x, i * 160);

        newPos = group.GetComponent<RectTransform>().anchoredPosition;
        if (!isFoundCurrent)
            newPos.y = -i * 160 / 2 -10;

        group.GetComponent<RectTransform>().anchoredPosition = newPos;

        int currntY = 0;
        for (i = 0; i < taskcnt; i++)
        {
            Toggle t = toggles[i];
            t.onValueChanged.AddListener(
                delegate (bool isOn)
                {
                    this.SelectToggle(t, isOn);
                }
            );

            RectTransform toggleRect = t.GetComponent<RectTransform>();

            GDETaskData currentTask = currentLevel.TaskList[i];

            RawImage TaskThumb = CommonUtils.GetChildComponent<RawImage>(toggleRect, "TaskThumb");



            if (currentTask.isLocked)
            {
                CommonUtils.SetChildActive(toggleRect, "Lock", true);
                TaskThumb.enabled = false;
                t.interactable = false;
            }
            else
            {

                CommonUtils.SetChildActive(toggleRect, "Lock", false);
                t.interactable = true;
                TaskThumb.enabled = true;
                TaskThumb.texture = Resources.Load(currentTask.TaskThumb) as Texture2D;
                if (!currentTask.isCleared)
                {
                    SelectToggle(t, true);
                    currntY = i;
                   // isFoundCurrent = true;
                }

            }
            CommonUtils.SetChildActive(toggleRect, "Cleared", currentTask.isCleared && !currentTask.isLocked);
            Text TaskNum = CommonUtils.GetChildComponent<Text>(toggleRect, "TaskNum");

            TaskNum.text = currentLevel.LevelNum1.ToString() + "-" + currentTask.TaskNum.ToString();
        }

        newPos = group.GetComponent<RectTransform>().anchoredPosition;
       // if (currntY != 0)
            newPos.y =  currntY * 160 -10;

        group.GetComponent<RectTransform>().anchoredPosition =  new Vector2(newPos.x, -group.GetComponent<RectTransform>().sizeDelta.y +newPos.y);

        Text taskName = CommonUtils.GetChildComponent<Text>(rect, "middle/Task/Text");

        taskName.text = currentLevel.LevelName;
        
        // SelectToggle(task, true);
        group.allowSwitchOff = false;
    }

    //toggle事件

    void SelectToggle(Toggle t, bool isOn)
    {
       
        t.isOn = isOn;

        if (isOn)
        {


            Text taskContent = CommonUtils.GetChildComponent<Text>(rect, "middle/Task/content");

            GDETaskData task = currentLevel.TaskList[int.Parse(t.name)];

            taskContent.text = task.TaskContent;
            GameValue.mapId = task.TaskNum;
            GameValue.taskData = task;
        }
    }

    void updateLevel(int value)
    {
        if (!GDEDataManager.DataDictionary.TryGetCustom(levels[value], out currentLevel))
        {
            currentLevel = null;
        }

        GameValue.level = currentLevel.LevelNum1;

        initToggle();

        updateDiffBtn();

    }

    void showShopMenu()
    {

        CommonUtils.SetChildActive(rect, "RawImage", false);

        Button ShopBtn = CommonUtils.GetChildComponent<Button>(rect, "bottom/RawImage/Shop");
        //ShopBtn.onClick.AddListener(delegate ()
        //{
        //    GameValue.shopType = "weapon";

        //    Application.LoadLevel("Shop");
        //});
        ShopBtn.onClick.AddListener(delegate ()
        {
            //LeanTween;
            CommonUtils.SetChildActive(rect, "RawImage", true);



            //LeanTween.moveX( avatarScale, avatarScale.transform.position.x + 5f, 5f).setEase(LeanTweenType.easeOutBounce);

          //  RectTransform rr = CommonUtils.GetChild(rect, "RawImage/RawImage");

            //Debug.Log(rr.position.y);

            LeanTween.moveY(rr, rr2.anchoredPosition.y, 0.4f);
            

            Button weaponBtn = CommonUtils.GetChildComponent<Button>(rect, "RawImage/RawImage/weapon");
            weaponBtn.onClick.AddListener(delegate ()
            {
                GameValue.shopType = "weapon";

                Application.LoadLevel("Shop");

            });

            Button bombBtn = CommonUtils.GetChildComponent<Button>(rect, "RawImage/RawImage/bomb");
            bombBtn.onClick.AddListener(delegate ()
            {
                GameValue.shopType = "bomb";
                Application.LoadLevel("Shop");
            });

            Button medicalBtn = CommonUtils.GetChildComponent<Button>(rect, "RawImage/RawImage/medical");
            medicalBtn.onClick.AddListener(delegate ()
            {
                GameValue.shopType = "medical";
                Application.LoadLevel("Shop");
            });




        });

        Button maskBtn = CommonUtils.GetChildComponent<Button>(rect, "RawImage");

        maskBtn.onClick.AddListener(delegate ()
        {
            //LeanTween;

            RectTransform rr = CommonUtils.GetChild(rect, "RawImage/RawImage");
            LeanTween.moveY(rr, rrOriginPositionY, 0.4f);



            LeanTween.delayedCall(0.4f, hideMask);


        });
    }

    void hideMask()
    {

        RectTransform rr = CommonUtils.GetChild(rect, "RawImage/RawImage");

        Debug.Log(rr.position.y);
        CommonUtils.SetChildActive(rect, "RawImage", false);
    }

    void setButtonFunc()
    {
        Button left = CommonUtils.GetChildComponent<Button>(rect, "middle/left");
        Button right = CommonUtils.GetChildComponent<Button>(rect, "middle/right");

        left.onClick.AddListener(delegate ()
        {
            if (currentLevel.LevelNum1 > 1)
            {

                updateLevel(currentLevel.LevelNum1 - 2);
            }
        });

        right.onClick.AddListener(delegate ()
        {
            if (currentLevel.LevelNum1 < levels.Count)
                updateLevel(currentLevel.LevelNum1);
        });

    }

    void updateDiffBtn()
    {

        ToggleGroup tg = CommonUtils.GetChildComponent<ToggleGroup>(rect, "bottom/bg/Panel");



        Toggle Ordinary = CommonUtils.GetChildComponent<Toggle>(rect, "bottom/bg/Panel/Ordinary");
        Ordinary.onValueChanged.AddListener(
            delegate (bool isOn)
            {



                GameValue.diffdegree = "Ordinary";


            }
        );

        Toggle Elite = CommonUtils.GetChildComponent<Toggle>(rect, "bottom/bg/Panel/Elite");



        if (currentLevel.TaskList[currentLevel.TaskList.Count - 1].isLocked == true)
        {
            Elite.isOn = false;
            Ordinary.isOn = true;
            Elite.enabled = false;
        }
        else
        {


            Elite.enabled = true;
        }

        Elite.onValueChanged.AddListener(
            delegate (bool isOn)
            {


                GameValue.diffdegree = "Elite";

            }
        );
    }

}
