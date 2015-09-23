using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class lsGiftClass : MonoBehaviour {

	public GameObject giftMenu;
	public int giftLevel;
	// Use this for initialization
	void Start () {
		if (initClass.progress[name] == 1) {
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		if (initClass.progress [name] == 0 && giftLevel <= initClass.progress["lastLevel"]) {

			GetComponent<Animator> ().Play ("button");
			giftMenu.SetActive (true);
			clickGift ();
		}
	}

	public void clickGift() {
		GameObject bonusesTemp = GameObject.Instantiate (GameObject.Find("bonuses temp"), new Vector2(0, 0), Quaternion.identity)  as GameObject;
		bonusesTemp.transform.parent = giftMenu.transform.GetChild(0).GetChild(2);
		bonusesTemp.transform.localScale = new Vector3(1, 1, 1);
		Dictionary<string, int> portions = new Dictionary<string, int>();
		portions["hints_1"] = 30;		portions["hints_2"] = 15;		portions["hints_3"] = 10;
		portions["webs_1"] = 150;		portions["webs_2"] = 75;			portions["webs_3"] = 50;
		portions["teleports_1"] = 150;	portions["teleports_2"] = 75;	portions["teleports_3"] = 50;
		portions["collectors_1"] = 150;		portions["collectors_2"] = 75;		portions["collectors_3"] = 50;
		portions["coins_50"] = 150;		portions["coins_100"] = 75;		portions["coins_250"] = 30;

		//Vector2[] positions = {new Vector2[], 6.6, 7.7};
		Vector3[] positions = {new Vector3(-50, -70, 0), new Vector3(250, 220, 0), new Vector3(-290, 240, 0), new Vector3(300, -320, 0), new Vector3(-220, -370, 0)};
		for (int i = 0; i < 5; i++) {
			int counter = 0;
			int bonusRand = Mathf.CeilToInt( UnityEngine.Random.Range(0, 1135));

			foreach (var portion in portions ) {
				if (bonusRand >= counter && bonusRand < counter + portion.Value) {
					string bonusName = portion.Key.Split(new Char[] {'_'})[0];
					int bonusCount = int.Parse(portion.Key.Split(new Char[] {'_'})[1]);
					initClass.progress [bonusName] = initClass.progress[bonusName] + bonusCount;
					GameObject bonusIcon = GameObject.Instantiate (GameObject.Find("bonuses/" + bonusName), positions[i], Quaternion.identity)  as GameObject;
					bonusIcon.transform.parent = bonusesTemp.transform;
					bonusIcon.transform.localScale = new Vector3(1, 1, 1);
					bonusIcon.transform.localPosition = positions[i];
					GameObject bonusBackAmount = GameObject.Instantiate (GameObject.Find("bonuses/back amount"), positions[i], Quaternion.identity)  as GameObject;
					bonusBackAmount.transform.parent = bonusesTemp.transform;
					bonusBackAmount.transform.localScale = new Vector3(1, 1, 1);
					bonusBackAmount.transform.localPosition = new Vector3(positions[i].x + 90, positions[i].y- 90, 1);
					GameObject bonusLabelAmount = GameObject.Instantiate (GameObject.Find("bonuses/label amount"),  positions[i], Quaternion.identity)  as GameObject;
					bonusLabelAmount.transform.parent = bonusesTemp.transform;
					bonusLabelAmount.transform.localScale = new Vector3(1, 1, 1);
					bonusLabelAmount.transform.localPosition = new Vector3(positions[i].x + 90, positions[i].y - 90, 1);
					bonusLabelAmount.GetComponent<UILabel>().text = bonusCount.ToString();

					GameObject bonusShine = GameObject.Instantiate (GameObject.Find("bonuses/shine"), positions[i], Quaternion.identity)  as GameObject;
					bonusShine.transform.parent = bonusesTemp.transform;
					bonusShine.transform.localScale = new Vector3(1, 1, 1);
					bonusShine.transform.localPosition = positions[i];
					break;
				}
			
			counter += portion.Value;
			}
		}

		StartCoroutine(coroutineCloseBonusMenuAnim());
		
	}
	
	IEnumerator coroutineClickOther() {
		yield return new WaitForSeconds(0.5F);
		StartCoroutine(coroutineCloseBonusMenuAnim());
	}
	
	IEnumerator coroutineCloseBonusMenuAnim() {
		yield return new WaitForSeconds(3F);
		giftMenu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
		StartCoroutine(coroutineCloseBonusMenu());
	}
	IEnumerator coroutineCloseBonusMenu() {
		yield return new WaitForSeconds(0.5F);
		Destroy (GameObject.Find ("bonuses temp(Clone)"));
		giftMenu.SetActive(false);
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
		//off for tests
		//initClass.progress [name] = 1; 
		initClass.saveProgress ();
	}
}
