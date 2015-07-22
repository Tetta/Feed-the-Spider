using UnityEngine;
using System.Collections;

public class gYetiClass : MonoBehaviour {

	public GameObject backYeti;
	public GameObject yetiSleep;
	public GameObject yetiZzz;
	public GameObject yetiBlow;

	private GameObject berry;
	private GameObject[] tumbleweeds;
	public static string yetiState = "";
	private GameObject[] chains;
	// Use this for initialization
	void Start () {
		berry = GameObject.Find("berry");
		tumbleweeds = GameObject.FindGameObjectsWithTag("tumbleweed");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress(bool isPressed) {
		if (isPressed)	return;
		staticClass.useYeti = true;
		gRecHintClass.recHint(transform);
		gHintClass.checkHint(gameObject);
		if (yetiState == "") {
			yetiState = "active";
			berry.GetComponent<Rigidbody2D>().angularVelocity = 0;
			berry.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

			chains = GameObject.FindGameObjectsWithTag("chain");
			for (int i = 0; i < chains.Length; i++) {
				chains[i].GetComponent<Rigidbody2D>().isKinematic = true;
			}
			foreach (GameObject item in tumbleweeds) {
				item.GetComponent<Rigidbody2D>().isKinematic = true;
			}
			Time.timeScale = 0;
			yetiBlow.SetActive(true);
			yetiZzz.SetActive(false);
			yetiSleep.SetActive(false);

			backYeti.SetActive(true);
		} else {
			yetiState = "";
			chains = GameObject.FindGameObjectsWithTag("chain");
			for (int i = 0; i < chains.Length; i++) {
				chains[i].GetComponent<Rigidbody2D>().isKinematic = false;
			}
			foreach (GameObject item in tumbleweeds) {
				item.GetComponent<Rigidbody2D>().isKinematic = false;
			}
			Time.timeScale = 1;
			yetiBlow.SetActive(false);
			yetiZzz.SetActive(true);
			yetiSleep.SetActive(true);
			backYeti.SetActive(false);
		}
		
	}
}
