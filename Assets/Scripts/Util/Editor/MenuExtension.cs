using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class MenuExtension : EditorWindow
{
    //[MenuItem("FUG/Mission/Mission Editor")]
    //private static void OpenMissionEditor()
    //{
    //    MissionWindowEditor.Init();
    //}

    [MenuItem("FUG/Tools/Delete Joint")]
    private static void DeleteAllJoint()
    {
        GameObject o = Selection.activeGameObject;
        Joint[] joints = o.GetComponentsInChildren<Joint>();
        foreach (Joint j in joints)
        {
            Rigidbody r = j.GetComponent<Rigidbody>();
            DestroyImmediate(j);
            DestroyImmediate(r);
        }
        Selection.selectionChanged();
    }

    [MenuItem("FUG/Tools/Add HitBody")]
    private static void AddHitBody()
    {
        //GameObject o = Selection.activeGameObject;
        //Hit_Body hb = o.GetComponentInChildren<Hit_Body>();
        //if (hb)
        //{
        //    Collider[] cs = o.GetComponentsInChildren<Collider>();
        //    if (cs.Length > 0)
        //    {
        //        UnityEditorInternal.ComponentUtility.CopyComponent(hb);
        //        foreach (Collider c in cs)
        //        {
        //            if (c.gameObject.GetComponent<Hit_Body>()) continue;
        //            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(c.gameObject);
        //        }

        //    }
        //}
        //else
        //{
        //    Debug.Log("Dont have Hit_body for copy !");
        //}
    }

    [MenuItem("FUG/Tools/Colider/Delete")]
    private static void DeleteColiders()
    {
        GameObject o = Selection.activeGameObject;
        Collider[] cs = o.GetComponentsInChildren<Collider>();
        foreach (Collider c in cs)
        {
            DestroyImmediate(c);
        }
        Selection.selectionChanged();
    }
    [MenuItem("FUG/Tools/Colider/Disable")]
    private static void DisableColiders()
    {
        GameObject o = Selection.activeGameObject;
        Collider[] cs = o.GetComponentsInChildren<Collider>();
        foreach (Collider c in cs)
        {
            //DestroyImmediate(c);
            c.enabled = false;
        }
        Selection.selectionChanged();
    }

    [MenuItem("FUG/Tools/Colider/Enable")]
    private static void EnableColiders()
    {
        GameObject o = Selection.activeGameObject;
        Collider[] cs = o.GetComponentsInChildren<Collider>();
        foreach (Collider c in cs)
        {
            //DestroyImmediate(c);
            c.enabled = true;
        }
        Selection.selectionChanged();
    }

    [MenuItem("FUG/Debug/print/Chapter Result")]
    private static void PrintResult()
    {
        Debug.Log(PlayerPrefs.GetString("cptRst", ""));
    }

    [MenuItem("FUG/Button/Set Audio")]
    private static void SetClickAudio()
    {
        //AudioClip ac = Resources.Load<AudioClip>("audio/fx_button1");
        //var buttons = GameObject.FindObjectsOfType<ButtonClick>();
        //foreach (ButtonClick b in buttons)
        //{
        //    b.clickClip = ac;
        //}
    }
}
