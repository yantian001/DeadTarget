using UnityEngine;
using System.Collections;

public class PlayerTakeMedkit : MonoBehaviour {

    public DamageManager dm = null;

    public GameObject particleMedkit;

    public float particleTime = 2f;

	// Use this for initialization
	void Start () {
	    if(dm == null)
        {
            dm = GetComponent<DamageManager>();
        }
	}

    public void MedkitCall()
    {
        if(dm && particleMedkit)
        {
            dm.AddMedkit();
           var g =(GameObject) GameObject.Instantiate(particleMedkit, transform.position, Quaternion.identity);
            Destroy(g, Mathf.Max(2, particleTime));
        }
    }
}
