using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyMask :Mask 
{
	protected override void Start ()
	{
		base.Start ();

		int width = Screen.width;
		int height = Screen.height;
		int designWidth = 900;//开发时分辨率宽
		int designHeight = 600;//开发时分辨率高
		float s1 = (float)designWidth / (float)designHeight;
		float s2 = (float)width / (float)height;

		//目标分辨率小于 960X640的 需要计算缩放比例
		float contentScale =1f;
		if(s1 > s2) {
			contentScale = s1/s2;
		}
		Canvas	canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		Vector2 pos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, transform.position, canvas.GetComponent<Camera>(), out pos)){
			ParticleSystem  [] particlesSystems	 = transform.GetComponentsInChildren<ParticleSystem>();
			RectTransform rectTransform = transform as RectTransform;
			float minX,minY,maxX,maxY;
			minX = rectTransform.rect.x  + pos.x;
			minY = rectTransform.rect.y+ pos.y;
			maxX = minX + rectTransform.rect.width ;
			maxY = minY + rectTransform.rect.height;


			//这里 100  是因为ugui默认的缩放比例是100  你也可以去改这个值，但是我觉得最好别改。
			foreach(ParticleSystem particleSystem in particlesSystems)
			{
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MinX",minX/100/contentScale);
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MinY",minY/100/contentScale);
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MaxX",maxX/100/contentScale);
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MaxY",maxY/100/contentScale);
			}
		}
	}
}
