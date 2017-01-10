using UnityEngine;
using System.Collections;
using System;
using GameDataEditor;
namespace FProject
{
    public class ZombineBase : MonoBehaviour
    {
        #region Static Property

        private static int curZombieId = 0;

        #endregion

        public int uuid;

        public int zombieId;

        public ZombieLivingState liveState = ZombieLivingState.ZOMBIE_DESPAWNED;

        protected Transform shadow;

        public int TotalHP
        {
            get;
            private set;
        }
        public int Damage
        {
            get;
            private set;
        }
        public int AttackSpeed
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public float MoveSpeed
        {
            get;
            set;
        }

        public Transform curWayPoint;
        [HideInInspector]
        public AppearanceType appearanceType;

        protected Vector3 startPos;

        protected bool isWillStartAction = false;
        protected bool isStartedAction = false;

        float moveDelay = 0.1f;

        public float STOP_DISTANCE = 2f;


        #region eventHandler

        private ZombieEventHandler _eventHandler;
        public ZombieEventHandler eventHandler
        {
            get
            {
                if (_eventHandler == null)
                {
                    _eventHandler = GetComponent<ZombieEventHandler>();
                }
                return _eventHandler;
            }
        }

        #endregion

        GDEZombieData zombieConfig;


        #region MonoBehavior Methods


        public void FixedUpdate()
        {
            if (isWillStartAction)
            {
                StartAction(appearanceType, startPos);
            }
        }

        #endregion


        /// <summary>
        /// 初始化僵尸相关
        /// </summary>
        /// <param name="zombieId"></param>
        /// <param name="biasHealth"></param>
        public virtual void Init(int zombieId, float biasHealth)
        {
            this.liveState = ZombieLivingState.ZOMBIE_DESPAWNED;
            if (shadow == null)
            {
                InitShadow();
            }
            this.zombieId = zombieId;
            liveState = ZombieLivingState.ZOMBIE_INITIALIZING;
            uuid = ZombineBase.curZombieId++;
            zombieConfig = SingletonMono<ConfigManager>.Instance.GetZombieById(zombieId);
            if (zombieConfig == null)
            {
                Debug.LogError("Miss Zombie Config!!!");
            }
            TotalHP = (int)(zombieConfig.TotalHP * biasHealth);
            transform.SendMessage("SetHP", TotalHP, SendMessageOptions.DontRequireReceiver);
            Damage = zombieConfig.Damage;
            AttackSpeed = zombieConfig.AttackSpeed;
            Name = zombieConfig.Name;
            //禁用Navemesh相关

            //禁用事件
            eventHandler.Move.Active = false;
            eventHandler.Injured.Active = false;
            eventHandler.Die.Active = false;
            eventHandler.Attack.Active = false;
            eventHandler.Start.Active = false;
            liveState = ZombieLivingState.ZOMBIE_INIT_PENDING;
        }

        private void InitShadow()
        {
            //throw new NotImplementedException();
        }

        public void SetCurrentPoint(Transform wayPoint)
        {
            curWayPoint = wayPoint;
            //throw new NotImplementedException();
        }

        public void SetOriginSpeed(float speed)
        {
            this.MoveSpeed = speed;
        }

        public bool StartAction(AppearanceType appType, Vector3 pos)
        {
            appearanceType = appType;
            if (appearanceType == AppearanceType.CreepOut)
            {
                eventHandler.Creep.Send();
            }
            startPos = pos;
            transform.SendMessage("SetStartPos", pos, SendMessageOptions.DontRequireReceiver);
            if (this.liveState == ZombieLivingState.ZOMBIE_INIT_PENDING)
            {
                liveState = ZombieLivingState.ZOMBIE_LIVING;
            }
            if (!this.IsInitialized())
            {
                this.isWillStartAction = true;
                return false;
            }
            ///延时0.2s,过渡动画
            vp_Timer.In(0.2f, () =>
                  {
                      Appear(appearanceType);
                      if (eventHandler.Start.TryStart())
                      {
                          isWillStartAction = false;
                          isStartedAction = true;
                      }
                  });


            return true;
            //  throw new NotImplementedException();
        }



        void Appear(AppearanceType appType)
        {
            transform.position = startPos;
        }

        private bool IsInitialized()
        {
            //throw new NotImplementedException();
            return true;
        }

        public void StopAction()
        {
            if (eventHandler.Start.Active)
            {
                eventHandler.Start.TryStop();
            }
            // throw new NotImplementedException();
        }

        public void StartAction()
        {
            if (!eventHandler.Start.Active)
                eventHandler.Start.TryStart();
        }
    }
}
