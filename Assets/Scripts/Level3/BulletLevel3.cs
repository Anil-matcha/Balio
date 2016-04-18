using UnityEngine;
using System.Collections;

public class BulletLevel3 : MonoBehaviour {

	// Use this for initialization
	Rigidbody2D rb;
	public float triggerprevioustime;
	public float id;
	public float starttime;
	public float bulletlifetime = 5;
	public bool stationary= false;
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		starttime = Time.time;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Wall" && Time.time-triggerprevioustime>0.05f) {
			triggerprevioustime = Time.time;
			print (other.gameObject.tag + rb.velocity.x + " " +rb.velocity.y);
		    rb.velocity = new Vector2(-rb.velocity.x,rb.velocity.y*2);
			if(rb.velocity.y > 10){
			    rb.velocity = new Vector2(-rb.velocity.x,10);
			}
			else if(rb.velocity.y < -10){
				rb.velocity = new Vector2(-rb.velocity.x,-10);
			}
			print (other.gameObject.tag + rb.velocity.x + " " +rb.velocity.y + (Time.time-triggerprevioustime));
		}
		else if (other.gameObject.tag == "Platform" && Time.time-triggerprevioustime>0.2f) {
			triggerprevioustime = Time.time;
			if(rb){
				rb.velocity = new Vector2(rb.velocity.x,-rb.velocity.y);
			}
		}
		else if (other.gameObject.tag == "WallTop") {
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (rb.velocity.x) < 1) {
			Destroy(gameObject);
		}
		if (Time.time - starttime > bulletlifetime && !stationary) {
			Destroy(gameObject);
		}
	}
}
