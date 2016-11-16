using UnityEngine;
using System.Collections;

public class MitrailleurReloadSMB : StateMachineBehaviour {

    EnemyGun gun;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        gun = animator.gameObject.GetComponent<EnemyGun>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        gun.Reload();
        animator.SetBool("Reload", false);
    }
}
