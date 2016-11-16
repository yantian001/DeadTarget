using UnityEngine;
using System.Collections;

public class PlayerBombSMB : CustomSMB {

    public PlayerTakeBomb takeBomb;
    public bool taked = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        takeBomb = tpsInput.GetComponent<PlayerTakeBomb>();
        taked = false;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if( stateInfo.normalizedTime>0.5 && !taked)
        {
            if(takeBomb)
            {
                takeBomb.TakeBomb();
                taked = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("bombed", false);
        tpsInput.Bombed = false;
    }

}
