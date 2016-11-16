using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;

public class TaskManager : MonoBehaviour
{

    private List<GDELevelData> levels = new List<GDELevelData>();

    private static TaskManager _instance;

    public static TaskManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TaskManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("TaskManagerHandler");
                    _instance = go.AddComponent<TaskManager>();
                }
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitFromGDE();
            //for test
            GameValue.taskData = GetTask(1, 1);
        }
        else
        {
            Destroy(gameObject);
        }



    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.GAMEFINISH, OnGameFinish);

    }


    void OnGameFinish(LTEvent evt)
    {
        GameRecords record = evt.data as GameRecords;
        if (record != null)
        {
            if (record.FinishType == GameFinishType.Completed)
            {
                GameValue.taskData.isCleared = true;
                UnlockTask(GameValue.level, GameValue.taskData.TaskNum + 1);
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void InitFromGDE()
    {

        GDEDataManager.Init("gde_data");
        {
            List<string> lstLevelName;
            GDEDataManager.GetAllDataKeysBySchema("Level", out lstLevelName);
            for (int i = 0; i < lstLevelName.Count; i++)
            {
                GDELevelData level = null;
                if (GDEDataManager.DataDictionary.TryGetCustom(lstLevelName[i], out level))
                {
                    levels.Add(level);
                }
            }
        }
    }

    public GDETaskData GetTask(int level, int task)
    {
        GDETaskData taskReturn = null;
        var l = levels.Find(p => p.LevelNum1 == level);
        if (l != null)
        {
            taskReturn = l.TaskList.Find(p => p.TaskNum == task);
        }
        return taskReturn;
    }
    /// <summary>
    /// 获得当前任务场景
    /// </summary>
    /// <returns></returns>
    public GDELevelData GetCurrentLevel()
    {
        GDELevelData levelCurrent = null;
        for (int i = 0; i < levels.Count; i++)
        {
            levelCurrent = levels[i];
            if (levelCurrent.TaskList.Find(p => { return (!p.isCleared) && (!p.isLocked); }) != null)
            {
                break;
            }

        }
        return levelCurrent;
    }

    /// <summary>
    /// 获取关卡
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public GDELevelData GetLevel(int level)
    {
        return levels.Find(p => { return p.LevelNum1 == level; });
    }

    void UnlockTask(int level, int tasknum)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            var task = levels[i].TaskList.Find(p => { return p.TaskNum == tasknum; });
            if (task != null)
            {
                task.isLocked = false;
                break;
            }
            //if(levels[i].TaskList.fi)
        }
    }
}
