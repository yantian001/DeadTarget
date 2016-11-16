using UnityEngine;
using System.Collections;

/// <summary>
/// 数据记录类
/// </summary>
public class GameRecords
{

    public int MapId { get; set; }
    public int LevelDiffcutly { get; private set; }
    public int Level = 1;

    private int _enemyKills = 0;
    /// <summary>
    /// 杀敌数
    /// </summary>
    public int EnemyKills
    {
        get { return _enemyKills; }
        set { _enemyKills = value; }
    }

    /// <summary>
    /// 爆头数
    /// </summary>
    private int _headShotCount = 0;
    public int HeadShotCount
    {
        get
        {
            return _headShotCount;
        }
        set
        {
            _headShotCount = value;
        }
    }

    private int _maxCombos;
    /// <summary>
    /// 最大连击数
    /// </summary>
    public int MaxCombos
    {
        get { return _maxCombos; }
        set { if (value > _maxCombos) _maxCombos = value; }
    }

    public GameFinishType FinishType { get; set; }

    public int TimeLeft { get; set; }
}


