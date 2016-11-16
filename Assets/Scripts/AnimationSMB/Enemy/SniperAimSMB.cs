using UnityEngine;
using BehaviorDesigner.Runtime;

public class SniperAimSMB : CustomSMB
{

    EnemyDrawLine edl;
    BehaviorTree bt;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        edl = animator.transform.GetComponent<EnemyDrawLine>();
        if (edl)
        {
            edl.BeginAim();
        }

        bt = animator.transform.GetComponent<BehaviorTree>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (edl.aimed && !animator.IsInTransition(layerIndex))
        {
            animator.SetTrigger("Aimed");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        bt.SetVariableValue("Aiming", false);
        animator.SetBool("Aiming", false);
        edl.EndAim();
    }
}
