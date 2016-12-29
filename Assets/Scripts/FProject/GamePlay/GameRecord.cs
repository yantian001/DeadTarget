using UnityEngine;
using System.Collections;

namespace FProject
{
    public class GameRecord
    {
        private int _zombieKills = 0;
        /// <summary>
        /// 僵尸击杀数
        /// </summary>
        public int ZombieKills
        {
            get; set;
        }

        private int _headshotCount = 0;
        public int HeadshotCount
        {
            get; set;
        }

        private int _maxCombos;
        public int MaxCombos
        {
            get { return _maxCombos; }
            set { if (value > _maxCombos) _maxCombos = value; }
        }

        private int _otherRewards;
        public int OtherRewards
        {
            get; set;
        }

        private int _missionRewards;
        public int missionRewards
        {
            get; set;
        }

        public int ZombieKillReward
        {
            get
            {
                return ZombieKills * 20;
            }
        }

        public int HeadShotReward
        {
            get
            {
                return HeadshotCount * 10;
            }
        }

        public int MaxComboReward
        {
            get
            {
                return MaxCombos * 5;
            }
        }

        public int TotalReward
        {
            get
            {
                return missionRewards + OtherRewards + ZombieKillReward + HeadShotReward + MaxComboReward;
            }
        }
    }

}