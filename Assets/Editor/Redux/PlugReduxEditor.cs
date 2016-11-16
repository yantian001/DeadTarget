using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Diagnostics;

/* Classe d茅coup茅e en deux parties :
 * 	-R茅duction d'un ou de tous les mat茅riaux et suppression de tous les mat茅riaux
 * 	-Mise 脿 jour (Et inversement) des mat茅riaux vers "mat茅riaux_reduxed" d'un ou de tous les objets de la sc猫ne
 * */

public class Paths
{
	public static string CookerPath
	{
		get
		{
			string p;
			if (Application.platform == RuntimePlatform.OSXEditor)
				p = "sbscooker";
			else
				p = "sbscooker.exe";
			
			return p;
		}
	}

	public static string MutatorPath
	{
		get
		{
			string p;
			if (Application.platform == RuntimePlatform.OSXEditor)
				p = "sbsmutator";
			else
				p = "sbsmutator.exe";
			
			return p;
		}
	}
}

public class PlugRedux : MonoBehaviour
{
	private static string m_fromPathSbsar;
	private static string m_toPathSbsar = "Assets/Substances/";
	private static string m_pathTools = Application.dataPath + "/Editor/Redux/BatchTools";
	private static string m_pathFilter = m_pathTools + "/Filters/B2S.sbsar";
	
	//private static const string m_mutator_bin = "sbsmutator";
		
    [MenuItem("CONTEXT/Material/Redux It")]
    static void reduxContext(MenuCommand command)
	{
		Material curMat = (Material)command.context;
		
		redux(curMat);
    }
	
	[MenuItem("Assets/Redux It")]
    static void reduxRight()
	{
		Material curMat = Selection.activeObject as Material;
		
		if (curMat is Material)
			redux(curMat);
    }
	
	static void redux(Material curMat)
	{
		if (curMat.name.Length < 8 || curMat.name.Substring(curMat.name.Length-8, 8).IndexOf("_reduxed") == -1)
		{
			m_fromPathSbsar = processRedux(curMat, /*Utils.getPathTools(ref m_pathTools)*/m_pathTools);
			if (m_fromPathSbsar != null)
			{
				reduxToMatAll();	// Prevents the loss of material objects in the scene
				ProceduralMaterial substanceTmp = loadSbsar(m_fromPathSbsar, m_toPathSbsar + curMat.name + ".sbsar");
				createNewMaterial(curMat, substanceTmp);
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Redux", "It's a reduxed material!", "Ok");
		}

		FileUtil.DeleteFileOrDirectory(m_pathTools + '/' + curMat.name.Replace(" ", "_") + ".sbs");
		FileUtil.DeleteFileOrDirectory(m_pathTools + '/' + curMat.name.Replace(" ", "_") + ".sbsar");
		AssetDatabase.Refresh();
	}
	
	static ProceduralMaterial loadSbsar(string fromPath, string toPath)
	{
		if (!Directory.Exists("Assets/Substances"))
			AssetDatabase.CreateFolder("Assets", "Substances");
		if (File.Exists(toPath))
			AssetDatabase.DeleteAsset(toPath);
				
		FileUtil.CopyFileOrDirectory(fromPath, toPath);
		AssetDatabase.Refresh();
		
		return (AssetDatabase.LoadAssetAtPath(toPath, typeof(ProceduralMaterial)) as ProceduralMaterial);
	}
	
	static void createNewMaterial(Material mat, ProceduralMaterial substanceTmp)
	{
		// Create new material
		Material matRedux = new Material(mat.shader);
		matRedux.CopyPropertiesFromMaterial(mat);
		if (mat.HasProperty("_MainTex"))
			matRedux.SetTexture("_MainTex", substanceTmp.GetTexture("_MainTex"));
		if (mat.HasProperty("_BumpMap") && mat.GetTexture("_BumpMap") != null)
			matRedux.SetTexture("_BumpMap", substanceTmp.GetTexture("_BumpMap"));
		
		// Create new material in Project
		string matPath = AssetDatabase.GetAssetPath(mat);
		AssetDatabase.CreateAsset(matRedux, matPath.Substring(0, matPath.Length-4) + "_reduxed" + ".mat");
		
		// End
		print("Redux: " + mat.name);
		
		// Reimport
		substanceTmp.RebuildTextures();
		AssetDatabase.ImportAsset(matPath);
		AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(matRedux));
		AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(substanceTmp));
		
		// Change options Sbsar
			// OutputSize : Doesn't work
		/*int maxDiffuse = Utils.getMaxTextureSize(mat.GetTexture("_MainTex"));
		int maxBump;
		int maxSize = maxDiffuse;
		if (mat.HasProperty("_BumpMap"))
		{
			maxBump = Utils.getMaxTextureSize(mat.GetTexture("_BumpMap"));
			maxSize = (maxDiffuse > maxBump ? maxDiffuse : maxBump);
		}
		float v = Mathf.Log(maxSize, 2);
		substanceTmp.SetProceduralVector("$outputsize", new Vector4(v, v, 0.0f, 0.0f));
		substanceTmp.RebuildTextures();*/
			// TextureAlphaSource
		SubstanceImporter sImp = SubstanceImporter.GetAtPath(AssetDatabase.GetAssetPath(substanceTmp)) as SubstanceImporter;
		ProceduralMaterial[] sMat = sImp.GetMaterials();
		Texture[] genTextures = substanceTmp.GetGeneratedTextures();
		foreach (Texture tex in genTextures)
		{
			ProceduralTexture procTex = tex as ProceduralTexture;
			if (procTex != null && procTex.GetProceduralOutputType() == ProceduralOutputType.Diffuse)
				sImp.SetTextureAlphaSource(substanceTmp, tex.name, ProceduralOutputType.Unknown);
		}

//		AssetDatabase.Refresh();
		sMat[0].shader = mat.shader;
	}
	
	[MenuItem("Redux/Redux All Materials")]
	static void reduxAllMat()
	{
		reduxToMatAll();	// Prevents the loss of material objects in the scene
		print("All materials (" + createOrDeleteMat(true, /*Utils.getPathTools(ref m_pathTools)*/m_pathTools) + ") reduxed!");
	}
	
	[MenuItem("Redux/Delete All Materials Redux")]
	static void deleteReduxAllMat()
	{
		reduxToMatAll();
		print("All redux materials (" + createOrDeleteMat(false, null) + ") deleted!");
	}
	
	static int createOrDeleteMat(bool createMat, string pathToolsSbs)
	{
		Material[] mat = Utils.getAllEditorMaterials();
		
		int countMat = 0;
		foreach (Material curMat in mat)
		{
			string assetPath = AssetDatabase.GetAssetPath(curMat);
			if (Path.GetExtension(assetPath) == ".mat")
			{
				if (createMat)
				{
					if (curMat.name.Length < 8 || curMat.name.Substring(curMat.name.Length-8, 8).IndexOf("_reduxed") == -1)
					{
						m_fromPathSbsar = processRedux(curMat, pathToolsSbs);
						if (m_fromPathSbsar != null)
						{
							ProceduralMaterial substanceTmp = loadSbsar(m_fromPathSbsar, m_toPathSbsar + curMat.name + ".sbsar");
							createNewMaterial(curMat, substanceTmp);
							countMat++;
						}
					}
				}
				else
				{
					if (curMat.name.Length > 8 && curMat.name.Substring(curMat.name.Length-8, 8).IndexOf("_reduxed") != -1)
					{
						AssetDatabase.DeleteAsset(assetPath);
						countMat++;
					}
				}
			}
		}

		return (countMat);
	}
	
	[MenuItem("Redux/Update Materials")]
	static void matToReduxAll()
	{
		GameObject[] go = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		
		foreach (GameObject g in go)
		{
			if (g.GetComponent<Renderer>())
				updateMat(g.GetComponent<MeshRenderer>(), true);
		}
	}
	
	[MenuItem("Redux/Revert Update Materials")]
	static void reduxToMatAll()
	{
		GameObject[] go = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		
		foreach (GameObject g in go)
		{
			if (g.GetComponent<Renderer>())
				updateMat(g.GetComponent<MeshRenderer>(), false);
		}
	}
	
	[MenuItem("CONTEXT/MeshRenderer/MatToRedux")]
	static void matToRedux(MenuCommand command)
	{
		updateMat((MeshRenderer)command.context, true);
	}
	
	[MenuItem("CONTEXT/MeshRenderer/ReduxToMat")]
	static void reduxToMat(MenuCommand command)
	{
		updateMat((MeshRenderer)command.context, false);
	}
	
	static void updateMat(MeshRenderer meshR, bool isMatToRedux)
	{
		//MeshRenderer meshR = (MeshRenderer)command.context;
		Material[] objMatCopy = meshR.GetComponent<Renderer>().sharedMaterials;
		Material[] allMat = Utils.getAllEditorMaterials();
		
		for (int i = 0 ; i < meshR.GetComponent<Renderer>().sharedMaterials.Length ; i++)
		{
			foreach (Material curAllMat in allMat)
			{
				if (objMatCopy[i] && objMatCopy[i].name == curAllMat.name)
				{
					if (isMatToRedux)
					{
						if (curAllMat.name.Length < 8 || curAllMat.name.Substring(curAllMat.name.Length-8, 8).IndexOf("_reduxed") == -1)
						{
							Material reduxMat = searchReduxOrMat(allMat, curAllMat.name + "_reduxed");
							if (reduxMat == null)
							{
								m_fromPathSbsar = processRedux(curAllMat, /*Utils.getPathTools(ref m_pathTools)*/m_pathTools);
								if (m_fromPathSbsar != null)
								{
									ProceduralMaterial substanceTmp = loadSbsar(m_fromPathSbsar, m_toPathSbsar + curAllMat.name + ".sbsar");
									createNewMaterial(curAllMat, substanceTmp);
									allMat = Utils.getAllEditorMaterials();
									reduxMat = searchReduxOrMat(allMat, curAllMat.name + "_reduxed");
								}
							}
							if (reduxMat)
								objMatCopy[i] = reduxMat;
						}
					}
					else
					{
						if (curAllMat.name.Length > 8 && curAllMat.name.Substring(curAllMat.name.Length-8, 8).IndexOf("_reduxed") != -1)
						{
							Material mat = searchReduxOrMat(allMat, curAllMat.name.Substring(0, curAllMat.name.Length-8));
							if (mat)
								objMatCopy[i] = mat;
						}
					}
				}
			}
		}
		meshR.GetComponent<Renderer>().sharedMaterials = objMatCopy;
	}
	
	static Material searchReduxOrMat(Material[] allMat, string searchName)
	{
		Material tmpMat = null;
		
		foreach (Material curMat in allMat)
		{
			if (curMat.name == searchName)
				tmpMat = curMat;
		}
		
		return (tmpMat);
	}
	
	static string processRedux(Material mat, string pathToolsSbs)
	{
		if (mat.mainTexture && pathToolsSbs != "")
		{
			string matName = mat.name.Replace(" ", "_");

			pathToolsSbs += "/";
			string qualityCompression = "0.80";
			string pathDiffuse = Application.dataPath + AssetDatabase.GetAssetPath(mat.GetTexture("_MainTex")).Substring(6);

			bool isDiffusePSD = false;

			// PSD case //
			if (pathDiffuse.EndsWith(".psd"))
			{
				isDiffusePSD = true;

				string pathAsset = AssetDatabase.GetAssetPath(mat.GetTexture("_MainTex"));
				
				TextureImporter texImporter = (TextureImporter) TextureImporter.GetAtPath(pathAsset);
				bool origReadable = texImporter.isReadable;
				TextureImporterFormat origFormat = texImporter.textureFormat;
				TextureImporterNPOTScale origNPOTScale = texImporter.npotScale;
				texImporter.isReadable = true;
				texImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				texImporter.npotScale = TextureImporterNPOTScale.None;
				AssetDatabase.ImportAsset(pathAsset, ImportAssetOptions.ForceUpdate);

				Texture2D diffuseTex = (Texture2D)AssetDatabase.LoadAssetAtPath(pathAsset, typeof(Texture2D));

				Utils.saveTexToPNG(diffuseTex, pathDiffuse.Replace(".psd", "_tmp.png"));

				pathDiffuse = pathDiffuse.Replace(".psd", "_tmp.png");

				texImporter.isReadable = origReadable;
				texImporter.textureFormat = origFormat;
				texImporter.npotScale = origNPOTScale;
			}

			string diffuseInput = "";
			string normalInput = "";
			//string nameSbsar = mat.name + "_reduxed.sbsar";
			string toPath = "Assets/Substances/Temp/" + "tmp.sbsar";
			// Copy Filter in /Substances/Temp
			if (!Directory.Exists("Assets/Substances"))
				AssetDatabase.CreateFolder("Assets", "Substances");
			if (!Directory.Exists("Assets/Substances/Temp"))
				AssetDatabase.CreateFolder("Assets/Substances", "Temp");
			if (File.Exists(toPath))
				AssetDatabase.DeleteAsset(toPath);
			FileUtil.CopyFileOrDirectory(m_pathFilter, toPath);
			AssetDatabase.Refresh();
			// Load ProceduralMaterial
			ProceduralMaterial pMat = AssetDatabase.LoadAssetAtPath(toPath, typeof(ProceduralMaterial)) as ProceduralMaterial;
			// Get Inputs
			foreach (ProceduralPropertyDescription tweak in pMat.GetProceduralPropertyDescriptions())
			{
				if (tweak.type == ProceduralPropertyType.Texture)
				{
					if (tweak.label.Contains("Diffuse"))
						diffuseInput = tweak.name;
					if (tweak.label.Contains("Normal"))
						normalInput = tweak.name;
				}
			}
			// Get Input Diffuse
			//string primaryInput = Utils.getSubstringByString("INPUT ", " COLOR", Utils.shellExecute(pathToolsSbs + "sbsmutator", pathToolsSbs, "info " + '"' + m_pathFilter + '"'));

//			string pathSbsMutator = "specialization --input " + '"' + m_pathFilter + '"' + " --presets-path " + '"' + pathToolsSbs + '"' + " --output-path " + '"' + pathToolsSbs + '"' + " --output-name Sbs --output-graph-name Sbs --connect-image " + '"' + diffuseInput + "@path@" + pathDiffuse + "@format@JPEG@level@" + qualityCompression + '"';
			string pathSbsMutator = "specialization --input " + '"' + m_pathFilter + '"' + " --presets-path " + '"' + pathToolsSbs + '"' + " --output-path " + '"' + pathToolsSbs + '"' + " --output-name " + matName + " --output-graph-name " + matName + " --connect-image " + '"' + diffuseInput + "@path@" + pathDiffuse + "@format@JPEG@level@" + qualityCompression + '"';

//			FileUtil.DeleteFileOrDirectory(pathToolsSbs + "Sbs.sbs");
			FileUtil.DeleteFileOrDirectory(pathToolsSbs + matName + ".sbs");
//			FileUtil.DeleteFileOrDirectory(pathToolsSbs + "Sbs.sbsar");
			FileUtil.DeleteFileOrDirectory(pathToolsSbs + matName + ".sbsar");
			
			if (mat.HasProperty("_BumpMap") && mat.GetTexture("_BumpMap") != null && normalInput.Length > 0)	// Processing with Diffuse and Normal
			{
				string pathNormal = Application.dataPath + AssetDatabase.GetAssetPath(mat.GetTexture("_BumpMap")).Substring(6);
				Texture2D texTmp = Utils.flipNormal(mat.GetTexture("_BumpMap"));
				pathNormal = Application.dataPath + "/" + mat.GetTexture("_BumpMap").name + "_tmp.png";
				Utils.saveTexToPNG(texTmp, pathNormal);
				
				Utils.shellExecute(pathToolsSbs + Paths.MutatorPath, pathSbsMutator + " --connect-image " + '"' + normalInput + "@path@" + pathNormal + "@format@JPEG@level@" + qualityCompression + '"');
//				Utils.shellExecute(pathToolsSbs + Paths.CookerPath, "--no-optimization --output-path " + '"' + pathToolsSbs + '"' + " Sbs.sbs");
				Utils.shellExecute(pathToolsSbs + Paths.CookerPath, "--no-optimization --output-path " + '"' + pathToolsSbs + "\" " + matName + ".sbs");
				
				FileUtil.DeleteFileOrDirectory(pathNormal);
			}
			else	// Processing with Diffuse only
			{
				Utils.shellExecute(pathToolsSbs + Paths.MutatorPath, pathSbsMutator);
//				Utils.shellExecute(pathToolsSbs + Paths.CookerPath, "--output-path " + '"' + pathToolsSbs + '"' + " Sbs.sbs");
				Utils.shellExecute(pathToolsSbs + Paths.CookerPath, "--output-path " + '"' + pathToolsSbs + "\" " + matName + ".sbs");
			}

			if (isDiffusePSD)
				FileUtil.DeleteFileOrDirectory(pathDiffuse);

			UnityEngine.Debug.Log(Application.dataPath + "/Substances/Temp");
			FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Substances/Temp");

//			return (pathToolsSbs + "Sbs.sbsar");
			return (pathToolsSbs + matName + ".sbsar");
		}
		
		if (pathToolsSbs == "")
			EditorUtility.DisplayDialog("Redux", "Path is wrong!\nPlease select a path for Tools.", "Ok");
		
		return (null);
	}
	
	/*[MenuItem("Redux/Change Path Tools")]
	static void setPathTools()
	{
		m_pathTools = "";
		FileUtil.DeleteFileOrDirectory(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + '/' + "cfg_pathTools");
		if (Utils.getPathTools(ref m_pathTools) == "")
			EditorUtility.DisplayDialog("Redux", "Path is wrong!\nPlease select a path for Tools.", "Ok");
	}*/
	
	[MenuItem("Redux/Change Filter")]
	static void setFilter()
	{
		string tmp = EditorUtility.OpenFilePanel("Filter Redux", m_pathFilter.Substring(0, m_pathFilter.LastIndexOf('/')), "");
		
		if (tmp != "")
			m_pathFilter = tmp;
	}
}