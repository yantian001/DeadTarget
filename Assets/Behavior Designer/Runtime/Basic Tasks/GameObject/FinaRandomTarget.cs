using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Finds a Transform by tag. Returns Success.")]
    public class FinaRandomTarget : Action
    {
        [Tooltip("The tag of the GameObject to find")]
        public SharedString tag;

        public SharedInt maxChildCount;
        [Tooltip("The object found by name")]
        [RequiredField]
        public SharedTransform storeValue;

        public override void OnStart()
        {
            //base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            //GameObject[] targets = GameObject.FindGameObjectsWithTag(tag.Value);
            //int total = targets.Length;
            //int r = Random.Range(0, total);
            //bool found = false;
            //int time = 0;
            //while (!found)
            //{
            //    if (targets[r].transform.childCount < maxChildCount.Value)
            //    {
            //        storeValue.Value = targets[r].transform;
            //        found = true;
            //    }
            //    r++;
            //    if(r >= total)
            //    {
            //        r -= total;
            //    }
            //    time++;
            //    if (time > total + 1)
            //        break;

            //}
            
            
            storeValue.Value = WaypointManager.Instance.GetWaypoint(transform);
            if (storeValue.Value != null)
                return TaskStatus.Success;
            else
            {
                return TaskStatus.Running;
            }
        }

        public override void OnReset()
        {
            tag.Value = null;
            storeValue.Value = null;
        }
    }
}