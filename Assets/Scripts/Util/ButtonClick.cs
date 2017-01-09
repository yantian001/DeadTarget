using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(UIButton))]
public class ButtonClick : MonoBehaviour
{

    public Events EventId;

    public AudioClip clickClip;

    UIButton btn;

    EventDelegate clickDelegate = null;

    public void Awake()
    {
        clickDelegate = new EventDelegate(this.OnButtonClick);
    }

    public void OnEnable()
    {
        btn = GetComponent<UIButton>();
        if (btn)
        {
            btn.onClick.Add(clickDelegate);
        }

        // StartCoroutine(CoroutineEffect());
    }


    //IEnumerator CoroutineEffect()
    //{
    //    if (effectTexture)
    //    {
    //        while (true)
    //        {
    //            yield return new WaitForSeconds(effectInterv);
    //            var effect = Instantiate(effectTexture);
    //            //effect.GetComponent<RectTransform>().SetParent(transform.GetComponent<RectTransform>());
    //          //  effect.GetComponent<ButtonLight>().StartEffect();
    //        }
    //    }

    //}


    void OnButtonClick()
    {
        if (clickClip)
        {
            LeanAudio.play(clickClip);
            Invoke("DelayCall", .2f);
        }
        else
        {
            DelayCall();
        }

        //  Debug.Log("button Clicked");
    }

    void DelayCall()
    {
        if (EventId != Events.NONE)
        {
            LeanTween.dispatchEvent((int)EventId);
        }

    }

    public void OnDisable()
    {
        if (btn)
        {
            btn.onClick.Remove(clickDelegate);
        }
    }
}
