using UnityEngine;
using GoogleMobileAds.Api;

namespace FUGSDK
{
    public delegate void InterstitialClosedEvent();
    public delegate void RewardVedioClosedEvent(bool completed);
    [System.Serializable]
    public class AdsConfig
    {
        public string banner = "ca-app-pub-4204987182299137/3274919206";
        public string interstitial = "ca-app-pub-4204987182299137/4751652407";
        public string rewarded = "ca-app-pub-4204987182299137/6228385601";
        public string native = "ca-app-pub-4204987182299137/5669982403";
    }

    public class Ads : MonoBehaviour
    {
        #region GooglePlay
        public AdsConfig googlePlayConfig;
        #endregion
        #region iOS
        public AdsConfig iOSConfig;
        #endregion
        private static Ads _instance = null;
        public static Ads Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Ads>();
                    if (_instance == null)
                    {
                        GameObject o = new GameObject("AdsContainer");
                        _instance = o.AddComponent<Ads>();
                    }
                }
                return _instance;
            }
            private set
            {

            }
        }

        //public bool cacheBanner = false;
        public bool cacheInterstitial = false;
        public bool cacheRewarded = false;
        public bool cacheNative = false;

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


        // Use this for initialization
        void Start()
        {
            ChartboostUtil.Instance.Initialize();
#if UNITY_ANDROID
            GoogleAdsUtil.Instance.Initialize(googlePlayConfig);
#elif UNITY_IPHONE
            GoogleAdsUtil.Instance.Initialize(iOSConfig);
#endif
            if (cacheInterstitial)
            {
                GoogleAdsUtil.Instance.RequestInterstitial();
            }
            if (cacheRewarded)
            {
                GoogleAdsUtil.Instance.RequestRewardVedio();
            }
            if (cacheNative)
            {
                GoogleAdsUtil.Instance.ReqeustNativeExpressAd();
            }
        }
        /// <summary>
        /// 在位置p上显示横幅广告
        /// </summary>
        /// <param name="p">显示横幅的位置，默认是在中下</param>
        public void ShowBanner(AdPosition p = AdPosition.Bottom)
        {
            GoogleAdsUtil.Instance.ShowBanner(p, AdSize.Banner);
        }

        public void HideBanner()
        {
            GoogleAdsUtil.Instance.HideBanner();
        }

        #region 插页广告
        /// <summary>
        /// 显示插页广告，并在插页广告关闭后执行closeEvent方法，不需要判断是否有广告
        /// </summary>
        /// <param name="closeEvent">广告关闭时执行的方法，默认为null</param>
        public void ShowInterstitial(InterstitialClosedEvent closeEvent = null)
        {

            if (GoogleAdsUtil.Instance.HasInterstital())
            {
                GoogleAdsUtil.Instance.ShowInterstital(closeEvent);
            }
            //else if (ChartboostUtil.Instance.HasInterstitialOnDefault())
            //{
            //    ChartboostUtil.Instance.ShowInterstitialOnDefault(closeEvent);
            //}
        }

        /// <summary>
        /// 是否有插页广告
        /// </summary>
        /// <returns></returns>
        public bool HasIntersititial()
        {
            return GoogleAdsUtil.Instance.HasInterstital();// || ChartboostUtil.Instance.HasInterstitialOnDefault();
        }
        #endregion

        #region 视频奖励广告
        /// <summary>
        /// 是否有奖励广告
        /// </summary>
        /// <returns></returns>
        public bool HasRewardVedio()
        {
            //return false;
            return GoogleAdsUtil.Instance.HasRewardedVedio();//|| ChartboostUtil.Instance.HasGameOverVideo();
            // return ChartboostUtil.Instance.HasGameOverVideo();
        }
        /// <summary>
        /// 播放视频奖励广告，并在广告播放完后执行ev方法，
        /// 
        /// </summary>
        /// <param name="ev"></param>
        public void ShowRewardVedio(RewardVedioClosedEvent ev)
        {
            if (!GoogleAdsUtil.Instance.ShowRewardVedio(ev))
            {
                //if (!ChartboostUtil.Instance.ShowGameOverVideo(ev))
                //{
                ev(false);
                //}
            }

        }
        #endregion

        #region More Game

        /// <summary>
        /// 是否有“更多游戏”广告
        /// </summary>
        /// <returns></returns>
        public bool HasMoreApp()
        {
            //return false;
            return ChartboostUtil.Instance.HasMoreAppOnDefault();
        }


        /// <summary>
        /// 显示更多游戏
        /// </summary>
        public void ShowMoreApp()
        {
            ChartboostUtil.Instance.ShowMoreAppOnDefault();
        }
        #endregion
    }
}