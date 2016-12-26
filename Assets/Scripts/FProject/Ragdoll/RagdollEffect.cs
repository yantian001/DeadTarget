using UnityEngine;

namespace FProject
{
    public class RagdollEffect
    {
        public RagdollEffectType type;

        public Vector3 force;

        public Vector3 forcePoint;

        public Vector3 explosionPoint;

        public float explosionForce;

        public float explosionRadius;

        public RagdollEffect()
        {
            this.type = RagdollEffectType.None;
            this.force = Vector3.zero;
            this.forcePoint = Vector3.zero;
            this.explosionPoint = Vector3.zero;
            this.explosionForce = 0f;
            this.explosionRadius = 0f;
        }

        public void SetForceEffect(Vector3 mforce, Vector3 mPoint)
        {
            this.type = RagdollEffectType.Force;
            Vector3 vector = new Vector3(mforce.x, mforce.y, mforce.z);
            this.force = vector;
            vector = new Vector3(mPoint.x, mPoint.y, mPoint.z);
            this.forcePoint = vector;
        }

        public void SetExplosionEffect(float exforce, Vector3 mPoint, float exRadius)
        {
            this.type = RagdollEffectType.Explosion;
            this.explosionForce = exforce;
            Vector3 vector = new Vector3(mPoint.x, -1f, mPoint.z);
            this.explosionPoint = vector;
            this.explosionRadius = exRadius;
        }

        public void SetMixEffect(Vector3 mforce, Vector3 mPoint, float exforce, Vector3 exPoint, float exRadius)
        {
            this.type = RagdollEffectType.Mix;
            Vector3 vector = new Vector3(mforce.x, mforce.y, mforce.z);
            this.force = vector;
            vector = new Vector3(mPoint.x, mPoint.y, mPoint.z);
            this.forcePoint = vector;
            this.explosionForce = exforce;
            vector = new Vector3(exPoint.x, -1f, exPoint.z);
            this.explosionPoint = vector;
            this.explosionRadius = exRadius;
        }
    }
}
