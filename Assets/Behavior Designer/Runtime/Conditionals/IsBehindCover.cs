using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success if the gameobject behind the cover")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=123")]
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    public class IsBehindCover : Conditional
    {

        [Tooltip("The GameObject to compare the property of")]
        public SharedGameObject targetGameObject;



        public override void OnStart()
        {
            targetGameObject = GetDefaultGameObject(targetGameObject.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if(targetGameObject.Value)
            {
                var waypoint = targetGameObject.Value.GetComponentInParent<Waypoint>();
                if(waypoint && waypoint.isCover)
                {
                    return TaskStatus.Success;

                }
            }
            return TaskStatus.Failure;
            //return eventReceived ? TaskStatus.Success : TaskStatus.Failure;
        }

       
    }
}