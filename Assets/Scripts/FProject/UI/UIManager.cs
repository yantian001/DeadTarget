using UnityEngine;
using System.Collections;

namespace FProject
{
    public class UIManager : SingletonMono<UIManager>
    {

        public GameObject pauseUI;

        public void Awake()
        {
            Instance = this;
            vp_Utility.Activate(pauseUI, false);
        }

        #region Pause

        public void TogglePause()
        {
            vp_Utility.Activate(pauseUI, !vp_Utility.IsActive(pauseUI));
            vp_TimeUtility.Paused = !vp_TimeUtility.Paused;

        }

        #endregion
    }
}