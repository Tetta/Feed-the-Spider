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
		levels[1, 0] = 3;
		levels[1, 1] = 201;
		levels[2, 0] = 2;
		levels[2, 1] = 100;
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
	public static string currentSkin = "skin1";

	public static string strProgressDefault = "googlePlay=0;lastLevel=76;currentLevel=2;coins=1000;gems=0;energyTime=0;energy=100;" +
		"hints=3;webs=3;collectors=3;teleports=3;complect=0;music=0;sound=1;dailyBonus=0;" +
			"skin1=2;skin2=0;skin3=0;skin4=0;skin5=0;" +
			"level1=0;level2=0;level3=0;level4=0;level5=0;level6=0;level7=0;level8=0;level9=0;level10=0;" +
			"level11=0;level12=0;level13=0;level14=0;level15=0;level16=0;level17=0;level18=0;level19=0;level20=0;" +
			"level21=0;level22=0;level23=0;level24=0;level25=0;level26=0;level50=0;level51=0;level75=0;level76=0;" +
			"score1_1=0;score1_2=0;score2_1=0;score2_2=0;" +
			"gift1=0;gift2=0;";

	public static IEnumerator waitForRealTime(float delay){
		while(true){
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			while (Time.realtimeSinceStartup < pauseEndTime){
				yield return 0;
			}
			break;
		}
	}
}

