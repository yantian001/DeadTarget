using UnityEngine;
using GameDataEditor;

public class EmenyAttr : MonoBehaviour
{

    public int id = 0;
    /// <summary>
    /// 开枪间隔
    /// </summary>
    public float fireRate = 0.2f;
    /// <summary>
    /// 躲避率
    /// 最大为100
    /// </summary>
    [Range(0, 100)]
    public float avoidRate = 60f;
    /// <summary>
    /// 武器伤害值Clip
    /// </summary>
    public int power;
    /// <summary>Power
    /// 弹夹数量
    /// </summary>
    public int clip;
    /// <summary>
    /// 命中率 
    /// 最大为100 ，最小为0
    /// </summary>
    [Range(0, 100)]
    public float hitRate;
    /// <summary>
    /// 最大血量
    /// </summary>
    public int maxHP;

    public int level = 1;

    public void Start()
    {
       // Debug.Log("enemy attr init");
        var data = EnemyAttrManager.Instance.GetEnemyAttr(id, level);
        if(data != null)
        {
            CopyData(data);
        }
    }

    void CopyData(GDEEnemyAttrData data)
    {
        this.maxHP = data.HP;
        this.power = data.Power;
        this.fireRate = data.FireRate;
        this.avoidRate = data.AvoidRate;
        this.hitRate = data.HitRate;
        this.clip = data.Clip;
    }
}
