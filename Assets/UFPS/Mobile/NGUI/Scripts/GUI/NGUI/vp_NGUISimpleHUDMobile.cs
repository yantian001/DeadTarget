/////////////////////////////////////////////////////////////////////////////////
//
//	vp_NGUISimpleHUDMobile.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	a version of the vp_SimpleHUD with a classic mobile FPS layout
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class vp_NGUISimpleHUDMobile : vp_SimpleHUDMobile
{

    protected UILabel m_AmmoLabelSprite = null;
    protected UILabel m_HealthLabelSprite = null;
    protected UILabel m_HintsLabelSprite = null;
    public UISprite m_HealthSpirte = null;

    /// <summary>
    ///
    /// </summary>
    protected override void Awake()
    {

        base.Awake();

        if (AmmoLabel != null) m_AmmoLabelSprite = AmmoLabel.GetComponentInChildren<UILabel>();
        if (HealthLabel != null) m_HealthLabelSprite = HealthLabel.GetComponentInChildren<UILabel>();
        if (HintsLabel != null) m_HintsLabelSprite = HintsLabel.GetComponentInChildren<UILabel>();
        //if (HealthLabel != null) m_HealthSpirte = HealthLabel.GetComponentInChildren<UISprite>();

        if (m_HintsLabelSprite != null)
        {
            m_HintsLabelSprite.text = "";
            m_HintsLabelSprite.color = Color.clear;
        }
    }


    /// <summary>
    /// just here to mute inherited 'desktop' HUD drawing
    /// </summary>
    protected override void OnGUI()
    {


    }


    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {

        if (m_AmmoLabelSprite != null)
        {
            if (m_PlayerEventHandler.CurrentWeaponIndex.Get() > 0)
            {
                m_AmmoLabelSprite.text = (m_PlayerEventHandler.CurrentWeaponAmmoCount.Get() + "/" +
                (m_PlayerEventHandler.CurrentWeaponClipCount.Get())).ToString();
            }
            else
                m_AmmoLabelSprite.text = "0/0";
        }

        if (m_HealthLabelSprite != null)
            m_HealthLabelSprite.text = (float)m_Health / m_MaxHealth * 100 + "%";
        if (m_HealthSpirte)
            m_HealthSpirte.fillAmount = (float)m_Health / m_MaxHealth;

    }


    /// <summary>
    /// updates the HUD message text and makes it fully visible
    /// </summary>
    protected override void OnMessage_HUDText(string message)
    {

        if (!ShowTips || m_HintsLabelSprite == null)
            return;

        m_PickupMessageMobile = (string)message;
        m_HintsLabelSprite.text = m_PickupMessageMobile;
        vp_NGUITween.ColorTo(m_HintsLabelSprite, Color.white, .25f, m_HUDTextTweenHandle, delegate
        {
            vp_NGUITween.ColorTo(m_HintsLabelSprite, m_InvisibleColorMobile, FadeDuration, m_HUDTextTweenHandle);
        });

    }


}

