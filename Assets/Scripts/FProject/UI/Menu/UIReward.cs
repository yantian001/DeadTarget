using UnityEngine;
using System.Collections;

public class UIReward : MonoBehaviour
{

    public GameObject rewardButton;

    public GameObject showRewardZone;

    // Update is called once per frame
    void Update()
    {
        if (FUGSDK.Ads.Instance.HasRewardVedio())
        {
            vp_Utility.Activate(rewardButton);
        }
        else
        {
            vp_Utility.Activate(rewardButton, false);
        }
    }

    public void OnRewardButtonClicked()
    {
        FUGSDK.Ads.Instance.ShowRewardVedio(OnRewardVedioCompleted);
    }

    public void OnRewardVedioCompleted(bool completed)
    {
        if (completed)
        {
            Player.CurrentUser.UseMoney(-500);
            showRewardZone.SetActive(true);
            vp_Timer.In(3f, () => { showRewardZone.SetActive(false); });
        }
    }

    public void OnOKClicked()
    {
        showRewardZone.SetActive(false);
    }
}
