using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	public void LoadByNo(int no)
	{
		if (no == 1) {
			//setting_UI
			SceneManager.LoadScene ("setting_UI", LoadSceneMode.Additive);
		}
		if (no == 2) {
			SceneManager.LoadScene ("notification_UI", LoadSceneMode.Additive);
		}
		if (no == 3) {
			SceneManager.LoadScene ("history_UI", LoadSceneMode.Additive);
		}
		if (no == 4) {
			SceneManager.LoadScene ("notice_UI", LoadSceneMode.Additive);
		}
	}
}
