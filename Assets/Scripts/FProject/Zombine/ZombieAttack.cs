using UnityEngine;
using System.Collections;
namespace FProject
{
    public class ZombieAttack : ZombieComponent
    {

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

            return RotateTowardsPlayer(Time.deltaTime, false);
        }

        public bool RotateTowardsPlayer(float deltaTime, bool useRigidbody)
        {
            Vector3 vector = playerT.transform.position - base.transform.position;
            float num = Vector3.Angle(vector, base.transform.forward);
            print("rotate toward player :" + num);
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

        public void OnStart_Attack()
        {
            // print("Start Attack");
            if (eventHandler.Move.Active)
                eventHandler.Move.TryStop();
        }

        public void OnStop_Attack()
        {
            if (attackHandler.Active)
                attackHandler.Cancel();
        }



        float timeSinceAttack = 0;
        public void Update()
        {
            if (!eventHandler.Start.Active)
                return;
            if (eventHandler.Attack.Active)
            {
                timeSinceAttack += Time.deltaTime;
                if (timeSinceAttack >= zombieBase.AttackSpeed)
                {
                    Attack();
                    timeSinceAttack = 0;
                }

            }
        }


        public float attackDelay = 0.5f;
        vp_Timer.Handle attackHandler = new vp_Timer.Handle();
        void Attack()
        {
            stateManager.SetState(ZombieAnimationState.Attack);
            vp_Timer.In(attackDelay, PlayAttackAnimation, attackHandler);
        }

        void PlayAttackAnimation()
        {
            playerT.GetComponent<vp_FPPlayerDamageHandler>().Damage(new vp_DamageInfo(zombieBase.Damage, this.transform, vp_DamageInfo.DamageType.Bullet));
        }

        public override void OnStop_Start()
        {
            base.OnStop_Start();
            if (eventHandler.Attack.Active)
                eventHandler.Attack.TryStop();
        }
    }
}