using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

	/*
		stars = 0 - 3;
		--------------
		none = 0;
		time = 1 - 99;
		web = 100 - 199;
		sluggish = 201;
		destroyer = 202;
		yeti = 203;
		groot = 204;
	*/
public class staticClass {

	//public static staticClass instance = new staticClass();

	//public static List<level> levels = new List<level>();
	public static int[,] levels = new int[101, 2];
	public static int[] levelBlocks = new int[101];

	void Update () {
	}

	public static void initLevels () {
		//всегда есть stars [0] + что-то еще [1]
		levels[1, 0] = 1;
		levels[1, 1] = 10;
		levels[2, 0] = 1;
		levels[2, 1] = 10;
        levels[3, 0] = 1;
        levels[3, 1] = 10;
        levels[4, 0] = 1;
        levels[4, 1] = 10;
        levels[5, 0] = 1;
        levels[5, 1] = 10;
        levels[6, 0] = 1;
        levels[6, 1] = 10;
        levels[7, 0] = 1;
        levels[7, 1] = 10;
        levels[8, 0] = 1;
        levels[8, 1] = 10;
        levels[9, 0] = 1;
        levels[9, 1] = 10;
        levels[10, 0] = 1;
        levels[10, 1] = 10;
        levels[11, 0] = 1;
        levels[11, 1] = 10;
        levels[12, 0] = 1;
        levels[12, 1] = 10;
        levels[13, 0] = 1;
        levels[13, 1] = 10;
        levels[14, 0] = 1;
        levels[14, 1] = 10;
        levels[15, 0] = 1;
        levels[15, 1] = 10;
        levels[16, 0] = 1;
        levels[16, 1] = 10;
        levels[17, 0] = 1;
        levels[17, 1] = 10;
        levels[18, 0] = 1;
        levels[18, 1] = 10;
        levels[19, 0] = 1;
        levels[19, 1] = 10;
        levels[20, 0] = 1;
        levels[20, 1] = 10;
        levels[21, 0] = 1;
        levels[21, 1] = 10;
        levels[22, 0] = 1;
        levels[22, 1] = 10;
        levels[23, 0] = 1;
        levels[23, 1] = 10;
        levels[24, 0] = 1;
        levels[24, 1] = 10;
        levels[25, 0] = 1;
        levels[25, 1] = 10;
        //----------------------------
        levels[26, 0] = 2;
        levels[26, 1] = 100;
        levels[51, 0] = 3;
        levels[51, 1] = 300;
        levels[76, 0] = 1;
        levels[76, 1] = 200;
        levelBlocks[5] = 3;

	}

	public static int useWeb = 0;
	public static int timer = 0;
	public static bool useSluggish = false;
	public static bool useDestroyer = false;
	public static bool useYeti = false;
	public static bool useGroot = false;
	public static int showAd = 0;
	public static int showAdColony = 0;
	public static bool loadAd = false;

	public static int testCounter = 0;

	
	//level1 = 0-3: 0-3 звезд
	//level1 = 4: пройдено испытание
	public static string currentSkin = "skin2";
	public static string currentHat = "hat4";
	public static string currentBerry = "berry1";

	public static string strProgressDefault = "googlePlay=0;lastLevel=76;currentLevel=2;coins=1000;gems=0;energyTime=0;energy=100;" +

        "boosters=3;hints=3;webs=3;collectors=3;teleports=3;complect=0;music=0;sound=1;dailyBonus=0;" +

		"hints=3;webs=3;collectors=3;teleports=3;complect=0;music=0;sound=1;dailyBonus=0;" +

			"berry1=2;berry2=1;berry3=1;berry4=1;berry5=1;" +
			"hat1=2;hat2=1;hat3=1;hat4=1;hat5=1;" +
			"skin1=2;skin2=1;skin3=1;skin4=1;skin5=1;" +
			"level1=0;level2=0;level3=0;level4=0;level5=0;level6=0;level7=0;level8=0;level9=0;level10=0;" +
			"level11=0;level12=0;level13=0;level14=0;level15=0;level16=0;level17=0;level18=0;level19=0;level20=0;" +
			"level21=0;level22=0;level23=0;level24=0;level25=0;level26=0;level50=0;level51=0;level75=0;level76=0;" +
			"level101=0;level102=0;level103=0;" +
            "score1_1=0;score1_2=0;score2_1=0;score2_2=0;score26_1=0;score26_2=0;" +
			"gift1=0;gift2=0;";

	public static Animator currentSkinAnimator;

	public static IEnumerator waitForRealTime(float delay){
		while(true){
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			while (Time.realtimeSinceStartup < pauseEndTime){
				yield return 0;
			}
			break;
		}
	}


	//включаем текущий скин и выключаем все остальные
	//public static void changeSkin(out Animator currentSkinAnimator){
	public static void changeSkin(){
		GameObject spider = GameObject.Find ("root/spider");
		//currentSkinAnimator = new Animator ();
		if (spider == null) return; 
		Transform spiderTr = spider.transform;
		for (int i = 0; i < 5; i++) { 
			if (spiderTr.GetChild (i).name == staticClass.currentSkin) {
				spiderTr.GetChild (i).gameObject.SetActive (true);
				currentSkinAnimator = spiderTr.GetChild (i).GetComponent<Animator> ();
			} else 
				spiderTr.GetChild (i).gameObject.SetActive (false);
		}
		//Debug.Log("Select skin: " + staticClass.currentSkin);
		changeHat ();
	}

	//включаем текущую шапку и выключаем все остальные
	public static void changeHat(){
		if (GameObject.Find ("root/spider") == null) return;
		Transform spider = GameObject.Find ("root/spider").transform;
		for (int i = 0; i < 5; i++) {
			if (spider.GetChild (i).name == staticClass.currentSkin) {
				for (int j = 0; j < 4; j++) {
					if (spider.GetChild (i).GetChild (0).GetChild (j).name == staticClass.currentHat) {
						spider.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (true);
					} else 
						spider.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (false);
				}
			} 
		}
		//Debug.Log("Select hat: " + staticClass.currentHat);

	}

	//включаем текущую ягоду и выключаем все остальные
	public static void changeBerry(){
		if (GameObject.Find ("root/berry") == null) return;
		Transform berry = GameObject.Find ("root/berry").transform;
		for (int i = 0; i < 5; i++) {
			if (berry.GetChild(i).name == staticClass.currentBerry) 
				berry.GetChild(i).gameObject.SetActive(true);
			else berry.GetChild(i).gameObject.SetActive(false);
		}
		
	}

}

