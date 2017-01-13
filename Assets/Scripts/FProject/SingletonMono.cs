using UnityEngine;
using System.Collections;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T singleton;
    public static T Instance
    {
        get
        {
            if (SingletonMono<T>.singleton == null)
            {
                SingletonMono<T>.singleton = (T)((object)(FindObjectOfType<T>()));
                if (SingletonMono<T>.singleton == null)
                {
                    SingletonMono<T>.singleton = new GameObject
                    {
                        name = "[@" + typeof(T).Name + "]"
                    }.AddComponent<T>();
                }
            }
            return SingletonMono<T>.singleton;
        }
        set
        {
            SingletonMono<T>.singleton = value;
        }
    }

    public static bool IsInstanceValid()
    {
        return SingletonMono<T>.singleton != null;
    }

    private void Reset()
    {
        base.gameObject.name = typeof(T).Name;
    }

    public void OnApplicationQuit()
    {
        SingletonMono<T>.singleton = null;
    }

    public virtual void OnDestroy()
    {
        SingletonMono<T>.singleton = null;
    }
}
