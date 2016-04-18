using UnityEngine;
using System.Collections;

public class BoxScript : MonoBehaviour {

	Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		print (other.gameObject.tag);
		if (other.gameObject.tag == "Bullet") {
			Destroy(other.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag=="Player"){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Rigidbody2D rp = player.GetComponent<Rigidbody2D>();
			//print ("x="+rp.velocity.x+"y="+rp.velocity.y);
			rb.AddForce(transform.right*rp.velocity.x);
		}
	}

}
