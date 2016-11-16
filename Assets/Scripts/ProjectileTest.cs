using UnityEngine;
using System.Collections;


public class ProjectileTest : MonoBehaviour
{
    public GameObject target;
    public float speed = 10;
    private float distanceToTarget;
    private bool move = true;


    void Start()
    {
        distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {

        while (move)
        {
            Vector3 targetPos = target.transform.position;
            this.transform.LookAt(targetPos);
            float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPos) / distanceToTarget) * 45;
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            float currentDist = Vector3.Distance(this.transform.position, target.transform.position);
            print("currentDist" + currentDist);
            if (currentDist < 0.5f)
                move = false;
            this.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            yield return null;
        }
    }
}