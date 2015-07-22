using UnityEngine;
using System.Collections;

public class gSluggishClass : MonoBehaviour {
	private string sluggishState = "";
	private GameObject berry;
	private float timer;

	// Use this for initialization
	void Start () {
		berry = GameObject.Find("berry");
	}
	
	// Update is called once per frame
	void Update () {
		if (sluggishState == "active") {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(gHintClass.checkHint(gameObject, true));
			Vector3 relative = transform.InverseTransformPoint(mousePosition);
			float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
			//transform.rotation = Quaternion.Euler(0, 0, 180 - angle);
			transform.Rotate(0, 0, 180 - angle);
		}
		if (sluggishState == "collision") {
			//berry.transform.rotation = Quaternion.Euler(0, 0, 0);
			//berry.transform.position = new Vector3(transform.position.x, transform.position.y - 0.32F * transform.localScale.y, 10);
		}
		if (sluggishState == "fly") {
			if (Time.time - timer > 0.3F) collisionEnter2D(); 
		}
	}

	void collisionEnter2D() {
		//tutorial
		gHandClass.addHand();
		berry.GetComponent<Rigidbody2D>().isKinematic = true;
		berry.transform.parent = transform;
		berry.transform.localRotation = Quaternion.identity;
		berry.transform.localPosition = new Vector3(0, - 165, 10);
		transform.localScale = new Vector3(0.98F, 1.02F, 1);
		berry.GetComponent<Collider2D>().isTrigger = true;
		transform.parent.GetComponent<Animation>()["sluggish eye"].speed = 1;
		transform.parent.GetComponent<Animation>().Blend("sluggish eye");
		sluggishState = "collision";
	}

	void OnCollisionEnter2D(Collision2D collisionObject) {
	//void OnTriggerEnter2D(Collider2D collisionObject) {
		//Debug.Log (123);
		if (collisionObject.gameObject.name == "berry" && sluggishState == "") {
			collisionEnter2D ();
		}
	}

	void OnTriggerExit2D(Collider2D collisionObject) {
	//void OnCollisionExit2D(Collision2D collisionObject) {
		Debug.Log ("OnTriggerExit2D");
		if (collisionObject.gameObject.name == "berry" && sluggishState == "fly") {
			berry.transform.parent = transform.parent.parent;
			berry.transform.localScale = new Vector3(1, 1, 1);
			berry.GetComponent<Collider2D>().isTrigger = false;
			sluggishState = "";
		}
	}

	void OnPress(bool flag) {
		if (sluggishState == "collision" && flag) {
			//tutorial
			gHandClass.delHand();

			GetComponent<HingeJoint2D>().enabled = false;
			GetComponent<Rigidbody2D>().isKinematic = true;
			transform.localScale = new Vector3(0.96F, 1.04F, 1);
			sluggishState = "active";
			transform.parent.GetComponent<Animation>().Stop();
			gHintClass.checkHint(gameObject);
		}
		
		if (sluggishState == "active" && !flag) {
			staticClass.useSluggish = true;
			if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) GooglePlayManager.instance.UnlockAchievement("achievement_use_sluggish");
			
			gRecHintClass.recHint(transform);
			berry.GetComponent<Rigidbody2D>().isKinematic = false;
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(gHintClass.checkHint(gameObject, true));
			Vector3 diff = transform.position - mousePosition;
			float pointBDiffC = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
			float maxDiffC = 230;
			if (gBerryClass.berryState == "") maxDiffC = 180;
			float diffX = maxDiffC / pointBDiffC * diff.x;
			float diffY = maxDiffC / pointBDiffC * diff.y;
			
			
			berry.GetComponent<Rigidbody2D>().AddForce( new Vector2(diffX, diffY));

			transform.parent.GetComponent<Animation>().Play("sluggish press false");
			transform.parent.GetComponent<Animation>()["sluggish eye"].speed = -1;
			transform.parent.GetComponent<Animation>().Blend("sluggish eye");
			transform.parent.GetComponent<Animation>().CrossFadeQueued("sluggish idle");
			GetComponent<HingeJoint2D>().enabled = true;
			GetComponent<Rigidbody2D>().isKinematic = false;
			transform.localScale = new Vector3(1, 1, 1);

			timer = Time.time;
			sluggishState = "fly";
		}
	}

}
