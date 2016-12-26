using System;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInfo
{
    public enum FormFactor
    {
        tablet,
        small,
        medium,
        large
    }

    public enum PerformanceLevel
    {
        Low,
        Medium,
        High
    }

    private static DeviceInfo _instance;

    public string deviceModel = SystemInfo.deviceModel;

    public readonly float dpi;

    public readonly DeviceInfo.FormFactor formFactor;

    public readonly bool isHighres = (float)Screen.height > 480f;

    public DeviceInfo.PerformanceLevel performanceLevel = DeviceInfo.PerformanceLevel.High;

    private List<string> lowDeviceList = new List<string>
    {
        "asus ASUS_T00I"
    };

    public static DeviceInfo instance
    {
        get
        {
            if (DeviceInfo._instance == null)
            {
                DeviceInfo._instance = new DeviceInfo();
            }
            return DeviceInfo._instance;
        }
    }

    private DeviceInfo()
    {
        if (Screen.height < 500)
        {
            this.formFactor = DeviceInfo.FormFactor.small;
        }
        else if (Screen.height < 900)
        {
            this.formFactor = DeviceInfo.FormFactor.medium;
        }
        else
        {
            this.formFactor = DeviceInfo.FormFactor.large;
        }
        if (this.isTablet())
        {
            this.formFactor = DeviceInfo.FormFactor.tablet;
        }
        if (Screen.height >= 500 && Screen.width > 320)
        {
            this.isHighres = true;
        }
        else
        {
            this.isHighres = false;
        }
        this.dpi = Screen.dpi;
        if (this.dpi <= 0f)
        {
            this.dpi = 300f;
        }
        if (this.isDeviceLowPerformance())
        {
            this.performanceLevel = DeviceInfo.PerformanceLevel.Low;
        }
        if (this.performanceLevel == DeviceInfo.PerformanceLevel.Low)
        {
            Debug.Log("Using low profile");
        }
        Debug.Log(string.Concat(new object[]
        {
            "Screen size: h = ",
            Screen.height,
            " w = ",
            Screen.width
        }));
        if (Screen.height <= 480)
        {
            Debug.Log("Low resolution using half texture size");
            QualitySettings.masterTextureLimit = 1;
        }
    }

    private bool isDeviceLowPerformance()
    {
        if (this.lowDeviceList.Contains(SystemInfo.deviceModel))
        {
            return true;
        }
        int processorCount = SystemInfo.processorCount;
        string processorType = SystemInfo.processorType;
        int systemMemorySize = SystemInfo.systemMemorySize;
        int graphicsMemorySize = SystemInfo.graphicsMemorySize;
        if (processorCount >= 4)
        {
            return false;
        }
        if (processorType.Contains("rev"))
        {
            int num = processorType.IndexOf("rev");
            string text = processorType.Substring(num + 3).Trim();
            if (text.Contains(" "))
            {
                int length = text.IndexOf(" ");
                int num2;
                if (int.TryParse(text.Substring(0, length).Trim(), out num2))
                {
                    bool flag = processorCount >= 2;
                    bool flag2 = num2 >= 6;
                    bool flag3 = systemMemorySize >= 512;
                    bool flag4 = graphicsMemorySize >= 250;
                    if (flag && flag2 && flag3 && flag4)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool isTablet()
    {
        float f = (Screen.dpi > 0f) ? ((float)Screen.width / Screen.dpi) : ((float)Screen.width);
        float f2 = (Screen.dpi > 0f) ? ((float)Screen.height / Screen.dpi) : ((float)Screen.height);
        double num = (double)Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
        return num >= 6.0;
    }
}
