using UnityEngine;
using System.Collections;

public class gTerrainClass : MonoBehaviour {

	public GameObject ps;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D collisionObject) {
		GameObject psNew = GameObject.Instantiate (ps, collisionObject.contacts[0].point, Quaternion.identity) as GameObject;
		Destroy (psNew, 1);
	}
}
