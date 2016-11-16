using UnityEngine;
using System.Collections;

public class TargetPosition : MonoBehaviour
{
    public string[] dontAllowedEnemy;

    public int id;

    public int groupId = -1;

    public bool CanCreateEnemy(string name)
    {
        bool b = true;
        if (dontAllowedEnemy.Length > 0)
        {
            for (int i = 0; i < dontAllowedEnemy.Length; i++)
            {
                if (dontAllowedEnemy[i] == name)
                {
                    b = false;
                    break;
                }
            }
        }
        return b;
    }
}
