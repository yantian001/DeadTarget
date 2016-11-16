using UnityEngine;
using System.Collections;

public class AutoDestroyByRemove : MonoBehaviour
{
    public void OnTransformParentChanged()
    {
        //Debug.Log("OnTransformParentChanged");
        if(transform.parent ==null)
        {
            //Debug.Log("removed");
            Destroy(gameObject);
        }
    }
}
