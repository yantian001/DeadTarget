using UnityEngine;
using System.Collections;
namespace FProject
{
    public class ZombieDamageHandler : vp_DamageHandler
    {


        ZombieEventHandler _eventHandler;
        ZombieEventHandler eventHandler
        {
            get
            {
                if (_eventHandler == null)
                    _eventHandler = GetComponent<ZombieEventHandler>();
                return _eventHandler;
            }
        }
        public override void Die()
        {
            if (!eventHandler.Die.Active)
                eventHandler.Die.TryStart();
            GamePlay.Instance.ZombieDie(this.GetComponent<ZombineBase>());
            vp_Utility.Destroy(this.gameObject, 2f);
        }

        protected override void CacheColider()
        {
            var coliders = transform.GetComponentsInChildren<Collider>();
            for (int i = 0; i < coliders.Length; i++)
            {
                Instances.Add(coliders[i], this);
            }
        }
    }
}