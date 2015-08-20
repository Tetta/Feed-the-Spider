using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gSpiderClass : MonoBehaviour {

	private GameObject[] guiStars = new GameObject[3];
	//private GameObject restart;
	private int fixedUpdateCount;
	private GameObject berry;
	public Animator currentSkinAnimator;
	private Rigidbody2D rigid2D;
	public static List<int> websSpider = new List<int>();
	//private GameObject completeMenu;
	//private GameObject berry;
	// Use this for initialization
	void Start () {
		guiStars[0] = GameObject.Find("gui star 1");
		guiStars[1] = GameObject.Find("gui star 2");
		guiStars[2] = GameObject.Find("gui star 3");	
		berry = GameObject.Find("berry");

		//включаем текущий скин и выключаем все остальные
		for (int i = 0; i < 5; i++) {
			if (transform.GetChild(i).name == staticClass.currentSkin) {
				transform.GetChild(i).gameObject.SetActive(true);
				currentSkinAnimator = transform.GetChild(i).GetComponent<Animator>();
				//включаем текущую шапку и выключаем все остальные
				for (int j = 0; j < 4; j++) {
					if (transform.GetChild (i).GetChild (0).GetChild (j).name == staticClass.currentHat) {
						transform.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (true);
					} else 
						transform.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (false);
				}
			} else 
				transform.GetChild(i).gameObject.SetActive(false);


		}

		rigid2D = GetComponent<Rigidbody2D> ();

		//completeMenu = GameObject.Find("gui").transform.Find("complete menu").gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < -4 || transform.position.x > 4 || transform.position.y < -6 || transform.position.y > 6) GameObject.Find("restart").SendMessage("OnClick");
		//if (completeMenu.activeSelf) transform.position = berry.transform.position;
	}

	void FixedUpdate () {

		/* было что-то... сейчас баг
		if ((berry.transform.position - transform.position).magnitude < 0.5F) {

			if (gBerryClass.berryState == "" && !rigid2D.isKinematic) {
				//rigid2D.isKinematic = true;
			}
		} else {
			if (rigid2D.isKinematic) {
				//rigid2D.isKinematic = false;
			}

		}
		*/
		if (fixedUpdateCount % 50 == 0 && gBerryClass.berryState != "start finish") {
			if ((berry.transform.position - transform.position).magnitude >= 0.5F) {
				if (currentSkinAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider open month") ||
				    currentSkinAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider open month legs")) {
					currentSkinAnimator.Play("spider idle", 1);
					//currentSkinAnimator.Play("spider breath", 0);
					//Debug.Log(currentSkinAnimator.Play("spider idle");
				}
				//if (currentSkinAnimator.GetCurrentAnimatorStateInfo(-1))
				//currentSkinAnimator.Play("spider breath", -1);
			}

			//check jump
			if (GetComponent<Rigidbody2D>().velocity.magnitude <= 0.003F && websSpider.Count == 0) {
				if (transform.rotation.z > 0.4F || transform.rotation.z < -0.4F) {
					currentSkinAnimator.Play("spider jump");
					StartCoroutine(coroutineJump());
				} else {
					if (!currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath") && !currentSkinAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider open month")) {
						if (currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider fly")) {
							//currentSkinAnimator.transform.FindChild("leg left 2").GetComponent<UISprite>().depth = 2;
							//currentSkinAnimator.transform.FindChild("leg right 2").GetComponent<UISprite>().depth = 2;
							currentSkinAnimator.Play("spider jump");
							StartCoroutine(coroutineJump());	
						} else
							currentSkinAnimator.Play("spider breath");
					}
				}
			} else {
				//if (currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath")) {
					/*for tests
					currentSkinAnimator.transform.FindChild("leg left 2").GetComponent<UISprite>().depth = 1;
					currentSkinAnimator.transform.FindChild("leg right 2").GetComponent<UISprite>().depth = 1;
					*/
					
				if (websSpider.Count != 0) currentSkinAnimator.Play("spider fly 2");
				 else {
					currentSkinAnimator.Play("spider fly");
				}
			}

		} else if (fixedUpdateCount % 10 == 0 && gBerryClass.berryState == "") {
			//check mouth
			if ((berry.transform.position - transform.position).magnitude < 0.5F) 
				if (currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath"))
				    currentSkinAnimator.Play("spider open month legs");
				else if (currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider fly") ||
				currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider fly 2")) {
				currentSkinAnimator.Play("spider open month"); } 
				
		}
		fixedUpdateCount ++;

	}
		
	void OnTriggerEnter2D(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "star") {
			Destroy(collisionObject.gameObject);
			guiStars[gBerryClass.starsCounter].GetComponent<UISprite>().color =  new Color(1, 1, 1, 1);
			gBerryClass.starsCounter ++;
		}
		
	}

	void OnClick () {
		Debug.Log (234);
		currentSkinAnimator.Play("spider blink", -1);
	}

	public IEnumerator coroutineJump(){
		yield return new WaitForSeconds(0.1F);
		transform.rotation = new Quaternion(0, 0, 0, 1);

	}

	public static IEnumerator coroutineCry(Animator spiderAnimator){
		Debug.Log (33);
		spiderAnimator.Play("spider sad", 1);
		yield return new WaitForSeconds(2F);
		GameObject.Find("restart").SendMessage("OnClick");
	}

	
}
