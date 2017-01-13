using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using FProject;
using GameDataEditor;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance = null;

    public GameObject menu;
    public GameObject shop;
    public GameObject itemShop;
    public GameObject weaponShop;

    public Transform weaponParent;
    public Transform LoadingT;
    public Transform levelParent;
    protected ChapterItem currentChapterItem;
    protected int selectLevel = -1;

    public ChapterItem[] chapterItems;
    public bool autoSearchChapterItems = false;

    public UIShopTab[] tabs;
    public AudioSource m_audio;
    public AudioClip ac_BuyAmmo;
    public AudioClip ac_NotEnough;
    public AudioClip ac_WeaponUp;


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
        InitWeapon();
        if (!LoadingT)
        {
            LoadingT = CommonUtils.GetChildComponent<Transform>(transform, "Loading");
        }
        if (LoadingT)
            LoadingT.localPosition = new Vector3(1000, 1000, 1000);
    }

    public void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        //显示广告
        FUGSDK.Ads.Instance.ShowBanner(GoogleMobileAds.Api.AdPosition.Top);
    }

    public void OnDisable()
    {
        //隐藏 条形 广告
        FUGSDK.Ads.Instance.HideBanner();
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
            UpdateLevelsDisplay(Player.CurrentUser.LastPlayedScene);
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
            foreach (var t in this.tabs)
            {
                t.Selected = false;
            }
        }
        else
        {
            LeanTween.dispatchEvent((int)Events.BACKTOSTART);
        }
    }

    #region Menu

    public void OnChapterSelectChange(ChapterItem item, bool selected)
    {
        //  Debug.Log(selected);
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
            Player.CurrentUser.LastPlayedScene = currentChapterItem.chapterId;
            StartCoroutine(LoadScene(currentChapterItem.chapterSceneName));
            if (LoadingT)
                LoadingT.localPosition = Vector3.zero;
        }
        // LeanTween.dispatchEvent((int)Events.GAMESTART);


        else
        {
            // Debug.Log("Select Level Pls.");
            MessageTips.Tips("Please select level!");
        }
    }
    AsyncOperation async = null;
    IEnumerator LoadScene(string sceneName)
    {
        FUGSDK.Ads.Instance.HideBanner();
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }

        CommonUtils.SetChildActive(LoadingT, "Loading", false);
        CommonUtils.SetChildActive(LoadingT, "Tap", true);
    }


    public void OnTapToContinue()
    {
        if (async != null && async.progress >= 0.9f)
        {
            async.allowSceneActivation = true;
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

    #region Weapon

    //Dictionary<int, UIWeaponItem> weaponItems = new Dictionary<int, UIWeaponItem>();
    List<UIWeaponItem> weaponItems = new List<UIWeaponItem>();
    public void InitWeapon()
    {
        if (weaponParent.childCount > 1)
        {
            for (int i = 1; i < weaponParent.childCount; i++)
            {
                Destroy(weaponParent.GetChild(i).gameObject);
            }
        }


        if (WeaponManager.Instance.Weapons.Count > 0)
        {
            weaponItems.Clear();
            UIWeaponItem item0 = weaponParent.GetChild(0).GetComponent<UIWeaponItem>();

            for (int i = 0; i < WeaponManager.Instance.Weapons.Count; i++)
            {
                UIWeaponItem item = null;
                if (i == 0)
                {
                    item = item0;

                }
                else
                    item = GameObject.Instantiate<UIWeaponItem>(item0);
                item.Weapon = WeaponManager.Instance.Weapons[i];
                item.transform.SetParent(weaponParent);
                item.transform.localScale = item0.transform.localScale;
                if (item.Weapon.Id == WeaponManager.Instance.GetCurrentWeaponId())
                {
                    item.Select();
                }
                weaponItems.Add(item);
            }
            weaponParent.GetComponent<UIGrid>().repositionNow = true;
        }

        // SelectWeapon(WeaponManager.Instance.GetCurrentWeapon());
    }

    public void UpdateWeaponItemDisplay()
    {
        for (int i = 0; i < weaponItems.Count; i++)
        {
            weaponItems[i].UpdateDisplay();
        }
    }

    GDEWeaponData currentWeapon = null;
    public void SelectWeapon(GDEWeaponData weapon)
    {
        if (!(weapon != null && currentWeapon != weapon))
        {
            return;
        }
        currentWeapon = weapon;

        UpdateWeaponDisplay();
    }

    void UpdateWeaponDisplay()
    {
        //更新武器名字
        CommonUtils.SetChildText(transform, "shop/weaponshop/lbWeaponName", currentWeapon.name);
        CommonUtils.SetChildActive(transform, "shop/weaponshop/btnEquip", CanEqWeapon(currentWeapon));
        CommonUtils.SetChildSpriteName(transform, "shop/weaponshop/spWeaponImage", currentWeapon.thumb);

        //更新武器数值
        CommonUtils.SetChildSpriteSliderValue(transform, "shop/weaponshop/sdPower/fill", (float)WeaponManager.GetWeaponPower(currentWeapon) / WeaponManager.MAX_POWER);
        CommonUtils.SetChildSpriteSliderValue(transform, "shop/weaponshop/sdFireRate/fill", (float)WeaponManager.GetWeaponFireRate(currentWeapon) / WeaponManager.MAX_FIRERATE);
        CommonUtils.SetChildSpriteSliderValue(transform, "shop/weaponshop/sdStab/fill", (float)WeaponManager.GetWeaponStab(currentWeapon) / WeaponManager.MAX_STAB);
        CommonUtils.SetChildSpriteSliderValue(transform, "shop/weaponshop/sdCapacity/fill", (float)WeaponManager.GetWeaponCapacity(currentWeapon) / WeaponManager.MAX_CAPACITY);

        //更新按钮
        CommonUtils.SetChildActive(transform, "shop/weaponshop/btnUpdateWeapon", currentWeapon.isowned && WeaponManager.WeaponHasNextLevel(currentWeapon));
        CommonUtils.SetChildText(transform, "shop/weaponshop/btnUpdateWeapon/Price", WeaponManager.GetWeaponUpgradePrice(currentWeapon).ToString());
        CommonUtils.SetChildActive(transform, "shop/weaponshop/btnBuyWeapon", !currentWeapon.isowned);
        CommonUtils.SetChildText(transform, "shop/weaponshop/btnBuyWeapon/Price", currentWeapon.cost.ToString());
        CommonUtils.SetChildActive(transform, "shop/weaponshop/btnBuyAmmo", currentWeapon.isowned);
        CommonUtils.SetChildActive(transform, "shop/weaponshop/bullet", currentWeapon.isowned);
        CommonUtils.SetChildText(transform, "shop/weaponshop/btnBuyAmmo/lbprice", currentWeapon.bulletprice.ToString());
        CommonUtils.SetChildText(transform, "shop/weaponshop/bullet/Label", string.Format("{0}/{1}", currentWeapon.bullet, WeaponManager.GetWeaponCapacity(currentWeapon)));
    }


    bool CanEqWeapon(GDEWeaponData w)
    {
        return w.isowned && !w.isEquipment;
    }

    public void OnWeaponBuyClicked()
    {
        if (currentWeapon != null)
        {
            if (!currentWeapon.isowned)
            {
                if (Player.CurrentUser.IsMoneyEnough(ConvertUtil.ToInt32(currentWeapon.cost, 1000000)))
                {
                    //WeaponManager.Instance.BuyWeapon()
                    Player.CurrentUser.UseMoney(ConvertUtil.ToInt32(currentWeapon.cost, 1000000));
                    currentWeapon.isowned = true;
                    if (currentWeapon.bullet <= 0)
                    {
                        currentWeapon.bullet = WeaponManager.GetWeaponCapacity(currentWeapon);
                    }
                    UpdateWeaponDisplay();
                    UpdateWeaponItemDisplay();
                    m_audio.PlayOneShot(ac_WeaponUp);
                }
                else
                {
                    MessageTips.Tips("Don't have enough money !!");
                    m_audio.PlayOneShot(ac_NotEnough);

                }
            }
        }
        else
        {
            MessageTips.Tips("Please select a weapon first !!");
        }
    }

    /// <summary>
    /// 点击了装备
    /// </summary>
    public void OnEquipWeaponClicked()
    {
        if (!currentWeapon.isowned)
        {
            MessageTips.Tips("Please buy weapon first!!");
            return;
        }
        if (currentWeapon.isEquipment)
        {
            MessageTips.Tips("Already equiped this weapon !!");

        }
        WeaponManager.Instance.EqWeaon(currentWeapon.Id);
        UpdateWeaponDisplay();
        UpdateWeaponItemDisplay();
        // InitWeapon();
    }
    /// <summary>
    /// 升级按钮事件
    /// </summary>
    public void OnUpgradeClicked()
    {
        if (currentWeapon == null)
        {
            MessageTips.Tips("Please select weapon first!!");
            return;
        }
        if (!WeaponManager.WeaponHasNextLevel(currentWeapon))
        {
            MessageTips.Tips("Weapon can't upgrade");
            return;
        }
        if (!Player.CurrentUser.IsMoneyEnough(WeaponManager.GetWeaponUpgradePrice(currentWeapon)))
        {
            MessageTips.Tips("Don't have enough money !!");
            m_audio.PlayOneShot(ac_NotEnough);
            return;
        }

        WeaponManager.UpgradeWeapon(currentWeapon);
        UpdateWeaponDisplay();
        m_audio.PlayOneShot(ac_WeaponUp);
    }

    /// <summary>
    /// 子弹购买点击事件
    /// </summary>
    public void OnBuyAmmoClicked()
    {
        if (currentWeapon == null)
        {
            MessageTips.Tips("Please select weapon first!!");
            return;
        }
        if (WeaponManager.Instance.BuyAmmo(currentWeapon.Id) == -1)
        {
            MessageTips.Tips("Don't have enough money !!");
            m_audio.PlayOneShot(ac_NotEnough);
            return;
        }
        else
        {
            UpdateWeaponDisplay();
            m_audio.PlayOneShot(ac_BuyAmmo);
        }
    }

    #endregion
    #endregion
}
