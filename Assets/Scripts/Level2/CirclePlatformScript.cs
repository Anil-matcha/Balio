using UnityEngine;
using System.Collections;

public class CirclePlatformScript : MonoBehaviour {

	public GameObject center;
	CenterScript cs;
	public float rotation;
	public bool test = false;
	public float maxposx = 0;
	public float maxposy = 0;
	public float minposx = 10000;
	public float minposy = 10000;
	public float radius = 8;
	public bool playerincontact = false;
	GameObject player;
	PlayerLevel2 pl2;
	Vector3 previouspos;
	// Use this for initialization
	void Start () {
		cs = center.GetComponent<CenterScript> ();
		player = GameObject.FindGameObjectWithTag("Player");
		pl2 = player.GetComponent<PlayerLevel2>();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			playerincontact = true;
		}
	}
	
	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			playerincontact = false;
		}
		
	}

	// Update is called once per frame
	void Update () {

		transform.position = new Vector2 (cs.transform.position.x,cs.transform.position.y)+new Vector2(radius*Mathf.Cos((cs.rotation+rotation)*Mathf.PI/180f),radius*Mathf.Sin((cs.rotation+rotation)*Mathf.PI/180f));
		if (player) {
			if (playerincontact) {
				player.transform.position = player.transform.position + new Vector3 (transform.position.x,transform.position.y,transform.position.z) - previouspos;
			}
		}
		//GetComponentInParent<Transform> ();
		previouspos = new Vector3 (transform.position.x,transform.position.y,transform.position.z);
		/*
		if (transform.position.x > maxposx) {
			maxposx = transform.position.x;
		}
		if (transform.position.y > maxposy) {
			maxposy = transform.position.y;
		}
		if (transform.position.x < minposx) {
			minposx = transform.position.x;
		}
		if (transform.position.y < minposy) {
			minposy = transform.position.y;
		}
		print (maxposx + " " + maxposy + "    " + minposx + " " + minposy);
		*/
	}
}
