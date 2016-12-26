using UnityEngine;
using System.Collections;
namespace FProject.Test
{
    public class RagdollTest : MonoBehaviour
    {
        public Vector3 force;
        private ZombineRagdoll myRagdoll;
        private RagdollEffect myRagdollEffect;
        private ZombineBase zombineBase;
        // Use this for initialization
        void Start()
        {
            zombineBase = GetComponent<ZombineBase>();
            myRagdollEffect = new RagdollEffect();
            myRagdoll = new ZombineRagdoll(zombineBase, RagdollBoneType.HumanBone);
        }

        public void Injured(object info)
        {
            vp_DamageInfo damageInfo = info as vp_DamageInfo;
            myRagdollEffect.SetForceEffect(force * 1000, damageInfo.Point);
            myRagdoll.CreateRagdoll();
            myRagdoll.ApplyRagdollEffect(myRagdollEffect);

        }
        
    }
}