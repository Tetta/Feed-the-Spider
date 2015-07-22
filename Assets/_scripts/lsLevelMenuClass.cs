using UnityEngine;
using System.Collections;

public class lsLevelMenuClass: MonoBehaviour {
	public GameObject levelMenu;
	public UILabel titleNumberLevel;
	public GameObject stars1;
	public GameObject stars2;
	public GameObject time;
	public GameObject web;
	public GameObject sluggish;
	public GameObject destroyer;
	public GameObject yeti;
	public GameObject groot;
	public GameObject gem1Active;
	public GameObject gem2Active;


	void setDefault () {
		// to default
		levelMenu.transform.GetChild(0).transform.GetChild(0).GetComponent<UIToggle>().value = true;
		time.SetActive(false);
		web.SetActive(false);
		sluggish.SetActive(false);
		destroyer.SetActive(false);
		yeti.SetActive(false);
		groot.SetActive(false);
		for (int i = 1; i <= 3; i++) {
			stars2.transform.GetChild(i - 1).GetComponent<UISprite>().color = new Color(0, 0, 0, 1);
		}
		gem1Active.SetActive(false);
		gem2Active.SetActive(false);
	}

	void setContent2 () {
		//init content2.stars
		int levelDemandsStars = staticClass.levels[initClass.progress["currentLevel"], 0];
		for (int i = 1; i <= levelDemandsStars; i++) {
			stars2.transform.GetChild(i - 1).GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
		}
		stars2.transform.GetChild(3).GetComponent<UILabel>().text = levelDemandsStars.ToString();
		
		int levelDemands = staticClass.levels[initClass.progress["currentLevel"], 1];
		//если условие только одно (про звезды)
		if (levelDemands == 0) {
			stars2.transform.localPosition = new Vector3(stars2.transform.localPosition.x, -30, stars2.transform.localPosition.z);
			//остальные условия
		} else if (levelDemands >= 1 && levelDemands <=99){
			time.SetActive(true);
			if (levelDemands < 10) time.transform.GetChild(0).GetComponent<UILabel>().text = "0" + levelDemands.ToString();
			else time.transform.GetChild(0).GetComponent<UILabel>().text = levelDemands.ToString();
		}	else if (levelDemands >= 100 && levelDemands <=199){
			web.SetActive(true);
			web.transform.GetChild(0).GetComponent<UILabel>().text = (levelDemands - 100).ToString();
			web.transform.GetChild(1).GetComponent<UILabel>().text = (levelDemands - 100).ToString();
			
		}	else if (levelDemands == 201){
			sluggish.SetActive(true);
		}	else if (levelDemands == 202){
			destroyer.SetActive(true);
		}	else if (levelDemands == 203){
			yeti.SetActive(true);
		}	else if (levelDemands == 204){
			groot.SetActive(true);
		}
	}

	// Use this for initialization
	public void levelMenuEnable () {

		setDefault ();

		//init gems
		int levelProgress = initClass.progress["level" + initClass.progress["currentLevel"]];
		if (levelProgress == 1 || levelProgress == 3) gem1Active.SetActive(true);
		if (levelProgress == 2 || levelProgress == 3) gem2Active.SetActive(true);
		
		titleNumberLevel.text = initClass.progress["currentLevel"].ToString();
		setContent2 ();
	}
	
	public void completeMenuEnable (int bonusTime, int bonusLevel, bool gem, int starsCount) {
		setDefault ();
		stars1.SetActive(false);
		stars2.SetActive(false);
		Transform score = levelMenu.transform.GetChild (0).GetChild (4);
		score.gameObject.SetActive (true);
		if (initLevelMenuClass.levelDemands == 1) {
			levelMenu.transform.GetChild(0).transform.GetChild(0).GetComponent<UIToggle>().value = false;
			levelMenu.transform.GetChild (0).GetChild (1).GetComponent<UIToggle> ().value = true;
		}
		Debug.Log ("completeMenuEnable");
		StartCoroutine(coroutineCompleteMenuScore(gem, starsCount));

		
		

	}
	IEnumerator coroutineCompleteMenuScore(bool gem, int starsCount) {
		//init gems
		int levelProgress = initClass.progress["level" + initClass.progress["currentLevel"]];
		if (levelProgress == 1 || levelProgress == 3) gem1Active.SetActive(true);
		if (levelProgress == 2 || levelProgress == 3) gem2Active.SetActive(true);
		Transform grid = levelMenu.transform.GetChild (0).GetChild (4).GetChild (0);
		UILabel scoreTime = grid.GetChild (0).GetChild (0).GetComponent<UILabel>();
		UILabel scoreLvl = grid.GetChild (1).GetChild (0).GetComponent<UILabel>();
		UILabel scoreGem1 = grid.GetChild (2).GetChild (0).GetComponent<UILabel>();
		UILabel scoreGem2 = grid.GetChild (3).GetChild (0).GetComponent<UILabel>();
		for (int i = 0; i <= 100; i += 5) {
			scoreTime.text = (Mathf.Round(400 * i / 100)).ToString();
			scoreLvl.text = (Mathf.Round(300 * i / 100)).ToString();
			scoreGem1.text = (Mathf.Round(200 * i / 100)).ToString();
			scoreGem2.text = (Mathf.Round(100 * i / 100)).ToString();
			yield return new WaitForSeconds (0.05F);
			if (starsCount >= 1 && i == 50) transform.GetChild (2).GetChild (0).gameObject.SetActive (true);
		}
		if (starsCount >= 2) transform.GetChild (3).GetChild (0).gameObject.SetActive (true);
		yield return new WaitForSeconds (0.2F);
		grid.gameObject.SetActive (false);
		Transform scoreAll = levelMenu.transform.GetChild (0).GetChild (4).GetChild (1);
		scoreAll.gameObject.SetActive (true);
		UILabel scoreAllLbl = scoreAll.GetChild (0).GetComponent<UILabel>();
		for (int i = 0; i <= 100; i += 5) {
			scoreAllLbl.text = (Mathf.Round(400 * i / 100)).ToString();
			yield return new WaitForSeconds (0.05F);
			if (starsCount >= 3 && i == 30) transform.GetChild (4).GetChild (0).gameObject.SetActive (true);
		}
		if (initLevelMenuClass.levelDemands == 0 && gem) {
			gem1Active.SetActive (true);
			gem1Active.GetComponent<Animation> ().Play ();
		}
		if (initLevelMenuClass.levelDemands == 1 && gem) {
			gem2Active.SetActive (true);
			gem2Active.GetComponent<Animation> ().Play ();
		}
		yield return new WaitForSeconds (1F);
		grid.parent.gameObject.SetActive (false);


		stars1.SetActive(true);
		stars2.SetActive(true);
		setContent2 ();
	}
		
	// Update is called once per frame
	void Update () {
	}
}
