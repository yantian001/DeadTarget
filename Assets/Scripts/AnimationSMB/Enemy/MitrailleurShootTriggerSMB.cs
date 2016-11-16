using UnityEngine;
using System.Collections;

public class MitrailleurShootTriggerSMB : StateMachineBehaviour
{

    public float shootDistance = 200f;
    public Transform playerF;
    public Transform selfT;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (playerF == null)
        {
            playerF = GameObject.FindGameObjectWithTag("Player").transform;
        }
        selfT = animator.transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (!(animator.GetBool("Reload")))
        {
            if (Vector3.Distance(selfT.position, playerF.position) < shootDistance)
            {
                animator.SetBool("isFiring", true);
            }
        }
        
    }


}
