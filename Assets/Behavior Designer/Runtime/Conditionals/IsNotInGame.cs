using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success if the gamestatu not equals ingame")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=123")]
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    public class IsNotInGame : Conditional
    {
       
        
        public override TaskStatus OnUpdate()
        {
            if (GameValue.staus == GameStatu.InGame)
                return TaskStatus.Failure;
            return TaskStatus.Success;
        }

       
    }
}