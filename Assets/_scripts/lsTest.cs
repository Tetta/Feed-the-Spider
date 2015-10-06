using UnityEngine;
using System.Collections;

public class lsTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (name == "scroll 2") transform.localPosition = -GameObject.Find("root/root").transform.localPosition / 2;
        if (name == "scroll 3") transform.localPosition = -GameObject.Find("root/root").transform.localPosition / 1.5F;
    }
}
