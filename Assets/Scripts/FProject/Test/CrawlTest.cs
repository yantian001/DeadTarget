using UnityEngine;
using System.Collections;

public class CrawlTest : MonoBehaviour
{

    Vector3 position;
    public AnimationCurve curve;
    // Use this for initialization
    void Start()
    {
        NavMeshHit meshHit;
        // Vector3 position;
        if (NavMesh.SamplePosition(transform.position, out meshHit, 2f, -1))
        {
            print("sample Position :" + meshHit.position);
            position = meshHit.position;
        }
    }
    bool standing = false;
    // Update is called once per frame
    void Update()
    {
       
        if (Mathf.Abs(position.y - transform.position.y) < 0.4)
        {
            if (transform.position != position)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, 1 * Time.deltaTime);
                RotateTowardsPlayer(Time.deltaTime, false);
            }
            else
            {
                if (!standing)
                {
                    GetComponent<Animation>().CrossFade("idle", 0.5f);
                    standing = true;
                }
            }
            if (standing)
            {
                Animation a = GetComponent<Animation>();

                if (!GetComponent<Animation>().isPlaying)
                {
                    GetComponent<Animation>().CrossFade("idle", 0.2f);
                }
            }
        }
    }

    public bool RotateTowardsPlayer(float deltaTime, bool useRigidbody)
    {
        Vector3 vector = position - base.transform.position;
        float num = Vector3.Angle(vector, base.transform.forward);
       // print("rotate toward player :" + num);
        if (num > 5f)
        {
            Quaternion quaternion = base.transform.rotation;
            Quaternion to = Quaternion.LookRotation(vector);
            quaternion = Quaternion.Slerp(quaternion, to, 10f * deltaTime);
            if (useRigidbody)
            {
                base.GetComponent<Rigidbody>().rotation = quaternion;
            }
            else
            {
                base.transform.rotation = quaternion;
            }
            return false;
        }
        return true;
    }
}
