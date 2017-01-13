using UnityEngine;
using System.Collections;
using AudienceNetwork;
public class Native : MonoBehaviour
{

    public static string nativePlacementID = "814021835405350_814023708738496";

    protected NativeAd nativeAd;

    public UILabel title;

    public UILabel socialContext;

    public UITexture coverSprite = null;

    public UITexture iconSprite = null;

    public UILabel callToAction;

    public UIButton[] callToActionButton;
    // Use this for initialization
    void Start()
    {
        nativeAd = new NativeAd(nativePlacementID);
        nativeAd.RegisterGameObjectForImpression(gameObject, callToActionButton, UICamera.currentCamera);
        nativeAd.NativeAdDidLoad = OnNativeAdDidLoad;
        nativeAd.LoadAd();
        // texture.te

    }

    void OnNativeAdDidLoad()
    {
        //Debug.Log("native ad Loaded.");
        StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
        StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
        title.text = nativeAd.Title;
        socialContext.text = nativeAd.SocialContext;
        callToAction.text = nativeAd.CallToAction;
        nativeAd.ExternalLogImpression();
    }

    public void OnGUI()
    {
        if (nativeAd != null)
        {
            if (nativeAd.IconImage)
            {
                this.iconSprite.mainTexture = nativeAd.IconImage.texture;
            }
            if (nativeAd.CoverImage)
            {
                this.coverSprite.mainTexture = nativeAd.CoverImage.texture;
            }
        }
    }
}
