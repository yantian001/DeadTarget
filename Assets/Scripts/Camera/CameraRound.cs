using UnityEngine;
using System.Collections;

public class CameraRound : MonoBehaviour
{

    Camera cam;
    public float speed = 1f;

    public float maxAngle = 20f;

    public float waitTime = 0.5f;

    float remainTime = 0;

    bool isPostive = true;
    // Use this for initialization
    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        // LeanTween.rotateAround(gameObject,transform.position + new Vector3(0,0,10), 30f, 5f);
        // LeanTween.rotateAround(gameObject,new Vector3(0,0,1), 30f, 5f);


    }

    // Update is called once per frame
    void Update()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            float s = 0;
            if (isPostive)
            {
                s = -speed - (speed / waitTime);
            }
            else
            {
                s = -speed + (speed / waitTime);
            }

            transform.RotateAround(transform.position + new Vector3(0, 0, 20), Vector3.up, Time.deltaTime * s);
        }
        else
        {
            float rotateY = transform.rotation.eulerAngles.y;
            if (isPostive)
            {
                if (rotateY < 300 && rotateY >= maxAngle)
                {
                    isPostive = false;
                    speed = -speed;
                    remainTime = waitTime;
                }
            }
            else
            {
                if (rotateY > 300 && rotateY <= 360 - maxAngle)
                {
                    isPostive = true;
                    speed = -speed;
                    remainTime = waitTime;
                }
            }
            // transform.RotateAround(Vector3.up, Time.deltaTime * 3f);
            transform.RotateAround(transform.position + new Vector3(0, 0, 20), Vector3.up, Time.deltaTime * speed);
            //Debug.Log(transform.rotation.eulerAngles.y);
        }

    }
}
