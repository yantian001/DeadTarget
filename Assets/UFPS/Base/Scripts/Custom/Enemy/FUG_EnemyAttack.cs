using UnityEngine;
using System.Collections;

public class FUG_EnemyAttack : MonoBehaviour
{
    FUG_EnemyEventHandler _eventHandler;
    FUG_EnemyEventHandler eventHandler
    {
        get
        {
            if (!_eventHandler)
                _eventHandler = GetComponent<FUG_EnemyEventHandler>();
            return _eventHandler;
        }
        set
        {
            _eventHandler = value;
        }

    }

    Animator _animator;
    Animator animator
    {
        get
        {
            if (!_animator)
                _animator = GetComponent<Animator>();
            return _animator;
        }
        set
        {
            _animator = value;
        }
    }

    Transform _playerT;
    Transform playerT
    {
        get
        {
            if (!_playerT)
            {
                _playerT = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return _playerT;
        }
    }

    public bool CanStart_Attack()
    {
        if (eventHandler.Die.Active || eventHandler.Injured.Active)
            return false;
        return true;
    }

    public void OnStart_Attack()
    {
        print("Start Attack");
        eventHandler.Move.TryStop();
    }

    public void OnStop_Attack()
    {
        if (attackHandler.Active)
            attackHandler.Cancel();
    }

    #region Monobehavior Method

    public void OnEnable()
    {
        eventHandler.Register(this);
    }

    public void OnDisable()
    {
        eventHandler.Unregister(this);
    }
    public float attackInterval = 10f;
    public float nextAllowAttackTime = 0f;
    public void Update()
    {
        if (eventHandler.Attack.Active)
        {
            if (Time.time >= nextAllowAttackTime)
            {
                Attack();
            }
        }
    }


    #endregion
    public float attackDelay = 0.5f;
    vp_Timer.Handle attackHandler = new vp_Timer.Handle();
    void Attack()
    {
        nextAllowAttackTime = Time.time + attackInterval;
        animator.SetTrigger("attack");
        vp_Timer.In(attackDelay, PlayAttackAnimation, attackHandler);

    }

    void PlayAttackAnimation()
    {
        playerT.GetComponent<vp_FPPlayerDamageHandler>().Damage(new vp_DamageInfo(1, this.transform, vp_DamageInfo.DamageType.Bullet));
    }
}
