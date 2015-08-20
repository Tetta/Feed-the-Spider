using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class iClickClass : MonoBehaviour {
	public static GameObject backTransition = null;

	public string functionStart = "";
	public string functionClick = "";
	public string functionPress = "";
	public string functionDragStart = "";
	public string functionDrag = "";
	public static GameObject currentButton = null;
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	private float timeSinceTouch = 0;
	private bool enableDrag = false;
	//public string functionDestroy = "";

	// Use this for initialization
	void Start () {
		if (functionStart != "") SendMessage(functionStart);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick() {
		if (functionClick != "") SendMessage(functionClick);
	}

	void OnPress(bool isPressed) {
		if (functionPress != "") SendMessage(functionPress, isPressed);
	}

	void OnDragStart () {
		if (functionDragStart != "") SendMessage(functionDragStart);
	}
	void OnDrag () {
		if (functionDrag != "") SendMessage(functionDrag);
	}


	void pressMarketItem(bool isPressed) {
		if (!isPressed) {
			GetComponent<Animator> ().Play ("button");
			Debug.Log (GetComponent<Animator> ());
			Debug.Log (44);
		}
		//off for tests
		//if (onFinished != null) EventDelegate.Execute(onFinished);

		//old code
		//transform.parent.GetComponent<UIScrollView>().Press(isPressed);
		//if (!isPressed) {
		//	GetComponent<UIPlayAnimation>().enabled = true;
		//	GetComponent<UIButtonScale>().enabled = true;
		//	enableDrag = false;
		//}
		//timeSinceTouch = Time.time;
	}
	
	void dragStartMarketItem() {
		if (Time.time - timeSinceTouch < 0.5F) {
			GetComponent<UIButtonScale>().enabled = false;
			GetComponent<UIPlayAnimation>().enabled = false;
			enableDrag = true;
		}
	}

	void dragMarketItem() {
		if (enableDrag) transform.parent.GetComponent<UIScrollView>().Drag();
	}

	void OnDestroy() {
	}



	public void backTransitionOpen ( ) {
		//animation.Play("back transition open");
	}

	public void backTransitionExit ( ) {
		//GetComponent<Animation>().Play("back transition exit");
		GetComponent<Animator>().Play("back transition exit unscaled");
		if (ActiveAnimation.current != null) {
			currentButton = ActiveAnimation.current.gameObject;
			//if (name == "market") marketMenu.SetActive(true);
		}
	}

	public void backTransitionExitFinished () {
		Debug.Log("currentButton: " + currentButton);
		if (currentButton != null) {
			if (currentButton.name == "button market") {
				marketClass.instance.gameObject.SetActive(true);
				//for new market
				//marketClass.instance.marketMainMenu.SetActive(true);
				marketClass.instance.coinsMenu.SetActive(true);
				GetComponent<Animation>().Play("back transition open");
			} else if  (currentButton.name == "button settings") {
				GameObject.Find("settings folder").transform.GetChild(0).gameObject.SetActive(true);
				GetComponent<Animation>().Play("back transition open");
			} else if (currentButton.name == "button settings back") {
				GameObject.Find("settings").SetActive(false);
				GetComponent<Animation>().Play("back transition open");
			} else if (currentButton.name == "button market back") {
				if (marketClass.instance.coinsMenu.activeSelf) {
					marketClass.instance.coinsMenu.SetActive(false);
					marketClass.instance.marketMainMenu.SetActive(true);

				} else if (marketClass.instance.hintsMenu.activeSelf) {
					marketClass.instance.hintsMenu.SetActive(false);
					marketClass.instance.marketMainMenu.SetActive(true);
					
				} else if (marketClass.instance.customizationMenu.activeSelf) {
					marketClass.instance.customizationMenu.SetActive(false);
					marketClass.instance.marketMainMenu.SetActive(true);
					
				} else marketClass.instance.gameObject.SetActive(false);
				GetComponent<Animation>().Play("back transition open");
			} else if (currentButton.name == "special" || currentButton.name == "coins" || currentButton.name == "hints" || currentButton.name == "customization") {
				marketClass.instance.marketMainMenu.SetActive(false);
				marketClass.instance.specialMenu.SetActive(false);
				marketClass.instance.hintsMenu.SetActive(false);
				marketClass.instance.notCoinsMenu.SetActive(false);
				marketClass.instance.coinsMenu.SetActive(false);
				marketClass.instance.customizationMenu.SetActive(false);
				GetComponent<Animation>().Play("back transition open");
				if (currentButton.name == "special") {
					marketClass.instance.specialMenu.SetActive(true);
					//отмечаем, если комплект куплен
					if (initClass.progress["complect"] == 1) {
						//убираем label price и label currency [0] и [2]
						marketClass.instance.specialMenu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
						marketClass.instance.specialMenu.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
						//добавляем accept [3]
						marketClass.instance.specialMenu.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(true);
					}
					ActiveAnimation.Play(marketClass.instance.specialMenu.transform.GetChild(0).GetChild(0).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
					ActiveAnimation.Play(marketClass.instance.specialMenu.transform.GetChild(0).GetChild(1).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
				} else if (currentButton.name == "coins") {
					marketClass.instance.gameObject.SetActive(true);
					Debug.Log(marketClass.instance.enabled);
					Destroy( GameObject.Find("root/Camera/UI Root/not coins menu"));
					marketClass.instance.coinsMenu.SetActive(true);
					ActiveAnimation.Play(marketClass.instance.coinsMenu.transform.GetChild(0).GetChild(0).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
					ActiveAnimation.Play(marketClass.instance.coinsMenu.transform.GetChild(0).GetChild(1).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
				} else if (currentButton.name == "hints") {
					Debug.Log(555);
					marketClass.instance.gameObject.SetActive(true);
					marketClass.instance.hintsMenu.SetActive(true);
					ActiveAnimation.Play(marketClass.instance.hintsMenu.transform.GetChild(0).GetChild(0).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
					ActiveAnimation.Play(marketClass.instance.hintsMenu.transform.GetChild(0).GetChild(1).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
				} else if (currentButton.name == "customization") {
					marketClass.instance.customizationMenu.SetActive(true);
					for (int j = 1; j <= 2; j++) {
						//отмечаем купленные скины
						if (initClass.progress["skin" + j] > 0) {
							//убираем label price и icon price [0] и [2]
							marketClass.instance.customizationMenu.transform.GetChild(0).GetChild(j - 1).GetChild(0).gameObject.SetActive(false);
							marketClass.instance.customizationMenu.transform.GetChild(0).GetChild(j - 1).GetChild(2).gameObject.SetActive(false);
							//добавляем accept [3]
							marketClass.instance.customizationMenu.transform.GetChild(0).GetChild(j - 1).GetChild(3).gameObject.SetActive(true);
							//красим accept в белый
							if (initClass.progress["skin" + j] == 2) marketClass.instance.customizationMenu.transform.GetChild(0).GetChild(j - 1).GetChild(3).GetComponent<UISprite>().color = new Color32(255, 255, 255, 255);


						}
					}
					ActiveAnimation.Play(marketClass.instance.customizationMenu.transform.GetChild(0).GetChild(0).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
					ActiveAnimation.Play(marketClass.instance.customizationMenu.transform.GetChild(0).GetChild(1).GetComponent<Animation>(), AnimationOrTween.Direction.Forward);
				}
			}
			currentButton = null;
		}
	}

	void initSettings () {
		if (name == "music button") {
			if (initClass.progress["music"] == 1) transform.GetChild(0).gameObject.SetActive(false);
			else transform.GetChild(0).gameObject.SetActive(true);
		}
		if (name == "soung button") {
			if (initClass.progress["sound"] == 1) transform.GetChild(0).gameObject.SetActive(false);
			else transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	void clickSound () {
		if (initClass.progress["sound"] == 0) {
			initClass.setSound(true);
			initClass.progress["sound"] = 1;
			initClass.saveProgress();
			transform.GetChild(0).gameObject.SetActive(false);
		} else {
			initClass.setSound(false);
			initClass.progress["sound"] = 0;
			initClass.saveProgress();
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	void clickMusic () {
		if (initClass.progress["music"] == 0) {
			GameObject.Find("music").GetComponent<AudioSource>().enabled = true;
			initClass.progress["music"] = 1;
			initClass.saveProgress();
			transform.GetChild(0).gameObject.SetActive(false);
		} else {
			GameObject.Find("music").GetComponent<AudioSource>().enabled = false;
			initClass.progress["music"] = 0;
			initClass.saveProgress();
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}



	void showAchievements () {
		//NGUIDebug.Log("mGooglePlayClass OnClick");
		GooglePlayManager.instance.ShowAchievementsUI();
	}

	void showLeaderboards () {
		GooglePlayManager.instance.ShowLeaderBoardsUI();
	}

	void connectGooglePlay () {
		try {
			if (initClass.progress["googlePlay"] == 0) {
				GooglePlayConnection.instance.connect ();
			} else {
				GooglePlayConnection.instance.disconnect ();
			}

		} catch (System.Exception ex) {
			Debug.Log(ex.StackTrace);
		}
	}

	void resetProgress () {

		//сброс прогресса
		PlayerPrefs.SetString("progress", staticClass.strProgressDefault);
		initClass.getProgress();
		GooglePlayManager.instance.ResetAllAchievements();
		GooglePlayManager.instance.SubmitScore("leaderboard_test_leaderboard", 0);
	}

	void loadLevel () {
		StartCoroutine(CoroutineLoadLevel());
	}


	void openMenu () {
		Debug.Log ("openMenu: " + name);
		GameObject menu = null;
		if (name == "button market") {
			//marketMenu.SetActive(true);
		}else if (name == "next level menu") {
			menu = GameObject.Find("level menu");
			menu.SetActive(false);
			menu = transform.parent.parent.GetChild(3).gameObject;
			menu.SetActive(true);
		} else if (name == "prev level menu") {
			menu = GameObject.Find("level menu 2");
			menu.SetActive(false);
			menu = transform.parent.parent.GetChild(2).gameObject;
			menu.SetActive(true);
		} else if (name == "pause") {
			menu = transform.parent.GetChild(1).gameObject;
			menu.SetActive(true);
			Time.timeScale = 0;
		} else if (name == "play") {
			menu = GameObject.Find("pause menu");
			menu.SetActive(false);
			Time.timeScale = 1;
			
		} else if (name == "exit energy menu") GameObject.Find("energyLabel").SendMessage("stopCoroutineEnergyMenu");
		
		
	}
	public void closeMenu () {
		StartCoroutine(coroutineCloseMenu ());
	}

	public IEnumerator coroutineCloseMenu () {
		yield return new WaitForSeconds(0F);
		Debug.Log(name);
		GameObject menu = null;
		if (name == "exit level menu") {
			menu = GameObject.Find("level menu");
			menu.transform.GetChild(0).GetComponent<Animation>().Play("menu exit");
			yield return new WaitForSeconds(0.2F);
			menu.SetActive(false);
		}else if (name == "next level menu") {
			menu = GameObject.Find("level menu");
			menu.SetActive(false);
			menu = transform.parent.parent.GetChild(3).gameObject;
			menu.SetActive(true);
		} else if (name == "prev level menu") {
			menu = GameObject.Find("level menu 2");
			menu.SetActive(false);
			menu = transform.parent.parent.GetChild(2).gameObject;
			menu.SetActive(true);
		} else if (name == "pause") {
			menu = transform.parent.GetChild(1).gameObject;
			menu.SetActive(true);
			Time.timeScale = 0;
		} else if (name == "play") {
			menu = GameObject.Find("pause menu");
			yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
			menu.SetActive(false);
			if (gYetiClass.yetiState == "") Time.timeScale = 1;
			
		} else if (name == "exit energy menu") {
			GameObject.Find("energy menu").transform.GetChild(0).GetComponent<Animation>().Play("menu exit");
			yield return new WaitForSeconds(0.2F);
			GameObject.Find("energy").SendMessage("stopCoroutineEnergyMenu");
		
		} else if (name == "exit not coins menu") {
			menu = GameObject.Find("root/Camera/UI Root/not coins menu");
			if (menu != null) {
				menu.transform.GetChild(0).GetComponent<Animation>().Play("menu exit");
				yield return new WaitForSeconds(0.2F);
				Destroy(menu);
			} else {
				menu = marketClass.instance.notCoinsMenu;
				menu.transform.GetChild(0).GetComponent<Animation>().Play("menu exit");
				yield return new WaitForSeconds(0.2F);
				menu.SetActive(false);
			}
		} else if (name == "exit thanks menu") {
			menu = GameObject.Find("root/Camera/UI Root/thanks menu");
			if (menu != null) {
				menu.transform.GetChild(0).GetComponent<Animation>().Play("menu exit");
				yield return new WaitForSeconds(0.2F);
				Destroy(menu);
			} else {
				menu = marketClass.instance.thanksMenu;
				menu.transform.GetChild(0).GetComponent<Animation>().Play("menu exit");
				yield return new WaitForSeconds(0.2F);
				menu.SetActive(false);
			}

		}	

	}

	//public IEnumerator CoroutineCloseMenu(){
	
	//}

	void selectLanguage() {
		Localization.language = name;
	}

	public IEnumerator CoroutineLoadLevel(){
		Time.timeScale = 1;

		yield return new WaitForSeconds(0.1F);
		GameObject.Find("back transition").GetComponent<Animation>().Play("back transition exit");
		yield return new WaitForSeconds(0.1F);
		if (name == "start level menu") Application.LoadLevel("level menu");
		else if (name == "button back") Application.LoadLevel("menu");
		if (name == "restart") Application.LoadLevel(Application.loadedLevel);
		else if (name == "next") {
			Application.LoadLevel("level menu");
		} else if (name == "play 0") {
			initLevelMenuClass.levelDemands = 0;
			Application.LoadLevel("level" + initClass.progress["currentLevel"]);
		} else if (name == "play 1") {
			initLevelMenuClass.levelDemands = 1;
			Application.LoadLevel("level" + initClass.progress["currentLevel"]);
		} else if (name.Substring(0, 5) == "level") {
			if (initClass.progress["lastLevel"] >= Convert.ToInt32(name.Substring(5)) - 1) Application.LoadLevel("level" + Convert.ToInt32(name.Substring(5)));
		}

	}

	void buyEnergy () {
		marketClass.instance.item = gameObject;
		marketClass.instance.purchaseForCoins();
	}

	void clickBonusesArrow () {
		if (name == "arrow right") {
			gameObject.SetActive(false);
			GameObject.Find("bonuses/tween").transform.GetChild(1).gameObject.SetActive(true);
		} else {
			gameObject.SetActive(false);
			GameObject.Find("bonuses/tween").transform.GetChild(0).gameObject.SetActive(true);
		}

	}



}
