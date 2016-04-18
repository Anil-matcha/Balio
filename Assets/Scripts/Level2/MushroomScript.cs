using UnityEngine;
using System.Collections;

public class MushroomScript : MonoBehaviour {

	// Use this for initialization
	public float collisionprevioustime;
	void Start () {
		collisionprevioustime = Time.time;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {

		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if(Time.time - collisionprevioustime > 0){
				collisionprevioustime = Time.time;
				other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
				PlayerLevel2 pl2 = other.gameObject.GetComponent<PlayerLevel2>();
				pl2.hitvelocity = new Vector2(8,15);
				pl2.hitbool = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
