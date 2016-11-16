using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
    AsyncOperation asyn = null;
	// Use this for initialization
	void Start () {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        asyn = Application.LoadLevelAsync(GameValue.s_CurrentSceneName);
        asyn.allowSceneActivation = true;
        yield return asyn;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
