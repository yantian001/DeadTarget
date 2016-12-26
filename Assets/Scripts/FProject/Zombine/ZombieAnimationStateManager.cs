using UnityEngine;
using System.Collections;
using System;

namespace FProject
{

    public class ZombieAnimationStateManager : MonoBehaviour
    {

        public Animator animator;
        //Clips
        public AnimationClip idleClip;
        public AnimationClip runClip;
        public AnimationClip zrunClip;
        public AnimationClip walkClip;
        public AnimationClip zwalkClip;
        public AnimationClip hit1Clip;
        public AnimationClip hit2Clip;
        public AnimationClip creepClip;
        public AnimationClip creephitClip;
        public AnimationClip creepDieClip;
        public AnimationClip jumpClip;
        public AnimationClip fallDownClip;
        public AnimationClip dieClip;
        public AnimationClip climbUpClip;
        public AnimationClip attack1Clip;
        public AnimationClip attack2Clip;
        public AnimationClip standUpClip;

        public ZombieEventHandler eventHandler;
        protected float speed = 0;
        protected ZombieAnimationState currentState = ZombieAnimationState.None;
        protected ZombieAnimationState nextState = ZombieAnimationState.Idle;

        static private readonly int HIT = Animator.StringToHash("hit");
        static private readonly int HITBLEND = Animator.StringToHash("hitBlend");
        static private readonly int SPEED = Animator.StringToHash("speed");
        static private readonly int ZOMBIE = Animator.StringToHash("zombie");
        static private readonly int ATTACK = Animator.StringToHash("attack");
        static private readonly int DIE = Animator.StringToHash("die");
        static private readonly int CREEP = Animator.StringToHash("creep");

        // Use this for initialization
        void Start()
        {
            if (!eventHandler)
                eventHandler = GetComponent<ZombieEventHandler>();
            if (!animator)
                animator = GetComponent<Animator>();

            InitAnimatorController();

        }

        private void InitAnimatorController()
        {
            // throw new NotImplementedException();
            AnimatorOverrideController overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
            if (idleClip) overrideController["idle"] = idleClip;
            if (attack1Clip) overrideController["attack1"] = attack1Clip;
            if (attack2Clip) overrideController["attack2"] = attack2Clip;
            if (creepClip) overrideController["creep"] = creepClip;
            if (creephitClip) overrideController["creephit"] = creephitClip;
            if (creepDieClip) overrideController["creepdie"] = creepDieClip;
            if (dieClip) overrideController["die"] = dieClip;
            if (hit1Clip) overrideController["hit1"] = hit1Clip;
            if (hit2Clip) overrideController["hit2"] = hit2Clip;
            if (runClip) overrideController["run"] = runClip;
            if (walkClip) overrideController["walk"] = walkClip;
            if (zrunClip) overrideController["z-run"] = zrunClip;
            if (zwalkClip) overrideController["z-walk"] = zwalkClip;
            if (standUpClip) overrideController["standup"] = standUpClip;
            animator.runtimeAnimatorController = overrideController;
        }

        public void OnEnable()
        {
            eventHandler.Register(this);
            currentState = ZombieAnimationState.Idle;
            nextState = ZombieAnimationState.None;
        }

        public void OnDisable()
        {
            eventHandler.Unregister(this);
        }



        //// Update is called once per frame
        //void Update()
        //{
        //    //SwitchNextState();
        //}

        void SwitchNextState()
        {
            if (nextState == ZombieAnimationState.None)
                return;
            if (nextState == ZombieAnimationState.Attack) animator.SetTrigger(ATTACK);
            else if (nextState == ZombieAnimationState.Die) animator.SetTrigger(DIE);
            else if (nextState == ZombieAnimationState.StandUp) animator.SetBool(CREEP, false);
            else if (nextState == ZombieAnimationState.Creep) animator.SetBool(CREEP, true);
            else if (nextState == ZombieAnimationState.Move)
            {
                animator.SetFloat(SPEED, speed);
            }
            else if (nextState == ZombieAnimationState.Injured)
            {
                animator.SetFloat(HITBLEND, UnityEngine.Random.Range(0.0f, 1.0f));
                animator.SetTrigger(HIT);

            }
            currentState = nextState;
            nextState = ZombieAnimationState.None;
        }

        public void SetState(ZombieAnimationState state, float speed = 0.0f)
        {
            this.nextState = state;
            this.speed = speed;
            SwitchNextState();
        }
    }
}
