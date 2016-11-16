using UnityEngine;
using System.Collections;

public class EnemyDrawLine : MonoBehaviour
{

    public LineRenderer line;
    public Transform muzzleT;
    public float aimTime = 2f;
    public float spread = 10f;
    public float aimInterval = 0.1f;
    public float aimStopDistance = 0.1f;
    /// <summary>
    /// 是否已经瞄准
    /// </summary>
    [HideInInspector]
    public bool aimed = false;
    /// <summary>
    /// 瞄准中
    /// </summary>
    private bool aiming = false;

    float lastAimTime = 0;
    float currentSpreed;
    float maxSpreedSpeed;
    float spreadVelocity;
    Transform playerT;
    private const int ignoreWalkThru = ~((1 << 29) | (1 << 2) | (1 << 27) | (1 << 4) | (1 << 26));
    [HideInInspector]
    public Vector3 targetPosition;
    Vector3 playerPosition;
    Vector3 currentPosition;
    Coroutine c;
    // Use this for initialization
    void Start()
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttr>().targetAttach;

        // DrawLine();
       // BeginAim();
    }

    /// <summary>
    /// 开始瞄准目标
    /// </summary>
    public void BeginAim()
    {
        aimed = false;
        aiming = true;
        currentSpreed = spread;
        maxSpreedSpeed = spread / aimTime;
        playerPosition = playerT.position;
        targetPosition = playerT.position;
        currentPosition = GetTargetPosition();
        c = StartCoroutine(Step());

    }
    /// <summary>
    /// 是否已经瞄准目标
    /// </summary>
    /// <returns></returns>
    public bool IsAimed()
    {
        return aiming && aimed;
    }
    /// <summary>
    /// 结束目标瞄准
    /// </summary>
    public void EndAim()
    {
        aiming = false;
        aimed = true;
        StopCoroutine(c);
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        // if (currentSpreed > 0 )
        {
            if (aiming)
            {
                line.enabled = true;
                line.useWorldSpace = true;
                line.SetPosition(0, muzzleT.position);
                if (Vector3.Distance(currentPosition, targetPosition) > aimStopDistance)
                {
                    currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime);
                    RaycastHit hit;
                    if (Physics.Raycast(muzzleT.position, (currentPosition - muzzleT.position).normalized, out hit, 1000, ignoreWalkThru))
                    {
                        line.SetPosition(1, hit.point);

                    }
                    lastAimTime = Time.time;
                }
                else
                {
                    aimed = true;
                }
            }


        }
    }

    Vector3 GetTargetPosition()
    {
        return playerPosition + new Vector3(Random.Range(-currentSpreed, currentSpreed), Random.Range(-currentSpreed, currentSpreed), Random.Range(-currentSpreed, currentSpreed));
    }

    IEnumerator Step()
    {
        while (true)
        {
            yield return new WaitForSeconds(aimInterval);
            currentSpreed -= aimInterval * maxSpreedSpeed;
            currentSpreed = Mathf.Max(0, currentSpreed);
           // Debug.Log(currentSpreed);
            targetPosition = GetTargetPosition();
            // DrawLine();
        }
    }

}
