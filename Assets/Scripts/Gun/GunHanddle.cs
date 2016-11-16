using UnityEngine;
using System.Collections;
using System;

public class GunHanddle : MonoBehaviour
{
    public Camera FPScamera;
    public Gun[] Guns;
    public int GunIndex = -1;
    [HideInInspector]
    public Gun CurrentGun;

    public Crosshair crossHair = null;

    bool isFirstFire = false;

    void OnEnable()
    {
        if (Guns.Length < 1)
        {
            Guns = this.gameObject.GetComponentsInChildren<Gun>();
        }
        if (crossHair == null)
        {
            crossHair = GameObject.FindGameObjectWithTag("CrossHair").GetComponent<Crosshair>();
        }
        SwitchGun();

    }

    private void OnPreviewStart(LTEvent obj)
    {
        // throw new NotImplementedException();
        SwitchGun();
    }

    public void Awake()
    {
        //LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
        //LeanTween.addListener((int)Events.PREVIEWSTART, OnPreviewStart);
    }

    public void Start()
    {
        if(!FPScamera)
        {
            FPScamera = Camera.main;
        }
    }

    public void OnDisable()
    {
        //LeanTween.removeListener((int)Events.GAMEFINISH, OnGameFinish);
        //LeanTween.removeListener((int)Events.PREVIEWSTART, OnPreviewStart);
    }


    void OnGameFinish(LTEvent evt)
    {
        //if (evt.data != null)
        //{
        //    GameRecords record = evt.data as GameRecords;
        //    if (record != null)
        //    {
        //        if (record.FinishType == GameFinishType.Failed)
        //        {
        //            //hide gun
        //            for(int i= 0;i<Guns.Length;i++)
        //            {
        //                Guns[GunIndex].SetActive(false);
        //            }
        //        }
        //    }
        //}
        for (int i = 0; i < Guns.Length; i++)
        {
            Guns[GunIndex].SetActive(false);
        }
    }

    void Hide(GameObject gameObject, bool show)
    {
        /*Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
    	foreach (Renderer r in renderers) {
        	r.enabled = show;
    	}*/
    }

    public void Reload()
    {
        if (CurrentGun)
            CurrentGun.Reload();
    }



    public void SwitchGun(int index)
    {
        //if (FPScamera.enabled)
        //  {
        for (int i = 0; i < Guns.Length; i++)
        {
            //Hide(Guns[i].gameObject, false);
            Guns[i].SetActive(false);
        }
        if (Guns.Length > 0 && index < Guns.Length && index >= 0)
        {
            GunIndex = index;
            CurrentGun = Guns[GunIndex];
            Hide(Guns[GunIndex].gameObject, true);
            Guns[GunIndex].SetActive(true);
        }
        //    }
    }

    public void SwitchGun()
    {

        // int currentId = WeaponManager.Instance.GetCurrentWeaponId();
        int currentId = WeaponManager.Instance.GetCurrentWeaponId();
        if (currentId == -1)
        {
            SwitchGun(0);

        }
        else
        {
            int select = -1;
            for (int i = 0; i < Guns.Length; i++)
            {
                if (Guns[i].id == currentId)
                {
                    select = i;
                    break;
                }
            }
            if (select == -1)
                SwitchGun(0);
            else
                SwitchGun(select);
        }
    }

    public void Shoot()
    {
        if (CurrentGun)
        {

            if (CurrentGun.Shoot())
            {
                crossHair.currentSpace += 10;
                isFirstFire = true;
                if (isFirstFire)
                {
                    // BehaviorDesigner.Runtime.GlobalVariables.Instance.SetVariableValue("Fired", true);
                    // LeanTween.dispatchEvent((int)Events.FIRED);
                }
            }

        }
    }

    public void HoldBreath(int noiseMult)
    {
        //if (CurrentGun)
        //    CurrentGun.FPSmotor.Holdbreath(noiseMult);
    }

    public bool CanFire()
    {
        return false;
    }
}
