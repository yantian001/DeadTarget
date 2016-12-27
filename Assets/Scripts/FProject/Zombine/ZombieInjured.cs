using UnityEngine;
using System.Collections;
namespace FProject
{
    public class ZombieInjured : ZombieComponent
    {
        
        public void OnStart_Injured()
        {
            if (eventHandler.Move.Active)
                eventHandler.Move.TryStop();
            if (eventHandler.Attack.Active)
            {
                eventHandler.Attack.TryStop();
            }
            PlayInjuredAnimation();
        }
        public bool CanStart_Injured()
        {
            if (eventHandler.Die.Active)
                return false;
            return true;
        }

        public void PlayInjuredAnimation()
        {
            this.stateManager.SetState(ZombieAnimationState.Injured);
        }
        public void Injured()
        {
           
            if (eventHandler.Injured.Active)
            {
                PlayInjuredAnimation();
            }
            else
                eventHandler.Injured.TryStart();
        }

        public void Hit1_Complete()
        {
            print("hit1 complete!");
            eventHandler.Injured.TryStop();
        }


        #region Die
        public void OnStart_Die()
        {
            if (eventHandler.Move.Active)
                eventHandler.Move.TryStop();
            if (eventHandler.Attack.Active)
                eventHandler.Attack.TryStop();
            stateManager.SetState(ZombieAnimationState.Die);
        }
        #endregion
    }
}