using UnityEngine;
using System.Collections;

public class HingeScript : MonoBehaviour {

	GameObject player;
	PlayerLevel2 pl2;
	HingeJoint2D hj;
	Rigidbody2D r2d;
	public float maxz = 30;
	public float minz = -30;
	public bool facing_left = true;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pl2 = player.GetComponent<PlayerLevel2>();
		hj = GetComponent<HingeJoint2D> ();
		r2d = GetComponent<Rigidbody2D> ();

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			//print ("collision");
			r2d.AddForce(new Vector2(100000,0));
			PlayerLevel2 pl2 = other.gameObject.GetComponent<PlayerLevel2>();
			pl2.hanging = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			print ("collision");
		}
	}

	// Update is called once per frame
	void Update () {
		if (transform.rotation.z < maxz && transform.rotation.z > minz) {
			print ("zone");
			if (facing_left) {
				r2d.AddForce (new Vector2 (-200, 0));
			} else {
				r2d.AddForce (new Vector2 (200, 0));
			}
		} else if (transform.rotation.z > maxz) {
			if (facing_left) {
				facing_left = false;
			}
			r2d.AddForce (new Vector2 (200, 0));
		} else if (transform.rotation.z < minz) {
			if (!facing_left) {
				facing_left = true;
			}
			r2d.AddForce (new Vector2 (-200, 0));
		} else {
			print (transform.rotation.z);
		}

		if (pl2.hanging) {
			player.transform.position = new Vector2(hj.transform.position.x,hj.transform.position.y);
		}
	}
}
