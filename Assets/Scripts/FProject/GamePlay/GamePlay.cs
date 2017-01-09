using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using System;
using Random = UnityEngine.Random;
namespace FProject
{
    public enum GameStatu
    {
        None,
        Init,
        InGame,
        WaitForVedio,
        GamePaused,
        GameFailed,
        GameSuccess
    }

    public class GamePlay : SingletonMono<GamePlay>
    {
        #region Static Member

        public readonly static Vector3 ZOMBIE_HIDDEN_PLACE = new Vector3(1000, 1000, 1000);

        #endregion

        #region Member


        private List<List<GDEMissionRecordData>> lstConfigMissionRecord = new List<List<GDEMissionRecordData>>();
        private float currentBiasHealth = 1f;
        int numberWaveBallte = 5;
        /// <summary>
        /// 当前Active的zombie的数量
        /// </summary>
        int currentZombieActive = 0;
        /// <summary>
        /// 当前剩余许产生的Zombie的数量
        /// </summary>
        int currentZombieSpwanRemain = 0;
        /// <summary>
        /// 当前alive的Zombie的数量
        /// </summary>
        int currentZombieAlive = 0;
        /// <summary>
        /// 各种Zombie Active的数量统计
        /// </summary>
        Dictionary<int, int> activeZombieCounts = new Dictionary<int, int>();
        /// <summary>
        /// 当前波数
        /// </summary>
        int currentWaveIndex = -1;
        /// <summary>
        /// 
        /// </summary>
        bool isWin = false;
        /// <summary>
        /// 时间的目标缩放倍数
        /// </summary>
        float timeScaleTarget = 1.0f;
        /// <summary>
        /// 游戏状态
        /// </summary>
        GameStatu gameStatu = GameStatu.None;
        /// <summary>
        /// 游戏纪录,纪录游戏杀敌相关
        /// </summary>
        GameRecord record = new GameRecord();

        private List<List<GPWave>> gamePlayWaves;
        private List<GDEZombieData> listZombieBattle;
        private List<float> listZombieMoveSpeed;
        private List<Dictionary<int, int>> maxSpwanZombie = new List<Dictionary<int, int>>();
        private List<ZombineBase> aliveZombies = new List<ZombineBase>();
        #endregion

        #region Property
        private vp_PlayerEventHandler _playerHandler;
        public vp_PlayerEventHandler playerHandler
        {
            get
            {
                if (_playerHandler == null)
                {
                    _playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_PlayerEventHandler>();
                }
                return _playerHandler;
            }
        }
        /// <summary>
        /// 僵尸的产生地点
        /// </summary>
        private List<ZombiePlacement> _lstZombieSpwanPoint;
        public List<ZombiePlacement> lstZombieSpwanPoint
        {
            get
            {
                if (_lstZombieSpwanPoint == null || _lstZombieSpwanPoint.Count == 0)
                {
                    _lstZombieSpwanPoint = new List<ZombiePlacement>();
                    ZombiePlacement[] placements = FindObjectsOfType<ZombiePlacement>();
                    //此处可以做筛选
                    _lstZombieSpwanPoint.AddRange(placements);

                }
                return _lstZombieSpwanPoint;
            }
        }

        #endregion

        #region Custom Method

        /// <summary>
        /// 获取参数僵尸的点
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="waypoint"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 getPositionComing(out AppearanceType appType, out Transform waypoint, int index)
        {
            appType = AppearanceType.Normal;
            if (index >= lstZombieSpwanPoint.Count)
            {
                index = 0;
            }
            ZombiePlacement p = lstZombieSpwanPoint[Random.Range(0, lstZombieSpwanPoint.Count)];
            Vector3 position = p.transform.position;
            appType = p.appearanceType;
            waypoint = p.GetTargetShouldBeGo();
            Vector3 vx = p.transform.right * Random.Range(-p.GetHalfWidth(), p.GetHalfWidth());
            Vector3 vz = p.transform.forward * Random.Range(-p.GetHalfLength(), p.GetHalfLength());
            return position + vx + vz;
        }

        public void Init()
        {
            SetGameStatu(GameStatu.Init);
            InitBallteData();
            InitZombieWaves();
            record = new GameRecord();
        }

        public void InitBallteData()
        {
            print("1.Init Ballte Data");
            if (listZombieBattle != null)
            {
                listZombieBattle.Clear();
            }
            else
            {
                listZombieBattle = new List<GDEZombieData>();
            }
            if (listZombieMoveSpeed != null)
            {
                listZombieMoveSpeed.Clear();
            }
            else
            {
                listZombieMoveSpeed = new List<float>();
            }
        }

        public void InitZombieWaves()
        {
            print("2.Init Zombie Wave");
            switch (DSystem.Instance.currentScene)
            {
                case 1:
                    if (DSystem.Instance.currentLevel < 4)
                        numberWaveBallte = 1;
                    else
                        numberWaveBallte = 2;
                    break;
                case 2:
                    numberWaveBallte = 3;
                    break;
                case 3:
                default:
                    numberWaveBallte = 4;
                    break;
            }
            ///每10关增加一波
            numberWaveBallte += DSystem.Instance.currentLevel / 10;
            int num = DSystem.Instance.currentLevel % 10;
            for (int i = 0; i < numberWaveBallte; i++)
            {
                //每一波包含的任务个数
                int numPerWave = Random.Range(num, numberWaveBallte);
                lstConfigMissionRecord.Add(ConfigManager.Instance.GetWaveMission(numberWaveBallte, numberWaveBallte + num, numPerWave));
                currentBiasHealth = ConfigManager.Instance.GetBiasHealthByLevel(DSystem.Instance.currentScene, DSystem.Instance.currentLevel);
            }
            currentZombieActive = 0;
            currentZombieAlive = 0;
            activeZombieCounts.Clear();
            aliveZombies.Clear();
            currentWaveIndex = -1;
            CalculateAllZombieBattle();
        }

        protected int totalZombies = 0;
        protected int _currentSpwanedZombies = 0;
        private int CurrentSpwanedZombies
        {
            get
            {
                return _currentSpwanedZombies;
            }
            set
            {
                _currentSpwanedZombies = value;
                UIManager.Instance.UpdateZombieSlider(_currentSpwanedZombies, totalZombies);
            }
        }
        private void CalculateAllZombieBattle()
        {
            totalZombies = 0;
            CurrentSpwanedZombies = 0;

            if (this.gamePlayWaves != null)
            {
                gamePlayWaves.Clear();
            }
            else
            {
                gamePlayWaves = new List<List<GPWave>>();
            }
            for (int i = 0; i < this.numberWaveBallte; i++)
            {
                List<GPWave> gpWaves = new List<GPWave>();
                List<GDEMissionRecordData> lstmrd = lstConfigMissionRecord[i];

                for (int j = 0; j < lstmrd.Count; j++)
                {
                    GPWave gpWave = new GPWave();
                    gpWave.alert = lstmrd[j].Alert == 1;
                    gpWave.maxZombies = lstmrd[j].MaxZombies;
                    gpWave.zombieId = lstmrd[j].IDZombie;
                    gpWave.totalAmountZombie = lstmrd[j].Number;
                    totalZombies += gpWave.totalAmountZombie;
                    gpWave.timeWaitforSpwan = lstmrd[j].TimeWaitingSpwan;
                    gpWave.speed = lstmrd[j].Speed;
                    int num2 = lstmrd[j].Position;
                    if (num2 >= lstZombieSpwanPoint.Count)
                    {
                        num2 = 0;
                    }
                    gpWave.zombieSpwanPointList.Add(num2);
                    if (DeviceInfo.instance.performanceLevel == DeviceInfo.PerformanceLevel.Low && gpWave.maxZombies > 8)
                    {
                        gpWave.maxZombies = 8;
                    }
                    if (gpWave.maxZombies > 20)
                    {
                        gpWave.maxZombies = 20;
                    }
                    gpWave.remainZombie = gpWave.totalAmountZombie;
                    gpWaves.Add(gpWave);
                }
                gamePlayWaves.Add(gpWaves);
            }

            if (maxSpwanZombie == null)
            {
                maxSpwanZombie = new List<Dictionary<int, int>>();
            }
            else
            {
                maxSpwanZombie.Clear();
            }

            for (int m = 0; m < gamePlayWaves.Count; m++)
            {
                List<GPWave> currents = gamePlayWaves[m];
                Dictionary<int, int> maxZombie = new Dictionary<int, int>();
                foreach (GPWave current in currents)
                {
                    if (maxZombie.ContainsKey(current.zombieId))
                    {
                        if (current.maxZombies > maxZombie[current.zombieId])
                        {
                            maxZombie[current.zombieId] = current.maxZombies;
                        }
                    }
                    else
                    {
                        maxZombie.Add(current.zombieId, current.maxZombies);
                    }
                    if (DeviceInfo.instance.performanceLevel == DeviceInfo.PerformanceLevel.Low && maxZombie[current.zombieId] > 5)
                    {
                        maxZombie[current.zombieId] = 5;
                    }
                    if (maxZombie[current.zombieId] > 8)
                        maxZombie[current.zombieId] = 8;
                }
                maxSpwanZombie.Add(maxZombie);
            }
        }

        #endregion

        #region MonoBehavior Method

        public void Awake()
        {
            GamePlay.Instance = this;
            Init();
            //EndGuiShow();

        }

        public void OnEnable()
        {
            playerHandler.Register(this);
        }

        public void OnDisable()
        {
            playerHandler.Unregister(this);
        }



        public void Start()
        {
            SetGameStatu(GameStatu.InGame);
            StartCoroutine(EndGuiShow());
        }

        void Update()
        {

        }
        #endregion

        #region Player Dead

        public bool CanStart_Dead()
        {
            return IsInGame();
        }

        public void OnStart_Dead()
        {
            StopAllZombie();
            OnStartPlayerDead();
        }

        public void OnStop_Dead()
        {
            this.SetGameStatu(GameStatu.InGame);
            vp_Timer.In(0.5f, () => { StartAllZombie(); });
        }

        private void OnStartPlayerDead()
        {
            //throw new NotImplementedException();
            //FUGSDK.Ads.Instance.ShowRewardVedio()
            SetGameStatu(GameStatu.WaitForVedio);
            if (FUGSDK.Ads.Instance.HasRewardVedio())
            {
                ShowVideoUI();
            }
            else
            {
                OnDeadVedioPlayed(false);
            }
        }

        private void ShowVideoUI()
        {
            //throw new NotImplementedException();
            UIManager.Instance.ShowGameOverVideo(OnVideoMenuClosed);
        }

        public void OnVideoMenuClosed(bool b)
        {
            if (b)
            {
                FUGSDK.Ads.Instance.ShowRewardVedio(OnDeadVedioPlayed);
            }
            else
            {
                OnDeadVedioPlayed(false);
            }
        }


        public void OnDeadVedioPlayed(bool b)
        {
            if (b)
            {
                playerHandler.SendMessage("Reset");
            }
            else
            {
                Debug.Log("Game Failure!");
                this.Failure();
            }
        }


        #endregion



        IEnumerator EndGuiShow()
        {
            WaitForSeconds wsCheckWave = new WaitForSeconds(0.1f);
            while (true)
            {
                if (gameStatu == GameStatu.InGame)
                {
                    // Debug.Log("存活:" + currentZombieActive + "还剩:" + currentZombieSpwanRemain);
                    if (currentZombieActive <= 0 && currentZombieSpwanRemain <= 0)
                    {
                        currentWaveIndex++;
                        if (currentWaveIndex < numberWaveBallte)
                        {
                            List<GPWave> waves = gamePlayWaves[currentWaveIndex];
                            for (int i = 0; i < waves.Count; i++)
                            {
                                currentZombieSpwanRemain += waves[i].remainZombie;
                            }
                            var c = StartCoroutine(StartWaveNormal(5f));

                        }
                        else
                            this.Win();
                    }
                }
                yield return wsCheckWave;
            }

        }

        private void Win()
        {
            // throw new NotImplementedException();
            isWin = true;
            Debug.Log("Win");
            SetGameStatu(GameStatu.GameSuccess);
            timeScaleTarget = 0.1f;
            vp_TimeUtility.FadeTimeScale(0.1f, 5f);
            vp_Timer.In(0.5f, () =>
            {
                vp_TimeUtility.FadeTimeScale(1, 5f);
                vp_Timer.In(0.5f, () =>
                {
                    WinAction();
                });
            });
        }

        private void WinAction()
        {
            UIManager.Instance.ShowWinUI(record);
        }

        private void Failure()
        {
            UIManager.Instance.ShowGameOverUI();
        }


        IEnumerator StartWaveNormal(float time)
        {
            yield return new WaitForSeconds(time);
            List<GPWave> waves = gamePlayWaves[currentWaveIndex];
            for (int i = 0; i < waves.Count; i++)
            {
                StartCoroutine(StartGPWave(waves[i]));
            }
        }

        IEnumerator StartGPWave(GPWave wave)
        {

            while (wave.remainZombie > 0)
            {
                SpwanZombie(wave);
                yield return new WaitForSeconds(wave.timeWaitforSpwan);
            }

        }

        private void SpwanZombie(GPWave wave)
        {
            // throw new NotImplementedException();
            if (!playerHandler.Dead.Active)
            {
                //判断有没有到最大的僵尸数
                Dictionary<int, int> curWaveMaxZombie = maxSpwanZombie[currentWaveIndex];
                if (curWaveMaxZombie.ContainsKey(wave.zombieId))
                {
                    if (activeZombieCounts.ContainsKey(wave.zombieId))
                    {
                        if (activeZombieCounts[wave.zombieId] >= curWaveMaxZombie[wave.zombieId])
                        {
                            return;
                        }
                    }
                }

                Transform zomTran =
                     ZombiePoolManager.Instance.SpwanZombieType((ZombieModel)wave.zombieId, GamePlay.ZOMBIE_HIDDEN_PLACE, true, currentBiasHealth);
                if (zomTran)
                {
                    wave.remainZombie--;
                    CurrentSpwanedZombies++;
                    currentZombieActive++;
                    currentZombieSpwanRemain--;
                    if (activeZombieCounts.ContainsKey(wave.zombieId))
                    {
                        activeZombieCounts[wave.zombieId]++;
                    }
                    else
                    {
                        activeZombieCounts[wave.zombieId] = 1;
                    }
                    this.StartZombieType(zomTran, wave.zombieSpwanPointList, wave.speed);
                }
            }
        }

        private void StartZombieType(Transform zomTran, List<int> zombieSpwanPointList, float speed)
        {
            int pointIndex = 0;
            if (zombieSpwanPointList.Count > 0)
            {
                pointIndex = Random.Range(0, zombieSpwanPointList.Count);

            }
            Vector3 pos = Vector3.zero;
            Transform wayPoint;
            AppearanceType appType = AppearanceType.Normal;
            pos = getPositionComing(out appType, out wayPoint, pointIndex);
            ZombineBase zomBase = zomTran.GetComponent<ZombineBase>();
            zomBase.SetCurrentPoint(wayPoint);
            zomBase.SetOriginSpeed(speed);
            zomBase.StartAction(appType, pos);
            aliveZombies.Add(zomBase);
        }

        public void ZombieDie(ZombineBase zombie)
        {
            Debug.Log("Zombie Die");
            currentZombieActive--;
            record.ZombieKills++;
            if (activeZombieCounts.ContainsKey(zombie.zombieId))
            {
                activeZombieCounts[zombie.zombieId]--;
            }
            if (aliveZombies.Contains(zombie))
            {
                aliveZombies.Remove(zombie);
            }

        }

        public void StopAllZombie()
        {
            for (int i = 0; i < aliveZombies.Count; i++)
            {
                aliveZombies[i].StopAction();
            }
        }

        public void StartAllZombie()
        {
            if (aliveZombies.Count > 0)
            {
                for (int i = 0; i < aliveZombies.Count; i++)
                {
                    aliveZombies[i].StartAction();
                }
            }
        }

        #region Game Statu

        public void SetGameStatu(GameStatu statu)
        {
            this.gameStatu = statu;
        }

        public bool IsInGame()
        {
            return this.gameStatu == GameStatu.InGame;
        }

        #endregion

    }
}