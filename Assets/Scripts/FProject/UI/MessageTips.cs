using UnityEngine;
using System.Collections;

public class MessageTips : MonoBehaviour
{

    public static MessageTips instance;
    public UILabel label;

    float disapperTime = 0;
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization

    public static void Tips(string msg)
    {
        instance._Tips(msg);
    }

    public void _Tips(string msg)
    {
        label.text = msg;
        disapperTime = 2f;
        vp_Utility.Activate(label.gameObject);

    }

    public void Update()
    {
        if (disapperTime > 0)
        {
            disapperTime -= Time.deltaTime;
            if (disapperTime <= 0)
            {
                vp_Utility.Activate(label.gameObject, false);
            }
        }
    }
}
