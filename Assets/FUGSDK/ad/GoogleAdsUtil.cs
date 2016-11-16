using UnityEngine;
using GoogleMobileAds.Api;
namespace FUGSDK
{

    public class GoogleAdsUtil : MonoBehaviour
    {

        string bannerUnitId = "";
        string intersititialId = "";

        InterstitialClosedEvent closeEvent = null;

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
        #endregion


        private void Banner_AdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            // throw new System.NotImplementedException();
            Debug.Log("Fail to load banner :" + e.Message);
        }

        #region Interstitial

        void RequestInterstitial()
        {
            if (intersititial != null)
                intersititial.Destroy();
            // Debug.Log("Start Interstitial");
            intersititial = new InterstitialAd(intersititialId);
            AdRequest request = new AdRequest.Builder().Build();
            intersititial.AdClosed += Intersititial_AdClosed;
            intersititial.LoadAd(request);
        }

        private void Intersititial_AdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            //throw new System.NotImplementedException();
            Debug.Log("Load google ads failed :" + e.Message);
        }

        private void Intersititial_AdLoaded(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            Debug.Log("Google Ads Loaded!" + e.ToString());
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

        public void OnDisable()
        {
            if (intersititial != null)
            {
                intersititial.AdClosed -= Intersititial_AdClosed;
                intersititial.Destroy();
            }
        }
    }
}
