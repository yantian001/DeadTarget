using UnityEngine;
using System.Collections.Generic;

public class Exposition : MonoBehaviour
{

    private const int ignoreWalkThru = ~((1 << 29) | (1 << 2) | (1 << 27) | (1 << 4) | (1 << 26));
    public float radiu = 5f;
    public int damege = 100;
    public string[] damegeTag;

    private List<AS_BulletHiter> hittedList = new List<AS_BulletHiter>();
    public void OnEnable()
    {
        DetectTarget();
    }

    void DetectTarget()
    {
        Collider[] coliders = Physics.OverlapSphere(transform.position, radiu, ignoreWalkThru);
        for (int i = 0; i < coliders.Length; i++)
        {
            if (tagCheck(coliders[i].tag) || tagCheck(coliders[i].transform.root.tag))
            {
                var hit = coliders[i].gameObject.GetComponent<AS_BulletHiter>();
                if (hitedCheck(hit))
                {
                    var dm = hit.RootObject.GetComponent<DamageManager>();
                    if (dm) dm.ApplyDamage(damege, Vector3.zero, 0);
                }
            }
        }

    }

    private bool hitedCheck(AS_BulletHiter hit)
    {
        if (!hit || !hit.RootObject)
        {
            return false;
        }

        for (int i = 0; i < hittedList.Count; i++)
        {

            if (hittedList[i].RootObject == hit.RootObject)
            {

                return false;
            }
        }

        hittedList.Add(hit);
        return true;
    }

    private bool tagCheck(string tag)
    {
        //Debug.Log("tag check:" + tag);
        for (int i = 0; i < damegeTag.Length; i++)
        {
            if (damegeTag[i] == tag)
            {
                return true;
            }
        }
        return false;
    }

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

    }
}
