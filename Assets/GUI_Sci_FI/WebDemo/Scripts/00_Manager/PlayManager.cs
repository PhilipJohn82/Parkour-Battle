using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayManager : Singleton<PlayManager>
{
	public bool _isDemoBarOn = false;
	public bool _isBackgroundOn = false;
	public bool _isFirstPlay = true;
	public int medalCount = 0;

	public WebDemoController _WebDemoController;

	public void InIt () {}

	public bool _isButtonDown = false;

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousepos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (mousepos.y > -4) {
				_isButtonDown = true;
			}
		} else if(Input.GetMouseButtonUp(0)){ 
				_isButtonDown = false;
		}
	}

	public void SceneLoad (string sceneName) {
		//_WebDemoController.TextColorReset ();

		AsyncOperation async = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single);
		async.allowSceneActivation = false;

		if (async.progress < 0.9f) {
			async.allowSceneActivation = true;
		}
	}

	//public void SceneLoad (string sceneName, bool _isTrue) {
	//	_WebDemoController.TextColorReset ();


	//	AsyncOperation async = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single);
	//	async.allowSceneActivation = false;

	//	if (async.progress < 0.9f) {
	//		async.allowSceneActivation = true;
	//	}
	//}


	//public void SceneLoad (string sceneName, bool _isPanelOn) {
	//	AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
	//	async.allowSceneActivation = false;

	//	if( async.progress < 0.9f ) {
	//		async.allowSceneActivation = true;
	//	}
	//}

	//public void SceneLoad (string sceneName) {
	//	GameObject loading = (GameObject)Instantiate(Resources.Load("Loading"), new Vector3(0f, 0f, 0f), Quaternion.identity);
	//	_Loading = loading.GetComponent<Loading> ();
	//	StartCoroutine (SceneLoadCo (sceneName));
	//}

	//public void SceneLoad (string sceneName, int num) { 
	//	GameObject loading = (GameObject) Instantiate (Resources.Load ("Loading_Camera"), new Vector3 (0f, 0f, 0f), Quaternion.identity);
	//	_Loading = loading.GetComponent<Loading> ();
	//	StartCoroutine (SceneLoadCo (sceneName));
	//}

	//IEnumerator SceneLoadCo (string sceneName) {
	//	yield return new WaitForSeconds (0.5f);
	//	AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
	//	async.allowSceneActivation = false;

	//	while(!async.isDone)
	//	{
	//		yield return new WaitForSeconds (0.5f);
	//		async.allowSceneActivation = true;

	//		yield return new WaitForSeconds (1f);
	//		_Loading.LoadingEnd ();
	//	}
	//}

}

