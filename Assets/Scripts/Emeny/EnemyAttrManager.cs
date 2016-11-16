using UnityEngine;
using GameDataEditor;
using System.Collections.Generic;

public class EnemyAttrManager : MonoBehaviour
{

    private static EnemyAttrManager _instance = null;

    private List<GDEEnemyAttrData> enemyAttrs = new List<GDEEnemyAttrData>();

    public static EnemyAttrManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyAttrManager>();
            }
            if (_instance == null)
            {
                GameObject obj = new GameObject("EnemyAttrMgrContainer");

                _instance = obj.AddComponent<EnemyAttrManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void OnEnable()
    {
        Debug.Log("on enable!");
        InitFromGDE();
    }
    

    // Use this for initialization
    void Start()
    {
        //Debug.Log("start");
        
    }

    /// <summary>
    /// 初始化敌人属性数据
    /// </summary>
    void InitFromGDE()
    {
        if (!GDEDataManager.Init("gde_data"))
        {
            Debug.Log("init gde data manager error @ EnemyAttrManager");
        }

        List<string> lstKeyNames = new List<string>();
        if (GDEDataManager.GetAllDataKeysBySchema("EnemyAttr", out lstKeyNames))
        {
            enemyAttrs.Clear();
            for (int i = 0; i < lstKeyNames.Count; i++)
            {
                GDEEnemyAttrData data;
                if (GDEDataManager.DataDictionary.TryGetCustom(lstKeyNames[i], out data))
                {
                    enemyAttrs.Add(data);
                }
            }

        }
    }

    /// <summary>
    /// 获取敌人的属性
    /// </summary>
    /// <param name="id">敌人的id</param>
    /// <param name="level">敌人的level</param>
    /// <returns></returns>
    public GDEEnemyAttrData GetEnemyAttr(int id, int level)
    {
        return enemyAttrs.Find(p => { return p.ID == id && p.Level == level; });
    }

}
