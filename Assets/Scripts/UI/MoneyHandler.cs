using UnityEngine;
using System.Collections;

public class MoneyHandler : MonoBehaviour
{

    public GameObject money;

    public RectTransform parentTran;

    public float headshotMult = 1.5f;

    public int MoneyPerTime = 5;
    
    void OnEnemyDie(LTEvent evt)
    {
        if (money == null || parentTran == null)
            return;
        if(evt.data != null)
        {
            EnemyDeadInfo edi = evt.data as EnemyDeadInfo;
            if(edi != null)
            {
                int m = edi.score;
                if (edi.headShot)
                    m = Mathf.CeilToInt( m * headshotMult);
                if (Camera.main != null && Camera.main.isActiveAndEnabled)
                {
                    var pos = Camera.main.WorldToScreenPoint(edi.transform.position + new Vector3(0f, 2f, 0f));
                    var o = Instantiate(money, pos, Quaternion.identity) as GameObject;
                    o.transform.SetParent(parentTran);
                   // CommonUtils.SetText(o, string.Format("+{0}$", m));
                    CommonUtils.SetChildText(o.GetComponent<RectTransform>(), "Text", string.Format("+ {0}", m));
                }
                
                LeanTween.dispatchEvent((int)(Events.MONEYUSED), -m);
            }
        }
    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
    }
}
