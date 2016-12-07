using UnityEngine;
using System.Collections;

public class FUG_DEMO : MonoBehaviour
{

    FUG_EnemyEventHandler eventHandler;
    // Use this for initialization
    void Start()
    {
        eventHandler = GetComponent<FUG_EnemyEventHandler>();
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
