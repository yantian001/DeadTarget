using UnityEngine;
using UnityEngine.UI;

public class Medkit : MonoBehaviour {

    public Button btnBomb;
    public Text txtBomb;
    TPSInput tpsInput;

    // Use this for initialization
    void Start()
    {
        if (!tpsInput)
        {
            tpsInput = GameObject.FindGameObjectWithTag("Player").GetComponent<TPSInput>();
        }
        if (!btnBomb)
        {
            btnBomb = GetComponent<Button>();
        }
        UpdateDisplay();
        //注册click时间
        btnBomb.onClick.AddListener(OnBombClick);
    }
    void UpdateDisplay()
    {
        if (!txtBomb)
        {
            txtBomb = transform.GetComponentInChildren<Text>();
        }
        txtBomb.text = WeaponManager.Instance.GetMedkitCount().ToString();
        if (WeaponManager.Instance.GetMedkitCount() > 0)
        {
            btnBomb.interactable = true;
        }
        else
        {
            btnBomb.interactable = false;
        }
    }
    void OnBombClick()
    {
        if (WeaponManager.Instance.GetMedkitCount() > 0)
        {
            if (tpsInput)
            {
                tpsInput.Medkit = true;
                WeaponManager.Instance.UseMedkit();
                UpdateDisplay();
            }
        }
    }
}
