using UnityEngine;
using ChartboostSDK;

namespace FUGSDK
{

    public class ChartboostUtil : MonoBehaviour
    {
        public InterstitialClosedEvent closedEvent;
        public RewardVedioClosedEvent rewardClosedEvent;

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

        public void Initialize()
        {

        }

        void Awake()
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
        //// Use this for initialization
        void Start()
        {
            var chartboost = FindObjectOfType<Chartboost>();
            if (!chartboost)
            {
                GameObject o = new GameObject("Chartboost");
                chartboost = o.AddComponent<Chartboost>();
            }
            //Chartboost.didDismissRewardedVideo += Chartboost_didDismissRewardedVideo;
            //Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;
            //Chartboost.didFailToLoadRewardedVideo += Chartboost_didFailToLoadRewardedVideo;
            //Chartboost.didDisplayRewardedVideo += Chartboost_didDisplayRewardedVideo;
            //Chartboost.didFailToLoadMoreApps += Chartboost_didFailToLoadMoreApps;
            //Chartboost.didCacheMoreApps += Chartboost_didCacheMoreApps;
            //Chartboost.didCloseInterstitial += Chartboost_didCloseInterstitial;
            //Chartboost.setAutoCacheAds(true);
            //Chartboost.cacheInterstitial(CBLocation.Default);
            //Chartboost.cacheInterstitial(CBLocation.HomeScreen);
            //Chartboost.cacheInterstitial(CBLocation.Quit);
            //Chartboost.cacheRewardedVideo(CBLocation.Default);
            Chartboost.cacheMoreApps(CBLocation.Default);

        }

        /// <summary>
        /// 插页广告关闭后回掉
        /// </summary>
        /// <param name="obj"></param>
        private void Chartboost_didCloseInterstitial(CBLocation obj)
        {
            if (closedEvent != null)
            {
                closedEvent();
                closedEvent = null;
            }
            Chartboost.cacheInterstitial(obj);
        }

        private void Chartboost_didCacheMoreApps(CBLocation obj)
        {
            // throw new System.NotImplementedException();
            Debug.Log("More app cached ");
        }

        private void Chartboost_didFailToLoadMoreApps(CBLocation arg1, CBImpressionError arg2)
        {
            //throw new System.NotImplementedException();
            Debug.Log("Load more apps failed :" + arg2);
        }

        private void Chartboost_didDisplayRewardedVideo(CBLocation obj)
        {
            //throw new System.NotImplementedException();
            Debug.Log("Show video ads at :" + obj);
        }

        private void Chartboost_didFailToLoadRewardedVideo(CBLocation arg1, CBImpressionError arg2)
        {
            // throw new System.NotImplementedException();
            DoAfterVedio();
            Debug.Log("Load rewarded video failed");
        }

        private void Chartboost_didCompleteRewardedVideo(CBLocation arg1, int arg2)
        {

            Debug.Log("Chartboost_didCompleteRewardedVideo");
            DoAfterVedio(true);
            //  LeanTween.dispatchEvent((int)Events.VIDEOREWARD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void Chartboost_didDismissRewardedVideo(CBLocation obj)
        {
            DoAfterVedio(false);
            Debug.Log("Chartboost_didDismissRewardedVideo");
        }

        /// <summary>
        /// 是否有游戏结束时的视频广告
        /// </summary>
        /// <returns></returns>
        public bool HasGameOverVideo()
        {
            if (Chartboost.hasRewardedVideo(CBLocation.Default))
            {
                return true;
            }
            Chartboost.cacheRewardedVideo(CBLocation.Default);
            return false;
        }
        /// <summary>
        /// 显示游戏结束时的奖励视频广告
        /// </summary>
        public void ShowGameOverVideo()
        {
            if (Chartboost.hasRewardedVideo(CBLocation.Default))
            {
                Chartboost.showRewardedVideo(CBLocation.Default);
            }
        }

        /// <summary>
        /// 显示游戏结束时的奖励视频广告
        /// </summary>
        public void ShowGameOverVideo(RewardVedioClosedEvent ev)
        {
            rewardClosedEvent = ev;
            if (Chartboost.hasRewardedVideo(CBLocation.Default))
            {
                Chartboost.showRewardedVideo(CBLocation.Default);
            }
            else
            {
                Chartboost.cacheRewardedVideo(CBLocation.Default);
                DoAfterVedio();
            }

        }
        /// <summary>
        /// 视频广告后事件
        /// </summary>
        /// <param name="b"></param>
        void DoAfterVedio(bool b = false)
        {
            if (rewardClosedEvent != null)
            {
                rewardClosedEvent(b);
                rewardClosedEvent = null;
            }
        }
        /// <summary>
        /// 是否有退出广告
        /// </summary>
        /// <returns></returns>
        public bool HasQuitInterstitial()
        {
            return Chartboost.hasInterstitial(CBLocation.Quit);
        }

        /// <summary>
        /// 显示退出广告
        /// </summary>
        public void ShowQuitInterstitial()
        {
            if (HasQuitInterstitial())
            {
                Chartboost.showInterstitial(CBLocation.Quit);
            }
        }

        public bool HasInterstitialOnDefault()
        {
            if (Chartboost.hasInterstitial(CBLocation.Default))
            {
                return true;
            }
            Chartboost.cacheInterstitial(CBLocation.Default);
            return false;
        }

        public void ShowInterstitialOnDefault()
        {
            Chartboost.showInterstitial(CBLocation.Default);
        }

        /// <summary>
        /// 显示chartboost默认位置广告，并在广告关闭后执行ev方法
        /// </summary>
        /// <param name="ev">并在广告关闭后执行的方法</param>
        public void ShowInterstitialOnDefault(InterstitialClosedEvent ev)
        {
            Chartboost.showInterstitial(CBLocation.Default);
            closedEvent = ev;
        }

        public bool HasInterstitialOnHomescreen()
        {
            return Chartboost.hasInterstitial(CBLocation.HomeScreen);

        }

        public void ShowInterstitialOnHomescreen()
        {
            if (HasInterstitialOnHomescreen())
                Chartboost.showInterstitial(CBLocation.HomeScreen);
        }


        public bool HasMoreAppOnLoading()
        {
            return Chartboost.hasMoreApps(CBLocation.Settings);
        }

        public void ShowMoreAppOnLoading()
        {
            if (HasMoreAppOnLoading())
            {
                Chartboost.showMoreApps(CBLocation.Settings);
            }
        }


        public bool HasMoreAppOnDefault()
        {
            if (Chartboost.hasMoreApps(CBLocation.Default))
            {
                return true;
            }
            return false;
        }

        public void ShowMoreAppOnDefault()
        {
            if (HasMoreAppOnDefault())
            {
                Chartboost.showMoreApps(CBLocation.Default);
            }
        }

    }
}
