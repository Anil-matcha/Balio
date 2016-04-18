using UnityEngine;
using System.Collections;

public class FallScript : MonoBehaviour {
	Rigidbody2D rb;
	public float startposx;
	public float endposx;
	public float posy;
	public bool set = false;
	GameObject player;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			if (player.transform.position.x > startposx && player.transform.position.x < endposx && Mathf.Abs (player.transform.position.y - posy) < 2) {
				if (UnityEngine.Random.Range (1, 3) % 3 == 1 && !set) {
					rb.isKinematic = false;
				}
				set = true;
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		print (other.gameObject.tag);
		if (other.gameObject.tag == "Player") {
			rb.isKinematic = false;
		}
	}
}
