using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EnemyGun : MonoBehaviour
{

    public int id = 0;

    public Transform target;

    public float fireRate = 0.2f;

    public int power = 10;

    public int clipSize = 30;

    protected int clip = 30;

    public GunState gunState = GunState.Ready;

    protected float timeFired = 0;

    public float Spread = 10f;

    public Transform muzzleTransform;

    public GameObject Bullet;

    public int bulletNum = 1;

    public GameObject muzzleFlash;

    public AudioClip soundGunFire;

    protected AudioSource audioSource;
    public float liftTimeBullet = 1f;

    public EmenyAttr attr;

    public Transform aimSpine;
    [HideInInspector]
    public Vector3 targetPosition;
    // Use this for initialization
    public virtual void Start()
    {
        if (attr == null)
        {
            attr = GetComponent<EmenyAttr>();
            if (attr == null)
            {
                Debug.LogError("Miss Enemy attr in " + gameObject.name);
            }

        }
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (target.GetComponent<PlayerAttr>())
            {
                target = target.GetComponent<PlayerAttr>().targetAttach;
            }
        }
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
        clip = attr.clip;
    }

    public virtual void Shoot()
    {

        if (timeFired + attr.fireRate < Time.time)
        {
            if (gunState == GunState.Ready)
            {
                PlaySound();
                ShowFlash();
                ShowBullet();

                clip -= 1;
                if (clip <= 0)
                {
                    gunState = GunState.Empty;
                }
                timeFired = Time.time;
            }
        }

    }

    public void Reload()
    {
        clip = attr.clip;
        gunState = GunState.Ready;
    }

    /// <summary>
    /// 选择目标
    /// </summary>
    public virtual void DetachTarget()
    {
        targetPosition = target.position;
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    public virtual void PlaySound()
    {
        if (soundGunFire && audioSource)
        {
            audioSource.PlayOneShot(soundGunFire);
        }
    }

    public virtual void ShowBullet()
    {
        if (muzzleTransform && Bullet && target)
        {
            for (int i = 0; i < bulletNum; i++)
            {
                Vector3 direction = (targetPosition - muzzleTransform.position).normalized ;
                Vector3 dirtectionSpread = new Vector3(Random.Range(-Spread, Spread) /Mathf.Max(1, attr.hitRate), Random.Range(-Spread, Spread) / Mathf.Max(1, attr.hitRate), 0);
                direction += dirtectionSpread;
                GameObject b = GameObject.Instantiate(Bullet, muzzleTransform.position, Quaternion.LookRotation(direction)) as GameObject;
                b.transform.forward = direction;
                var asBullet = b.GetComponent<AS_Bullet>();
                asBullet.source = muzzleTransform;
                asBullet.Damage = attr.power;
                Destroy(b, liftTimeBullet);
            }
        }
    }

    public virtual void ShowFlash()
    {
        if (muzzleFlash && muzzleTransform)
        {
            muzzleFlash.transform.position = muzzleTransform.transform.position;
            muzzleFlash.transform.rotation = muzzleTransform.transform.rotation;
            // Debug.Log(muzzleFlash.transform.position);
        }
        muzzleFlash.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
    }

}
