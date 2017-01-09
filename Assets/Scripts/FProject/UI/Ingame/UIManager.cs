using UnityEngine;
using System.Collections;
using System;

namespace FProject
{
    public class UIManager : SingletonMono<UIManager>
    {
        public delegate void VedioCloseEvent(bool b);

        public VedioCloseEvent vedioCloseEvent;
        /// <summary>
        /// 游戏暂停UI
        /// </summary>
        public GameObject pauseUI;
        /// <summary>
        /// 结束前播放视频UI
        /// </summary>
        public GameObject gameoverVideoUI;
        /// <summary>
        /// 游戏失败UI
        /// </summary>
        public GameObject gameoverUI;
        /// <summary>
        /// 游戏结算画面
        /// </summary>
        public GameObject winUI;

        public GameObject controlUI;

        public UISlider zombiesSlider;

        public AudioClip numberCliper;

        protected Coroutine vedioCounterCoroutine = null;

        #region Property
        private UILabel _lbVedioRemain;
        public UILabel lbVedioRemain
        {
            get
            {
                if (!_lbVedioRemain)
                {
                    if (gameoverVideoUI)
                    {
                        _lbVedioRemain = CommonUtils.GetChildComponent<UILabel>(gameoverVideoUI.transform, "lbTimeLeft");
                    }
                }
                return _lbVedioRemain;
            }
        }
        #endregion

        public void Awake()
        {
            Instance = this;
            vp_Utility.Activate(pauseUI, false);
            vp_Utility.Activate(gameoverVideoUI, false);
        }

        #region Pause

        public void TogglePause()
        {
            vp_Utility.Activate(pauseUI, !vp_Utility.IsActive(pauseUI));
            vp_TimeUtility.Paused = !vp_TimeUtility.Paused;

        }

        public void OnRestartClicked()
        {
            //Debug.Log("click restart!");
        }

        #endregion

        #region Show Vedio Before Game Over

        public void ShowGameOverVideo(VedioCloseEvent e)
        {
            //throw new NotImplementedException();
            vp_Utility.Activate(gameoverVideoUI, true);
            this.vedioCloseEvent = e;
            vedioCounterCoroutine = StartCoroutine(VedioCounter(0.5f));
        }

        IEnumerator VedioCounter(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            int timeLeft = 10;
            while (timeLeft >= 0)
            {
                UpdateVedioTime(timeLeft.ToString());
                yield return new WaitForSeconds(1f);
                timeLeft--;
            }
            OnVideoMenuClose();
        }

        void UpdateVedioTime(string str)
        {

            TweenScale.Begin(lbVedioRemain.gameObject, 0.2f, new Vector3(1.2f, 1.2f, 1.2f)).AddOnFinished(() =>
            {
                lbVedioRemain.text = str;
                TweenScale.Begin(lbVedioRemain.gameObject, 0.2f, new Vector3(1.0f, 1.0f, 1.0f));
            });

        }

        public void OnVideoMenuClose()
        {
            CloseVedioMenu(false);
        }

        public void OnVedioPlayClicked()
        {
            CloseVedioMenu(true);
        }

        void CloseVedioMenu(bool b)
        {
            if (vedioCounterCoroutine != null)
            {
                StopCoroutine(vedioCounterCoroutine);
                vedioCounterCoroutine = null;
            }
            vp_Utility.Activate(gameoverVideoUI, false);
            if (vedioCloseEvent != null)
            {
                vedioCloseEvent(b);
            }
        }

        #endregion

        #region Game Over UI

        public void ShowGameOverUI()
        {
            vp_Utility.Activate(gameoverUI);
        }
        #endregion

        #region Game Win UI
        public void ShowWinUI(GameRecord record)
        {
            //throw new NotImplementedException();
            vp_Utility.Activate(winUI);
            var panel = winUI.GetComponent<UIPanel>();
            panel.alpha = 0;
            TweenAlpha.Begin(winUI, 1f, 1).AddOnFinished(() =>
            {
                //显示zombile Kill reward
                UILabel zombiekill = CommonUtils.GetChildComponent<UILabel>(winUI.transform, "zongshuchu/ZombieKillBonus/Value");
                StartCoroutine(DynamicDisplayMoeny(zombiekill, record.ZombieKillReward, () =>
                {
                    UILabel comboreward = CommonUtils.GetChildComponent<UILabel>(winUI.transform, "zongshuchu/ComboBonus/Value");
                    StartCoroutine(DynamicDisplayMoeny(comboreward, record.MaxComboReward, () =>
                    {
                        UILabel headshowReward = CommonUtils.GetChildComponent<UILabel>(winUI.transform, "zongshuchu/HeadshotBonus/Value");
                        StartCoroutine(DynamicDisplayMoeny(headshowReward, record.HeadShotReward, () =>
                        {
                            UILabel totalReward = CommonUtils.GetChildComponent<UILabel>(winUI.transform, "zongshuchu/TotalValue");
                            StartCoroutine(DynamicDisplayMoeny(totalReward, record.TotalReward, () =>
                            {

                            }));
                        }));
                    }));
                }));
            });
        }

        IEnumerator DynamicDisplayMoeny(UILabel text, int to, System.Action onFinish, int from = 0, int time = 10)
        {
            if (numberCliper)
            {
                LeanAudio.play(numberCliper);
            }
            if (text != null)
            {
                int diff = to - from;
                //int normal = Mathf.CeilToInt((float)diff / time);
                while (from != to)
                {
                    if (Mathf.Abs(to - from) >= time)
                    {
                        from += time;
                    }
                    else
                        from = to;

                    text.text = from.ToString();

                    //yield return new  WaitForSeconds(0.05f);
                    yield return null;
                }
            }
            if (onFinish != null)
                onFinish();
        }

        public void UpdateZombieSlider(int _currentSpwanedZombies, int totalZombies)
        {
            // throw new NotImplementedException();
            if (zombiesSlider)
            {
                zombiesSlider.value = (float)_currentSpwanedZombies / totalZombies;
            }
        }

        #endregion
    }
}