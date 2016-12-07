using UnityEngine;
using System.Collections;

public class FUG_Movement : vp_Component
{

    FUG_EnemyEventHandler _eventHandler;
    FUG_EnemyEventHandler eventHandler
    {
        get
        {
            if (_eventHandler == null)
                _eventHandler = GetComponent<FUG_EnemyEventHandler>();
            return _eventHandler;
        }
        set
        {
            _eventHandler = value;
        }
    }

    NavMeshAgent _aget;
    NavMeshAgent aget
    {
        get
        {
            if (_aget == null)
            {
                _aget = this.GetComponent<NavMeshAgent>();
            }
            return _aget;
        }
        set
        {
            _aget = value;
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
        set
        {
            _playerT = value;
        }
    }

    Animator _animator;
    Animator animator
    {
        get
        {
            if (!_animator)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
        set
        {
            _animator = value;
        }

    }

    public void OnStart_Move()
    {
        print("Move Start!");
        aget.enabled = true;
        aget.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        aget.stoppingDistance = 2;
        animator.SetFloat("speed", aget.speed);
    }

    public void OnStop_Move()
    {
        print("Move Stoped");
        aget.Stop();
        aget.enabled = false;
        animator.SetFloat("speed", 0);

    }

    public bool CanStart_Move()
    {
     //   print("Can Start Move");
        if (eventHandler.Die.Active || eventHandler.Injured.Active)
            return false;
        return true;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        //判断移动
        //if (!eventHandler.Move.Active)
        //{
        //    if (!eventHandler.Die.Active || eventHandler.Injured.Active)
        //    {
        //        print(Vector3.SqrMagnitude(playerT.position - transform.position));
        //        if (Vector3.SqrMagnitude(playerT.position - transform.position) < 4)
        //        {
        //            eventHandler.Attack.TryStart();
        //        }
        //        else
        //        {
        //            eventHandler.Move.TryStart();
        //        }
        //    }
        //}
        // print(Vector3.SqrMagnitude(playerT.position - transform.position));
        if ((Vector3.SqrMagnitude(playerT.position - transform.position) < 4))
        {
            if (!eventHandler.Attack.Active)
                eventHandler.Attack.TryStart();
        }
        else
        {
            if (!eventHandler.Move.Active)
            {
                eventHandler.Move.TryStart();
            }
        }
    }
}
