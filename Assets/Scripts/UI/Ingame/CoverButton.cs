using UnityEngine;
using UnityEngine.UI;
using CnControls;

public class CoverButton : MonoBehaviour
{

    public TPSInput tpsInput = null;

    public SimpleButton btnCover;

    public RawImage img;
    // Use this for initialization
    void Start()
    {
        if (tpsInput == null)
        {
            tpsInput = GameObject.FindGameObjectWithTag("Player").GetComponent<TPSInput>();
        }
        if (!btnCover)
        {
            btnCover = GetComponent<SimpleButton>();
        }
        if(!img)
        {
            img = GetComponent<RawImage>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(tpsInput && btnCover && img)
        {
            if (tpsInput.IsAim)
            {
                btnCover.enabled = true;
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            }
            else
            {
                btnCover.enabled = false;
                img.color = new Color(img.color.r, img.color.g, img.color.b,0.5f);
            }

        }
    }
}
