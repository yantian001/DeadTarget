using UnityEngine;
using System.Collections;

public class WeaponReloadSMB : CustomSMB
{

    GunHanddle gunHanddle = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (gunHanddle == null)
        {
            gunHanddle = tpsInput.GetComponent<GunHanddle>();
        }

        gunHanddle.CurrentGun.gunState = GunState.Reloading;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
     
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit");
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool("reload", false);
        tpsInput.Reload = false;
        gunHanddle.CurrentGun.Reload();
    }
}
