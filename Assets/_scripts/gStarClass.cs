using UnityEngine;
using System.Collections;

public class gStarClass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (startStar());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		GameObject collectors = GameObject.Find ("bonuses/tween/collectors");
		if (collectors.GetComponent<gBonusClass> ().bonusState == "collectors") {
			GameObject tempGo = Instantiate(collectors.transform.GetChild(2).gameObject, new Vector2(0, 0), Quaternion.identity) as GameObject;
			tempGo.transform.parent = GameObject.Find("root").transform;
			tempGo.transform.localScale = new Vector3(2, 2, 1);
			tempGo.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(-0.2F, -0.2F, 0); 
			Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			StartCoroutine(coroutineCollectStar(collectors, tempGo));
			
			
		}
	}
	private IEnumerator coroutineCollectStar(GameObject collectors, GameObject tempGo){
		Debug.Log("collect star");
		yield return StartCoroutine(staticClass.waitForRealTime(0.4F));
		GameObject[] guiStars = new GameObject[3];
		guiStars[0] = GameObject.Find("gui star 1");
		guiStars[1] = GameObject.Find("gui star 2");
		guiStars[2] = GameObject.Find("gui star 3");	
		guiStars[gBerryClass.starsCounter].GetComponent<UISprite>().color =  new Color(1, 1, 1, 1);
		gBerryClass.starsCounter ++;
		Destroy(tempGo);
		StartCoroutine (destroyStar());
		collectors.GetComponent<gBonusClass> ().bonusState = "";
		
	}

	private IEnumerator startStar(){
		yield return StartCoroutine(staticClass.waitForRealTime(Random.value * 2));
		GetComponent<Animation> ().Play ("star 4");

		
	}

	public IEnumerator destroyStar(){
		GetComponent<Animation> ().Play ("star destroy");
		yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
		Destroy (gameObject);
	}
}
