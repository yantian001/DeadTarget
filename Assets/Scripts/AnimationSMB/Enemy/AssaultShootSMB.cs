using UnityEngine;

public class AssaultShootSMB : StateMachineBehaviour
{

    // public Transform target;
    EnemyGun gun;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        //Debug.Log(animator.gameObject);
        gun = animator.gameObject.GetComponent<EnemyGun>();
        // target = GameObject.FindGameObjectWithTag("Player").transform;
        gun.DetachTarget();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (gun.gunState == GunState.Ready)
        {
            // animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, Quaternion.LookRotation(target.position - gun.muzzleTransform.position), 1);
            gun.Shoot();
        }
        else
        {
            if (gun.gunState == GunState.Empty)
            {
                animator.SetBool("isFiring", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("Reload", true);
            }
        }
    }
}
