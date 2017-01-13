using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using FProject;

public class GameLogic : MonoBehaviour
{


    /// <summary>
    /// Loading界面
    /// </summary>
    public string s_LoadingScene = "Loading";
    public string s_MenuScene = "Menu";
    public string s_StartScene = "Start";
    public string s_ShopScene = "Shop";
    public string s_iOSId = "1041667864";

    private static GameLogic _logic = null;

    public static GameLogic Instance
    {
        get
        {
            if (_logic == null)
            {
                _logic = FindObjectOfType<GameLogic>();
                if (_logic == null)
                {
                    GameObject logicContainer = new GameObject();
                    logicContainer.name = "GameLogicContainer";
                    _logic = logicContainer.AddComponent<GameLogic>();
                }
            }
            return _logic;
        }
    }

    void Awake()
    {
        if (_logic == null)
        {
            _logic = this;
            DontDestroyOnLoad(gameObject);
            LeanTween.addListener((int)Events.GAMERESTART, OnGameRestart);
            LeanTween.addListener((int)Events.MAINMENU, OnGameMainMenu);
            LeanTween.addListener((int)Events.BACKTOSTART, BackToStart);
            LeanTween.addListener((int)Events.GAMENEXT, OnGameNext);
            LeanTween.addListener((int)Events.GAMESTART, OnGameStart);
            LeanTween.addListener((int)Events.GAMEQUIT, OnGameQuit);
            LeanTween.addListener((int)Events.GAMERATE, OnGameRate);
            LeanTween.addListener((int)Events.SHOP, OnShop);
            LeanTween.addListener((int)Events.GAMEMORE, OnGameMore);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnShop(LTEvent obj)
    {
        //throw new NotImplementedException();
        Application.LoadLevel(s_ShopScene);
    }

    private void OnGameRate(LTEvent obj)
    {
        //  throw new NotImplementedException();
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.bundleIdentifier);
#elif UNITY_IOS
       Application.OpenURL("itms-apps://itunes.apple.com/app/id" + s_iOSId);
#endif
    }

    private void OnGameMore(LTEvent obj)
    {
        // Debug.Log("GameMore");
        // throw new NotImplementedException();
        //  ChartboostUtil.Instance.ShowMoreAppOnDefault();
        FUGSDK.Ads.Instance.ShowMoreApp();
    }

    private void OnGameStart(LTEvent obj)
    {
        DSystem.s_CurrentSceneName = DSystem.Instance.sceneName;
        Loading(false);
    }

    void OnGameRestart(LTEvent evt)
    {
        // Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnGameNext(LTEvent evt)
    {
        OnGameMainMenu(evt);
        //if (DSystem.IsMapLastLevel(DSystem.mapId, DSystem.level))
        //    OnGameMainMenu(evt);
        //else
        //{
        //    DSystem.level += 1;
        //    Application.LoadLevel(Application.loadedLevel);
        //}
    }


    void OnGameMainMenu(LTEvent evt)
    {

        DSystem.s_CurrentSceneName = s_MenuScene;
        bool isShowLoading = false;
        if (evt.data != null)
        {
            bool.TryParse(evt.data.ToString(), out isShowLoading);
        }
        Loading(isShowLoading);
    }

    // Update is called once per frame
    public void OnDestroy()
    {
        // Debug.Log("OnDestroy");
    }

    public void OnDisable()
    {
        // Debug.Log("OnDisable");
        //LeanTween.removeListener((int)Events.GAMERESTART, OnGameRestart);
        // LeanTween.removeListener((int)Events.GAMESTART, OnGameStart);
        //LeanTween.removeListener((int)Events.MAINMENU, OnGameMainMenu);
        //LeanTween.removeListener((int)Events.BACKTOSTART, BackToStart);
        //LeanTween.removeListener((int)Events.GAMENEXT, OnGameNext);
    }

    public void Loading(bool showLoading = true)
    {
        //DSystem.s_CurrentSceneName = DSystem.GetMapSceneName();
        if (showLoading)
            Application.LoadLevel(s_LoadingScene);
        else
        {
            Application.LoadLevel(DSystem.s_CurrentSceneName);
        }

    }


    public void BackToStart(LTEvent evt)
    {
        // GameGlobalValue.s_CurrentScene = 0;
        DSystem.s_CurrentSceneName = s_StartScene;
        Loading(false);
    }

    void Update()
    {
        string levelIndex = Application.loadedLevelName;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (levelIndex == s_StartScene)
            {
                OnGameQuit(null);

            }
            else if (levelIndex == s_ShopScene)
            {
                OnGameMainMenu(null);
            }
            else if (levelIndex == s_MenuScene)
            {
                BackToStart(null);
            }
            else if (levelIndex == s_LoadingScene)
            {

            }
            else
            {
                //if (DSystem.staus == GameStatu.InGame)
                //{
                //    LeanTween.dispatchEvent((int)Events.GAMEPAUSE);
                //}
                //else if (DSystem.staus == GameStatu.Paused)
                //{
                //    LeanTween.dispatchEvent((int)Events.GAMECONTINUE);
                //}
                //else if (DSystem.staus == GameStatu.Completed || DSystem.staus == GameStatu.Failed)
                //{
                //    OnGameMainMenu(null);
                //}
            }
        }
    }

    void OnGameQuit(LTEvent evt)
    {
        if (FUGSDK.Ads.Instance.HasIntersititial())
        {
            FUGSDK.Ads.Instance.ShowInterstitial(OnInterstitialClosed);
        }
        else
        {
            Application.Quit();
        }
    }

    public void OnInterstitialClosed()
    {
        Application.Quit();
    }

}
