using UnityEngine;
using GameDataEditor;


public enum GunState
{
    Ready,
    Reloading,
    Empty
}

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    public int id = 0;
    public bool Active = true;
    public GameObject Bullets; // Bullet prefeb
    public float LifeTimeBullet = 5;
    public GameObject Shell; // Shell prefeb
    public Transform ShellSpawn; // shell spawing position
    public Camera NormalCamera;// FPS camera
    public float FireRate = 0.2f;
    public float CooldownTime = 0.8f;
    public float BoltTime = 0.35f;
    public bool HideGunWhileZooming = true;
    public AudioClip SoundGunFire;
    public AudioClip SoundBoltEnd;
    public AudioClip SoundBoltStart;
    public AudioClip SoundReloadStart;
    public AudioClip SoundReloadEnd;
    public AudioClip SoundEmpty;

    public float Power = 0;
    public bool SemiAuto;
    public bool InfinityAmmo = true;
    public int BulletNum = 1;
    public float Spread = 0;
    public int Clip = 30;
    public int ClipSize = 30;
    public int AmmoIn = 1;
    public int AmmoPack = 90;
    public int AmmoPackMax = 90;
    public float stab = 1;

    private float timefire = 0;

    [HideInInspector]
    public GunState gunState = GunState.Ready;
    private AudioSource audiosource;
    [HideInInspector]

    private float cooldowntime = 0;

    public Transform muzzleTransform;
    public GameObject muzzleFlash;

    public void Awake()
    {
        GDEWeaponData weapon = WeaponManager.Instance.GetWeaponById(id);
        //Power = weapon.huoli;
        //Clip = ClipSize = weapon.danjia;
        //FireRate = 1f / weapon.shesu;
        //stab = weapon.wendingxing;
        //if (stab <= 0)
        //    stab = 10;


    }

    public void Start()
    {

        if (GetComponent<AudioSource>())
        {
            audiosource = GetComponent<AudioSource>();
        }

        if (!NormalCamera)
        {
            NormalCamera = Camera.main;
        }
    }

    public void OnEnable()
    {
        if (!NormalCamera)
        {
            NormalCamera = Camera.main;
        }
    }

    public void SetActive(bool active)
    {
        Active = active;
        this.gameObject.SetActive(active);

    }

    public void Reload()
    {
        Clip = ClipSize;
        gunState = GunState.Ready;
    }

    /// <summary>
    /// 是否满子弹
    /// </summary>
    /// <returns></returns>
    public bool IsFullClip()
    {
        return Clip == ClipSize;
    }


    public bool Shoot()
    {
        if (!Active)
            return false;

        if (timefire + FireRate < Time.time)
        {
            if (gunState == GunState.Ready)
            {
                if (SoundGunFire && audiosource != null)
                {
                    audiosource.PlayOneShot(SoundGunFire);
                }

                for (int i = 0; i < BulletNum; i++)
                {
                    if (Bullets)
                    {
                        Vector3 point = NormalCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

                        GameObject bullet = (GameObject)Instantiate(Bullets, point, Quaternion.LookRotation(NormalCamera.transform.forward));
                        var asBuulet = bullet.GetComponent<AS_Bullet>();
                        if (asBuulet)
                        {
                            asBuulet.Damage = ConvertUtil.ToInt32(Power);
                        }
                        bullet.transform.forward = NormalCamera.transform.forward + new Vector3(Random.Range(-Spread / 10 * stab, Spread / 10 * stab), Random.Range(-Spread / 10 * stab, Spread / 10 * stab), Random.Range(-Spread / 10 * stab, Spread / 10 * stab));

                        Destroy(bullet, LifeTimeBullet);
                    }
                }
                ShowFlash();
                timefire = Time.time;
                cooldowntime = Time.time;

                if (Shell && ShellSpawn)
                {
                    GameObject shell = (GameObject)Instantiate(Shell, ShellSpawn.position, ShellSpawn.rotation);
                    shell.GetComponent<Rigidbody>().AddForce(ShellSpawn.transform.right * 2);
                    shell.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * 10);
                    GameObject.Destroy(shell, 5);
                }
                if (Clip > 0)
                {
                    Clip -= 1;
                }
                else
                {
                    gunState = GunState.Empty;
                }
                if (Clip <= 0)
                {
                    gunState = GunState.Empty;
                }
                return true;
            }
            else if (gunState == GunState.Empty)
            {
                audiosource.PlayOneShot(SoundEmpty);
            }
        }
        return false;
    }

    /// <summary>
    /// 显示弹壳
    /// </summary>
    public void ShowShell()
    {
        if (Shell && ShellSpawn)
        {
            GameObject shell = (GameObject)Instantiate(Shell, ShellSpawn.position, ShellSpawn.rotation);
            shell.GetComponent<Rigidbody>().AddForce(ShellSpawn.transform.right * 2);
            shell.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * 10);
            GameObject.Destroy(shell, 5);
        }
    }

    /// <summary>
    /// 显示枪口特效
    /// </summary>
    public void ShowFlash()
    {
        if (muzzleFlash && muzzleTransform)
        {
            muzzleFlash.transform.position = muzzleTransform.transform.position;
            muzzleFlash.transform.rotation = muzzleTransform.transform.rotation;
        }
        muzzleFlash.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
    }
}
