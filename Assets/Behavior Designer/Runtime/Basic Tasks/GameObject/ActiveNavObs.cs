using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Returns the component of Type type if the game object has one attached, null if it doesn't. Returns Success.")]
    public class ActiveNavObs : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The type of component")]
        public SharedBool active;

        public override TaskStatus OnUpdate()
        {
            var obs  = GetDefaultGameObject(targetGameObject.Value).GetComponent<NavMeshObstacle>();
            obs.enabled = active.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            active.Value = false;

        }
    }
}