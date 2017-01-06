using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIShopTab : UIWidgetContainer
{

    static protected List<UIShopTab> lists = new List<UIShopTab>();

    public delegate void OnChange(int index);
    public OnChange onChange;
    public int index = -1;
    [SerializeField]
    private bool _interactable = true;
    public bool Interactable
    {
        get
        {
            return _interactable;
        }
        set
        {
            _interactable = value;

        }
    }
    [SerializeField]
    private bool _selected = false;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            if (_selected == value)
                return;
            _selected = value;
            UpdateDisplay();
            if (_selected)
            {
                DeSelectOther();
                if (onChange != null)
                {
                    onChange(index);
                }
            }
        }
    }

    public GameObject activeGameObject;
    public GameObject inactiveGameObject;

    private void DeSelectOther()
    {
        //throw new NotImplementedException();
        for (int i = 0; i < lists.Count; i++)
        {
            if (lists[i] != this)
            {
                lists[i].Selected = false;
            }


        }
    }

    private void UpdateDisplay()
    {
        //throw new NotImplementedException();
        vp_Utility.Activate(activeGameObject, Selected);
        vp_Utility.Activate(inactiveGameObject, !Selected);
    }

    void OnClick()
    {
        // Debug.Log("OnClick LevelItem");
        if (Interactable)
        {
            Selected = true;
        }
    }
    public void OnEnable()
    {
        lists.Add(this);
    }
    public void OnDisable()
    {
        lists.Remove(this);
    }
    
}
