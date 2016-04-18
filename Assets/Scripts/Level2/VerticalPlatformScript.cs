using UnityEngine;
using System.Collections;

public class VerticalPlatformScript : MonoBehaviour {

	// Use this for initialization
	public float startposy;
	public float endposy;
	public bool facing_up = true;
	public bool moving = false;
	public float speedy = 5;
	public GameObject player = null;
	public bool reset = false;
	public bool movestart = false;
	void Start () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			player = other.gameObject;
			if(other.gameObject.transform.position.y > transform.position.y){
				moving = true;
				reset = false;
			}
			else{
				print (other.gameObject.transform.position.y + " " + transform.position.y);
			}
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
		if (movestart) {
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
			} else {

				if (player) {
					if (Mathf.Abs (player.transform.position.y - transform.position.y) > 15 || Mathf.Abs (player.transform.position.x - transform.position.x) > 12) {
						if (player.transform.position.y - transform.position.y > 1) {
							transform.position = new Vector2 (transform.position.x, endposy);
							reset = true;
						} else if (player.transform.position.y - transform.position.y < -1){
							transform.position = new Vector2 (transform.position.x, startposy);
							reset = true;
						}
					}
				}

			}
		}
	}
}
