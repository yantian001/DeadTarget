using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class FileUtils
{
    public static void Save2File(string filename ,string fileContent )
    {
        string file = Application.persistentDataPath + "/" + filename;
        //FileInfo f = new FileInfo(file);
        FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(fileContent);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    public static string ReadFile(string filename)
    {
        string s = "";
        string file = Application.persistentDataPath + "/" + filename;
        Debug.Log("read content from " +file);
        if (File.Exists(file))
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            s = sr.ReadToEnd();
            sr.Close();
            fs.Close();
        }
        return s;
    }
}

