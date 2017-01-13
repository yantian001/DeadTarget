using UnityEngine;
using System.Collections;
using FProject;
namespace FProject
{
    public class ZombieMovement : ZombieComponent
    {

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

        NavMeshObstacle _obstacle;
        NavMeshObstacle obstacle
        {
            get
            {
                if (_obstacle == null)
                {
                    _obstacle = GetComponent<NavMeshObstacle>();

                }
                return _obstacle;
            }
        }

        /// <summary>
        /// 是否到达目标地
        ///   - 第一次到达目标点后，将该值设置为true,后续不再判断位置
        /// </summary>
        bool arriveTarget = false;
        bool arriveNavigationStart = false;

        public override void OnEnable()
        {
            base.OnEnable();
            aget.enabled = false;
            obstacle.enabled = false;
        }

        public void OnStart_Move()
        {
            if (eventHandler.Attack.Active)
            {
                eventHandler.Attack.TryStop();
            }
            // print("Move Start!");
            obstacle.enabled = false;
            vp_Timer.In(0.1f, SetNavigationDestination);
        }

        void SetNavigationDestination()
        {
            NavMeshHit meshHit;

            if (NavMesh.SamplePosition(transform.position, out meshHit, 2, -1))
            {
                transform.position = meshHit.position;
            }

            aget.enabled = true;
            aget.SetDestination(zombieBase.curWayPoint.position);
            aget.stoppingDistance = zombieBase.STOP_DISTANCE;
            aget.speed = isCreep ? zombieBase.MoveSpeed * .8f : zombieBase.MoveSpeed;
            stateManager.SetState(ZombieAnimationState.Move, aget.speed);
        }

        void PauseNavigation()
        {
            try
            {
                stateManager.SetState(ZombieAnimationState.Move, 0);
                if (aget.enabled)
                {
                    //aget.Stop();
                    aget.enabled = false;
                }
            }
            catch (System.Exception e)
            {
                print(e.Message);
            }
            finally
            {
                if (aget.enabled)
                {
                    aget.Stop();
                    aget.enabled = false;
                }
            }

        }

        public void OnStop_Move()
        {
            //print("Move Stoped");
            PauseNavigation();
            vp_Timer.In(0.1f, () => { obstacle.enabled = true; });

        }

        public bool CanStart_Move()
        {
            //   print("Can Start Move");
            if (eventHandler.Die.Active || eventHandler.Injured.Active)
                return false;
            return true;
        }

        bool isStanding = false;
        protected Vector3 lastPosition;
        protected float stayTime = 0.0f;
        protected void LateUpdate()
        {
            if (!eventHandler.Start.Active)
                return;
            if (!arriveTarget)
            {
                float distance = Vector3.Distance(zombieBase.curWayPoint.position, new Vector3(transform.position.x, zombieBase.curWayPoint.position.y, transform.position.z));
                if (distance <= zombieBase.STOP_DISTANCE)
                //if (aget.enabled && aget.isOnNavMesh && aget.pathStatus == NavMeshPathStatus.PathComplete && aget.remainingDistance <= zombieBase.STOP_DISTANCE)
                {
                    arriveTarget = true;
                }
                else
                {
                    if (!eventHandler.Move.Active)
                    {
                        eventHandler.Move.TryStart();
                    }
                }
            }
            else
            {
                if (isCreep)
                {
                    if (!isStanding)
                    {
                        isStanding = true;
                        stateManager.SetState(ZombieAnimationState.StandUp);
                        vp_Timer.In(.2f, () =>
                        {
                            isCreep = false;
                            isStanding = false;
                        });
                    }
                }
                else
                {
                    if (!eventHandler.Attack.Active)
                        eventHandler.Attack.TryStart();
                }

            }
            ///处理卡死 
            if (eventHandler.Move.Active)
            {
                if (lastPosition == transform.position)
                {
                    stayTime += Time.deltaTime;
                    if(stayTime > 1)
                    {
                        float distance = Vector3.Distance(zombieBase.curWayPoint.position, new Vector3(transform.position.x, zombieBase.curWayPoint.position.y, transform.position.z));
                        arriveTarget = true;
                    }
                    if (stayTime > 8)
                    {
                        transform.SendMessage("Die");
                    }
                }
                else
                {
                    lastPosition = transform.position;
                    stayTime = 0;
                }
            }
        }

        public void Update()
        {
            //if (eventHandler.Die.Active)
            //{
            if (aget.enabled && this.obstacle.enabled)
            {
                this.obstacle.enabled = false;
            }
            if (eventHandler.Die.Active)
            {
                if (aget.enabled)
                    aget.enabled = false;
            }
            //}

        }


        public override void OnStop_Start()
        {
            base.OnStop_Start();
            if (eventHandler.Move.Active)
            {
                eventHandler.Move.TryStop();
            }
        }

        public bool RotateTowardsPosition(Vector3 position, float deltaTime, bool useRigidbody)
        {
            Vector3 vector = position - base.transform.position;
            float num = Vector3.Angle(vector, base.transform.forward);
            // print("rotate toward player :" + num);
            if (num > 5f)
            {
                Quaternion quaternion = base.transform.rotation;
                Quaternion to = Quaternion.LookRotation(vector);
                quaternion = Quaternion.Slerp(quaternion, to, 10f * deltaTime);
                if (useRigidbody)
                {
                    base.GetComponent<Rigidbody>().rotation = quaternion;
                }
                else
                {
                    base.transform.rotation = quaternion;
                }
                return false;
            }
            return true;
        }
    }
}