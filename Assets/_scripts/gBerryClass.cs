using UnityEngine;
using System.Collections;
using System;
//using UnityEngine.Advertisements;
using System.Collections.Generic;

public class gBerryClass : MonoBehaviour {

	public static string berryState; 
	public static int starsCounter;

	private GameObject completeMenu;
	private UILabel guiTimer;
	private GameObject pauseMenu;
	private GameObject[] guiStars = new GameObject[3];
	//private UISprite[] guiStarsComplete = new UISprite[3];
	private Animator spiderAnimator;
	//private GameObject restart;
	private GameObject back;
	private Vector3 dir = new Vector3(0, 0, 0);
	private float t = 0;

	// Use this for initialization
	void Start () {

		Time.timeScale = 1;
		//notifer
		/* OFF FOR TESTS
		List<LocalNotificationTemplate> PendingNotifications;
		PendingNotifications = AndroidNotificationManager.instance.LoadPendingNotifications();
		foreach (var PendingNotification in PendingNotifications) {
			if (PendingNotification.title == Localization.Get("notiferTitleEnergy")) {
				AndroidNotificationManager.instance.CancelLocalNotification(PendingNotification.id);
			}
		}
		*/
		//energy, если нет комплекта
		if (initClass.progress.Count == 0) initClass.getProgress();
		//запись текущего уровня
		initClass.progress["currentLevel"] = int.Parse(Application.loadedLevelName.Substring(5));
		//for tests
		initClass.progress["level2"] = 0;
		//
		initClass.saveProgress ();
	

		/* OFF FOR TESTS
		if (initClass.progress["complect"] == 0) {
			int energyCount = lsEnergyClass.checkEnergy(false);
			if (energyCount == 0) {
				AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleEnergy"), Localization.Get("notiferMessageEnergy"), lsEnergyClass.costEnergy * lsEnergyClass.maxEnergy);
				lsEnergyClass.energyMenuState = "energy";
				Application.LoadLevel("level menu");
			}
		}
		*/
		staticClass.useWeb = 0;
		staticClass.timer = 0;
		staticClass.useSluggish = false;
		staticClass.useDestroyer = false;
		staticClass.useYeti = false;
		staticClass.useGroot = false;

		starsCounter = 0;
		berryState = "";
		GameObject gui = GameObject.Find("gui");
		completeMenu = gui.transform.Find("complete menu").gameObject;
		pauseMenu = gui.transform.Find("pause menu").gameObject;
		guiStars[0] = GameObject.Find("gui star 1");
		guiStars[1] = GameObject.Find("gui star 2");
		guiStars[2] = GameObject.Find("gui star 3");

		//guiStarsComplete[0] = GameObject.Find("gui").transform.GetChild(0).GetChild(2).GetComponent<UISprite>();
		//guiStarsComplete[1] = GameObject.Find("gui").transform.GetChild(0).GetChild(3).GetComponent<UISprite>();
		//guiStarsComplete[2] = GameObject.Find("gui").transform.GetChild(0).GetChild(4).GetComponent<UISprite>();
		//restart = GameObject.Find("restart");
	
		spiderAnimator = GameObject.Find ("spider").transform.GetChild(int.Parse(staticClass.currentSkin.Substring(4, 1)) - 1).GetComponent<Animator>();
		//timer
		if (initLevelMenuClass.levelDemands == 1) {
			int levels = staticClass.levels[Convert.ToInt32(Application.loadedLevelName.Substring(5)), 1];
			if (levels >= 1 && levels <=99) {
				GameObject.Find("gui timer").GetComponent<UILabel>().enabled = true;
				guiTimer = GameObject.Find("gui timer 2").GetComponent<UILabel>();
				guiTimer.enabled = true;
			}
		}

		/*
		//showAd, если нет комплекта
		if (initClass.progress["complect"] == 0) {
			staticClass.showAd ++;

			if (staticClass.showAdColony < 2 && staticClass.showAd >= 5) {
				if (!Advertisement.isReady("defaultVideoAndPictureZone")) {
					staticClass.showAd = 4;
					Debug.Log("!isReady 1");
				} else { 
					Debug.Log("isReady 1");
					staticClass.showAdColony ++;
					staticClass.showAd = 0;
					Advertisement.Show("defaultVideoAndPictureZone");
				}
			} else if (staticClass.showAdColony >= 2 && staticClass.showAd >= 5) {
				if (Advertisement.isReady("rewardedVideoZone")) {
					Debug.Log("isReady 2");
					staticClass.showAdColony = 0;
					staticClass.showAd = 0;
					Advertisement.Show("rewardedVideoZone");
				} 
			}
		}
		*/
		/*
		staticClass.showAd ++;
		NGUIDebug.Log(AdColony.StatusForZone("vz3da177fe9cf44ef9b9")); 
		if (staticClass.showAd >= 5 && !staticClass.loadAd) {
			staticClass.showAd = 4;
			NGUIDebug.Log(staticClass.testCounter + ": showAd >= 5 && !loadAd");
			AndroidAdMobController.instance.LoadInterstitialAd ();
		} else if (staticClass.showAd >= 5 && staticClass.loadAd) {
			NGUIDebug.Log(staticClass.testCounter + ": showAd >= 5 && loadAd");
			staticClass.showAdColony ++;
			staticClass.loadAd = false;
			staticClass.showAd = 0;
			if (staticClass.showAdColony >= 3) {
				NGUIDebug.Log(staticClass.testCounter + ": showAdColony");
				staticClass.showAdColony = 0;
				staticClass.loadAd = true;
				// Check to see if a video is available in the zone.
				AdColony.ShowVideoAd("vz3da177fe9cf44ef9b9"); 
				if(AdColony.IsVideoAvailable("vz3da177fe9cf44ef9b9")) {
					NGUIDebug.Log(staticClass.testCounter + ": Play AdColony Video");
					AdColony.ShowVideoAd("vz3da177fe9cf44ef9b9"); 
				} else {
					NGUIDebug.Log(staticClass.testCounter + ": Video Not Available");
				}
			} else AndroidAdMobController.instance.ShowInterstitialAd ();
		}
		*/

		back = GameObject.Find("back forest");
		if (back == null) back = GameObject.Find("back rock");
		if (back == null) back = GameObject.Find("back ice");
		if (back == null) back = GameObject.Find("back desert");
		Debug.Log (back);
	}
	
	// Update is called once per frame
	void Update () {
		//acceleration start
		/* off for tests
		if (Time.time - t > 0.02F) {
			t = Time.time;
			dir.y = Input.acceleration.y;
			dir.x = Input.acceleration.x;
			back.GetComponent<Rigidbody2D>().AddForce((-dir - back.transform.localPosition / 100) * 5);

			back.GetComponent<Rigidbody2D>().drag = (1 - (-new Vector2(dir.x, dir.y) - 
			                             new Vector2(back.transform.localPosition.x, back.transform.localPosition.y)  / 100).magnitude) * 10;
		}
		*/
		//acceleration end

		if (transform.position.x < -4 || transform.position.x > 4 || transform.position.y < -6 || transform.position.y > 6) 
			if (!spiderAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider sad") &&
			    !spiderAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider sad 0"))
				StartCoroutine(gSpiderClass.coroutineCry(spiderAnimator));

		//timer
		if (initLevelMenuClass.levelDemands == 1) {
			int levels = staticClass.levels[Convert.ToInt32(Application.loadedLevelName.Substring(5)), 1];
			if (levels >= 1 && levels <=99) 
			if (Mathf.Ceil(Time.timeSinceLevelLoad) > staticClass.timer) {
				staticClass.timer = Convert.ToInt32(Mathf.Ceil(Time.timeSinceLevelLoad));
				if(levels - staticClass.timer <= 0)	guiTimer.text = "00";
				else if (levels - staticClass.timer < 10) guiTimer.text = "0" + (levels - staticClass.timer).ToString();
				else guiTimer.text = (levels - staticClass.timer).ToString();
			}
		}

		if (Input.GetKey(KeyCode.Escape) && !completeMenu.activeSelf) {
			pauseMenu.SetActive(true);
			Time.timeScale = 0;

		}

		if (Time.frameCount % 100 == 0) {
			//Debug.Log(Time.deltaTime);
			//GC.Collect();
		}
		//столкновение berry и spider
		//if (Input.GetKey(KeyCode.Escape) && !completeMenu.activeSelf) {
			//GetComponent<Collider2D>().
	}

	void OnCollisionEnter2D (Collision2D collisionObject) {
		if (collisionObject.gameObject.name == "spider") {
			//cut ropes
			GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
			foreach (var web in webs) {
				if (web.GetComponent<gWebClass>().webStateCollisionBerry) web.SendMessage("OnClick");
			}



			//tutorial
			gHandClass.delHand();
			berryState = "start finish";

			if (spiderAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath"))
				spiderAnimator.Play("spider open month legs");
			else 
				spiderAnimator.Play("spider open month");

			//GetComponent<Animation>().Play();
			//transform.position = collisionObject.gameObject.transform.position;
			if (initClass.progress.Count == 0) initClass.getProgress();
			GetComponent<Rigidbody2D>().isKinematic = true;
			GetComponent<Collider2D>().enabled = false;
			collisionObject.rigidbody.isKinematic = true;
			StartCoroutine(coroutineEat(collisionObject));
		}

	}

	void OnTriggerEnter2D(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "star") {
			StartCoroutine(collisionObject.gameObject.GetComponent<gStarClass>().destroyStar());
			//Destroy(collisionObject.gameObject);
			guiStars[starsCounter].GetComponent<UISprite>().color =  new Color(1, 1, 1, 1);
			starsCounter ++;
		}

	}
	public IEnumerator coroutineEat(Collision2D collisionObject){
		collisionObject.rigidbody.isKinematic = false;
		for (float i = 0; i < 5; i+=0.5F) {
			transform.GetChild(0).GetComponent<AnimatedAlpha>().alpha = 0.8F - i * 0.2F;
			transform.localScale = new Vector2(1 - i * 0.05F, 1 - i * 0.05F);
			transform.position = transform.position + (collisionObject.transform.position - transform.position) * 0.2F;
			yield return new WaitForSeconds(0.015F);
		}

		if (spiderAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath"))
			spiderAnimator.CrossFade ("spider eat legs", 0.5F);
		else 
			spiderAnimator.CrossFade ("spider eat", 0.5F);
		StartCoroutine(Coroutine(collisionObject));

	}

	public IEnumerator Coroutine(Collision2D collisionObject){
		// остановка выполнения функции на costEnergy секунд
		yield return new WaitForSeconds(1.5F);
		collisionObject.transform.GetChild(0).GetComponent<Animator>().Play("spider idle", 1);
		completeMenu.SetActive(true);
		completeMenu.GetComponent<lsLevelMenuClass> ().completeMenuEnable (400, 200, true, 3);

		berryState = "finish";

		int lvlNumber = Convert.ToInt32(Application.loadedLevelName.Substring(5));

		//initClass.progress["stars"] = 3;
		if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			GooglePlayManager.instance.SubmitScore ("leaderboard_forest", initClass.progress["stars"]);
			if (Application.loadedLevelName == "level1") GooglePlayManager.instance.UnlockAchievement("achievement_complete_first_level");

			if (lvlNumber > initClass.progress["lastLevel"]) {
				GooglePlayManager.instance.IncrementAchievement("achievement_complete_5_levels", 1);
				GooglePlayManager.instance.IncrementAchievement("achievement_complete_the_game", 1);

			}


		}

		//gems
		if (initLevelMenuClass.levelDemands == 0) {
			//for (int i = 0; i < starsCounter ; i++) {
				//guiStarsComplete[i].color = new Color32(255, 255, 255, 255);
			//}
			if (starsCounter == 3 && initClass.progress[Application.loadedLevelName] != 1 && initClass.progress[Application.loadedLevelName] != 3) {
				initClass.progress["gems"] ++;
				if (initClass.progress[Application.loadedLevelName] == 0) initClass.progress[Application.loadedLevelName] = 1;
				else initClass.progress[Application.loadedLevelName] = 3;
			}
	}
		//initClass.progress["stars"] = initClass.progress["stars"] + starsCounter - initClass.progress["level" + lvlNumber];

		if (initLevelMenuClass.levelDemands == 1 && initClass.progress[Application.loadedLevelName] < 2 && staticClass.levels[lvlNumber, 0] == starsCounter) {
			int levels = staticClass.levels[lvlNumber, 1];
			bool flag = false;
			if (levels == 0) flag = true;
			else if (levels >= 1 && levels <=99 && staticClass.timer <= levels) flag = true;
			else if (levels >= 100 && levels <=199 && staticClass.useWeb == levels) flag = true;
			else if (levels == 201 && staticClass.useSluggish == false) flag = true;
			else if (levels == 202 && staticClass.useDestroyer == false) flag = true;
			else if (levels == 203 && staticClass.useYeti == false) flag = true;
			else if (levels == 204 && staticClass.useGroot == false) flag = true;
			if (flag) {
				initClass.progress["gems"] ++;
				if (initClass.progress[Application.loadedLevelName] == 0) initClass.progress[Application.loadedLevelName] = 2;
				else initClass.progress[Application.loadedLevelName] = 3;
			}
		}

		if (initClass.progress["level" + lvlNumber] < starsCounter) {
			if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) GooglePlayManager.instance.IncrementAchievement("achievement_collect_all_stars", starsCounter - initClass.progress["level" + lvlNumber]);
			initClass.progress["level" + lvlNumber] = starsCounter;
		}
		
		if (lvlNumber >= initClass.progress["lastLevel"]) initClass.progress["lastLevel"] = lvlNumber;
		
		initClass.saveProgress();

	}


}
