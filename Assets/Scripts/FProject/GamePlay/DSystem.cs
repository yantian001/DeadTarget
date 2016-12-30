using UnityEngine;
using System.Collections;

namespace FProject
{
    public class DSystem : SingletonMono<DSystem>
    {
        static public string s_CurrentSceneName;
        /// <summary>
        /// 当前的场景编号
        /// </summary>
        public int currentScene = 1;
        /// <summary>
        /// 当前的关卡数
        /// </summary>
        public int currentLevel = 1;

        public string sceneName = "";
        public void Awake()
        {
            if (SingletonMono<DSystem>.Instance == null)
            {
                SingletonMono<DSystem>.Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
    }

}