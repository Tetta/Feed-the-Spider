using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//D:\Programs\AndroidSDK\platform-tools\adb logcat ActivityManager:I com.feedthespider/com.unity3d.player.UnityPlayerNativeActivity:D *:S

public class initClass : MonoBehaviour {

	//private GameObject testLabel;
	public GameObject googlePlus;
	public GameObject achievements;
	public GameObject leaderboards;
	public GameObject closeMenu;
	public GameObject market;
	public GameObject spider;

	static public Dictionary<string, int> progress = new Dictionary<string, int>();
	//static public string mainMenuState = "start";

	private int i;
	//private int LastNotificationId = 0;


	// Use this for initialization
	void Start () { 
		if (progress.Count == 0) {
			getProgress();
			staticClass.initLevels();
			if (progress["sound"] == 0) setSound(false);
			if (progress["music"] == 1) GameObject.Find("music").GetComponent<AudioSource>().enabled = true;
			//опции
			GameObject.Find("settings folder").transform.GetChild(0).gameObject.SetActive(true);
			GameObject.Find(Localization.language).GetComponent<UIToggle>().value = true;
			GameObject.Find("settings folder").transform.GetChild(0).gameObject.SetActive(false);
			//
			//push
			// bug GameThrive.Init("0e3f3ad6-c7ee-11e4-9aca-47e056863935", "660292827653", HandleNotification);
			GoogleCloudMessageService.instance.InitPushNotifications ();
			Debug.Log( "......................................");
			//AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleDay"), "notiferTitleDay", 600);
			//AndroidNotificationManager.instance.OnNotificationIdLoaded += OnNotificationIdLoaded;
			//AndroidNotificationManager.instance.LocadAppLaunchNotificationId();

			List<LocalNotificationTemplate> PendingNotifications;
			PendingNotifications = AndroidNotificationManager.instance.LoadPendingNotifications();
			//Debug.Log (PendingNotifications);
			bool flagNotifer = false;
			if (PendingNotifications != null) { 
				foreach (var PendingNotification in PendingNotifications) {
					if (PendingNotification.title == Localization.Get("notiferTitleDay")) {
						if (PendingNotification.fireDate.Day == DateTime.Now.Day) {
							AndroidNotificationManager.instance.CancelLocalNotification(PendingNotification.id);
							AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleDay"), Localization.Get("notiferMessageDay"), 60 * 60 * 24);
						}
						flagNotifer = true;
					}
				}
			}
			//bug!!!
			//if (!flagNotifer) AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleDay"), Localization.Get("notiferMesssageDay"), 60 * 60 * 24);

			Debug.Log( "......................................");
			//


		}

		//market
		Debug.Log ("initClass: market.SetActive(true)");
		market.SetActive(true);

		//включаем текущий скин и выключаем все остальные
		for (int i = 0; i < 5; i++) {
			if (spider.transform.GetChild(i).name == staticClass.currentSkin) {
				spider.transform.GetChild(i).gameObject.SetActive(true);
				//включаем текущую шапку и выключаем все остальные
				for (int j = 0; j < 4; j++) {
					if (spider.transform.GetChild (i).GetChild (0).GetChild (j).name == staticClass.currentHat) {
						spider.transform.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (true);
					} else 
						spider.transform.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (false);
				}
			} else 
				spider.transform.GetChild(i).gameObject.SetActive(false);
		}

		if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			achievements.SetActive(true);
			leaderboards.SetActive(true);
			googlePlus.SetActive(false);
		} else if (progress["googlePlay"] == 1) GooglePlayConnection.instance.connect ();

		//listen for GooglePlayConnection events
		//off (new plugin)
		//GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		//GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
	}

	// Update is called once per frame
	void Update () {

		//Application.RegisterLogCallback(handleLog);
		if (Input.GetKey(KeyCode.Escape)) {
			closeMenu.SetActive(true);
		}
	}

	private void OnPlayerConnected() {
		Debug.Log("OnPlayerConnected");
		achievements.SetActive(true);
		leaderboards.SetActive(true);
		googlePlus.SetActive(false);
		initClass.progress["googlePlay"] = 1;
		initClass.saveProgress();
	}

	private void OnPlayerDisconnected() {
		Debug.Log("OnPlayerDisconnected");
		GooglePlayConnection.instance.disconnect ();
		achievements.SetActive(false);
		leaderboards.SetActive(false);
		googlePlus.SetActive(true);
		initClass.progress["googlePlay"] = 0;
		initClass.saveProgress();
	}

	static public void saveProgress() {
		string strProgress = "";
		foreach (var item in progress ) {
			strProgress += item.Key + "=" + item.Value + ";";
		}
		PlayerPrefs.SetString("progress", strProgress);
		PlayerPrefs.Save();
	}
	
	static public void getProgress() {

		string strProgressDefault = staticClass.strProgressDefault;
		//сброс прогресса
		//PlayerPrefs.SetString("progress", strProgressDefault);
		string strProgress = PlayerPrefs.GetString("progress");
		Debug.Log (strProgress);
		//NGUIDebug.Log(strProgress);
		if (strProgress == "") strProgress = strProgressDefault;
		string strKey = "", strValue = "";
		bool flag = true;
		for (int i = 0; i < strProgress.Length; i++) {
			if (strProgress.Substring(i, 1) == "=") flag = false;
			
			else if (strProgress.Substring(i, 1) == ";") {
				flag = true;
				progress[strKey] = int.Parse(strValue);
				//скины и шапки. запись в статик переменную
				if (strKey.Substring(0, 4) == "skin") if (progress[strKey] == 2) staticClass.currentSkin = strKey;
				if (strKey.Substring(0, 3) == "hat") if (progress[strKey] == 2) staticClass.currentHat = strKey;
				if (strKey.Length == 6) if(strKey.Substring(0, 5) == "berry") if (progress[strKey] == 2) staticClass.currentBerry = strKey;
				//if (strKey.Substring(0, 4) == "skin") {Debug.Log (strKey); Debug.Log (progress[strKey]);}
				strKey = "";
				strValue = "";
			} else if (flag) strKey += strProgress.Substring(i, 1);
			else if (!flag) strValue += strProgress.Substring(i, 1);
			
		}
		for (int i = 0; i < strProgressDefault.Length; i++) {
			if (strProgressDefault.Substring(i, 1) == "=") flag = false;
			
			else if (strProgressDefault.Substring(i, 1) == ";") {
				flag = true;
				if (!progress.ContainsKey(strKey)) progress[strKey] = int.Parse(strValue);
				strKey = "";
				strValue = "";
			} else if (flag) strKey += strProgressDefault.Substring(i, 1);
			else if (!flag) strValue += strProgressDefault.Substring(i, 1);
		}
		//Debug.Log (strProgress);
		saveProgress();
	}

	/*
	static public void updateProgress() {
		goldLabel.text = progress["coins"].ToString();
		starsLabel.text = progress["stars"].ToString();

	}
	*/

	private void OnDestroy() {
		if(!GooglePlayConnection.IsDestroyed) {
			//off (new plugin)
			//GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
			//GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		}
	}

	static public void setSound(bool flag) {
		UIPlaySound[] sounds = Resources.FindObjectsOfTypeAll(typeof(UIPlaySound))as UIPlaySound[];
		foreach (UIPlaySound sound in sounds) {
			if (flag) sound.enabled = true;
			else sound.enabled = false;
		}

	}

	// Gets called when the player opens the notification. (GameThrive)
	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive) {
		Debug.Log("HandleNotification");
		Debug.Log(message);
	}

	// Gets called when the player opens the notification. (Local)
	private void OnNotificationIdLoaded (int notificationid){
		Debug.Log( "App was laucnhed with notification id: " + notificationid);
	}



}