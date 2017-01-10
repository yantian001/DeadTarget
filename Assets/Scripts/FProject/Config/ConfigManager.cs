using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using System;
using Random = UnityEngine.Random;
namespace FProject
{



    public class ConfigManager : SingletonMono<ConfigManager>
    {
        public static float HP_RATIO = 0.2f;
        #region 属性
        /// <summary>
        /// 僵尸数据
        /// </summary>
        private List<GDEZombieData> zombies = new List<GDEZombieData>();
        /// <summary>
        /// 任务记录
        /// </summary>
        private List<GDEMissionRecordData> missionRecords = new List<GDEMissionRecordData>();

        private Dictionary<int, List<GDEMissionRecordData>> dicMissionWithsWaveIndex = new Dictionary<int, List<GDEMissionRecordData>>();

        #endregion
        public void Awake()
        {
            if (!SingletonMono<ConfigManager>.IsInstanceValid())
            {
                DontDestroyOnLoad(this);
                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        void Init()
        {
            if (GDEDataManager.Init("gde_data"))
            {
                //初始化僵尸
                InitZombies();
                //初始化任务
                InitMissionRecord();
            }
            else
            {
                Debug.LogError("Init GDEDataManager failed!");
            }

        }
        #region Zombies
        void InitZombies()
        {
            zombies.Clear();
            List<string> wsKeys;
            if (GDEDataManager.GetAllDataKeysBySchema("Zombie", out wsKeys))
            {
                for (int i = 0; i < wsKeys.Count; i++)
                {
                    GDEZombieData zd = null;
                    if (GDEDataManager.DataDictionary.TryGetCustom(wsKeys[i], out zd))
                    {
                        if (zd != null)
                        {
                            zombies.Add(zd);
                        }
                    }
                    else
                    {
                        Debug.LogError(string.Format("Try Get {0} error!", wsKeys[i]));
                    }
                }
            }
            else
            {
                Debug.LogError("Init Zombies data Error!");
            }
        }

        /// <summary>
        /// 获取僵尸的数据
        /// </summary>
        /// <param name="zombieId"></param>
        /// <returns></returns>
        public GDEZombieData GetZombieById(int zombieId)
        {
            return zombies.Find(p => { return p.Id == zombieId; });
        }
        #endregion

        #region Mission

        public void InitMissionRecord()
        {
            missionRecords.Clear();
            List<string> wsKeys;
            if (GDEDataManager.GetAllDataKeysBySchema("MissionRecord", out wsKeys))
            {
                for (int i = 0; i < wsKeys.Count; i++)
                {
                    GDEMissionRecordData mrdata = null;
                    if (GDEDataManager.DataDictionary.TryGetCustom(wsKeys[i], out mrdata))
                    {
                        if (mrdata != null)
                        {
                            missionRecords.Add(mrdata);
                            if (!dicMissionWithsWaveIndex.ContainsKey(mrdata.Wave))
                            {
                                dicMissionWithsWaveIndex.Add(mrdata.Wave, new List<GDEMissionRecordData> { mrdata });
                            }
                            else
                            {
                                dicMissionWithsWaveIndex[mrdata.Wave].Add(mrdata);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Get MissionRecord " + wsKeys[i] + " Error!");
                    }
                }
            }
            else
            {
                Debug.LogError("Get MissionRecord Keys Error!");
            }
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="missionId"></param>
        /// <returns></returns>
        public List<GDEMissionRecordData> GetMissionById(int missionId)
        {
            List<GDEMissionRecordData> list = new List<GDEMissionRecordData>();
            for (int i = 0; i < missionRecords.Count; i++)
            {
                if (missionRecords[i].MissionID == missionId)
                {
                    list.Add(missionRecords[i]);
                }
            }
            return list;
            //return missionRecords.Find(p => { return p.MissionID == missionId; });
        }


        public List<GDEMissionRecordData> GetConfigEndlessMission(int fromwave, int towave, ref int missionId)
        {
            bool flag = false;
            int num = fromwave;
            int num2 = towave;
            int count = 0;
            if (fromwave > 45)
            {
                int num3 = Random.Range(7, towave / 5 + 1);
                towave = num3 * 5;
                fromwave = towave - 4;
                flag = true;
            }
            List<int> missionIds = GetMissionId(fromwave, towave);
            while (missionIds.Count == 0 && ++count < 20)
            {
                int num4 = UnityEngine.Random.Range(7, num2 / 5 + 1);
                towave = num4 * 5;
                fromwave = towave - 4;
                flag = true;
                missionIds = this.GetMissionId(fromwave, towave);
            }
            if (missionIds.Count > 0)
            {
                int index = Random.Range(0, missionIds.Count);
                missionId = missionIds[index];
                List<GDEMissionRecordData> configMission = GetMissionById(missionId);
                if (flag)
                {
                    int diff = num - configMission[0].Wave;
                    for (int i = 0; i < configMission.Count; i++)
                    {
                        configMission[i].Wave += diff;
                    }
                }
                return configMission;
            }
            return null;
        }

        public float GetBiasHealthByLevel(int currentScene, int currentLevel)
        {
            return currentScene + currentLevel * HP_RATIO;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 获取waveCount个由Wave编号从fromwave到toWave的随机任务组合
        /// </summary>
        /// <param name="fromWave"></param>
        /// <param name="toWave"></param>
        /// <param name="waveCount"></param>
        /// <returns></returns>
        public List<GDEMissionRecordData> GetWaveMission(int fromWave, int toWave, int waveCount)
        {
            List<GDEMissionRecordData> list = new List<GDEMissionRecordData>();
            int waveIndex = -1;
            int repearCount = 0;
            for (int i = 0; i < waveCount; i++)
            {
                repearCount = 0;
                waveIndex = -1;

                while (waveIndex == -1)
                {
                    waveIndex = Random.Range(fromWave, toWave);
                    if (dicMissionWithsWaveIndex.ContainsKey(waveIndex))
                    {
                        int missionIndex = Random.Range(0, dicMissionWithsWaveIndex[waveIndex].Count);
                        list.Add(dicMissionWithsWaveIndex[waveIndex][missionIndex]);
                    }
                    else
                        waveIndex = -1;
                    if (++repearCount > 10)
                        break;
                }

            }
            return list;

        }

        /// <summary>
        /// 获取任务的id
        /// </summary>
        /// <param name="fromWave"></param>
        /// <param name="toWave"></param>
        /// <returns></returns>
        private List<int> GetMissionId(int fromWave, int toWave)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < missionRecords.Count; i++)
            {
                GDEMissionRecordData m = missionRecords[i];
                if (m.Wave >= fromWave && m.Wave <= toWave && !list.Contains(m.Wave))
                {
                    list.Add(m.MissionID);
                }
            }
            return list;
        }

        public int GetNumberWaveByMission(int missionId)
        {
            List<GDEMissionRecordData> configMission = GetMissionById(missionId);
            if (configMission != null)
            {
                return configMission.Count;
            }
            return 0;
        }

        /// <summary>
        /// 获取生命基数
        /// </summary>
        /// <param name="missionId"></param>
        /// <returns></returns>
        public float GetBiasHealthByMissionId(int missionId)
        {
            return 1f;
        }
        #endregion
    }
}