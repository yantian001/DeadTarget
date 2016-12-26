using UnityEngine;
using System.Collections;
namespace FProject
{
    public class TargetPositionZombie : SingletonMono<TargetPositionZombie>
    {
        private Transform _playerT;
        /// <summary>
        /// Player Tramsform
        /// </summary>
        public Transform playerT
        {
            get
            {
                if (_playerT == null)
                {
                    _playerT = GameObject.FindGameObjectWithTag("Player").transform;
                }
                return _playerT;
            }
        }

        private Vector3 position = Vector3.zero;
        // Use this for initialization
        void Start()
        {
            UpdatePosition();
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition();
        }

        void UpdatePosition()
        {
            if (!playerT)
            {
                this.position.x = playerT.position.x;
                this.position.z = playerT.position.z;
                this.position.y = playerT.position.y;
                base.transform.position = position;
            }
        }

        public Transform GetTarget()
        {
            return playerT;
        }

        public Transform GetPlayerT()
        {
            return playerT;
        }
    }
}