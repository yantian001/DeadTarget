using UnityEngine;
using System.Collections;

public class WaypointManager : MonoBehaviour
{

    public Waypoint[] wayPoints;

    /// <summary>
    /// 限制移动
    /// </summary>
    public bool restrictMove = false;

    private static WaypointManager _instance = null;

    public static WaypointManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public void Awake()
    {
        if (_instance)
        {
            GameObject.Destroy(_instance);
        }
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        if (wayPoints == null || wayPoints.Length <= 0)
        {
            wayPoints = transform.GetComponentsInChildren<Waypoint>();
        }
    }

    /// <summary>
    /// 是否有空的可以移动的点
    /// </summary>
    /// <param name="childCount">每个点最大的敌人数,默认为1</param>
    /// <returns></returns>
    public bool IsHaveEmptyPoint(int childCount = 1)
    {
        //for (int i = 0; i < wayPoints.Length; i++)
        //{
        //    if (wayPoints[i].transform.childCount < childCount)
        //    {
        //        return true;
        //    }
        //}
        //return false;
        return IsHaveEmptyPoint(-1, childCount);
    }

    /// <summary>
    /// 是否有空节点，可以移动
    /// </summary>
    /// <param name="groupid"></param>
    /// <param name="childCount"></param>
    /// <returns></returns>
    public bool IsHaveEmptyPoint(int groupid ,int childCount = 1)
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (wayPoints[i].transform.childCount < childCount && wayPoints[i].groupId == groupid)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取下一个移动点
    /// </summary>
    /// <param name="current"></param>
    /// <param name="maxChildren"></param>
    /// <returns></returns>
    public Transform GetWaypoint(Transform current,int maxChildren = 1)
    {
        if (current == null)
            return null;
        int groupId = -1;
        if(restrictMove)
        {
            var wp = current.GetComponentInParent<Waypoint>();
            if(!wp)
            {
               var tp = current.GetComponentInParent<TargetPosition>();
               groupId = tp.groupId;
            }
            else
            {
                groupId = wp.groupId;
            }
        }
        int r = Random.Range(0, wayPoints.Length);
        bool found = false;
        int time = 0;
        while(!found)
        {
            if(wayPoints[r].transform.childCount < maxChildren && wayPoints[r].groupId == groupId)
            {
                return wayPoints[r].transform;
            }
            r++;
            r++;
            if (r >= wayPoints.Length)
            {
                r -= wayPoints.Length;
            }
            time++;
            if (time > wayPoints.Length + 1)
                break;
        }
        return null;
    }
}
