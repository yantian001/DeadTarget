using UnityEngine;
using System.Collections;

public class MitrailleurShootSMB : StateMachineBehaviour
{

    EnemyGun gun;
    Vector3 target;
    Transform muzzle;
    Transform aimSpin;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
       // Debug.Log(animator.gameObject);
        gun =animator.gameObject.GetComponent<EnemyGun>();
        muzzle = gun.muzzleTransform;
        aimSpin = gun.aimSpine;

        
        gun.DetachTarget();
        target = gun.targetPosition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if(gun.gunState == GunState.Ready)
        {
            Quaternion q = Quaternion.LookRotation(target - muzzle.position);
            if (Quaternion.Angle(aimSpin.rotation, q) < 0.5)
            {
                gun.Shoot();
            }
            else
            {
                aimSpin.rotation = Quaternion.RotateTowards(aimSpin.rotation, Quaternion.LookRotation(target - muzzle.position), 3f);
            }
            
        }
        else
        {
            if(gun.gunState == GunState.Empty)
            {
                animator.SetBool("isFiring", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("Reload", true);
            }
        }
    }
}
