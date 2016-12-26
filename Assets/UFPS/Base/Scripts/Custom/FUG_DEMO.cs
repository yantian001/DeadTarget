using UnityEngine;
using System.Collections;

public class FUG_DEMO : MonoBehaviour
{

    ZombieEventHandler eventHandler;
    // Use this for initialization
    void Start()
    {
        eventHandler = GetComponent<ZombieEventHandler>();
        eventHandler.Move.TryStart();
    }

    public void Update()
    {
        if(!eventHandler.Move.Active)
        {
            eventHandler.Move.TryStart();
        }
    }

    
}
