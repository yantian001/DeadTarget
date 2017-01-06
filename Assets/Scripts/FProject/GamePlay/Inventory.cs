using UnityEngine;
using System.Collections;

namespace FProject
{
    public class Inventory : MonoBehaviour
    {
        public vp_PlayerEventHandler Player;
        private vp_WeaponHandler m_WeaponHandler = null;    // should never be referenced directly
        protected vp_WeaponHandler WeaponHandler    // lazy initialization of the weapon handler field
        {
            get
            {
                if (m_WeaponHandler == null)
                    m_WeaponHandler = transform.GetComponent<vp_WeaponHandler>();
                return m_WeaponHandler;
            }
        }
        // Use this for initialization
        void Start()
        {
            if (!Player)
            {
                Player = gameObject.GetComponent<vp_PlayerEventHandler>();
            }
        }

        public void OnEnable()
        {
            Player.Register(this);
        }

        public void OnDisable()
        {
            Player.Unregister(this);
        }

        protected virtual int OnValue_CurrentWeaponAmmoCount
        {
            get
            {
                return WeaponManager.Instance.GetCurrentWeapon().currentbullet;
            }
            set
            {
                //vp_UnitBankInstance weapon = CurrentWeaponInstance as vp_UnitBankInstance;
                //if (weapon == null)
                //    return;
                //weapon.TryGiveUnits(value);
                WeaponManager.Instance.TryGiveAmmo(value);
            }
        }

        protected virtual int OnValue_CurrentWeaponMaxAmmoCount
        {
            get
            {
                return WeaponManager.GetWeaponCapacity(WeaponManager.Instance.GetCurrentWeapon());
            }
        }


        protected virtual int OnValue_CurrentWeaponClipCount
        {
            get
            {

                return WeaponManager.Instance.GetCurrentWeapon().bullet;

            }

        }

        protected virtual bool OnAttempt_DepleteAmmo()
        {

            // TODO: perhaps this should be checked in vp_Inventory

            //if (CurrentWeaponIdentifier == null)
            //    return MissingIdentifierError();

            //if (WeaponHandler.CurrentWeapon.AnimationType == (int)vp_Weapon.Type.Thrown)
            //    TryReload(CurrentWeaponInstance as vp_UnitBankInstance);
            if (WeaponManager.Instance.GetCurrentWeapon().currentbullet <= 0)
                return false;
            WeaponManager.Instance.GetCurrentWeapon().currentbullet -= 1;
            return true;
            //return TryDeduct(CurrentWeaponIdentifier.Type as vp_UnitBankType, CurrentWeaponIdentifier.ID, 1);

        }

        protected virtual bool TryReload()
        {
            return WeaponManager.Instance.TryReload();
        }

        protected virtual bool CanStart_SetWeapon()
        {

            int index = (int)Player.SetWeapon.Argument;
            if (index == 0)
                return true;

            if ((index < 1) || index > (WeaponHandler.Weapons.Count))
                return false;

            bool haveItem = WeaponManager.Instance.HaveItem(index);

            //// see if weapon is thrown
            //if (haveItem && (vp_Weapon.Type)WeaponHandler.Weapons[index - 1].AnimationType == vp_Weapon.Type.Thrown)
            //{

            //    if (GetAmmoInWeapon(WeaponHandler.Weapons[index - 1]) < 1)
            //    {
            //        vp_UnitBankType uType = weaponIdentifier.Type as vp_UnitBankType;
            //        if (uType == null)
            //        {
            //            Debug.LogError("Error (" + this + ") Tried to wield thrown weapon " + WeaponHandler.Weapons[index - 1] + " but its item identifier does not point to a UnitBank.");
            //            return false;
            //        }
            //        else
            //        {
            //            if (!TryReload(uType, weaponIdentifier.ID)) // NOTE: ID might not work for identification here because of multiplayer add-on pickup logic
            //            {
            //                //Debug.Log("uType: " + uType + ", weapon.ID: " + weapon.ID);
            //                //Debug.Log("failed because: no thrower wielded and no extra ammo");
            //                return false;
            //            }
            //        }
            //        //Debug.Log("success because: no thrower wielded but we have extra ammo");
            //    }
            //    //else
            //    //Debug.Log("success because: thrower wielded");

            //}
            if(haveItem)
            {
                TryReload();
            }
            return haveItem;

        }

        protected virtual bool OnAttempt_RefillCurrentWeapon()
        {

            //if (CurrentWeaponIdentifier == null)
            //    return MissingIdentifierError();

            return TryReload();

        }
    }
}
