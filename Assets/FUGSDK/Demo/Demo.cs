using UnityEngine;
using System.Collections.Generic;
public class Demo : MonoBehaviour
{
    private static List<string> delegateHistory = new List<string>();

    #region 广告
    /// <summary>
    /// 显示更多应用
    /// </summary>
    public void ShowMoreApp()
    {
        if (FUGSDK.Ads.Instance.HasMoreApp())
        {
            FUGSDK.Ads.Instance.ShowMoreApp();
        }
        else
        {
            AddLog("Dont have more game");
        }
    }
    /// <summary>
    /// 显示插页广告
    /// </summary>
    public void ShowIntersitital()
    {
        if (FUGSDK.Ads.Instance.HasIntersititial())
        {
            FUGSDK.Ads.Instance.ShowInterstitial(() =>
            {
                AddLog("Intersititial Closed");
            });
        }
        else
        {
            AddLog("Dont have intersititial ad");
        }
    }
    /// <summary>
    /// 显示视频奖励广告
    /// </summary>
    public void ShowRewardVedio()
    {
        //if (FUGSDK.Ads.Instance.HasRewardVedio())
        //{
        //    FUGSDK.Ads.Instance.ShowRewardVedio((b) =>
        //    {
        //        //视频成功播放完成后，返回 b = true,
        //        //可以在这里处理奖励
        //        if (b)
        //        {
        //            AddLog("reward vedio completed");
        //        }
        //        else
        //        {
        //            AddLog("reward vedio fail");
        //        }
        //    });
        //}
        if (FUGSDK.GoogleAdsUtil.Instance.HasRewardedVedio())
        {
            FUGSDK.GoogleAdsUtil.Instance.ShowRewardVedio(null);
        }
        else
        {
            AddLog("Dont have reward vedio");
        }
    }

    /// <summary>
    /// 显示banner条
    /// </summary>
    public void ShowBanner()
    {
        FUGSDK.Ads.Instance.ShowBanner();
    }

    #endregion

    #region 排行榜

    public void Login()
    {
        //FUGSDK.Leadbord.SocialManager.Instance.Authenticate(b =>
        //{
        //    AddLog("Authenticate :" + b);
        //});
    }

    /// <summary>
    /// 上传分数
    /// </summary>
    public void ReportScore()
    {
        //if (FUGSDK.Leadbord.SocialManager.Instance.ISAuthenticated())
        //{
        //    FUGSDK.Leadbord.SocialManager.Instance.ReportScore(100, default(string), b =>
        //    {
        //        AddLog("Report Socre " + b);
        //    });
        //}
        //else
        //{
        //    AddLog("You must call Authenticated before call any api from leadboard!");
        //}
    }

    /// <summary>
    /// 显示排行榜
    /// </summary>
    public void ShowLeadboardUI()
    {
        //if (FUGSDK.Leadbord.SocialManager.Instance.ISAuthenticated())
        //{
        //    FUGSDK.Leadbord.SocialManager.Instance.ShowLeardBoardUI();
        //}
        //else
        //{
        //    AddLog("You must call Authenticated before call any api from leadboard!");
        //}
    }

    public void LoadBanner()
    {
        FUGSDK.GoogleAdsUtil.Instance.ShowBanner(GoogleMobileAds.Api.AdPosition.Bottom, GoogleMobileAds.Api.AdSize.Banner);
    }

    public void LoadInterstitial()
    {
        FUGSDK.GoogleAdsUtil.Instance.RequestInterstitial();
    }

    public void ShowInterstitial()
    {
        FUGSDK.GoogleAdsUtil.Instance.ShowInterstital();
    }

    public void LoadRewardedVedio()
    {
        FUGSDK.GoogleAdsUtil.Instance.RequestRewardVedio();
    }

    public void LoadNative()
    {
        FUGSDK.GoogleAdsUtil.Instance.ReqeustNativeExpressAd();
    }

    public void ShowNative()
    {
        FUGSDK.GoogleAdsUtil.Instance.ShowNativeAd();
    }

    #endregion


    #region Log
    public static void AddLog(string text)
    {
        Debug.Log(text);
        delegateHistory.Insert(0, text + "\n");
        int count = delegateHistory.Count;
        if (count > 20)
        {
            delegateHistory.RemoveRange(20, count - 20);
        }
    }

    public void OnGUI()
    {
        string str = "";
        foreach (string s in delegateHistory)
        {
            str += s;
        }
        GUILayout.TextArea(str, GUILayout.Width(200), GUILayout.Height(200));
    }
    #endregion


}
