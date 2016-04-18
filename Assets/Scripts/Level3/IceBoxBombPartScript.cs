using UnityEngine;
using System.Collections;

public class IceBoxBombPartScript : MonoBehaviour {
	
	// Use this for initialization
	public bool facing_up = false;
	public bool moving = false;
	public float startposy;
	public float endposy;
	public float speedx = 3;
	public float viewrange = 20;
	public int id;
	GameObject player;
	public Vector2 startpos;
	public bool touching = false;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player"); 
		id = UnityEngine.Random.Range (1,10000);
		startpos = transform.position;
	}

	public void reset(){
			transform.position = startpos;
			moving = false;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			player = other.gameObject;
			moving = true;
			touching = true;
			speedx = 5;
		}
	}
	
	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			player = coll.gameObject;
			touching = false;
			speedx = 3;
		}
		
	}
	
	
	// Update is called once per frame
	void Update () {

		if (moving) {
			transform.position = new Vector2(transform.position.x + speedx * Time.deltaTime, transform.position.y);
		}
		if (touching) {
			player.transform.position = new Vector2(player.transform.position.x + speedx * Time.deltaTime , player.transform.position.y);
		}
	}
}
