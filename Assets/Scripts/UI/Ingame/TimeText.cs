using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{

    public TimeController timeController;
    public Text timeText;
    // Use this for initialization
    void Start()
    {
        if (!timeController)
        {
            timeController = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
        }
        if(!timeText)
        {
            timeText = GetComponent<Text>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timeController && timeText)
        {
            timeText.text = timeController.GetLeftTimeString();
        }

    }
}
