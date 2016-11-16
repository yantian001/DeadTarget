using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    public bool isCover = false;

    /// <summary>
    /// 不能产生的敌人的名字
    /// </summary>
    public string[] CantCreateName;

    public int groupId = -1;

    /// <summary>
    /// 是否可以创建名为 name 的敌人
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CanCreate(string name)
    {
        bool r = true;
        if(CantCreateName!=null && CantCreateName.Length >0)
        {
            for(int i=0;i<CantCreateName.Length;i++)
            {
                if(CantCreateName[i] == name)
                {
                    r = false;
                    break;
                }
            }
        }
        return r;
    }
}
