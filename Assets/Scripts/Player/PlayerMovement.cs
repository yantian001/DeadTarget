using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;

    public Transform[] targets;

    public NavMeshAgent agent;

    public CharacterController character;

    Vector3 currentTarget;

    public Animator animator;

    int index = 1;//当前位置
    public bool isMoving = false; //是否在移动中

    bool started = false;
    // Use this for initialization
    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        //agent.SetDestination(targets[index].position);
        if (character == null)
        {
            character = GetComponent<CharacterController>();

        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        // agent.enabled = false;
        character.enabled = false;
        MoveTo(index);
    }
    /// <summary>
    /// 是否可以向左边移动
    /// </summary>
    /// <returns></returns>
    public bool CanMoveLeft()
    {
        return index > 0;
    }
    /// <summary>
    /// 是否可以向右边移动
    /// </summary>
    /// <returns></returns>
    public bool CanMoveRight()
    {
        return index < targets.Length - 1;
    }

    void Update()
    {
        if (agent.enabled)
        {
            transform.LookAt(agent.destination);
           // Debug.Log(transform.forward);
            if (agent.remainingDistance < 0.1f)
            {
                agent.enabled = false;
                animator.SetFloat("_speed", 0);
                animator.SetFloat("_distance", 0);
                //旋转角度
                Quaternion q = Quaternion.FromToRotation(transform.forward, targets[index].forward);
                transform.Rotate(q.eulerAngles);

                //通知游戏开始
                if (!started)
                {
                    started = true;
                    LeanTween.dispatchEvent((int)Events.PREVIEWSTART);
                }
            }
            else
            {
                animator.SetFloat("_speed", agent.velocity.magnitude);
                animator.SetFloat("_distance", agent.remainingDistance);
            }
        }
    }
    /// <summary>
    /// 向左或向右移动
    /// </summary>
    /// <param name="i">i>0时向右移动, i < 0 时向左移动</param>
    public void MoveDirection(int i)
    {
        animator.SetInteger("_direction", -i);
        if (i > 0)
        {
            MoveTo(index + 1);
        }
        else
        {
            MoveTo(index - 1);
        }
    }
    /// <summary>
    /// 移动到位置数组中的第几个位置
    /// </summary>
    /// <param name="i">数组的位置</param>
    public void MoveTo(int i)
    {
        animator.applyRootMotion = true;
        int len = targets.Length;
        if (len <= 0 || isMoving)
            return;
        index = i;
        while (index < 0 || index >= len)
        {
            if (index < 0)
                index += len;
            if (index >= len)
                index -= len;
        }
        agent.enabled = true;

        agent.SetDestination(targets[index].position);
    }
}
