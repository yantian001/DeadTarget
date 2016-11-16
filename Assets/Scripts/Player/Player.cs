using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

public class SceneResult
{
	public int id;
	public int currentLevel;
	public int randomLevel = -1;
	public bool bossFinished;
	public SceneResult()
	{}

	public SceneResult(string str)
	{
		if (!string.IsNullOrEmpty (str)) {
			string[] strs = str.Split(new char[]{','});
			id = ConvertUtil.ToInt32(strs[0]);
			currentLevel = ConvertUtil.ToInt32(strs[1]);
			randomLevel = ConvertUtil.ToInt32(strs[2]);
			bossFinished = strs[3] == "1";
		}
	}

	public string FormatString()
	{
		return string.Format("{0},{1},{2},{3}",id,currentLevel,randomLevel,bossFinished ? 1:0);

	}
}

public class Player : MonoBehaviour
{

    public int Money;
    /// <summary>
    /// 上次played的场景
    /// </summary>
    public int LastPlayedScene
    {
        get
        {
            return PlayerPrefs.GetInt("lastScene", 0);
        }
        set
        {
            SetKeyIntValue("lastScene", value);
        }
    }
    private static Player _instance = null;

    public static Player CurrentUser
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
                if (_instance == null)
                {
                    GameObject p = new GameObject("PlayerHandler");
                    _instance = p.AddComponent<Player>();
                }
            }
            return _instance;
        }
        private set
        {

        }
    }

    public void SetKeyIntValue(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
        PlayerPrefs.Save();
    }
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
		sceneResults = new List<SceneResult> ();
		//if (sceneResults == null) {
			string jsonStr = PlayerPrefs.GetString("sceneresult","");
			if(string.IsNullOrEmpty(jsonStr))
			{
				sceneResults = new List<SceneResult>();
			}
			else
			{
//				sceneResults = (List<SceneResult>)Json.Deserialize(jsonStr);
				sceneResults = DeserializeSceneResult(jsonStr);
			}
		//

        if (!PlayerPrefs.HasKey("money"))
        {
            UseMoney(-50);
        }
        Money = PlayerPrefs.GetInt("money", 0);


    }

	/// <summary>
	/// Deserializes the scene result.
	/// </summary>
	/// <returns>The scene result.</returns>
	/// <param name="str">String.</param>
	public List<SceneResult> DeserializeSceneResult(string str)
	{
		List<SceneResult> results = new List<SceneResult> ();
		if (!string.IsNullOrEmpty (str)) {
			string[] strs = str.Split(new char[]{';'});
			for(int i =0;i<strs.Length;i++)
			{
				results.Add(new SceneResult(strs[i]));
			}
		}
		return results;
	}
	/// <summary>
	/// Serializes the scene result.
	/// </summary>
	/// <returns>The scene result.</returns>
	/// <param name="results">Results.</param>
	public string SerializeSceneResult(List<SceneResult> results)
	{
		string res = "";
		if (results != null) {
			for(int i=0;i<results.Count;i++)
			{
				if(results[i] != null){
					res+= results[i].FormatString();
					if(i!=results.Count -1)
					{
						res +=";";
					}
				}
			}
		}
		return res;
	}

	#region scene results
	public List<SceneResult> sceneResults;

	/// <summary>
	/// Gets the scene current level.
	/// </summary>
	/// <returns>The scene current level.</returns>
	/// <param name="scene">Scene id</param>
	/// <param name="total">Total scene level count</param>
	public int GetSceneCurrentLevel(int scene,int total = -1)
	{
		SceneResult result = GetSceneResult (scene);
		if (total == -1) {
			return result.currentLevel + 1;
		} else {
			return Mathf.Min(result.currentLevel + 1,total);
		}
	}

	/// <summary>
	/// Scenes the level complete.
	/// </summary>
	/// <param name="scene">Scene.</param>
	/// <param name="type">Type.</param>
	public void SceneLevelComplete(int scene,LevelType type)
	{
		SceneResult result = GetSceneResult (scene);
		switch (type) {
		case LevelType.BossTask:
			result.bossFinished = true;
			break;
		case LevelType.LoopTask:
			result.randomLevel = -1;
			break;
		case LevelType.MainTask:
			result.currentLevel += 1;
			break;
		}
		SaveSceneResult2File ();
	}
	/// <summary>
	/// Gets the scene random level.
	/// </summary>
	/// <returns>The scene random level.</returns>
	/// <param name="scene">Scene.</param>
	public int GetSceneRandomLevel(int scene)
	{
		SceneResult result = GetSceneResult (scene);
		int level = result.randomLevel;

		if (level == -1) {
			level = Random.Range(0,result.currentLevel);
			result.randomLevel = level;
			SaveSceneResult2File();
		}
		return level;
	}
	/// <summary>
	/// Gets the scene boss level finished.
	/// </summary>
	/// <returns><c>true</c>, if scene boss level finished was gotten, <c>false</c> otherwise.</returns>
	/// <param name="scene">Scene.</param>
	public bool GetSceneBossLevelFinished(int scene)
	{
		SceneResult result = GetSceneResult (scene);
		return result.bossFinished;
	}
	/// <summary>
	/// Determines whether this instance is main task completed the specified scene total.
	/// </summary>
	/// <returns><c>true</c> if this instance is main task completed the specified scene total; otherwise, <c>false</c>.</returns>
	/// <param name="scene">Scene.</param>
	/// <param name="total">Total.</param>
	public bool IsMainTaskCompleted(int scene,int total)
	{
		SceneResult result = GetSceneResult (scene);
		return result.currentLevel + 1 >= total;
	}
	/// <summary>
	/// Gets the scene result.
	/// </summary>
	/// <returns>The scene result.</returns>
	/// <param name="scene">scene id</param>
	public SceneResult GetSceneResult(int scene)
	{
		SceneResult sr = sceneResults.Find (p => {
			return p.id == scene;
		});

		if (sr == null) {
			sr = new SceneResult();
            sr.id = scene;
			sceneResults.Add(sr);
			SaveSceneResult2File();
		}
		return sr;
	}
	/// <summary>
	/// Saves the scene result2 file.
	/// </summary>
	public void SaveSceneResult2File()
	{
		string strJson = SerializeSceneResult (sceneResults);
		PlayerPrefs.SetString ("sceneresult", strJson);
		PlayerPrefs.Save ();
	}

	#endregion

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.MONEYUSED, OnMoneyUsed);
        
    }

    private void OnGameFinish(LTEvent obj)
    {
        //throw new NotImplementedException();
        if (obj.data != null)
        {
            var record = obj.data as GameRecords;
            if (record != null)
            {
                UseMoney(-GameValue.moneyPerTimeLeft * record.TimeLeft);
                if (record.FinishType == GameFinishType.Completed)
                {
                    SetLevelRecord(record.MapId, record.Level);
                }
            }
        }
    }

    public void SetLevelRecord(int mapid, int level)
    {
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.MONEYUSED, OnMoneyUsed);
        //  LeanTween.removeListener((int)Events.GAMEFINISH, OnGameFinish);
    }

    void OnMoneyUsed(LTEvent evt)
    {
        if (evt.data != null)
        {
            UseMoney(ConvertUtil.ToInt32(evt.data, 0));
        }
    }

    /// <summary>
    /// 使用金钱
    /// </summary>
    /// <param name="moneyUse"></param>
    public void UseMoney(int moneyUse)
    {
        Money -= moneyUse;
        if (Money <= 0)
            Money = 0;
        PlayerPrefs.SetInt("money", Money);
        PlayerPrefs.Save();
        LeanTween.dispatchEvent((int)Events.MONEYCHANGED);
    }
    /// <summary>
    /// 判断金币是否足够
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public bool IsMoneyEnough(int money)
    {
        return Money >= money;
    }
  
}
