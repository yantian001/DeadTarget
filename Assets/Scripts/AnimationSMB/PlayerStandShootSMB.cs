using UnityEngine;
using System.Collections;

public class PlayerStandShootSMB : CustomSMB {

    static GunHanddle gunHandle = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if(gunHandle == null)
        {
            gunHandle = tpsInput.GetComponent<GunHanddle>();
        }

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if(tpsInput.IsFirePressed && gunHandle.CurrentGun.gunState == GunState.Ready)
        {
            gunHandle.Shoot();
        }
        else
        {
            animator.SetBool("fired", false);
            tpsInput.IsFirePressed = false;
        }
    }
}
