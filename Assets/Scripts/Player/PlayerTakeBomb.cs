using UnityEngine;
using System.Collections;

public class PlayerTakeBomb : MonoBehaviour {

    public GameObject prefabBomb;

    public Transform NormalCamera;
    // Use this for initialization
    void Start () {
	    if(NormalCamera == null)
        {
            NormalCamera = Camera.main.transform;
        }
	}
	
	public void TakeBomb()
    {
        if(prefabBomb!=null)
        {
            //Vector3 point = NormalCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            //point += NormalCamera.transform.forward * 2;
            //GameObject bullet = (GameObject)Instantiate(prefabBomb, point, Quaternion.LookRotation(NormalCamera.transform.forward));
            //var asBuulet = bullet.GetComponent<AS_Bullet>();

            //bullet.transform.forward = NormalCamera.transform.forward;

           // Destroy(bullet, LifeTimeBullet);
        }
    }
}
