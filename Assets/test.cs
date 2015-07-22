using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	IEnumerator Start() {
		AsyncOperation async = Application.LoadLevelAsync("menu");
		AsyncOperation async2 = Application.LoadLevelAsync("level menu");
		async2.allowSceneActivation = false;
		//yield return async2;
		yield return async;
		//Debug.Log("Loading complete");
	}
}