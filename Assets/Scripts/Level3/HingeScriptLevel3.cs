using UnityEngine;
using System.Collections;

public class HingeScriptLevel3 : MonoBehaviour {
	
	GameObject player;
	PlayerLevel3 pl3;
	//HingeJoint2D hj;
	Rigidbody2D r2d;
	public float minz = 30;
	public float maxz = -30;
	public bool facing_left = true;
	public bool start = false;
	public float force = 20f;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pl3 = player.GetComponent<PlayerLevel3>();
		//hj = GetComponent<HingeJoint2D> ();
		r2d = GetComponent<Rigidbody2D> ();
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerLevel3 pl3 = other.gameObject.GetComponent<PlayerLevel3>();
			pl3.hanging = true;
		}
	}
	
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			print ("collision");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			if (transform.eulerAngles.z < maxz && transform.eulerAngles.z > minz) {
				if (facing_left) {
					//print ("zoneleft" + transform.eulerAngles.z);
					r2d.AddForce (new Vector2 (-force, 0));
				} else {
					//print ("zoneright" + transform.eulerAngles.z);
					r2d.AddForce (new Vector2 (force, 0));
				}
			} else if (transform.eulerAngles.z > maxz) {
				//print ("maxz" + transform.eulerAngles.z);
				if (!facing_left) {
					facing_left = true;
				}
				r2d.AddForce (new Vector2 (-force/10, 0),ForceMode2D.Impulse);
			} else if (transform.eulerAngles.z < minz) {
				//print ("minz");
				if (facing_left) {
					facing_left = false;
				}
				r2d.AddForce (new Vector2 (force/10, 0),ForceMode2D.Impulse);
			} else {
				//print (transform.eulerAngles.z);
			}
		
			if (pl3.hanging) {
				//player.transform.position = new Vector2 (hj.transform.position.x, hj.transform.position.y);
			}
		}
	}
}
