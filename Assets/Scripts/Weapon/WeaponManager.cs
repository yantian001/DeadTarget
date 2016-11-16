using UnityEngine;
using GameDataEditor;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{

    #region 单例
    private static WeaponManager _instance;
    public static WeaponManager Instance
    {
        private set
        {
            _instance = value;
        }
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeaponManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("WeaponManagerContainer");
                    _instance = obj.AddComponent<WeaponManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    List<GDEWeaponData> weapons;

    GDEWeaponData currentWeapon;

    GDEBombData bombData;

    GDEMedicalData medicalData;

    #region Monobehavior
    void Awake()
    {

        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
            Init();
            //BuyWeapon(0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Method

    void Init()
    {
        weapons = new List<GDEWeaponData>();
        //if (GDEDataManager.Init("gde_data"))
        GDEDataManager.Init("gde_data");
        {
            List<string> wsKeys;
            //weapon 
            GDEDataManager.GetAllDataKeysBySchema("Weapon", out wsKeys);

            for (int i = 0; i < wsKeys.Count; i++)
            {
                GDEWeaponData wd = null;
                GDEDataManager.DataDictionary.TryGetCustom(wsKeys[i], out wd);

                if (wd != null)
                {
                   // wd.ResetAll();
                    weapons.Add(wd);
                    if (wd.isEquipment)
                        currentWeapon = wd;
                }
            }

            //Bomb
            // GDEBombData bombData = null;
            GDEDataManager.DataDictionary.TryGetCustom(GDEItemKeys.Bomb_Bomb, out bombData);
            GDEDataManager.DataDictionary.TryGetCustom(GDEItemKeys.Medical_MedicalBox, out medicalData);
           
        }
    }

    /// <summary>
    /// 获取炸弹数量
    /// </summary>
    /// <param name="def"></param>
    /// <returns></returns>
    public int GetBombCount(int def = 0)
    {
        if(bombData != null)
        {
            def = bombData.number;
        }
        return Mathf.Max(0, def);
    }
    /// <summary>
    /// 使用炸弹
    /// </summary>
    /// <param name="u"></param>
    public void UseBomb(int u = 1)
    {
        if(bombData != null && bombData.number > 0)
        {
            bombData.number -= u;
        }
    }
    /// <summary>
    /// 获取医疗包的数量
    /// </summary>
    /// <param name="def"></param>
    /// <returns></returns>
    public int GetMedkitCount(int def = 0)
    {
        if(medicalData != null)
        {
            def = medicalData.number;
        }
        return Mathf.Max(0, def);
    }

    /// <summary>
    /// 使用医疗包
    /// </summary>
    /// <param name="u"></param>
    public void UseMedkit(int u = 1)
    {
        if (medicalData != null && medicalData.number > 0)
        {
            medicalData.number -= u;
        }
    }

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="id"></param>
    public void EqWeaon(int id)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Id != id)
            {
                weapons[i].isEquipment = false;
            }
            else
            {
                weapons[i].isEquipment = true;
                currentWeapon = weapons[i];
            }
        }
    }

    /// <summary>
    /// 购买武器
    /// </summary>
    /// <param name="id">武器id</param>
    /// <returns>返回是否购买成功</returns>
    public bool BuyWeapon(int id)
    {

        GDEWeaponData w = weapons.Find((p) => { return p.Id == id; });
        if (w != null)
        {
            if (w.isowned)
            {
                return true;
            }
            if (Player.CurrentUser.IsMoneyEnough(ConvertUtil.ToInt32(w.cost, 1000000)))
            {
                Player.CurrentUser.UseMoney(ConvertUtil.ToInt32(w.cost, 1000000));
                w.isowned = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region 判断是否达到要求
    /// <summary>
    /// 判断武器属性是否达到要求 
    /// </summary>
    /// <param name="attrid">属性值id</param>
    /// <param name="val">要求属性</param>
    /// <returns></returns>
    public bool IsWeaponMeetReq(int attrid, float val)
    {
        if (currentWeapon == null)
        {
            return false;
        }
        return true;
       // return currentWeapon.GetAttributeCurrentVal(attrid) >= val;
    }

    #endregion

    void RefreshGun()
    {
        for(int i=0;i<weapons.Count;i++)
        {
            if(weapons[i].isEquipment)
            {
                currentWeapon = weapons[i];
            }
        }
    }

    /// <summary>
    /// 获取当前武器ID
    /// </summary>
    /// <returns></returns>
    public int GetCurrentWeaponId()
    {
        Init();
        if (currentWeapon != null)
        {
            return currentWeapon.Id;
        }
        else
            return -1;
    }

    /// <summary>
    /// 通过ID 获取武器
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GDEWeaponData GetWeaponById(int id)
    {
        GDEWeaponData wd = weapons.Find(p => { return p.Id == id; });
        return wd;
    }
}
