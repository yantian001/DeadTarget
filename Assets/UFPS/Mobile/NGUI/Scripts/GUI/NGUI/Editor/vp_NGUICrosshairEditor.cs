/////////////////////////////////////////////////////////////////////////////////
//
//	vp_NGUICrosshairEditor.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	custom inspector for the vp_NGUICrosshair class
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(vp_NGUICrosshair))]
public class vp_NGUICrosshairEditor : vp_NGUIControlEditor
{

	protected vp_NGUICrosshair m_Target = null;
	
	protected override void OnEnable()
	{
	
		base.OnEnable();
		
		m_Target = (vp_NGUICrosshair)target;
	
	}
	

	/// <summary>
	/// 
	/// </summary>
	public override void OnInspectorGUI()
	{

		Undo.RecordObject(target, "NGUI Crosshair Snapshot");

		GUI.color = Color.white;
		
		GUILayout.Space(10);
		
		GUILayout.BeginHorizontal();
		m_Target.StartingTexture = (Texture)EditorGUILayout.ObjectField("Crosshair", m_Target.StartingTexture, typeof(Texture), false);
		m_Target.EnemyCrosshair = (Texture)EditorGUILayout.ObjectField("Enemy Crosshair", m_Target.EnemyCrosshair, typeof(Texture), true);
		GUILayout.EndHorizontal();
		m_Target.EnemyMask = vp_EditorGUIUtility.LayerMaskField("Enemy Mask", m_Target.EnemyMask, true);
		m_Target.EnemyColor = EditorGUILayout.ColorField("Enemy Color", m_Target.EnemyColor);
		m_Target.ColorChangeDuration = EditorGUILayout.FloatField("Color Change Duration", m_Target.ColorChangeDuration);
		m_Target.InteractIconSize = EditorGUILayout.Slider("Interact Icon Size", m_Target.InteractIconSize, 0, 10);
		GUILayout.BeginHorizontal();
		m_Target.ScaleOnRun = EditorGUILayout.Toggle("Scale on Run", m_Target.ScaleOnRun);
		if(m_Target.ScaleOnRun)
		{
			GUILayout.Space(10);
			GUILayout.Label("Multiplier");
			m_Target.ScaleMultiplier = EditorGUILayout.Slider(m_Target.ScaleMultiplier, 0, 10);
		}
		GUILayout.EndHorizontal();
		m_Target.AssistedTargetingFoldout = EditorGUILayout.Foldout(m_Target.AssistedTargetingFoldout, "Assisted Targeting");
		if(m_Target.AssistedTargetingFoldout)
		{
			EditorGUI.indentLevel++;
			m_Target.AssistedTargeting = EditorGUILayout.Toggle("Enabled", m_Target.AssistedTargeting);
			m_Target.AssistedTargetingSpeed = EditorGUILayout.Slider("Tracking Speed", m_Target.AssistedTargetingSpeed, 0, 100);
			m_Target.AssistedTargetingRadius = EditorGUILayout.Slider("Lock On Radius", m_Target.AssistedTargetingRadius, 0, 10);
			m_Target.AssistedTargetingTrackingRadius = EditorGUILayout.Slider("Tracking Radius", m_Target.AssistedTargetingTrackingRadius, 0, 10);
			EditorGUI.indentLevel--;
		}
		
		GUILayout.Space(10);

		// update
		if (GUI.changed)
			EditorUtility.SetDirty(target);

	}
	

}

