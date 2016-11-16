using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {


    public GameObject popupMessage;

    private static Message _instance;
    public static Message Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if(Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
        // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PopMessage(string msg, float destory = 2f)
    {
        if(popupMessage != null)
        {
            var message = Instantiate<GameObject>(popupMessage);
            message.transform.SetParent(transform);
            message.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            message.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            CommonUtils.SetChildText(message.GetComponent<RectTransform>(), "Text", msg);
            if(destory > 0)
            {
                LeanTween.alpha(message, 0, destory).setDestroyOnComplete(true);
            }
        }
    }

    /// <summary>
    /// 显示信息，destory秒后自动删除
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="destory"></param>
    public static void PopupMessage(string msg ,float destory = 2f)
    {
        Instance.PopMessage(msg, destory);
    }
    
}
