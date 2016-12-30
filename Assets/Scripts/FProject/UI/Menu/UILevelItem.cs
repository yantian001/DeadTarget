using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UILevelItem : UIWidgetContainer
{
    public enum Statu
    {
        UnInteractable,
        Checked,
        UnChecked,
    }

    static protected List<UILevelItem> s_LevelItems = new List<UILevelItem>();

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
                    onChange(LevelID);
                }
            }
        }
    }

    private int _levelID;
    public int LevelID
    {
        get
        {
            return _levelID;
        }
        set
        {
            _levelID = value;
            if(label)
            {
                label.text = _levelID.ToString();
            }
        }
    }
    public UILabel label;
    public UISprite mask;
    public Color disableColor;
    public Color currentColor;
    private void UpdateDisplay()
    {
        //throw new NotImplementedException();
        if(!Interactable)
        {
            label.color = disableColor;
            mask.gameObject.SetActive(false);
        }
        else
        {
            label.color = Color.white;
            if(Selected)
            {
                mask.gameObject.SetActive(true);
                label.color = currentColor;
            }
            else
            {
                mask.gameObject.SetActive(false);

            }
        }
    }

    public delegate void OnChange(int i);
    public OnChange onChange;
    // Use this for initialization
    void Start()
    {
        UpdateDisplay();
    }

    public void OnEnable()
    {
        s_LevelItems.Add(this);
    }

    public void OnDisable()
    {
        s_LevelItems.Remove(this);
    }
    
    void OnClick()
    {
        // Debug.Log("OnClick LevelItem");
        if (Interactable)
        {
            Selected = true;
        }
    }

    void DeSelectOther()
    {
        for (int i = 0; i < s_LevelItems.Count; i++)
        {
            if (s_LevelItems[i] != this)
            {
                s_LevelItems[i].Selected = false;
            }


        }
    }
}