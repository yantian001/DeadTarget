using UnityEngine;
using BehaviorDesigner.Runtime;
public class DamageManager : MonoBehaviour
{

    public GameObject[] deadbody;
    public AudioClip[] hitsound;
    public int hp = 100;
    public int Score = 10;
    private float distancedamage;
    private bool isDied = false;
    public bool isEnemy = true;

    public float tipsTime = 0.5f;

    public float tipsSpeed = 100f;

    public float hight = 2;

    public Texture2D hpSliderBg;

    public Texture2D hpSlider;
    float tipStartTime = 0f;

    bool showHitTips = false;
    public BehaviorTree behavior;
    public int maxHp = 0;
    HitPosition hpos;
    public int depth = 0;
    public EmenyAttr attr;

    AudioSource source;

    float hpSliderDisplayBeginTime = 0;

    public void Awake()
    {
        maxHp = hp;
    }

    void Start()
    {
        if (behavior == null)
        {
            behavior = GetComponent<BehaviorTree>();
        }
        if (isEnemy)
        {
            if (attr == null)
            {
                attr = GetComponent<EmenyAttr>();
            }
            if (attr)
            {
                maxHp = attr.maxHP;
                hp = maxHp;
            }
        }
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hp <= 0 && !isDied)
        {
            Dead(Random.Range(0, deadbody.Length));
            isDied = true;
            showHitTips = true;
            tipStartTime = Time.time;
        }
        if (showHitTips)
        {
            if (tipStartTime + tipsTime <= Time.time)
                showHitTips = false;
        }
    }

    public void ApplyDamage(int damage, Vector3 velosity, float distance)
    {
        ////  Debug.Log("hp :" + hp + " Damage :" + damage);
        if (hp <= 0)
        {
            return;
        }
        distancedamage = distance;
        hp -= damage;
        if (hp <= 0)
        {
            Dead(0);
        }
        //Debug.Log(damage);
    }

    public void ApplyDamage(int damage, Vector3 velosity, float distance, int suffix)
    {
        if (hp <= 0)
        {
            return;
        }
        distancedamage = distance;
        hp -= damage;
        if (hp <= 0)
        {
            Dead(suffix);
        }
        DetermineCover();

    }

    void DetermineCover()
    {
        if (isEnemy && attr)
        {
            if (Random.Range(0, 100) > attr.avoidRate)
            {
                behavior.SetVariableValue("isNeedCover", true);
            }
        }
    }


    public void ApplyDamage(int damage, Vector3 velosity, float distance, int suffix, HitPosition hitPos)
    {
        hpos = hitPos;
        if (hp <= 0)
        {
            return;
        }
        distancedamage = distance;
        hp -= damage;
        if (hp <= 0)
        {
            //behavior.SetVariableValue("IsDead", true);
            Dead(0);
        }
        DetermineCover();
    }

    public void Dead(int suffix, HitPosition hitPos)
    {
        // throw new NotImplementedException();
        PlaySound();
        if (isEnemy)
        {
            if (deadbody.Length > 0 && suffix >= 0 && suffix < deadbody.Length)
            {
                // this Object has removed by Dead and replaced with Ragdoll. the ObjectLookAt will null and ActionCamera will stop following and looking.
                // so we have to update ObjectLookAt to this Ragdoll replacement. then ActionCamera to continue fucusing on it.
                GameObject deadReplace = (GameObject)Instantiate(deadbody[suffix], this.transform.position, this.transform.rotation);
                // copy all of transforms to dead object replaced
                CopyTransformsRecurse(this.transform, deadReplace);
                // destroy dead object replaced after 5 sec
                Destroy(deadReplace, 3);
                // destry this game object.
                Destroy(this.gameObject, 1);
                this.gameObject.SetActive(false);

            }
            AfterDead(suffix);
        }
        else
        {
            //  LeanTween.rotateZ(transform.root.gameObject, 90, 0.5f);
            if (deadbody.Length > 0 && suffix >= 0 && suffix < deadbody.Length)
            {
                // this Object has removed by Dead and replaced with Ragdoll. the ObjectLookAt will null and ActionCamera will stop following and looking.
                // so we have to update ObjectLookAt to this Ragdoll replacement. then ActionCamera to continue fucusing on it.
                GameObject deadReplace = (GameObject)Instantiate(deadbody[suffix], this.transform.position, this.transform.rotation);
                // copy all of transforms to dead object replaced
                CopyTransformsRecurse(this.transform, deadReplace);
                // destroy dead object replaced after 5 sec
                // Destroy(deadReplace, 5);
                // destry this game object.
                //  Destroy(this.gameObject, 1);
                this.gameObject.SetActive(false);

            }
            LeanTween.dispatchEvent((int)Events.PLAYERDIE);
        }
    }

    void PlaySound()
    {
        if (hitsound.Length > 0)
        {
            //source.PlayOneShot(hitsound[Random.Range(0, hitsound.Length)]);
            LeanAudio.play(hitsound[Random.Range(0, hitsound.Length)]);
        }

    }

    public void AfterDead(int suffix)
    {

        EnemyDeadInfo edi = new EnemyDeadInfo();
        edi.score = Score;
        edi.transform = this.transform;
        //edi.headShot = suffix == 2;
        edi.hitPos = hpos;
        //  edi.animal = this.GetComponent<Animal>();
        LeanTween.dispatchEvent((int)Events.ENEMYDIE, edi);
    }
    /// <summary>
    /// 是否受伤
    /// </summary>
    /// <returns></returns>
    public bool IsInjured()
    {
        if (isDied)
            return false;
        if (hp < maxHp)
            return true;
        return false;
    }

    public bool IsDie()
    {
        return isDied;
    }

    //获得当前血量的百分比
    public float GetHPPrectage()
    {
        return (float)hp / maxHp;
    }

    public void Dead(int suffix)
    {
        PlaySound();
        if (isEnemy)
        {
            if (deadbody.Length > 0 && suffix >= 0 && suffix < deadbody.Length)
            {
                // this Object has removed by Dead and replaced with Ragdoll. the ObjectLookAt will null and ActionCamera will stop following and looking.
                // so we have to update ObjectLookAt to this Ragdoll replacement. then ActionCamera to continue fucusing on it.
                GameObject deadReplace = (GameObject)Instantiate(deadbody[suffix], this.transform.position, this.transform.rotation);
                // copy all of transforms to dead object replaced
                CopyTransformsRecurse(this.transform, deadReplace);
                // destroy dead object replaced after 5 sec
                Destroy(deadReplace, 3);
                // destry this game object.
                Destroy(this.gameObject, 1);
                this.gameObject.SetActive(false);

            }
            AfterDead(suffix);
        }
        else
        {
            if (deadbody.Length > 0 && suffix >= 0 && suffix < deadbody.Length)
            {
                // this Object has removed by Dead and replaced with Ragdoll. the ObjectLookAt will null and ActionCamera will stop following and looking.
                // so we have to update ObjectLookAt to this Ragdoll replacement. then ActionCamera to continue fucusing on it.
                GameObject deadReplace = (GameObject)Instantiate(deadbody[suffix], this.transform.position, this.transform.rotation);
                // copy all of transforms to dead object replaced
                CopyTransformsRecurse(this.transform, deadReplace);
                // destroy dead object replaced after 5 sec
                // Destroy(deadReplace, 5);
                // destry this game object.
                //  Destroy(this.gameObject, 1);
                this.gameObject.SetActive(false);

            }
            LeanTween.dispatchEvent((int)Events.PLAYERDIE);
            // LeanTween.rotateZ(transform.root.gameObject, 90, 0.5f);
        }
    }

    // Copy all transforms to Ragdoll object
    public void CopyTransformsRecurse(Transform src, GameObject dst)
    {
        //Debug.Log(src.localPosition);
        dst.transform.position = src.position;
        dst.transform.rotation = src.rotation;
        foreach (Transform child in dst.transform)
        {
            var curSrc = src.Find(child.name);
            if (curSrc)
            {
                CopyTransformsRecurse(curSrc, child.gameObject);
            }
        }
    }

    /// <summary>
    /// 加血包
    /// </summary>
   public void AddMedkit()
    {
        hp = Mathf.Min(maxHp,hp + maxHp / 2);
    }
}
