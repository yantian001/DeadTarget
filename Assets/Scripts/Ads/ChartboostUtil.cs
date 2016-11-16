using UnityEngine;
using System.Collections;
using ChartboostSDK;
using System;

public class ChartboostUtil : MonoBehaviour
{

    static ChartboostUtil _instance = null;
    
    public static ChartboostUtil Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ChartboostUtil>();
                if (_instance == null)
                {
                    GameObject signton = new GameObject();
                    signton.name = "Chartboost Container";
                    _instance = signton.AddComponent<ChartboostUtil>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
            LeanTween.addListener((int)Events.GAMEPAUSE, OnGamePause);
            LeanTween.addListener((int)Events.MENULOADED, OnGameMenu);
            LeanTween.addListener((int)Events.GAMEMORE, OnGameMore);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnGameMore(LTEvent obj)
    {
        //  throw new NotImplementedException();
        //this.ShowMoreAppOnDefault();
        FUGSDK.Ads.Instance.ShowMoreApp();
    }

    private void OnGameMenu(LTEvent obj)
    {
        //throw new NotImplementedException();
        // this.ShowInterstitialOnHomescreen();
        FUGSDK.Ads.Instance.ShowInterstitial();
    }

    private void OnGamePause(LTEvent obj)
    {
        //throw new NotImplementedException();
        // this.ShowInterstitialOnDefault();
        FUGSDK.Ads.Instance.ShowInterstitial();
    }

    private void OnGameFinish(LTEvent obj)
    {
        //throw new NotImplementedException();
        // this.ShowInterstitialOnDefault();
        FUGSDK.Ads.Instance.ShowInterstitial();
    }

    // Use this for initialization


    //void Start()
    //{

    //    Chartboost.setAutoCacheAds(true);


    //    Chartboost.cacheInterstitial(CBLocation.Default);
    //    Chartboost.cacheInterstitial(CBLocation.HomeScreen);
    //    Chartboost.cacheInterstitial(CBLocation.Quit);
    //    //Chartboost.cacheRewardedVideo(CBLocation.Default);
    //    Chartboost.cacheMoreApps(CBLocation.Default);
    //    Chartboost.cacheMoreApps(CBLocation.Settings);

    //    Chartboost.didDismissRewardedVideo += Chartboost_didDismissRewardedVideo;
    //    Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;
    //    Chartboost.didFailToLoadRewardedVideo += Chartboost_didFailToLoadRewardedVideo;
    //    Chartboost.didDisplayRewardedVideo += Chartboost_didDisplayRewardedVideo;
    //    Chartboost.didFailToLoadMoreApps += Chartboost_didFailToLoadMoreApps;
    //    Chartboost.didCacheMoreApps += Chartboost_didCacheMoreApps;

    //    Chartboost.didCloseInterstitial += Chartboost_didCloseInterstitial;
    //    // Chartboost.showInterstitial(CBLocation.HomeScreen);
    //    //Chartboost.showRewardedVideo(CBLocation.Default);
    //}

    //private void Chartboost_didCloseInterstitial(CBLocation obj)
    //{
    //    //throw new System.NotImplementedException();
    //    LeanTween.dispatchEvent((int)Events.INTERSTITIALCLOSED);
    //    Chartboost.cacheInterstitial(obj);
    //}

    //private void Chartboost_didCacheMoreApps(CBLocation obj)
    //{
    //    // throw new System.NotImplementedException();
    //    Debug.Log("More app cached ");
    //}

    //private void Chartboost_didFailToLoadMoreApps(CBLocation arg1, CBImpressionError arg2)
    //{
    //    //throw new System.NotImplementedException();
    //    Debug.Log("Load more apps failed :" + arg2);
    //}

    //private void Chartboost_didDisplayRewardedVideo(CBLocation obj)
    //{
    //    //throw new System.NotImplementedException();
    //    Debug.Log("Show video ads at :" + obj);
    //}

    //private void Chartboost_didFailToLoadRewardedVideo(CBLocation arg1, CBImpressionError arg2)
    //{
    //    // throw new System.NotImplementedException();

    //    Debug.Log("Load rewarded video failed");
    //}

    //private void Chartboost_didCompleteRewardedVideo(CBLocation arg1, int arg2)
    //{

    //    Debug.Log("Chartboost_didCompleteRewardedVideo");
    //    //  LeanTween.dispatchEvent((int)Events.VIDEOREWARD);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="obj"></param>
    //private void Chartboost_didDismissRewardedVideo(CBLocation obj)
    //{
    //    Debug.Log("Chartboost_didDismissRewardedVideo");
    //    LeanTween.dispatchEvent((int)Events.VIDEOCLOSED);
    //}

    ///// <summary>
    ///// 是否有游戏结束时的视频广告
    ///// </summary>
    ///// <returns></returns>
    //public bool HasGameOverVideo()
    //{
    //    return Chartboost.hasRewardedVideo(CBLocation.Default);
    //}
    ///// <summary>
    ///// 显示游戏结束时的奖励视频广告
    ///// </summary>
    //public void ShowGameOverVideo()
    //{
    //    if (Chartboost.hasRewardedVideo(CBLocation.Default))
    //    {
    //        Chartboost.showRewardedVideo(CBLocation.Default);
    //    }

    //}

    ///// <summary>
    ///// 是否有退出广告
    ///// </summary>
    ///// <returns></returns>
    //public bool HasQuitInterstitial()
    //{
    //    return Chartboost.hasInterstitial(CBLocation.Quit);

    //}

    ///// <summary>
    ///// 显示退出广告
    ///// </summary>
    //public void ShowQuitInterstitial()
    //{
    //    if (HasQuitInterstitial())
    //    {
    //        Chartboost.showInterstitial(CBLocation.Quit);
    //    }
    //}

    //public bool HasInterstitialOnDefault()
    //{
    //    return Chartboost.hasInterstitial(CBLocation.Default);
    //}

    //public void ShowInterstitialOnDefault()
    //{
    //    Chartboost.showInterstitial(CBLocation.Default);
    //}

    //public bool HasInterstitialOnHomescreen()
    //{
    //    return Chartboost.hasInterstitial(CBLocation.HomeScreen);

    //}

    //public void ShowInterstitialOnHomescreen()
    //{
    //    if (HasInterstitialOnHomescreen())
    //        Chartboost.showInterstitial(CBLocation.HomeScreen);
    //}


    //public bool HasMoreAppOnLoading()
    //{
    //    return Chartboost.hasMoreApps(CBLocation.Settings);
    //}

    //public void ShowMoreAppOnLoading()
    //{
    //    if (HasMoreAppOnLoading())
    //    {
    //        Chartboost.showMoreApps(CBLocation.Settings);
    //    }
    //}


    //public bool HasMoreAppOnDefault()
    //{
    //    if (Chartboost.hasMoreApps(CBLocation.Default))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //public void ShowMoreAppOnDefault()
    //{
    //    if (HasMoreAppOnDefault())
    //    {
    //        Chartboost.showMoreApps(CBLocation.Default);
    //    }
    //}

}
