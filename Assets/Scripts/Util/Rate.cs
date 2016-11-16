using UnityEngine;
using System.Collections;

public class Rate : MonoBehaviour
{
    public string bundleId;

    public void OnRateClicked()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + bundleId);
    #endif
    }
}
