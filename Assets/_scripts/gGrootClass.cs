using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gGrootClass : MonoBehaviour {

	public GameObject line;
	public GameObject chainPrefab;
	public GameObject chainFirst;

	private GameObject berry;
	private GameObject spider;
	private int globalCounter = 0;
	private string grootState = "";
	private float  chainLength = 0.19F;
	private int maxChainCount = 10;
	private int chainCount = 0;
	private GameObject[] chain;
	private float angle;
	private HingeJoint2D jointGroot;
	private float diffX;
	private float diffY;
	private GameObject[] terrains;
	public struct terrainGrootChain {
		public GameObject terrain;
		public GameObject chain;
	}
	public static  List<terrainGrootChain>  terrainGrootChains = new List<terrainGrootChain>();

	// Use this for initialization
	void Start () {
		chain = new GameObject[11];
		jointGroot = GetComponent<HingeJoint2D> ();
		terrains = GameObject.FindGameObjectsWithTag("terrain");
		spider = GameObject.Find("spider");
		berry = GameObject.Find("berry");

	}
	
	// Update is called once per frame
	void Update () {
		if (grootState == "creating") {
			for (int j = 0; j < 8; j++) {
				if (chainCount < maxChainCount && grootState == "creating") {
					createRope();
				}
			}
		}

		if (grootState == "enable" || grootState == "creating") {
			GetComponent<LineRenderer>().SetVertexCount (chainCount);
			for(int i = 1; i <= chainCount; i++) {
				GetComponent<LineRenderer>().SetPosition (i - 1, new Vector3(chain[i].transform.position.x, chain[i].transform.position.y, 1.1F));
			}
		}

		if (grootState == "noCollisions" || grootState == "destroying") {
			for (int j = 0; j < 2; j++) {
				if (chainCount > 0) {
					Destroy(chain[globalCounter], 0);
					chainCount--;
					globalCounter ++;
					
				} else {
					grootState = "";
				}
			}
			GetComponent<LineRenderer>().SetVertexCount (chainCount);
			for(int i = 0; i < chainCount; i++)
				GetComponent<LineRenderer>().SetPosition (i, new Vector3(chain[i + globalCounter].transform.position.x, chain[i + globalCounter].transform.position.y, 1.1F)); 
			
			chainFirst.SetActive(false);

		}
	}

	void OnPress(bool isPressed) {
		if (isPressed) {
			if (grootState == "") {
				//tutorial
				gHandClass.delHand ();

				staticClass.useGroot = true;
				grootState = "drag";
				gHintClass.checkHint (gameObject);
			}		
			if (grootState == "enable") {
				staticClass.useGroot = true;
				grootState = "destroying";
				gHintClass.checkHint (gameObject);
				gRecHintClass.recHint (transform);
				globalCounter = 1;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				chainFirst.SetActive(false);

			}
		} else {
			if (grootState == "drag") {
				gRecHintClass.recHint (transform);

				Vector3 mousePosition = Camera.main.ScreenToWorldPoint (gHintClass.checkHint (gameObject, true));
				Vector3 diff = mousePosition - transform.position;
				//Debug.Log (diff);
				float pointBDiffC = Mathf.Sqrt (diff.x * diff.x + diff.y * diff.y);
				diffX = chainLength / pointBDiffC * diff.x;
				diffY = chainLength / pointBDiffC * diff.y;
				grootState = "creating";

			}
		}
	}
	
	void OnDrag() {
		if (grootState == "drag") {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(gHintClass.checkHint(gameObject, true));
			Vector3 relative = transform.InverseTransformPoint(mousePosition);
			angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
			transform.Rotate(0, 0, - angle);
		}
	}

	void createRope () {
		chainCount ++; 
		int i = chainCount;
		chain[i] = Instantiate(chainPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		chain[i].SetActive(true);
		chain[i].transform.Rotate(0, 0, transform.rotation.eulerAngles.z - 90);

		chain[i].name = "chain " + i;
		chain[i].transform.parent = transform.parent;
		chain[i].transform.localScale = new Vector3(1, 1, 1);
		if (i == 1) {
			jointGroot.connectedBody = chain[i].GetComponent<Rigidbody2D>();
			jointGroot.enabled = true;
		} else {
			for (int y = 1; y <= i; y++) {
				chain[y].transform.position = new Vector3(diffX * (i - y + 2), diffY * (i - y + 2), 0) + transform.position;
			}
			HingeJoint2D joint = chain[i].GetComponent<HingeJoint2D> ();
			joint.connectedBody = chain[i - 1].GetComponent<Rigidbody2D>();
			joint.enabled = true;
			jointGroot.connectedBody = chain[i].GetComponent<Rigidbody2D>();

		}
		foreach (GameObject terrain in terrains) {
			//first point
			if (terrain.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX * 0.75F, diffY * 0.75F, 0))) {
				chainFirst.SetActive(true);
				chainFirst.transform.position = chain[1].transform.position + new Vector3(diffX * 0.75F, diffY * 0.75F, 0);
				chainFirst.GetComponent<HingeJoint2D> ().connectedBody = chain[1].GetComponent<Rigidbody2D>();
				grootState = "enable";
				terrainGrootChains.Add(new terrainGrootChain() {terrain = terrain, chain = chain[1]});
				GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(chainCount, 1);

			} 
			//second point
			if (terrain.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX * 0.5F, diffY * 0.5F, 0))) {
				chainFirst.SetActive(true);
				chainFirst.transform.position = chain[1].transform.position + new Vector3(diffX * 0.5F, diffY * 0.5F, 0);
				chainFirst.GetComponent<HingeJoint2D> ().connectedBody = chain[1].GetComponent<Rigidbody2D>();
				grootState = "enable";
				terrainGrootChains.Add(new terrainGrootChain() {terrain = terrain, chain = chain[1]});
				GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(chainCount, 1);
			} 

			//3 point
			if (terrain.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX, diffY, 0))) {
				chainFirst.SetActive(true);
				chainFirst.transform.position = chain[1].transform.position + new Vector3(diffX, diffY, 0);
				chainFirst.GetComponent<HingeJoint2D> ().connectedBody = chain[1].GetComponent<Rigidbody2D>();
				grootState = "enable";
				terrainGrootChains.Add(new terrainGrootChain() {terrain = terrain, chain = chain[1]});
				GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(chainCount, 1);
				
			} 
		}
		if (chainCount == maxChainCount || spider.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position) || berry.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position)) {
			grootState = "noCollisions";
			globalCounter = 1;
		}
	}

}
