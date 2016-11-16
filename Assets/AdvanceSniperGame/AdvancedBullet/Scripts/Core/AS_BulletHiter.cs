// Bullet marker. Using to adding into any objects that you want to have a Camera Action.
using UnityEngine;
using System.Collections;

public class AS_BulletHiter : MonoBehaviour {

    public HitPosition HitPos = HitPosition.NORMAL;
	public GameObject ParticleHit;
    public int Sort = 0;
	public GameObject RootObject;
	public GameObject DecayFX;
	public float DecayDuration = 10;
	public AudioClip[] Sounds;
	
	void Start(){
		
	}
	void Awake () {
		if(!RootObject){
			RootObject = this.transform.root.gameObject;	
		}
	}
	
	public virtual void OnHit(RaycastHit hit,AS_Bullet bullet){

		if (DecayFX) {
			GameObject decay = (GameObject)GameObject.Instantiate(DecayFX,hit.point,Quaternion.identity);
			decay.transform.forward = bullet.transform.forward - (Vector3.up*0.2f);
			GameObject.Destroy(decay,DecayDuration);
		}
	}
	
	public void AddAudio(Vector3 point){
		GameObject sound = new GameObject("SoundHit");
		sound.AddComponent<AS_SoundOnHit>();
		GameObject soundObj = (GameObject)GameObject.Instantiate(sound,point,Quaternion.identity);
		soundObj.GetComponent<AS_SoundOnHit>().Sounds = Sounds;
		GameObject.Destroy(soundObj,3);
	}
}
