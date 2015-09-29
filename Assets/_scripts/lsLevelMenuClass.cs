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

    //public void completeMenuEnable(int bonusTime, int bonusLevel, bool gem, int starsCount) {
    public void completeMenuEnable (float timeLevel, bool gem, int starsCount, int lvlNumber) {
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
		StartCoroutine(coroutineCompleteMenuScore(timeLevel, gem, starsCount, lvlNumber));

		
		

	}
	IEnumerator coroutineCompleteMenuScore(float timeLevel, bool gem, int starsCount, int lvlNumber) {
        Debug.Log("initLevelMenuClass.levelDemands: " + initLevelMenuClass.levelDemands);
        if (initLevelMenuClass.levelDemands == 1) transform.GetChild(1).gameObject.SetActive(false);
        //init gems
        int levelProgress = initClass.progress[Application.loadedLevelName];
		if (levelProgress == 1 || levelProgress == 3 && !(gem && initLevelMenuClass.levelDemands == 0)) gem1Active.SetActive(true);
		if (levelProgress == 2 || levelProgress == 3 && !(gem && initLevelMenuClass.levelDemands == 1)) gem2Active.SetActive(true);
		Transform grid = levelMenu.transform.GetChild (0).GetChild (4).GetChild (0);
		UILabel scoreLvl = grid.GetChild (1).GetChild (0).GetComponent<UILabel>();
		UILabel scoreCh = grid.GetChild (3).GetChild (0).GetComponent<UILabel>();
        int score1 = 0;
        int score2 = 0;
        int scoreFinal = 0;
        if (initLevelMenuClass.levelDemands == 0) score1 = Mathf.RoundToInt(1000 * starsCount + 3000 - 100 * timeLevel);
        else score1 = initClass.progress["score" + lvlNumber + "_1"];
        if (levelProgress == 2 || levelProgress == 3) score2 = 2000;
        Transform scoreAll = levelMenu.transform.GetChild(0).GetChild(4).GetChild(1);
        if (score1 + score2 > initClass.progress["score" + lvlNumber + "_1"] + initClass.progress["score" + lvlNumber + "_2"]) {
            scoreFinal = score1 + score2;
            //включаем +100 coins
            scoreAll.GetChild(2).gameObject.SetActive(true);
            scoreAll.GetChild(2).GetChild(1).GetComponent<UILabel>().text = Mathf.RoundToInt((scoreFinal - (initClass.progress["score" + lvlNumber + "_1"] + initClass.progress["score" + lvlNumber + "_2"]))/100).ToString();
            initClass.progress["coins"] = Mathf.RoundToInt((scoreFinal - (initClass.progress["score" + lvlNumber + "_1"] + initClass.progress["score" + lvlNumber + "_2"]))/100);
        }
        else scoreFinal = initClass.progress["score" + lvlNumber + "_1"] + initClass.progress["score" + lvlNumber + "_2"];
        for (int i = 0; i <= 100; i += 5) {
			scoreLvl.text = (Mathf.Round(score1 * i / 100)).ToString();
            scoreCh.text = (Mathf.Round(score2 * i / 100)).ToString();
			yield return new WaitForSeconds (0.05F);
			if (starsCount >= 1 && i == 50) transform.GetChild (1).GetChild(0).GetChild (0).gameObject.SetActive (true);
		}
		if (starsCount >= 2) transform.GetChild (1).GetChild(1).GetChild (0).gameObject.SetActive (true);
		yield return new WaitForSeconds (0.2F);
		grid.gameObject.SetActive (false);
		scoreAll.gameObject.SetActive (true);
		UILabel scoreAllLbl = scoreAll.GetChild (0).GetComponent<UILabel>();
		for (int i = 0; i <= 100; i += 5) {
			scoreAllLbl.text = (Mathf.Round(scoreFinal * i / 100)).ToString();
			yield return new WaitForSeconds (0.05F);
			if (starsCount >= 3 && i == 30) transform.GetChild (1).GetChild(2).GetChild (0).gameObject.SetActive (true);
		}
        //если еще не получал кристалл
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
        //сохранение очков в progress
        //if (initClass.progress["score" + lvlNumber + "_1"] <  (3000 - 100 * timeLevel + 1000 * starsCount)) initClass.progress["score" + lvlNumber + "_1"] = Mathf.RoundToInt(3000 - 100 * timeLevel + 1000 * starsCount);
        //if (initLevelMenuClass.levelDemands == 1 && gem && initClass.progress["score" + lvlNumber + "_2"] == 0) initClass.progress["score" + lvlNumber + "_2"] = 2000;
        if (initClass.progress["score" + lvlNumber + "_1"] < score1) initClass.progress["score" + lvlNumber + "_1"] = score1;
        if (initClass.progress["score" + lvlNumber + "_2"] < score2) initClass.progress["score" + lvlNumber + "_2"] = score2;
        
        initClass.saveProgress();
    }

    // Update is called once per frame
    void Update () {
	}
}
