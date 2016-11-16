using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{


    public bool isComboShow = false;

    public GameManager gm;

    RectTransform rect;

    public Text comboText;

    public Image slider;

    public float moveTime = 0.2f;

    int currentCombo;
    Vector3 localscale;
    // Use this for initialization
    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        rect = GetComponent<RectTransform>();
        if (!comboText)
        {
            comboText = CommonUtils.GetChildComponent<Text>(rect, "Text");
           
        }
        localscale = comboText.rectTransform.localScale;
        if (!slider)
        {
            slider = CommonUtils.GetChildComponent<Image>(rect, "slider");
        }

        rect.anchoredPosition = rect.anchoredPosition + new Vector2(500, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (gm.isCombo)
        {
            if (!isComboShow && gm.currentCombo > 1)
            {
                MoveIn();
                isComboShow = true;
            }
            if (isComboShow)
            {
                if(currentCombo != gm.currentCombo)
                {
                    
                    LeanTween.scale(comboText.rectTransform, localscale * 1.5f, 0.05f).setOnComplete(()=> {
                        currentCombo = gm.currentCombo;
                        comboText.text = currentCombo.ToString();
                        LeanTween.scale(comboText.rectTransform, localscale, 0.05f);
                    });
                }
                
                slider.fillAmount = gm.GetComboRemainPrectage();
            }


        }
        else
        {
            if (isComboShow)
            {
                MoveOut();
                isComboShow = false;
            }
        }
    }

    /// <summary>
    /// 进入
    /// </summary>
    void MoveIn()
    {
        LeanTween.moveX(rect, rect.anchoredPosition.x - 500, moveTime);
    }

    /// <summary>
    /// 移出
    /// </summary>
    void MoveOut()
    {
        LeanTween.moveX(rect, rect.anchoredPosition.x + 500, moveTime);
    }
}
