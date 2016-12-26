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
            print("Move Start!");
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
            print("Move Stoped");
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
        protected void LateUpdate()
        {
            if (!eventHandler.Start.Active)
                return;
            if (!arriveTarget)
            {
                if ((Vector3.Distance(zombieBase.curWayPoint.position, transform.position) < zombieBase.STOP_DISTANCE))
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
        }

        public void Update()
        {
            //if (eventHandler.Die.Active)
            //{
            if (aget.enabled && this.obstacle.enabled)
            {
                this.obstacle.enabled = false;
            }
            //}
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