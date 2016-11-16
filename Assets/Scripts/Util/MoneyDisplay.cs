using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{

    Text text;

    int currentDisplay = 0;

    int addPerTime = 10;

    int addTo = 0;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        currentDisplay = Player.CurrentUser.Money;
        if (text)
        {
            text.text = Player.CurrentUser.Money.ToString();
        }
    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.MONEYCHANGED, OnMoneyChanged);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.MONEYCHANGED, OnMoneyChanged);
    }

    void OnMoneyChanged(LTEvent evt)
    {
        addTo = Player.CurrentUser.Money;
        StartCoroutine(DynamicDisplayMoeny(addTo, currentDisplay, addPerTime));
        // currentDisplay = addTo;
    }

    IEnumerator DynamicDisplayMoeny(int to, int from = 0, int time = 5)
    {
        if (text != null)
        {
            int diff = to - from;
            int normal = Mathf.CeilToInt((float)diff / time);
            while (from != to)
            {
                if (Mathf.Abs(to - from) >= Mathf.Abs(normal))
                {
                    from += normal;
                }
                else
                    from = to;

                text.text = from.ToString();

                yield return null;
            }
        }
        currentDisplay = to;
    }


}
