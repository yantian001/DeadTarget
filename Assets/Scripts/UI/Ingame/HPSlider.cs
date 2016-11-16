using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{

    public DamageManager dm;

    public Slider slider;
    // Use this for initialization
    void Start()
    {
        if (!dm)
        {
            dm = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageManager>();
        }
        if(!slider)
        {
            slider = GetComponent<Slider>();
        }
        slider.maxValue = dm.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
        slider.value = dm.hp;
    }
}
