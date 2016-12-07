using UnityEngine;
using System.Collections;

public class FUG_EnemyInjured : vp_Component
{


    /// <summary>
    /// 
    /// </summary>
    FUG_EnemyEventHandler _eventHandler;
    FUG_EnemyEventHandler eventHandler
    {
        get
        {
            if (_eventHandler == null)
            {
                _eventHandler = Transform.root.GetComponent<FUG_EnemyEventHandler>();
            }
            return _eventHandler;
        }
        set
        {
            _eventHandler = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    Animator _animator;
    Animator animator
    {
        get
        {
            if (_animator == null)
                _animator = Transform.root.GetComponent<Animator>();
            return _animator;
        }
        set
        {
            _animator = value;
        }
    }

    public void OnStart_Injured()
    {
        if (eventHandler.Move.Active)
            eventHandler.Move.TryStop();
        if (eventHandler.Attack.Active)
        {
            eventHandler.Attack.TryStop();
        }
        PlayInjuredAnimation();
    }
    public bool CanStart_Injured()
    {
        if (eventHandler.Die.Active)
            return false;
        return true;
    }

    public void PlayInjuredAnimation()
    {
        animator.SetFloat("hitBlend", Random.Range(0f, 1.0f));
        animator.SetTrigger("hit");
    }
    public void Injured()
    {
       // print("injured!!!");
        //// Debug.
        //am.CrossFade(ac.name, 0.1f, PlayMode.StopAll);
        ////am.Play(ac.name, PlayMode.StopAll);
        //am.PlayQueued("idle");
        //eventHandler.Move.TryStop();
        //animator.SetFloat("hitBlend", Random.Range(0f, 1.0f));
        //animator.SetTrigger("hit");
        if (eventHandler.Injured.Active)
        {
            PlayInjuredAnimation();
        }
        else
            eventHandler.Injured.TryStart();
    }

    public void Hit1_Complete()
    {
        print("hit1 complete!");
        eventHandler.Injured.TryStop();
    }
}
