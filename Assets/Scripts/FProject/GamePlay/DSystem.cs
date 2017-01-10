using UnityEngine;
using System.Collections;

namespace FProject
{
    public class DSystem : MonoBehaviour
    {

        private static DSystem singleton;
        public static DSystem Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = ((DSystem)(FindObjectOfType<DSystem>()));
                    if (singleton == null)
                    {
                        singleton = new GameObject
                        {
                            name = "[@" + typeof(DSystem).Name + "]"
                        }.AddComponent<DSystem>();
                    }
                }
                return singleton;
            }
            set
            {
                singleton = value;
            }
        }


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
            if (singleton == null)
            {
                singleton = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

}