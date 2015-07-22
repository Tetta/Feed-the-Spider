using UnityEngine;
using System.Collections;

public class gCloudClass : MonoBehaviour {

	private string cloudState = "";
	// Use this for initialization
	void Start () {
		GetComponent<Animator>().StopPlayback();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress (bool isPressed) {
		if (!isPressed) {
			if (cloudState == "") {
				//tutorial
				gHandClass.delHand ();

				gRecHintClass.recHint (transform);
				gHintClass.checkHint (gameObject);
				GetComponent<Animator>().Play("cloud disabled");
				GetComponent<Collider2D>().isTrigger = true;
				cloudState = "disabled";

				//gameObject.SetActive (false);
			} else {
				//tutorial
				gHandClass.delHand ();
				
				gRecHintClass.recHint (transform);
				gHintClass.checkHint (gameObject);
				GetComponent<Animator>().Play("cloud enabled");
				GetComponent<Collider2D>().isTrigger = false;
				cloudState = "";
				//gameObject.SetActive (false);

			}
		}
	}
}
