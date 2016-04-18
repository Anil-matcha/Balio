using UnityEngine;
using System.Collections;

public class BlockEnemyScript : MonoBehaviour {

	// Use this for initialization
	public float maxscale = 3;
	public Vector2 startpos;
	void Start () {
		startpos = transform.position;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Arrow") {
			Vector3 theScale = transform.localScale;
			theScale.y *= 0.01f;
			Destroy(other.gameObject);
			transform.localScale = theScale;
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 theScale = transform.localScale;
		if (theScale.y < maxscale) {
			theScale.y = theScale.y + 0.0035f;
			transform.localScale = theScale;
		} else {
			theScale.y = maxscale;
			transform.localScale = theScale;
		}
		//transform.position = startpos;
	}
}
