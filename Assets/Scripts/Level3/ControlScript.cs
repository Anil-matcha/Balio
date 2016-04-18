using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour {

	public bool dirnup = false;
	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (transform.parent.gameObject.GetComponent<verticalplatformlevel3> ()) {
			transform.parent.gameObject.GetComponent<verticalplatformlevel3> ().facing_up = dirnup;
		} else if (transform.parent.gameObject.GetComponent<IceBoxBombPartScript> ()) {
			transform.parent.gameObject.GetComponent<IceBoxBombPartScript> ().facing_up = dirnup;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
