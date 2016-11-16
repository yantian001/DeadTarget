using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{

    public Events EventId;

    public AudioClip clickClip;

    Button btn;

    public void OnEnable()
    {
        btn = GetComponent<Button>();
        if(btn)
        {
            btn.onClick.AddListener(OnButtonClick);
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
        if(btn)
        {
            btn.onClick.RemoveListener(OnButtonClick);
        }
    }
}
