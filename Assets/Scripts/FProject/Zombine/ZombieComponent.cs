using UnityEngine;
using System.Collections;

namespace FProject
{
    public abstract class ZombieComponent : MonoBehaviour
    {
        ZombieEventHandler _eventHandler;
        public ZombieEventHandler eventHandler
        {
            get
            {
                if (!_eventHandler)
                {
                    _eventHandler = GetComponent<ZombieEventHandler>();
                    if (!_eventHandler)
                    {
                        _eventHandler = gameObject.AddComponent<ZombieEventHandler>();
                    }
                }
                return _eventHandler;
            }

        }

        protected ZombieAnimationStateManager _stateManager;
        public ZombieAnimationStateManager stateManager
        {
            get
            {
                if (!_stateManager)
                {
                    _stateManager = GetComponent<ZombieAnimationStateManager>();
                    if (!_stateManager)
                    {
                        Debug.LogError("Miss Component ZombieAnimationStateManager");
                    }
                }
                return _stateManager;
            }
        }

        protected ZombineBase _zombieBase;
        public ZombineBase zombieBase
        {
            get
            {
                if (!_zombieBase)
                {
                    _zombieBase = GetComponent<ZombineBase>();
                    if (!_zombieBase)
                    {
                        Debug.LogError("Miss Component ZombineBase");
                    }
                }
                return _zombieBase;
            }
        }
        /// <summary>
        /// 是否已经开始
        /// </summary>
        protected bool started = false;

        protected bool isCreep = false;

        public virtual void OnStart_Start()
        {
            this.started = true;
            //if (zombieBase.appearanceType == AppearanceType.CreepOut)
            //{
            //    this.stateManager.SetState(ZombieAnimationState.Creep);
            //    isCreep = true;
            //}
            //else
            //{
            //    isCreep = false;
            //}
        }

        public void OnMessage_Creep()
        {
            this.stateManager.SetState(ZombieAnimationState.Creep);
            isCreep = true;
        }

        public virtual void OnStop_Start()
        {
            this.started = false;
        }

        public virtual void OnEnable()
        {
            eventHandler.Register(this);
        }

        public virtual void OnDisable()
        {
            eventHandler.Unregister(this);
        }
    }
}