using UnityEngine;
using System.Collections;

public class PlayerEnterSceneSMB : CustomSMB {

    PlayerMovement playerM;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        tpsInput.InputEable = false;
        if(playerM == null)
        {
            playerM = tpsInput.GetComponent<PlayerMovement>();
        }

        playerM.MoveTo(1);
    }

}
