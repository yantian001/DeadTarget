using UnityEngine;
using System.Collections;
using GameDataEditor;
public class UIWeaponItem : MonoBehaviour
{

    public UILabel lbWeapnName;
    public UISprite spWeaonIcon;
    public GameObject makerEquiped;
    public UIToggle toggle;
    GDEWeaponData _weapon;
    public GDEWeaponData Weapon
    {
        get
        {
            return _weapon;
        }
        set
        {
            if (_weapon != value)
            {
                _weapon = value;
                UpdateDisplay();
            }
        }
    }

    public void UpdateDisplay()
    {
        if (_weapon != null)
        {
            lbWeapnName.text = Weapon.name;
            spWeaonIcon.spriteName = Weapon.thumb;
            makerEquiped.SetActive(Weapon.isEquipment);
        }
    }

    public void OnSelect(bool s)
    {
        if (s)
        {
            MenuManager.Instance.SelectWeapon(Weapon);
        }
    }

    public void Select()
    {
        toggle.Set(true);
    }
}
