using UnityEngine;
using System.Collections;

public class AP_SlowHitPreset : AS_ActionPreset
{
	public float ZoomMulti = 1;
	public override void Shoot (GameObject bullet)
	{
		if (!ActionCam) {
			return;	
		}
		ActionCam.SetPosition (bullet.transform.position, false);
		base.Shoot (bullet);
	}
	
	public override void FirstDetected (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		if (!ActionCam.InAction) {
			ActionCam.Slowmotion (0.1f, 0.5f);
			ActionCam.Follow = true;
		}
		
		
		base.FirstDetected (bullet, target, point);
	}
	public override void TargetDetected (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		
		if (!ActionCam) {
			return;	
		}
		
		if (!ActionCam.HitTarget) {
			ActionCam.ObjectLookAt = bullet.gameObject;
			ActionCam.Follow = true;
			ActionCam.SlowmotionNow (0.02f, 3.0f);
			ActionCam.SetPosition(point + (target.transform.right * ActionCam.Length),false);
			ActionCam.ActionBullet (10.0f);
		}
		base.TargetDetected (bullet, target, point);
	}
	
	public override void TargetHited (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		
		if (ActionCam.Detected) {
			ActionCam.ObjectLookAt = null;
			ActionCam.ActionBullet (3.0f);
			ActionCam.Follow = false;
		}else{
			ActionCam.ActionBullet (2.0f);
			ActionCam.ObjectLookAt = null;
			ActionCam.SlowmotionNow (0.1f, 1.6f);
			ActionCam.Follow = false;
			ActionCam.lookAtPosition = point;
			ActionCam.SetPositionDistance (point, true);	
		}
		
		base.TargetHited (bullet, target, point);
		
	}
	
}
