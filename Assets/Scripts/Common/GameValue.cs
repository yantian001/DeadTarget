using GameDataEditor;

public enum LevelType
{
    /// <summary>
    /// The main task.
    /// </summary>
    MainTask,
    /// <summary>
    /// The loop task.
    /// </summary>
    LoopTask,
    /// <summary>
    /// The boss task.
    /// </summary>
    BossTask,
}


public class GameValue
{

    public static string s_CurrentSceneName = "";

    //public static bool s_IsRandomObjective = false;

    public static LevelType s_LevelType = LevelType.MainTask;


    public static int mapId = -1;
    /// <summary>
    /// current level
    /// </summary>
    public static int level = -1;

    public static string diffdegree = null;

    /// <summary>
    /// 当前任务的关卡信息
    /// </summary>
    public static GDETaskData taskData = null;

	public static string shopType = "weapon";

    public static string GetMapSceneName()
    {
        if (mapId == 1)
        {
            return "changjing-1";
        }
        else if (mapId == 2)
        {
            return "changjing-2";

        }
        else if (mapId == 3)
        {
            return "changjing-3";
        }
        return "";
    }

    public static int maxLevelPerMap = 40;

    public static int[] MapLevelConfig = new int[] { 40, 40, 40 };

    public static int moneyPerTimeLeft = 5;

    public static GameStatu staus = GameStatu.Init;

    public static bool IsMapLastLevel(int mapid, int levelid)
    {
        if (mapid > MapLevelConfig.Length)
        {
            return true;
        }
        else
        {
            return levelid >= MapLevelConfig[mapid - 1];
        }
    }

    public static int GetMapLevelCount(int mapid)
    {
        if (mapid > MapLevelConfig.Length)
            return 0;
        else
            return MapLevelConfig[mapid - 1];
    }


}
