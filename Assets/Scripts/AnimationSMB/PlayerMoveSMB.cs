using UnityEngine;
using System.Collections;

public class PlayerMoveSMB : CustomSMB
{

    NavMeshAgent movemont;
    Transform playerT;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movemont = tpsInput.GetComponent<NavMeshAgent>();
        playerT = tpsInput.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //movemont.
        if (movemont.enabled)
        {
            //Vector3 p1 = playerT.position;
            //Vector3 p2 = movemont.destination;
            //p1.y = p2.y;

            //Debug.Log("NavMeshAgent remain :" + movemont.remainingDistance + ",current :" + Vector3.Distance(p1, p2));
        }
        else
        {
            animator.SetFloat("_distance", 0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
