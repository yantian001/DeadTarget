using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Diagnostics;

public static class Utils
{
	//! @brief Execute a command
	//! @return true on success, false otherwise
	public static bool shellExecute(string process, string args)
    {
		string wdir = Path.GetDirectoryName(process);
		
		
		
	    ProcessStartInfo pStartInfo = new ProcessStartInfo(process)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardInput = false,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = wdir,
            Arguments = args
        };
		Process p = new Process();
		p.StartInfo = pStartInfo;
		p.Start();
		
		p.WaitForExit();
		
		string stdOut = p.StandardOutput.ReadToEnd();
		string stdErr = p.StandardError.ReadToEnd();
		
		if (p.ExitCode != 0)
		{
			EditorUtility.DisplayDialog("Redux Mobile Error", wdir + stdOut + stdErr, "OK");
			return false;
		}
		
		p.Close();
		
		return true;
	}
	
	public static string getSubstringByString(string firstStr, string lastStr, string inStr)
	{
		return (inStr.Substring((inStr.IndexOf(firstStr) + firstStr.Length), (inStr.IndexOf(lastStr) - inStr.IndexOf(firstStr) - firstStr.Length)));
    }
	
	public static Texture2D flipNormal(Texture tex)
	{
		copyTextureToRoot(tex);
		
		Texture2D texTmp = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/" + tex.name + "_tmp.png", typeof(Texture2D));
		setReadable(texTmp);
		setTextureFormat(TextureImporterFormat.RGBA32, texTmp);
		
		int w = texTmp.width;
		int h = texTmp.height;
		for (int i = 0 ; i < w ; i++)
		{
			for (int j = 0 ; j < h ; j++)
			{
				Color c = texTmp.GetPixel(i, j);
				c.g = 1 - c.g;
				texTmp.SetPixel(i, j, c);
			}
		}
		
		return (texTmp);
	}
	
	public static void copyTextureToRoot(Texture tex)
	{
		string toPath = Application.dataPath + "/" + tex.name + "_tmp.png";
		
		FileUtil.DeleteFileOrDirectory(toPath);
		FileUtil.CopyFileOrDirectory(AssetDatabase.GetAssetPath(tex), toPath);
		AssetDatabase.Refresh();
	}
	
	public static void saveTexToPNG(Texture2D tex, string pathFile)
	{
		byte[] bytes = tex.EncodeToPNG();
		
		File.WriteAllBytes(pathFile, bytes);
	}
	
	public static Material[] getAllEditorMaterials()
	{
		string[] allAssetsPaths = AssetDatabase.GetAllAssetPaths();
		ArrayList tmpMatList = new ArrayList();
		Material tmpMat;
		
		foreach (string path in allAssetsPaths)
		{
			tmpMat = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
			if(tmpMat != null)
				tmpMatList.Add(tmpMat);
		}

		return ((Material[])tmpMatList.ToArray(typeof(Material)));
	}
	
	public static int getMaxTextureSize(Texture tex)
	{
		string path = AssetDatabase.GetAssetPath(tex);
		TextureImporter texImp = TextureImporter.GetAtPath(path) as TextureImporter;
		
		return (texImp.maxTextureSize);
	}
	
	public static string getPathTools()
	{
		string pathTools = "";
		string tmpPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + '/' + "cfg_pathTools";
		
		if (File.Exists(tmpPath))
		{
			using (StreamReader sr = new StreamReader(tmpPath))
			{
				pathTools = sr.ReadLine();
			}
		}
		
		return (pathTools);
	}
	
	public static string getPathTools(ref string pathTools)
	{
		if (!File.Exists(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + '/' + "cfg_pathTools"))
		{
			if (pathTools == "")
			{
				pathTools = EditorUtility.OpenFolderPanel("Tools Redux", (pathTools == "" ? "" : pathTools.Substring(0, pathTools.LastIndexOf('/'))), "ToolsRedux");
				if (pathTools != "")
				{
					if (File.Exists(pathTools + '/' + "sbsmutator"))
						setPathTools(pathTools);
					else
						pathTools = "";
				}
			}
		}
		else
		{
			if (pathTools == "")
				pathTools = getPathTools();
		}
		
		return (pathTools);
	}
	
	public static void setReadable(Texture tex)
	{
		TextureImporter texImp = TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
		
		if (!texImp.isReadable)
			texImp.isReadable = true;
	}
	
	public static void setTextureFormat(TextureImporterFormat newFormat, Texture tex)
	{
		string path = AssetDatabase.GetAssetPath(tex);
		TextureImporter texImp = TextureImporter.GetAtPath(path) as TextureImporter;
		
		texImp.textureFormat = newFormat;
		AssetDatabase.ImportAsset(path);
	}
	
	public static void setPathTools(string pathTools)
	{
		using (StreamWriter sw = new StreamWriter(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + '/' + "cfg_pathTools"))
		{
			sw.WriteLine(pathTools);
		}
	}
}