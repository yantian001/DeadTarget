using System;


public class ConvertUtil
{
    /// <summary>
    /// 转换为Int
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int ToInt32(object val,int ret = 0)
    {
        //int ret = 0;
        if(val != null )
        {
            int.TryParse(ToString(val), out ret);
        }
        return ret;
    }

    /// <summary>
    /// 转换为float
    /// </summary>
    /// <param name="val"></param>
    /// <param name="ret">默认值</param>
    /// <returns></returns>
    public static float ToFloat(object val, float ret = 0.0f)
    {
        //float ret = 0.0f;
        if(val != null)
        {
            float.TryParse(ToString(val), out ret);
        }
        return ret;
    }
    /// <summary>
    /// 转换为string
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string ToString(object val,string ret = "")
    {
        //string ret = "";
        if (val != null)
        {
            
            ret = val.ToString();
        }
        return ret;
    }

    public static bool ToBool(object val,bool ret = false)
    {
        if(val != null)
        {
             bool.TryParse(ToString(val),out ret);
        }
        return ret;
    }
}

