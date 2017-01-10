using UnityEngine;
using System.Collections;
using System;

public class UIBuyAmmo : MonoBehaviour
{

    public Transform template;
    public Vector3 startPos;
    public Vector3 endPos;
    public float duration = 1.0f;

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.BUYAMMO, OnBuyAmmo);
        LeanTween.addListener((int)Events.NEEDAMMO, OnNeedAmmo);
    }

    private void OnNeedAmmo(LTEvent obj)
    {
        //throw new NotImplementedException();
        int cost = WeaponManager.Instance.TryBuyAmmo();
        if (cost > 0)
        {
            ShowAmmoBuyDisplay(cost);
        }
    }

    private void OnBuyAmmo(LTEvent obj)
    {
        //throw new NotImplementedException();
        if (obj.data == null)
            return;
        int price = ConvertUtil.ToInt32(obj.data);
        ShowAmmoBuyDisplay(price);
    }

    void ShowAmmoBuyDisplay(int cost)
    {
        var m = Transform.Instantiate<Transform>(template);
        m.SetParent(template.parent);
        m.localScale = template.localScale;
        m.position = startPos;
        m.localPosition = startPos;
        CommonUtils.SetChildText(m, "Label", "-" + cost.ToString());
        m.gameObject.SetActive(true);
        TweenPosition.Begin(m.gameObject, duration, endPos).SetOnFinished(() => { Destroy(m.gameObject); });
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.BUYAMMO, OnBuyAmmo);
    }


 
}
