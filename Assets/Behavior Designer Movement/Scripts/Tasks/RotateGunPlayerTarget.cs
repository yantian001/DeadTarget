using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Rotates towards the specified rotation. The rotation can either be specified by a transform or rotation. If the transform " +
                     "is used then the rotation will not be used.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=2")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}RotateTowardsIcon.png")]
    public class RotateGunPlayerTarget : Action
    {

        [Tooltip("The agent is done rotating when the square magnitude is less than this value")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("Max rotation delta")]
        public SharedFloat maxLookAtRotationDelta;
        [Tooltip("The transform that the agent is rotating towards")]
        public SharedTransform targetTransform;

        private Transform muzzle;

        private Transform aimSpine;
        public override void OnStart()
        {
            if ((targetTransform == null || targetTransform.Value == null))
            {
                //Debug.LogError("Error: A RotateTowards target value is not set.");
                var playerT = GameObject.FindGameObjectWithTag("Player").transform;
                if (playerT.GetComponent<PlayerAttr>())
                {
                    targetTransform.Value = playerT.GetComponent<PlayerAttr>().targetAttach;
                }
                if (targetTransform.Value == null)
                {
                    targetTransform.Value = playerT;
                }
                //targetRotation = new SharedVector3(); // create a new SharedQuaternion to prevent repeated errors
            }
            var eg = transform.GetComponent<EnemyGun>();
            if (eg && eg.muzzleTransform)
            {
                muzzle = eg.muzzleTransform;
            }
            else
            {
                muzzle = transform;
            }

            if (eg && eg.aimSpine)
            {
                aimSpine = eg.aimSpine;
            }
            else
            { aimSpine = transform; }
        }

        public override TaskStatus OnUpdate()
        {
            var rotation = Target();
            // Return a task status of success once we are done rotating
            if (Quaternion.Angle(aimSpine.rotation, rotation) < rotationEpsilon.Value)
            {
                return TaskStatus.Success;
            }
            // We haven't reached the target yet so keep rotating towards it
            aimSpine.rotation = Quaternion.RotateTowards(aimSpine.rotation, rotation, maxLookAtRotationDelta.Value);
            return TaskStatus.Running;
        }

        // Return targetPosition if targetTransform is null
        private Quaternion Target()
        {

            var position = targetTransform.Value.position - muzzle.position;
            return Quaternion.LookRotation(position);
        }

        // Reset the public variables
        public override void OnReset()
        {
            rotationEpsilon = 0.5f;
        }
    }
}