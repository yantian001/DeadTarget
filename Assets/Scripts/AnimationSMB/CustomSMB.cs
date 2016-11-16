using UnityEngine;
using System.Collections;

public class CustomSMB : StateMachineBehaviour {

    protected TPSInput tpsInput = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if(tpsInput == null)
        {
            tpsInput = GameObject.FindGameObjectWithTag("Player").GetComponent<TPSInput>();
           // Debug.Log("get tpsinpt in " + this.ToString());
        }
    }
}
