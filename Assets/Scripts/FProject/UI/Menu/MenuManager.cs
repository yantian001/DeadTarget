using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using FProject;

public class MenuManager : MonoBehaviour
{


    public GameObject menu;
    public GameObject shop;
    public GameObject itemShop;
    public GameObject weaponShop;

    public Transform levelParent;
    protected ChapterItem currentChapterItem;
    protected int selectLevel = -1;

    public ChapterItem[] chapterItems;
    public bool autoSearchChapterItems = false;

    public UIShopTab[] tabs;
    // public Transform templateItem;
    protected Vector3 tempScrollPos;
    protected int currentTabIndex = -1;
    int moveHight;
    // Use this for initialization
    void Start()
    {
        UpdateTabDisplay(0);
        //Player.CurrentUser.SetLevelRecord(2, 25);
        tempScrollPos = levelParent.parent.position;
        if (autoSearchChapterItems)
            chapterItems = FindObjectsOfType<ChapterItem>();
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].onChange = OnShopTabChange;
        }
    }



    /// <summary>
    /// 更新页面显示
    /// 0 - 显示选关
    /// 1 - 显示武器购买
    /// 2 - 显示道具购买
    /// </summary>
    /// <param name="index"></param>
    void UpdateTabDisplay(int index)
    {
        if (currentTabIndex == index)
        {
            return;
        }
        currentTabIndex = index;
        if (currentTabIndex == 0)
        {
            vp_Utility.Activate(menu);
            vp_Utility.Activate(shop, false);
            UpdateLevelsDisplay(0);
        }
        else
        {
            //显示商店
            if (currentTabIndex > 0)
            {
                vp_Utility.Activate(shop);
                vp_Utility.Activate(menu, false);
                if (currentTabIndex == 1)
                {
                    vp_Utility.Activate(itemShop, false);
                    vp_Utility.Activate(weaponShop);
                }
                else
                {
                    if (currentTabIndex == 2)
                    {
                        //weaponShop
                        vp_Utility.Activate(weaponShop, false);
                        vp_Utility.Activate(itemShop);
                    }
                }
            }
        }
    }

    public void OnBackClicked()
    {
        if (currentTabIndex > 0)
        {
            UpdateTabDisplay(0);
        }
        else
        {
            LeanTween.dispatchEvent((int)Events.BACKTOSTART);
        }
    }

    #region Menu

    public void OnChapterSelectChange(ChapterItem item, bool selected)
    {
        Debug.Log(selected);
        if (selected)
        {
            if (currentChapterItem != item)
            {
                currentChapterItem = item;
                UpdateLevelsDisplay();
            }
        }
        else
        {
            //if (currentChapterItem == item)
            //{
            //    currentChapterItem = null;
            //}
        }
    }

    protected void GenerateLevelItems()
    {

        for (int i = 1; i < levelParent.childCount; i++)
        {
            Destroy(levelParent.GetChild(i).gameObject);
        }
        Transform templateItem = levelParent.GetChild(0);
        int currentLevel = Player.CurrentUser.GetSceneCurrentLevel(currentChapterItem.chapterId);
        for (int i = 0; i < currentChapterItem.chapterLevelCount; i++)
        {
            Transform itemNew;
            if (i == 0)
            {
                itemNew = templateItem;
            }
            else
            {
                itemNew = Transform.Instantiate<Transform>(templateItem);
                itemNew.SetParent(levelParent);
                itemNew.localScale = Vector3.one;
                //itemNew.position = templateItem.position;
                itemNew.SetParent(levelParent);
            }

            // CommonUtils.SetChildText(itemNew, "Label", (i + 1).ToString());
            var t = itemNew.GetComponent<UILevelItem>();
            t.onChange = OnLevelSelect;
            t.LevelID = i + 1;

            if (i == currentLevel)
            {
                t.Selected = true;
            }
            else
            {
                t.Selected = false;
                if (i > currentLevel)
                {
                    t.Interactable = false;
                }
            }

        }
        var table = levelParent.GetComponent<UITable>();
        //自动从新排列
        table.repositionNow = true;
        //移动到当前关卡
        int rowHeight = 80;
        int rowcount = currentChapterItem.chapterLevelCount % table.columns == 0 ? currentChapterItem.chapterLevelCount / table.columns : currentChapterItem.chapterLevelCount / table.columns + 1;
        int currentRow = currentLevel % table.columns == 0 ? currentLevel / table.columns : currentLevel / table.columns + 1;
        moveHight = 0;
        if ((currentRow > 3))
        {
            if (rowcount - currentRow < 4)
            {
                moveHight = (rowcount - 4) * rowHeight;
            }
            else
                moveHight = currentRow * rowHeight;
        }
        levelParent.parent.GetComponent<UIScrollView>().MoveRelative(new Vector3(0, moveHight, 0));

    }

    public void OnLevelSelect(int level)
    {

        Debug.Log("Select Level " + level);
        selectLevel = level;
    }

    private void UpdateLevelsDisplay(int id)
    {
        if (chapterItems.Length > 0)
        {
            for (int i = 0; i < chapterItems.Length; i++)
            {
                if (chapterItems[i].chapterId == id)
                {
                    currentChapterItem = chapterItems[i];
                    break;
                }
            }
            if (currentChapterItem == null)
            {
                currentChapterItem = chapterItems[0];
            }
            var toggle = currentChapterItem.gameObject.GetComponent<UIToggle>();
            toggle.Set(true, false);
            UpdateLevelsDisplay();

        }
        else
        {
            Debug.LogError("Miss chapter lists");
        }
    }

    private void UpdateLevelsDisplay()
    {
        if (currentChapterItem != null)
        {
            if (levelParent)
            {
                levelParent.parent.GetComponent<UIScrollView>().MoveRelative(new Vector3(0, -moveHight, 0));
                GenerateLevelItems();
            }
            else
            {
                Debug.LogError("Miss Level Parent!!!");
            }
        }
        //throw new NotImplementedException();
    }

    public void OnPlayClicked()
    {
        if (selectLevel > 0 && currentChapterItem != null && selectLevel <= currentChapterItem.chapterLevelCount)
        {
            DSystem.Instance.currentLevel = selectLevel;
            DSystem.Instance.currentScene = currentChapterItem.chapterId;
            DSystem.Instance.sceneName = currentChapterItem.chapterSceneName;
            LeanTween.dispatchEvent((int)Events.GAMESTART);
        }
        else
        {
            Debug.Log("Selec Level Pls.");
        }
    }

    #endregion

    #region Shop

    public void OnShopClicked()
    {
        // UpdateTabDisplay(1);
        tabs[0].Selected = true;
    }

    private void OnShopTabChange(int index)
    {
        // throw new NotImplementedException();
        Debug.Log("Select Tab" + index);
        UpdateTabDisplay(index);
    }
    #endregion
}
