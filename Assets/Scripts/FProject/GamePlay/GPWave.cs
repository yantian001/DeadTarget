using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FProject
{
    public class GPWave
    {

        public bool alert;

        public List<int> zombieIndexList;

        public List<int> zombieSpwanPointList;

        public Dictionary<int, float> timeWaitingSpwan;

        public float timeWaitforSpwan;

        public int zombieId;

        public int totalAmountZombie;

        public int remainZombie;

        public int maxZombies;

        public List<ZombieSpwaned> zombieList;

        public float speed;

        public GPWave()
        {
            zombieIndexList = new List<int>();
            zombieSpwanPointList = new List<int>();
            timeWaitingSpwan = new Dictionary<int, float>();
            zombieList = new List<ZombieSpwaned>();
        }

    }
}