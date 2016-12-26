using UnityEngine;


public class FUG_Movement_SMB : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
       // Debug.Log("StateMachine Enter!");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
       // Debug.Log("StateMachine Exit!");
        animator.SendMessage("Hit1_Complete");
    }

    //public override void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    //{
    //    Debug.Log("StateMachine Exit!");
    //    base.OnStateMachineExit(animator, stateMachinePathHash, controller);
    //    animator.SendMessage("Hit1_Complete");
    //}
}
