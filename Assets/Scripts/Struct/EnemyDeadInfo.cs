using UnityEngine;
using System.Collections;

public class EnemyDeadInfo  {

    public int score = 0;

    public Transform transform;

    public bool headShot
    {
        get
        {
            return hitPos == HitPosition.HEAD;
        }
        private set
        {

        }
    }

    public HitPosition hitPos;

    //public Animal animal;

}
