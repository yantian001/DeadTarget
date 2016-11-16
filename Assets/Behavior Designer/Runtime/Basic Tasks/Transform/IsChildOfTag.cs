using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Returns Success if the transform is a child of the specified GameObject.")]
    public class IsChildOfTag : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The interested transform")]
        public SharedString tagName;

        private Transform targetTransform;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                targetTransform = currentGameObject.GetComponent<Transform>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetTransform == null || targetTransform.parent == null) {
                Debug.LogWarning("Transform is null");
                return TaskStatus.Failure;
            }

            return targetTransform.parent.tag == tagName.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            tagName = null;
        }
    }
}