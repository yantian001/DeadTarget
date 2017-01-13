using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace FUGSDK
{

    public class GoogleAdsUtil : MonoBehaviour
    {

        string bannerUnitId = "";
        string intersititialId = "";
        string rewardId = "ca-app-pub-4204987182299137/6228385601";
        string nativeId = "ca-app-pub-4204987182299137/5669982403";

        InterstitialClosedEvent closeEvent = null;
        RewardVedioClosedEvent rewardCloseEvent = null;

        public static GoogleAdsUtil Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GoogleAdsUtil>();
                    if (_instance == null)
                    {
                        GameObject container = new GameObject();
                        container.name = "Google Ads Container";
                        _instance = container.AddComponent<GoogleAdsUtil>();
                    }
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private static GoogleAdsUtil _instance;

        InterstitialAd intersititial = null;
        BannerView banner = null;

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Initialize(string banner, string interstitial)
        {
            bannerUnitId = banner;
            intersititialId = interstitial;
            RequestInterstitial();
        }

        internal void Initialize(AdsConfig gpConfig)
        {
            //throw new NotImplementedException();
            bannerUnitId = gpConfig.banner;
            intersititialId = gpConfig.interstitial;
            rewardId = gpConfig.rewarded;
            nativeId = gpConfig.native;
        }

        #region Banner
        /// <summary>
        /// 在pos显示banner广告
        /// </summary>
        /// <param name="pos">要显示banner广告的位置</param>
        public void ShowBanner(AdPosition pos, AdSize size)
        {
            RequestBanner(pos, size);
        }

        public void HideBanner()
        {
            if (banner != null)
                banner.Hide();
        }

        void RequestBanner(AdPosition pos, AdSize size)
        {
            if (banner != null)
            {
                banner.Destroy();
            }
            banner = new BannerView(bannerUnitId, size, pos);
           
            AdRequest request = new AdRequest.Builder().Build();
            banner.LoadAd(request);
            // banner.
        }

        private void Banner_AdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            // throw new System.NotImplementedException();
            Debug.Log("Fail to load banner :" + e.Message);
        }

        #endregion

        #region Interstitial

        public void RequestInterstitial()
        {
            Demo.AddLog("***********************\n Request Interstitial \n**********************");
            if (intersititial != null)
                intersititial.Destroy();
            // Debug.Log("Start Interstitial");
            intersititial = new InterstitialAd(intersititialId);
            AdRequest request = new AdRequest.Builder()
                        .Build();
            intersititial.OnAdClosed += Intersititial_AdClosed;
            intersititial.OnAdFailedToLoad += Intersititial_OnAdFailedToLoad;

            // intersititial.AdClosed += Intersititial_AdClosed;
            intersititial.LoadAd(request);
        }



        private void Intersititial_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n Request Interstitial Error:" + e.Message + "\n**********************");

        }

        private void Intersititial_AdClosed(object sender, System.EventArgs e)
        {
            if (intersititial != null)
            {
                intersititial.Destroy();
            }
            if (closeEvent != null)
            {
                closeEvent();
                closeEvent = null;
            }

            RequestInterstitial();
        }
        private void Intersititial_AdLoaded(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            Debug.Log("Google Ads Loaded!" + e.ToString());
            Demo.AddLog("***********************\n Interstitial Loaded! \n**********************");
        }



        public bool HasInterstital()
        {
            if (intersititial != null && intersititial.IsLoaded())
                return true;
            RequestInterstitial();
            return false;
        }

        /// <summary>
        /// 显示广告
        /// </summary>
        public void ShowInterstital()
        {
            if (HasInterstital())
                intersititial.Show();
        }

        /// <summary>
        ///  显示广告,并在广告关闭时执行ev方法
        /// </summary>
        /// <param name="ev">广告关闭时执行的方法</param>
        public void ShowInterstital(InterstitialClosedEvent ev)
        {
            if (HasInterstital())
            {
                intersititial.Show();
                closeEvent = ev;
            }
        }



        #endregion

        #region Rewarded Vedio
        bool rewardedLoaded = false;
        /// <summary>
        /// 请求奖励广告
        /// </summary>
        public void RequestRewardVedio()
        {
            rewardedLoaded = false;
            Demo.AddLog("***********************\n Request Rewarded Vedio \n**********************");
            AdRequest request = new AdRequest.Builder().Build();
            RewardBasedVideoAd.Instance.OnAdLoaded += Instance_OnAdLoaded;
            RewardBasedVideoAd.Instance.OnAdFailedToLoad += Instance_OnAdFailedToLoad;
            RewardBasedVideoAd.Instance.OnAdOpening += Instance_OnAdOpening;
            RewardBasedVideoAd.Instance.OnAdStarted += Instance_OnAdStarted;
            RewardBasedVideoAd.Instance.OnAdClosed += Instance_OnAdClosed;

            RewardBasedVideoAd.Instance.OnAdRewarded += Instance_OnAdRewarded;
            RewardBasedVideoAd.Instance.LoadAd(request, rewardId);

        }

        private void Instance_OnAdRewarded(object sender, Reward e)
        {
            //throw new NotImplementedException();
            if (rewardCloseEvent != null)
            {
                rewardCloseEvent(true);
                rewardCloseEvent = null;
            }
        }

        public bool HasRewardedVedio()
        {
            return rewardedLoaded;
            //return RewardBasedVideoAd.Instance.IsLoaded();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 显示奖励广告
        /// </summary>
        public bool ShowRewardVedio(RewardVedioClosedEvent ev)
        {
            if (HasRewardedVedio())
            {
                rewardCloseEvent = ev;
                RewardBasedVideoAd.Instance.Show();
                return true;
            }
            return false;

        }

        private void Instance_OnAdClosed(object sender, System.EventArgs e)
        {
            Demo.AddLog("***********************\n Reward Vedio Closed \n**********************");
            if (rewardCloseEvent != null)
            {
                rewardCloseEvent(false);
            }
            RequestRewardVedio();
        }

        private void Instance_OnAdStarted(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n Rewarded Vedio Start \n**********************");
        }

        private void Instance_OnAdOpening(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n Rewarded Vedio Opening \n**********************");
        }

        private void Instance_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n Rewarded Vedio " + e.Message + " \n**********************");
        }

        private void Instance_OnAdLoaded(object sender, System.EventArgs e)
        {
            rewardedLoaded = true;
            Demo.AddLog("***********************\n Rewarded Vedio Loaded! \n**********************");
            //throw new System.NotImplementedException();
        }

        #endregion

        #region Native Vedio
        private NativeExpressAdView nativeAdView;
        public void ReqeustNativeExpressAd()
        {
            Demo.AddLog("***********************\n Request NativeAd! \n**********************");
            if (nativeAdView != null) 
            {
                nativeAdView.Destroy();

            }
            nativeAdView = new NativeExpressAdView(nativeId, new AdSize(320, 300), AdPosition.Top);
            nativeAdView.OnAdClosed += NativeAdView_OnAdClosed;
            nativeAdView.OnAdFailedToLoad += NativeAdView_OnAdFailedToLoad;
            nativeAdView.OnAdLeavingApplication += NativeAdView_OnAdLeavingApplication;
            nativeAdView.OnAdLoaded += NativeAdView_OnAdLoaded;
            nativeAdView.OnAdOpening += NativeAdView_OnAdOpening;
            //ativeAdView.LoadAd()
            nativeAdView.LoadAd(new AdRequest.Builder().Build());

        }

        public void ShowNativeAd()
        {
            nativeAdView.Show();
        }

        private void NativeAdView_OnAdOpening(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n NativeAd Opening! \n**********************");
        }

        private void NativeAdView_OnAdLoaded(object sender, System.EventArgs e)
        {
            // throw new System.NotImplementedException();
            Demo.AddLog("***********************\n NativeAd Loaded! \n**********************");

        }

        private void NativeAdView_OnAdLeavingApplication(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n NativeAd Leaving Application! \n**********************");

        }

        private void NativeAdView_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            //throw new System.NotImplementedException();
            Demo.AddLog("***********************\n NativeAd Failed To Load " + e.Message + "! \n**********************");

        }

        private void NativeAdView_OnAdClosed(object sender, System.EventArgs e)
        {
            Demo.AddLog("***********************\n NativeAd Closed! \n**********************");

        }

        #endregion

        public void OnDisable()
        {
            if (intersititial != null)
            {
                intersititial.OnAdClosed -= Intersititial_AdClosed;
                intersititial.Destroy();
            }
        }
    }
}
