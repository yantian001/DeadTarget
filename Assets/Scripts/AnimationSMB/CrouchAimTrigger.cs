using UnityEngine;
using System.Collections;

public class CrouchAimTrigger : CustomSMB
{

    //private TPSInput tpsInput;
    private static PlayerMovement playerMove = null;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);
        tpsInput.InputEable = true;
        if (playerMove == null)
        {
            if (tpsInput)
            {
                playerMove = tpsInput.GetComponent<PlayerMovement>();
            }
        }
        //  Debug.Log(tpsInput);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime > 0)
        {
            return;
        }
        tpsInput.IsMoveing = false;

        if (tpsInput.IsMoveLeftPressed)
        {
            playerMove.MoveDirection(-1);
            tpsInput.IsMoveLeftPressed = false;
        }
        else if (tpsInput.IsMoveRightPressed)
        {
            playerMove.MoveDirection(1);
            tpsInput.IsMoveRightPressed = false;
        }
        else
        {
            if (tpsInput.IsAim)
            {
                animator.SetBool("isAim", true);
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
