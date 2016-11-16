using UnityEngine;
using System.Collections;

public class Hit_Player : AS_BulletHiter {

    public DamageManager damageManage;
    // Use this for initialization
    void Start () {
	   if(damageManage == null)
        {
            damageManage = this.RootObject.GetComponent<DamageManager>();
        }
	}

    public override void OnHit(RaycastHit hit, AS_Bullet bullet)
    {
        AddAudio(hit.point);
        if(damageManage)
        {
            damageManage.ApplyDamage(bullet.Damage, bullet.transform.forward * bullet.HitForce, 0f);
            vp_DamageInfo info = new vp_DamageInfo(bullet.Damage, bullet.source, vp_DamageInfo.DamageType.Bullet);
            // SendMessageUpwards("OnMessage_HUDDamageFlash", info, SendMessageOptions.DontRequireReceiver);
            RootObject.BroadcastMessage("OnMessage_HUDDamageFlash", info, SendMessageOptions.DontRequireReceiver);
            
        }
        base.OnHit(hit, bullet);
    }
}
