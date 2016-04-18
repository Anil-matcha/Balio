using UnityEngine;
using System.Collections;

public class MoonScript : MonoBehaviour {

	Rigidbody2D rigidbody;
	GameObject player;
	public float force = 1;
	public float startx;
	PlayerLevel2 pl2;
	public float gravitydistance;
	public bool up = false;
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		pl2 = player.GetComponent<PlayerLevel2> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rtv = transform.rotation.eulerAngles;
		rtv.z = Time.time * 30;
		transform.rotation = Quaternion.Euler (rtv);

		Vector3 diff = player.transform.position - transform.position;
		/*
		if (diff.x >= 0) {
			if (diff.x < 3) {
				diff.x = 3;
			}
		} else {
			if(diff.x > -3){
				diff.x = -3;
			}
		}
		if (diff.y >= 0) {
			if (diff.y < 3) {
				diff.y = 3;
			}
		} else {
			if(diff.y > -3){
				diff.y = -3;
			}
		}
		*/
		diff.z = 0;
		if (Mathf.Abs (player.transform.position.x - startx) < gravitydistance ) {
			//print (diff.normalized.x + " "+ diff.normalized.y + " "+diff.normalized.z);
			if(pl2.up && up){
				rigidbody.AddForce ((diff) * force * rigidbody.mass, ForceMode2D.Impulse);
			}
			else{
				if(!pl2.up){
					pl2.m_Rigidbody2D.AddForce((-diff).normalized * force * rigidbody.mass, ForceMode2D.Impulse);
				}
			}
			if (Mathf.Abs (rigidbody.velocity.x) > Mathf.Abs (rigidbody.velocity.y)) {

				if (rigidbody.velocity.x >= 0) {
					rigidbody.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
				} else {
					rigidbody.transform.position = new Vector3 (transform.position.x, transform.position.y, 5);
				}

			} else {

				if (rigidbody.velocity.y >= 0) {
					rigidbody.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
				} else {
					rigidbody.transform.position = new Vector3 (transform.position.x, transform.position.y, 5);
				}

			}
		}

	}
}
