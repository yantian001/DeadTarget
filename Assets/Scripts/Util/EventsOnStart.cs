using UnityEngine;
using System.Collections;

public class EventsOnStart : MonoBehaviour
{

    public Events[] events;
    // Use this for initialization
    void Start()
    {
        if (events != null && events.Length > 0)
        {
            for (int i = 0; i < events.Length; i++)
            {
                LeanTween.dispatchEvent((int)events[i]);
            }
        }
    }


}
