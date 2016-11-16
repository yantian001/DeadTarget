using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour
{

    public float timeBomb;

    public GameObject expoPrefab;

    public float velocity = 5;

    public Rigidbody rb;
    float timeStart;
    // Use this for initialization
    void Start()
    {
        if (timeBomb > 0 && expoPrefab)
        {
            Invoke("Exposion", timeBomb);
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
      //  print(transform.forward);
        rb.velocity = transform.forward * velocity;

    }

    public void Exposion()
    {
        GameObject.Instantiate(expoPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //  //  print(collision.collider.name);
    //}
}
