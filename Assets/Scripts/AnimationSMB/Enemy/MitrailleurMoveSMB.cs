using UnityEngine;
using System.Collections;

public class MitrailleurMoveSMB : StateMachineBehaviour {

    public Transform target;
    public bool lookAtTartget = true;
    public float speed = 3f;
    public float arriveDistance = 50f;
    public float maxLookAtRotationDelta = 1f;

    public NavMeshAgent agent = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if(target  == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        agent = animator.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        //Debug.Log(Vector3.Distance(animator.transform.position, target.position));
        agent.SetDestination(target.position);
     
        if (agent.remainingDistance >arriveDistance)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        //if(Vector3.Distance(animator.transform.position ,target.position) > arriveDistance)
        //{
        //    animator.SetBool("isWalking", true);
        //    animator.transform.position = Vector3.MoveTowards(animator.transform.position, target.position, speed * Time.deltaTime);
        //    if (lookAtTartget)
        //    {
        //        animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, Quaternion.LookRotation(target.position - animator.transform.position), maxLookAtRotationDelta);
        //    }
        //}
        //else
        //{
        //    animator.SetBool("isWalking", false);
        //}
       
    }

}
