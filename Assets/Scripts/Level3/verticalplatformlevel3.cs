using UnityEngine;
using System.Collections;

public class verticalplatformlevel3 : MonoBehaviour {

	// Use this for initialization
	public bool facing_up = false;
	public bool moving = false;
	public float startposy;
	public float endposy;
	public float speedy = 3;
	public bool releasestart = false;
	GameObject player;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player"); 
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			if(!releasestart){
				GameObject.FindGameObjectWithTag("HenPower").GetComponent<ReleaseScript>().start = true;
				releasestart = true;
			}
			player = other.gameObject;
			moving = true;
		} 
	}
	
	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			player = coll.gameObject;
			moving = false;
		}
		
	}


	// Update is called once per frame
	void Update () {
		if (moving) {
			if (facing_up) {
				if (transform.position.y < endposy) {
					transform.position = new Vector2 (transform.position.x, transform.position.y + speedy * Time.deltaTime);
				} else {
					facing_up = false;
				}
			} else {
				if (transform.position.y > startposy) {
					transform.position = new Vector2 (transform.position.x, transform.position.y - speedy * Time.deltaTime);
				} else {
					facing_up = true;
				}
			}
		}
	}
}
