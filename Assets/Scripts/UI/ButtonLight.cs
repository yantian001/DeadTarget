using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour
{

    public float interval = 5f;

    RectTransform rect;

    public void Start()
    {

        rect = GetComponent<RectTransform>();
        rect.pivot = new Vector2(0, 0.5f);
        rect.localScale = new Vector3(1, 1, 1);
        rect.anchoredPosition = new Vector2(-rect.rect.width - 50, 0);
        StartCoroutine("StartEffect");
    }

    public IEnumerator StartEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            rect.anchoredPosition = new Vector2(-rect.rect.width , 0);
            RectTransform parent = rect.parent.GetComponent<RectTransform>();
            LeanTween.moveX(rect, parent.rect.width + 50, 0.5f);
        }

    }


}
