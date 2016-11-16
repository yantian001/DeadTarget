using UnityEngine;
using System.Collections;

public class WeaponReloadTriggerSMB : CustomSMB
{

    GunHanddle gunHanddle = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        //Debug.Log("Enter");
        if (gunHanddle == null)
        {
            gunHanddle = tpsInput.GetComponent<GunHanddle>();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime > 0)
        {
            return;
        }
        // Debug.Log(gunHanddle.CurrentGun.gunState);

        if (tpsInput.Reload && gunHanddle.CurrentGun.IsFullClip())
        {
            tpsInput.Reload = false;
        }
        if (tpsInput.Reload || gunHanddle.CurrentGun.gunState == GunState.Empty)
        {
            tpsInput.CanFire = false;
            animator.SetBool("reload", true);
        }
        else if (gunHanddle.CurrentGun.gunState == GunState.Ready)
        {
            tpsInput.CanFire = true;
        }
    }
}
