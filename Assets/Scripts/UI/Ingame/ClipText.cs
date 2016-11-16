using UnityEngine;
using UnityEngine.UI;

public class ClipText : MonoBehaviour
{

    public GunHanddle gunHanddle = null;
    public Text text;
    // Use this for initialization
    void Start()
    {
        if (gunHanddle == null)
        {
            gunHanddle = GameObject.FindGameObjectWithTag("Player").GetComponent<GunHanddle>();
        }
        if (text == null)
        {
            text = GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gunHanddle && gunHanddle.CurrentGun && text)
        {
            text.text = string.Format("{0}/{1}", gunHanddle.CurrentGun.Clip, gunHanddle.CurrentGun.ClipSize);
        }
    }
}
