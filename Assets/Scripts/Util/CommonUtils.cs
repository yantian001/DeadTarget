using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommonUtils
{

    /// <summary>
    /// 是否能正常联网
    /// </summary>
    /// <returns></returns>
    public static bool IsNetworkOk()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }

    /// <summary>
    /// 设置子节点的文本值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="text"></param>
    public static void SetChildText(RectTransform parent, string child, string text)
    {
        var childNode = parent.FindChild(child);
        if (childNode != null)
        {
            var textNode = childNode.GetComponent<Text>();
            if (textNode)
            {
                textNode.text = text;
            }
        }
    }
    /// <summary>
    /// 设置文本组件内容
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="text"></param>
    public static void SetText(GameObject obj, string text)
    {
        if (obj == null)
            return;
        var textNode = obj.GetComponent<Text>();
        if (textNode)
            textNode.text = text;
    }

    /// <summary>
    /// 设置RawImage子节点的Texture2d值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="texture"></param>
    public static void SetChildRawImage(RectTransform parent, string child, Texture2D texture)
    {
        if (parent == null || texture == null || string.IsNullOrEmpty(child))
            return;
        var childNode = parent.FindChild(child);
        if (childNode != null)
        {
            var img = childNode.GetComponent<RawImage>();
            if (img != null)
            {
                img.texture = texture;
            }
        }
    }
    /// <summary>
    /// 设置子节点是否可见
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="isActive"></param>
    public static void SetChildActive(RectTransform parent, string child, bool isActive)
    {
        if (parent == null || string.IsNullOrEmpty(child))
            return;
        var childNode = parent.FindChild(child);
        if (childNode != null)
        {
            childNode.gameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// 设置子对象的Slider值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="value"></param>
    public static void SetChildSliderValue(RectTransform parent, string child, float value)
    {
        var childTran = parent.FindChild(child);
        if (childTran)
        {
            Slider slider = childTran.GetComponentInChildren<Slider>();
            if (slider)
            {
                slider.value = value;
            }
        }
    }

    public static void SetChildButtonActive(RectTransform parent, string child, bool b)
    {
        if (parent == null || string.IsNullOrEmpty(child))
            return;

        var childNode = parent.FindChild(child);
        if (childNode)
        {
            var button = childNode.GetComponent<Button>();
            button.interactable = b;
        }

    }

    public static void SetChildToggleOn(RectTransform parent, string child, bool isOn)
    {
        if (parent == null || string.IsNullOrEmpty(child))
            return;
        var childNode = parent.FindChild(child);
        if (childNode)
        {
            var toggle = childNode.GetComponent<Toggle>();
            toggle.isOn = isOn;
        }
    }

    public static void SetChildToggleInteractable(RectTransform parent, string child, bool interactable)
    {
        if (parent == null || string.IsNullOrEmpty(child))
            return;
        var childNode = parent.FindChild(child);
        if (childNode)
        {
            var toggle = childNode.GetComponent<Toggle>();
            toggle.interactable = interactable;
        }
    }
    /// <summary>
    /// 拷贝UI对象的位置，以及父对象
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void CopyRectTransfrom(RectTransform from, RectTransform to)
    {
        if (from == null || to == null)
            return;
        to.SetParent(from.parent);
        to.anchoredPosition = from.anchoredPosition;
        to.anchorMax = from.anchorMax;
        to.anchorMin = from.anchorMin;
        to.pivot = from.pivot;
    }

    /// <summary>
    /// 设置按钮的回调
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="action"></param>
    public static void SetChildButtonCallBack(RectTransform parent, string child, UnityEngine.Events.UnityAction action)
    {
        if (parent == null || string.IsNullOrEmpty(child) || action == null)
            return;

        var childNode = parent.FindChild(child);
        if (childNode)
        {
            var button = childNode.GetComponent<Button>();
            if (button)
            {
                button.onClick.RemoveListener(action);
                button.onClick.AddListener(action);
            }
        }
    }
    /// <summary>
    /// 获得子节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public static RectTransform GetChild(RectTransform parent, string child)
    {
        RectTransform rect = null;
        if (parent != null)
        {
            Transform t = parent.FindChild(child);
            if (t)
            {
                rect = t.GetComponent<RectTransform>();
            }
        }
        return rect;
    }

    /// <summary>
    /// 获取指定子节点的特定类型的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public static T GetChildComponent<T>(RectTransform parent, string child)
    {
        T ret = default(T);
        RectTransform rt = GetChild(parent, child);
        if (rt)
            ret = rt.GetComponent<T>();
        return ret;
    }

    public static void SetChildComponentActive<T>(Transform t, bool b)
    {
        T[] ts = t.GetComponents<T>();
        if (ts != null)
        {
            for (int i = 0; i < ts.Length; i++)
            {
                (ts[i] as MonoBehaviour).enabled = b;
            }
        }
    }

    /// <summary>
    /// 检测地面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3 DetectGround(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, -Vector3.up, out hit, 1000.0f))
        {
            return hit.point;
        }
        return position;
    }


}
