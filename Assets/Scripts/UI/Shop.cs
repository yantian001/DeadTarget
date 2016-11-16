using UnityEngine;
using System.Collections;
using GameDataEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class Shop : MonoBehaviour {
	
	RectTransform rect;

	int currentpos = 0;
	int maxpos = 10;

	List<string> objs;

	IGDEData item = null;
	 
	string WEAPON="weapon";
	string MEDICAL="medical";
	string BOMB="bomb";

	Button buy;
	Button equipment;

	Dictionary<string,object> weapons;
	string type;
	// Use this for initialization
	void Start () {

		type = GameValue.shopType;

		rect = GetComponent<RectTransform>();
		GDEDataManager.Init("gde_data");

		if(type == WEAPON)
		{
			initWeapon();
		}else if(type == MEDICAL)
		{
			initMedical();
		}else if(type == BOMB)
		{
			initBomb();
		}else {
			return;
		}

		currentpos = 0;
		maxpos = objs.Count;

		initBtn();

		updateObject();
		setupBtn();
		UpdateFuncBtn();

	}

	void initWeapon()
	{
		if(!GDEDataManager.GetAllDataKeysBySchema("Weapon",out objs))
		{
			return;
		}

		if(!GDEDataManager.GetAllDataBySchema("Weapon", out weapons))
		{
			return;
		}
	}

	void initMedical()
	{
		if(!GDEDataManager.GetAllDataKeysBySchema("Medical",out objs))
		{
			return;
		}
	}

	void initBomb()
	{
		if(!GDEDataManager.GetAllDataKeysBySchema("Bomb",out objs))
		{
			return;
		}
	}


	void updateObject()
	{
		if(type == WEAPON)
		{
			updateWeapon();
		}else if(type == MEDICAL)
		{
			updateMediacal();
		}else if(type == BOMB)
		{
			updateBomb();
		}else {
			return;
		}
	}

	void updateWeapon()
	{

		GDEWeaponData weapon;
		
		if (!GDEDataManager.DataDictionary.TryGetCustom(objs[currentpos], out weapon))
		{
			weapon = null;
		}

		item = weapon;

 

		Text name = CommonUtils.GetChildComponent<Text>(rect,"middle/details/name");
		name.text = "Attribute details";
		
		CommonUtils.SetChildActive(rect,"middle/details/attr",true);

		CommonUtils.SetChildActive(rect,"middle/details/detail",false);
		
		name = CommonUtils.GetChildComponent<Text>(rect,"middle/info/Image/name");
		name.text = weapon.name;
		
		name = CommonUtils.GetChildComponent<Text>(rect,"middle/info/cost/Text");
		name.text = weapon.cost.ToString();
		
		RawImage thumb = CommonUtils.GetChildComponent<RawImage>(rect,"middle/info/thumb");

		Texture2D res = Resources.Load(weapon.thumb) as Texture2D;



		thumb.GetComponent<RectTransform>().sizeDelta = new Vector2(res.width,res.height);

		thumb.texture = res;


		Slider slider = CommonUtils.GetChildComponent<Slider>(rect,"middle/details/attr/huoli/Slider");
		slider.value = weapon.huoli;


		slider = CommonUtils.GetChildComponent<Slider>(rect,"middle/details/attr/shesu/Slider");
		slider.value = weapon.shesu;


		slider = CommonUtils.GetChildComponent<Slider>(rect,"middle/details/attr/danjia/Slider");
		slider.value = weapon.danjia;

		slider = CommonUtils.GetChildComponent<Slider>(rect,"middle/details/attr/wendingxing/Slider");
		slider.value = weapon.wendingxing;

	}

	void updateMediacal()
	{

		GDEMedicalData medical;
		
		if (!GDEDataManager.DataDictionary.TryGetCustom(objs[currentpos], out medical))
		{
			medical = null;
		}
		
		item = medical;


		CommonUtils.SetChildActive(rect,"middle/details/attr",false);
		
		CommonUtils.SetChildActive(rect,"middle/details/detail",true);

		Text name = CommonUtils.GetChildComponent<Text>(rect,"middle/details/name");
		name.text = "Item details";

		Text desc = CommonUtils.GetChildComponent<Text>(rect,"middle/details/detail");
		 
		desc.text = medical.desc;

		name = CommonUtils.GetChildComponent<Text>(rect,"middle/info/Image/name");
		name.text = medical.name;

		name = CommonUtils.GetChildComponent<Text>(rect,"middle/info/cost/Text");
		name.text = medical.cost.ToString();

		RawImage thumb = CommonUtils.GetChildComponent<RawImage>(rect,"middle/info/thumb");
		
		Texture2D res = Resources.Load(medical.thumb) as Texture2D;
		
		thumb.GetComponent<RectTransform>().sizeDelta = new Vector2(204,158);
		
		thumb.texture = res;

	}

	void updateBomb()
	{

		GDEBombData bomb;
		
		if (!GDEDataManager.DataDictionary.TryGetCustom(objs[currentpos], out bomb))
		{
			bomb = null;
		}
		
		item = bomb;

		Text name = CommonUtils.GetChildComponent<Text>(rect,"middle/details/name");
		name.text = "Item details";
		
		CommonUtils.SetChildActive(rect,"middle/details/attr",false);
		
		CommonUtils.SetChildActive(rect,"middle/details/detail",true);
		
		Text desc = CommonUtils.GetChildComponent<Text>(rect,"middle/details/detail");
		 
		desc.text = bomb.desc;
		
		name = CommonUtils.GetChildComponent<Text>(rect,"middle/info/Image/name");
		name.text = bomb.name;
		
		name = CommonUtils.GetChildComponent<Text>(rect,"middle/info/cost/Text");
		name.text = bomb.cost.ToString();
		
		RawImage thumb = CommonUtils.GetChildComponent<RawImage>(rect,"middle/info/thumb");
		
		Texture2D  res = Resources.Load(bomb.thumb) as Texture2D ;

		thumb.texture = res;

		thumb.GetComponent<RectTransform>().sizeDelta = new Vector2(175,208);
	}


	void setupBtn()
	{
		buy = CommonUtils.GetChildComponent<Button>(rect,"bottom/buy");
		
		buy.onClick.AddListener(delegate() {
			if(type == WEAPON)
			{
				GDEWeaponData weapon = (GDEWeaponData)item;
				if(Player.CurrentUser.IsMoneyEnough(weapon.cost))
				{
					weapon.isowned = true;
					Player.CurrentUser.UseMoney(weapon.cost);
				}
			}
			else if(type == MEDICAL)
			{
				GDEMedicalData medical = (GDEMedicalData)item;
				
				if(Player.CurrentUser.IsMoneyEnough(medical.cost))
				{
					medical.number += 1;
					Player.CurrentUser.UseMoney(medical.cost);
				}
				
				
			}else if(type == BOMB)
			{
				GDEBombData bomb = (GDEBombData)item;
				if(Player.CurrentUser.IsMoneyEnough(bomb.cost))
				{
					bomb.number += 1;
					Player.CurrentUser.UseMoney(bomb.cost);
				}
			}
			UpdateFuncBtn();
		});


		equipment = CommonUtils.GetChildComponent<Button>(rect,"bottom/equipment");

		equipment.onClick.AddListener(delegate() {
			
			GDEWeaponData weapon = (GDEWeaponData)item;

			weapon.isEquipment = true;

			foreach (var w in weapons)
			{
				if(w.Key != objs[currentpos])
				{
					GDEWeaponData curWeapon;
					if (GDEDataManager.DataDictionary.TryGetCustom(w.Key, out curWeapon))
					{
						curWeapon.isEquipment = false;
					}
				}
			}

			UpdateFuncBtn();

		});

	}

	void UpdateFuncBtn()
	{



		if(type != WEAPON)
		{
			equipment.interactable = false;
		}

		if(type.Equals(WEAPON))
		{
			GDEWeaponData weapon = (GDEWeaponData)item;


			if(weapon.isowned == true )
			{
				buy.interactable = false;

				if(weapon.isEquipment == true)
					equipment.interactable = false;
				else
					equipment.interactable = true;

			}
			else
			{
				buy.interactable = true;
				equipment.interactable = false;
			}
		}
	}

	void initBtn()
	{


		Button left = CommonUtils.GetChildComponent<Button>(rect,"middle/left");
		Button right = CommonUtils.GetChildComponent<Button>(rect,"middle/right");



		left.onClick.AddListener(delegate() {
			if(currentpos >= 1)
			{
				currentpos  -= 1;
				updateObject();
				UpdateFuncBtn();
			}
		});
		
		right.onClick.AddListener(delegate() {
			if(currentpos < maxpos-1)
				currentpos+=1;
				updateObject();
				UpdateFuncBtn();
		});
	}

	// Update is called once per frame
	void Update () {
		
	}
}
