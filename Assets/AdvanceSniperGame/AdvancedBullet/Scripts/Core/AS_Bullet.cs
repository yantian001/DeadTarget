using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class casthit
{
    public int index;
    public float distance;
    public string name;
    public int sort = -1;
}

[RequireComponent(typeof(Rigidbody))]
public class AS_Bullet : MonoBehaviour
{

    public GameObject ParticleHit;
    public bool ParticleSticky;
    public int Damage = 10;
    public string DamageMethodName = "ApplyDamage";
    public string DoHitMethodName = "DoHit";
    public float MuzzleVelocity = 790;
    public int LifeTime = 3;
    public int HitForce = 3000;
    public int HitCountMax = 10;
    public bool DestroyWhenHit = true;
    public float RunningRaylength = 40;
    public float FirstRaylength = 20;
    public float DetectorLength = 2000;
    public string[] IgnoreTag = { "Player" };
    public string[] DestroyerTag = { "scene" };
    public bool IsEnemy = false;
    [HideInInspector]
    public Transform source;
    public LineRenderer lineRenderer;
    [HideInInspector]
    public float HitDistance;
    private bool hited = false;
    private bool firsthited = false;
    private bool destroyed = false;
    private AS_ActionPreset actionPreset;
    [HideInInspector]
    public Vector3 pointShoot;
    private const int ignoreWalkThru = ~((1 << 29) | (1 << 2) | (1 << 27) | (1 << 4) | (1 << 26));
    private Vector3 initialPosition;
    private Vector3 initialVelocity;
    private Vector3 initialDirection;
    private List<Collider> hittedList = new List<Collider>();

    public void Awake()
    {
        initialPosition = this.gameObject.transform.position;
        if (!IsEnemy)
        {
            AS_ActionCamera actioncam = (AS_ActionCamera)GameObject.FindObjectOfType(typeof(AS_ActionCamera));

            if (actioncam != null)
            {
                actionPreset = actioncam.GetPresets();
                if (actionPreset != null)
                {
                    actionPreset.Shoot(this.gameObject);
                }
            }
        }
        this.GetComponent<Renderer>().enabled = true;
    }

    public void Start()
    {
        initialVelocity = this.transform.forward * MuzzleVelocity;
        initialDirection = this.transform.forward;
        //initialPosition = this.gameObject.transform.position;
        pointShoot = this.gameObject.transform.position;
        latestPosition = this.gameObject.transform.position;
        GetComponent<Rigidbody>().mass = 1;
        GetComponent<Rigidbody>().drag = 0;
        GetComponent<Rigidbody>().angularDrag = 0;

        hited = false;
        firsthited = false;
        destroyed = false;

        //Debug.Log ("Shoot");
        if (!RayShoot(true))
        {
            if (GetComponent<Rigidbody>().useGravity)
            {
                PredictionTrajectory();
            }
            else
            {
                FirstDetectTarget();
            }
        }
        GameObject.Destroy(this.gameObject, LifeTime);
        this.GetComponent<Rigidbody>().velocity = (initialVelocity);
        this.transform.forward = initialVelocity.normalized;
    }

    public Vector3 GetInitialPosistion()
    {
        return initialPosition;
    }

    private bool tagCheck(string tag)
    {
        //Debug.Log("tag check:" + tag);
        for (int i = 0; i < IgnoreTag.Length; i++)
        {
            if (IgnoreTag[i] == tag)
            {
                return false;
            }
        }
        return true;
    }

    private bool tagDestroyerCheck(string tag)
    {
        for (int i = 0; i < DestroyerTag.Length; i++)
        {
            if (DestroyerTag[i] == tag)
            {
                return true;
            }
        }
        return false;
    }

    private bool hitedCheck(Collider ob)
    {
        foreach (Collider trans in hittedList)
        {
            if (ob == trans)
            {
                return false;
            }
        }

        hittedList.Add(ob);
        return true;
    }

    void FixedUpdate()
    {
        if (GetComponent<Rigidbody>())
        {
            this.transform.forward = GetComponent<Rigidbody>().velocity.normalized;
        }
        if (!destroyed)
        {
            RayShoot(false);
            if (!hited)
                RunningDetectTarget();
        }
        latestPosition = this.transform.position;
    }

    private float runningmMagnitude;

    void Update()
    {

        runningmMagnitude = (this.transform.position - latestPosition).magnitude;
        if (runningmMagnitude <= 0)
            runningmMagnitude = 0.2f;


    }

    private Vector3 latestPosition;
    private int hitcount;

    public bool RayShoot(bool first)
    {
        bool res = false;
        RaycastHit[] hits;
        List<casthit> castHits = new List<casthit>();

        float raySize = runningmMagnitude;
        Vector3 direction = GetComponent<Rigidbody>().velocity.normalized;

        if (first)
        {
            raySize = FirstRaylength;
            direction = initialDirection;
        }

        if (raySize <= 2.0f)
        {
            raySize = 2.0f;
        }

        Vector3 pos1 = this.transform.position - (direction * raySize);
        //Vector3 pos2 = pos1 + (direction * raySize);

        if (first)
        {
            pos1 = initialPosition;
            //pos2 = pos1 + (direction * raySize);
        }

        if (lineRenderer)
        {
            LineRenderer line = (LineRenderer)GameObject.Instantiate(lineRenderer, pos1, Quaternion.identity);
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos1 + (raySize * direction));
            line.name = "laser " + raySize + " direction " + direction;
            GameObject.Destroy(line, 10);
            //Debug.Log ("Shoot with size " + raySize);
        }


        // shoot ray to cast all objects
        int castcount = 0;
        RaycastHit[] casterhits = Physics.RaycastAll(pos1, direction, raySize, ignoreWalkThru);
        for (int i = 0; i < casterhits.Length; i++)
        {
            if (casterhits[i].collider && Vector3.Dot((casterhits[i].point - initialPosition).normalized, initialDirection) > 0.5f)
            {
                if ( (tagCheck(casterhits[i].collider.tag) && (tagCheck(casterhits[i].transform.root.tag)))&& casterhits[i].collider.tag != this.gameObject.tag)
                {
                    if (hitedCheck(casterhits[i].collider))
                    {
                        castcount++;
                        casthit casted = new casthit();
                        casted.distance = Vector3.Distance(initialPosition, casterhits[i].point);
                        casted.index = i;
                        casted.name = casterhits[i].collider.name;
                        //Debug.Log(casterhits[i].collider.transform.root.gameObject.name);
                        var bh = casterhits[i].collider.GetComponent<AS_BulletHiter>();
                        if (bh)
                        {
                            casted.sort = bh.Sort;
                        }
                        else
                            casted.sort = -1;
                        castHits.Add(casted);
                        //Debug.Log("cast " + casterhits[i].collider.name + "  (" + Vector3.Distance(initialPosition, casterhits[i].point) + ")");
                    }
                }
            }
            //Debug.Log ("all cast " + casterhits [i].collider.name);
        }

        // sorted first to the last
        hits = new RaycastHit[castcount];
        //ÐÞ¸ÄÅÅÐò·½Ê½
         castHits.Sort((x, y) => x.distance.CompareTo(y.distance));
        //castHits.Sort((x, y) =>
        //{
        //    return y.sort.CompareTo(x.sort);
        //});


        for (int i = 0; i < castHits.Count; i++)
        {
            hits[i] = casterhits[castHits[i].index];
           // Debug.Log("soted cast "+castHits[i].index+" to "+i+" "+castHits[i].name+"  ("+castHits[i].distance+")");
        }

        for (var i = 0; i < hits.Length && hitcount < HitCountMax; i++)
        {
            RaycastHit hit = hits[i];

            if (first)
            {
                firsthited = true;
            }
            else
            {
                hited = true;
            }

            GameObject hitparticle = null;

            if (hit.collider.GetComponent<Rigidbody>())
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(direction * HitForce, ForceMode.Force);

            }

            if (hit.collider.gameObject.GetComponent<AS_BulletHiter>())
            {

                AS_BulletHiter bulletHit = hit.collider.gameObject.GetComponent<AS_BulletHiter>();

                if (bulletHit != null)
                {

                    if (bulletHit.ParticleHit)
                    {
                        hitparticle = (GameObject)Instantiate(bulletHit.ParticleHit, hit.point, hit.transform.rotation);
                    }
                    if (actionPreset )
                    // if (actionPreset && !firsthited)
                    {
                        actionPreset.TargetHited(this, bulletHit, hit.point);
                    }

                    this.transform.position = hit.point;

                    bulletHit.OnHit(hit, this);
                    hit.collider.SendMessageUpwards(DamageMethodName, (float)Damage, SendMessageOptions.DontRequireReceiver);
                    this.SendMessageUpwards(DoHitMethodName, SendMessageOptions.DontRequireReceiver);
                }

            }
            else if(!(hit.collider is CharacterController))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (ParticleHit)
                {
                    hitparticle = (GameObject)Instantiate(ParticleHit, hit.point, hit.transform.rotation);

                }
            }

            if (hitparticle != null)
            {
                hitparticle.transform.forward = hit.normal;
                if (ParticleSticky)
                    hitparticle.transform.parent = hit.collider.transform;
                GameObject.Destroy(hitparticle, 7);
            }

            res = true;
            hitcount++;
            if (DestroyWhenHit || hitcount >= HitCountMax || tagDestroyerCheck(hit.collider.tag))
            {
                destroyed = true;

            }

        }
        if (destroyed)
        {
            if (actionPreset)
            {
                actionPreset.OnBulletDestroy();
            }
            GameObject.Destroy(this.gameObject);
        }

        return res;
    }

    private bool targetdetected = false;

    public void RunningDetectTarget()
    {
        RaycastHit[] camerahits;
        camerahits = Physics.RaycastAll(transform.position, transform.forward, RunningRaylength);

        for (var i = 0; i < camerahits.Length; i++)
        {
            RaycastHit hitcam = camerahits[i];
            if (hitcam.collider)
            {
                if (tagCheck(hitcam.collider.tag) && hitcam.collider.tag != this.gameObject.tag)
                {
                    if (hitcam.collider.GetComponent<AS_BulletHiter>())
                    {
                        AS_BulletHiter bulletHit = hitcam.collider.gameObject.GetComponent<AS_BulletHiter>();
                        if (actionPreset && !firsthited && !targetdetected)
                        {
                            actionPreset.TargetDetected(this, bulletHit, hitcam.point);
                            targetdetected = true;
                        }
                    }
                }
            }
        }
    }

    public void FirstDetectTarget()
    {
        RaycastHit[] camerahits;
        camerahits = Physics.RaycastAll(transform.position, transform.forward, DetectorLength);
        for (var i = 0; i < camerahits.Length; i++)
        {
            RaycastHit hitcam = camerahits[i];
            if (hitcam.collider)
            {
                if (tagCheck(hitcam.collider.tag) && hitcam.collider.tag != this.gameObject.tag)
                {
                    if (hitcam.collider.GetComponent<AS_BulletHiter>())
                    {
                        AS_BulletHiter bulletHit = hitcam.collider.gameObject.GetComponent<AS_BulletHiter>();
                        if (actionPreset && !firsthited)
                        {
                            actionPreset.FirstDetected(this, bulletHit, hitcam.point);
                        }
                    }
                }
            }
        }
    }

    void PredictionTrajectory()
    {
        Vector3 gravity = Vector3.zero;
        if (GetComponent<Rigidbody>().useGravity)
        {
            gravity = Physics.gravity;
        }
        int numSteps = (int)DetectorLength;
        float timeDelta = 1.0f / initialVelocity.magnitude;

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        Vector3 lastpos = initialPosition;
        bool targetdetected = false;

        for (int i = 0; i < numSteps && !targetdetected; ++i)
        {
            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;
            targetdetected = RayPrediction(lastpos, position, initialPosition, timeDelta);
            lastpos = position;
        }
    }

    bool RayPrediction(Vector3 lastpos, Vector3 currentpos, Vector3 initialPosition, float delta)
    {
        RaycastHit[] hits;
        Vector3 dir = (currentpos - lastpos);
        dir.Normalize();

        hits = Physics.RaycastAll(lastpos, dir, 1);

        for (var i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            AS_BulletHiter bulletHit = hit.collider.gameObject.GetComponent<AS_BulletHiter>();
            if (bulletHit)
            {
                if (actionPreset && !firsthited)
                {
                    actionPreset.FirstDetected(this, bulletHit, hit.point);
                    return true;
                }
            }

        }
        return false;
    }

}
