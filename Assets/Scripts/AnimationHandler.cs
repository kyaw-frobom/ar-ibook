using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour {

	public Sprite background;

	public void SettingUISlideBack()
	{
		GameObject.Find ("SettingUI").GetComponent<Animator>().Play("UISettingSlideBack");
		StartCoroutine (WaitForUIAni ("setting_UI"));
	}

	//to slide back notification UI
	public void NotificationUISlideBack()
	{
		GameObject.Find ("NotiUI").GetComponent<Animator>().Play("UISettingSlideBack");
		StartCoroutine (WaitForUIAni ("notification_UI"));
		GameObject.Find ("Npanel").GetComponent<Image> ().sprite = background;
	}

	public void HistoryUISlideBack()
	{
		GameObject.Find ("HistoryUI").GetComponent<Animator>().Play("UISettingSlideBack");
		StartCoroutine (WaitForUIAni ("history_UI"));
		GameObject.Find ("Hpanel").GetComponent<Image> ().sprite = background;
	}

	public void NoticeUISlideBack()
	{
		GameObject.Find ("NoticeUI").GetComponent<Animator>().Play("UISettingSlideBack");
		StartCoroutine (WaitForUIAni ("notice_UI"));
		GameObject.Find ("NoPanel").GetComponent<Image> ().sprite = background;
	}

	//wait for UI slide Back animation and then destroy this scene
	IEnumerator WaitForUIAni(string scene_name)
	{
		yield return new WaitForSeconds(1);
		SceneManager.UnloadScene(scene_name);
	}
}
