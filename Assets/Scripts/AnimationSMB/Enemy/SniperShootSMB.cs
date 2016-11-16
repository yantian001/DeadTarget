using UnityEngine;
using System.Collections;

public class SniperShootSMB : CustomSMB {

    EnemyGun gun;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        gun = animator.gameObject.GetComponent<EnemyGun>();
        EnemyDrawLine edl = animator.gameObject.GetComponent<EnemyDrawLine>();
        
        if(gun)
        {
            gun.targetPosition = edl.targetPosition;
            gun.Shoot();
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
