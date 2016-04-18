using UnityEngine;
using System.Collections;

public class GoliScript : MonoBehaviour {


	// Use this for initialization
	public float speedx;
	public float speedy;
	GameObject player;
	PlayerLevel3 pl3;
	public bool highspeedgoli;
	public Vector2 startpos;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pl3 = player.GetComponent<PlayerLevel3> ();
		if (highspeedgoli) {
			pl3.attackmaxtime = 0.1f;
		}
		startpos = transform.position;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Pot") {
			player = GameObject.FindGameObjectWithTag ("Player");
			pl3 = player.GetComponent<PlayerLevel3> ();
			pl3.addPot();
			Destroy(gameObject);
			Destroy(other.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector2 (transform.position.x + speedx * Time.deltaTime, transform.position.y + speedy * Time.deltaTime);
		if (transform.position.x - startpos.x > 10) {
			Destroy(gameObject);
		}
	}
}
