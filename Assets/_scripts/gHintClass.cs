using UnityEngine;
using System.Collections;


public class gHintClass : MonoBehaviour {


	public static int counter = 0;
	public static action[] actions;
	public static float time;
	public static string hintState = "";

	private static GameObject hint;
	private bool flagTransform = false;

	// Use this for initialization
	void Start () {
		hint = GameObject.Find("hint folder");
		//foreach (AnimationState state in hint.GetComponent<Animation>()) {
		//	state.speed = 20F;
		//}
		if (initClass.progress.Count == 0) initClass.getProgress();
		transform.GetChild(0).GetComponent<UILabel>().text = initClass.progress["hints"].ToString();
		//if (initClass.progress["hints"] != 0) GetComponent<UIPlayAnimation>().enabled = false;

		if (hintState == "enable bonus picture") {
			GameObject.Find("bonuses pictures").transform.GetChild(0).gameObject.SetActive(true);
			GameObject.Find("bonuses pictures").transform.GetChild(1).gameObject.SetActive(true);
			StartCoroutine(coroutineBonusPictureEnable());

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hintState == "start" && counter <= actions.Length - 1) {

			//Debug.Log(Time.time - time);
			//hint.transform.position = new Vector3(-4, 0, 0);
			if (Mathf.Round((Time.unscaledTime - time) * 10) == Mathf.Round(actions[counter].time * 10) || flagTransform) {
				//hintState = "pause";
				Time.timeScale = 0;
				if (!flagTransform && counter == 0) {
				}
				flagTransform = true;
				//перемещение подсказки
				//t
				float t = Time.unscaledTime - actions[counter].time - time;

				Vector2 startPosHint = new Vector2(-4, 0);
				if  (counter != 0) startPosHint = actions[counter - 1].id;
				float offsetX = 0.2F;
				//offsetX = 0;
				if (startPosHint.x > actions[counter].id.x) offsetX = -offsetX;
				hint.transform.position = сalculateBezierPoint(t, startPosHint, new Vector2(startPosHint.x, startPosHint.y - 0.5F),  new Vector2(actions[counter].id.x, actions[counter].id.y + 0.5F),   new Vector2(actions[counter].id.x - offsetX, actions[counter].id.y + 0.3F));
				//заканчиваем перемещение подсказки
				if (t >= 1) {
					hintState = "pause";
					flagTransform = false;
					hint.transform.GetChild(0).GetComponent<Animator>().Play("hint show");
				}



				//hint.transform.position = actions[counter].id;
				//Debug.Log(123);
			}

		}

	}
	void OnPress(bool flag) {
		if (!flag) {
			Debug.Log("use hint");
			Time.timeScale = 1;
			hintState = "enable bonus picture";
			counter = 0;
			Application.LoadLevel(Application.loadedLevel);
			SendMessage(Application.loadedLevelName); 
		}
	}

	public static Vector3 checkHint(GameObject obj, bool flag = false) {
		Time.timeScale = 1;

		if (hintState == "pause") { 
			if (actions[counter].id == obj.transform.position) {
				if (flag) return actions[counter].mouse;
				//hint.transform.position = new Vector3(-4, 0, 0);

				if (obj.name == "destroyer" || obj.name == "groot") {
					obj.SendMessage("OnDrag");
					obj.SendMessage("OnPress", false);
				}
				if (obj.name == "sluggish physics") {
					obj.SendMessage("OnPress", false);
					Debug.Log("OnPress false");
				}

				counter ++;
				time = Time.unscaledTime;
				if (counter <= actions.Length - 1) if (actions[counter - 1].id.x > actions[counter].id.x) hint.transform.localScale = new Vector2(-hint.transform.localScale.x, hint.transform.localScale.y);
				//Debug.Log(hint.transform.position.x > actions[counter - 1].id.x);
				hintState = "start";
			
			} else {
				gHintClass.hintState = "";
				hint.transform.position = new Vector3(-4, 0, 0);
			} 
		}	else if (hintState == "start" && !flag) {
			hintState = "";
			hint.transform.position = new Vector3(-4, 0, 0);
		}


		return new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
	}
	
	public struct action {
		public Vector3 id;
		public Vector3 mouse;
		public float time;
	}

	void level1 () {
		actions = new action[2];
		actions[0].id = new Vector3(0.051342F, 0.49288F, 0F);
		actions[0].time = 0.1F;
		actions[0].mouse = new Vector3(209, 386, 0);
		actions[1].id = new Vector3(0.051342F, 0.49288F, 0F);
		actions[1].time = 1.094343F;
		actions[1].mouse = new Vector3(209, 386, 0);
	}

	void level2 () {
		actions = new action[4];
		actions[0].id = new Vector3(-0.7480469F, 0.7851563F, 0F);
		actions[0].time = 0.11164F;
		actions[0].mouse = new Vector3(129, 504, 0);
		actions[1].id = new Vector3(0.7734375F, 0.4960938F, 0F);
		actions[1].time = 0.160526F;
		actions[1].mouse = new Vector3(407, 450, 0);
		actions[2].id = new Vector3(0.7734375F, 0.4960938F, 0F);
		actions[2].time = 0.1330956F;
		actions[2].mouse = new Vector3(407, 450, 0);
		actions[3].id = new Vector3(-0.7480469F, 0.7851563F, 0F);
		actions[3].time = 0.1805542F;
		actions[3].mouse = new Vector3(135, 499, 0);
	}

	void level20 () {
		actions = new action[8];
		actions[0].id = new Vector3(-1.707522F, -1.798797F, -0.1F);
		actions[0].time = 1.49423F;
		actions[0].mouse = new Vector3(85, 205, 0);
		actions[1].id = new Vector3(0.051342F, 0.49288F, 0F);
		actions[1].time = 0.6248536F;
		actions[1].mouse = new Vector3(199, 379, 0);
		actions[2].id = new Vector3(-2.2448F, 1.1385F, 0F);
		actions[2].time = 1.624653F;
		actions[2].mouse = new Vector3(53, 413, 0);
		actions[3].id = new Vector3(-1.6338F, 1.9594F, 0F);
		actions[3].time = 1.689129F;
		actions[3].mouse = new Vector3(282, 196, 0);
		actions[4].id = new Vector3(1.3F, 2F, 0F);
		actions[4].time = 1.248973F;
		actions[4].mouse = new Vector3(192, 265, 0);
		actions[5].id = new Vector3(2.053608F, 0.27878F, 0F);
		actions[5].time = 0.9984751F;
		actions[5].mouse = new Vector3(366, 371, 0);
		actions[6].id = new Vector3(2.053608F, 0.27878F, 0F);
		actions[6].time = 0.6565351F;
		actions[6].mouse = new Vector3(365, 371, 0);
		actions[7].id = new Vector3(0.051342F, 0.49288F, 0F);
		actions[7].time = 1.199567F;
		actions[7].mouse = new Vector3(208, 379, 0);
	}
	void level51 () {
		actions = new action[2];
		actions [0].id = new Vector3 (-0.7617188F, -0.3242188F, -0.01953125F);
		actions [0].time = 2.10682F;
		actions [0].mouse = new Vector3 (149, 375, 0);
		actions [1].id = new Vector3 (-0.7617188F, -0.3242188F, -0.01953125F);
		actions [1].time = 2.946235F;
		actions [1].mouse = new Vector3 (149, 375, 0);
	}
	void level75 () {
		actions = new action[3];
		actions[0].id = new Vector3(2.1232F, 0.1807F, 0F);
		actions[0].time = 0.644765F;
		actions[0].mouse = new Vector3(321, 329, 0);
		actions[1].id = new Vector3(-1.026575F, 0.7992552F, -0.1F);
		actions[1].time = 1.100898F;
		actions[1].mouse = new Vector3(125, 376, 0);
		actions[2].id = new Vector3(2.1232F, 0.1807F, 0F);
		actions[2].time = 1.292115F;
		actions[2].mouse = new Vector3(351, 363, 0);
	}

	IEnumerator coroutineBonusPictureEnable() {

		yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
		//yield return new WaitForSeconds(0.5F);
		GameObject.Find("bonuses pictures").transform.GetChild(1).gameObject.GetComponent<Animator>().Play("menu exit unscaled");
		GameObject.Find("bonuses pictures").transform.GetChild(0).gameObject.SetActive(false);
		yield return StartCoroutine(staticClass.waitForRealTime(0.3F));
		//yield return new WaitForSeconds(0.3F);

		GameObject.Find("bonuses pictures").transform.GetChild(1).gameObject.SetActive(false);
		hintState = "start";
		time = Time.unscaledTime;

	}

	Vector3 сalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		
		float u = 1 - t;
		float tt = t*t;
		float uu = u*u;
		float uuu = uu * u;
		float ttt = tt * t;
		
		Vector3 p = uuu * p0;    //first term
		p += 3 * uu * t * p1;    //second term
		p += 3 * u * tt * p2;    //third term
		p += ttt * p3;           //fourth term
		
		return p;
	}
}
