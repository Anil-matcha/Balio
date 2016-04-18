using UnityEngine;
using System.Collections;

public class BulletNogravity : MonoBehaviour {
	
	// Use this for initialization
	Rigidbody2D rb;
	public float triggerprevioustime;
	public float id;
	public float starttime;
	public float bulletlifetime = 5;
	public bool stationary = false;
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		starttime = Time.time;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Wall" && Time.time-triggerprevioustime>0.05f) {
			Destroy(gameObject);
		}
		else if (other.gameObject.tag == "Platform" && Time.time-triggerprevioustime>0.2f) {
			Destroy(gameObject);
		}
		else if (other.gameObject.tag == "WallTop") {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - starttime > bulletlifetime && !stationary) {
			Destroy(gameObject);
		}
	}
}
