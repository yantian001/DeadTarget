using UnityEngine;
using System.Collections;

public class AP_HitSlowPreset : AS_ActionPreset
{
	public float ZoomMulti = 1;

	public override void Shoot (GameObject bullet)
	{
		if (!ActionCam) {
			return;	
		}
		ActionCam.ObjectFollowing = null;
		ActionCam.ObjectLookAt = null;
		ActionCam.InAction = false;
		ActionCam.Follow = false;
		ActionCam.SetPosition (bullet.transform.position, false);
		base.Shoot (bullet);
	}
	
	public override void FirstDetected (AS_Bullet bullet, AS_BulletHiter target, Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		if (!ActionCam.InAction) {
			ActionCam.Follow = true;
			ActionCam.SetPosition (bullet.transform.position + (bullet.transform.right * ZoomMulti) - (bullet.transform.forward * 2 * ZoomMulti), false);
		}
		
		
		base.FirstDetected (bullet, target, point);
	}

	public override void TargetDetected (AS_Bullet bullet, AS_BulletHiter target, Vector3 point)
	{
		
		if (!ActionCam) {
			return;	
		}

		base.TargetDetected (bullet, target, point);
	}
	
	public override void TargetHited (AS_Bullet bullet, AS_BulletHiter target, Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		
		ActionCam.ActionBullet (3.0f);
		ActionCam.ObjectLookAt = null;
		ActionCam.SlowmotionNow (0.02f, 1.0f);
		ActionCam.Follow = true;
		ActionCam.lookAtPosition = point;
		ActionCam.SetPosition (point + (target.transform.right * ZoomMulti) - (target.transform.forward * 2 * ZoomMulti), true);
		ActionCam.LengthMult = 0.5f;
		
		base.TargetHited (bullet, target, point);
		
	}
}
