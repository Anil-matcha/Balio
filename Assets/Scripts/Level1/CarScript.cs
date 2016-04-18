using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour {

	Rigidbody2D rb;
	int i = 0;
	public bool start = false;
	public Vector2 position;
	public bool cargone = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		position = transform.position;
		//rb.velocity = new Vector2 (-2, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			rb.AddForce (new Vector2 (-2500, 0));
		}
		position = transform.position;
		if (transform.position.y < -5) {
//			MovementScript.movestart = true;
			GameObject hmv = GameObject.FindGameObjectWithTag ("HorizontalMoveBlock");
			HorizontalMovementScript hmvs = hmv.GetComponent<HorizontalMovementScript>();
			hmvs.movestart = true;
			Destroy(gameObject);
		}
		    //rb.AddRelativeForce(new Vector2(1000, 0));
			//rb.AddForce(new Vector2(1000, 0));
	}

	public void reset(){
		position = new Vector2(126f,1.7f);
		transform.position = position;
		start = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Fire") {
			//rb.AddForce(new Vector2(1000,0));
			//Destroy(other.gameObject);
		}
	}
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Fire") {
			//rb.AddForce(new Vector2(1000,0));
			Destroy(other.gameObject);
		}
	}
}
