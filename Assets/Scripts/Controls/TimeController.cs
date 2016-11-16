using UnityEngine;

public class TimeController : MonoBehaviour {

    public int total = 0;

    /// <summary>
    /// 剩余时间
    /// </summary>
    public float left = 0;
	// Use this for initialization
	void Start () {
        total = GameValue.taskData.Info.TotalTime;
        left = total + 5;
	}
	
	// Update is called once per frame
	void Update () {
	    if(GameValue.staus == GameStatu.InGame)
        {
            if(left <= 0)
            {
                LeanTween.dispatchEvent((int)Events.TIMEUP);
            }
            else
            {
                left -= Time.fixedDeltaTime;
            }
            
        }
	}

    public float GetLeftTime()
    {
        return left;
    }

    public string GetLeftTimeString()
    {
        if(left <= 0)
        {
            return "00:00";
        }
        int m = Mathf.CeilToInt(left) / 60;
        int s = Mathf.CeilToInt(left) % 60;
        return string.Format("{0:00}:{1:00}",m,s);
    }
}
