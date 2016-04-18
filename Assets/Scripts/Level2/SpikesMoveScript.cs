using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

public class SpikesMoveScript : MonoBehaviour {
	
	public bool movestart = false;
	public bool facing_right = true;
	public float startposition = 2;
	public float endposition = -3;
	public float previoustime = 0;
	public float speed = 5;
	public bool playerincontact = false;
	GameObject player;
	PlayerLevel2 pl2;
	// Use this for initialization
	void Start () {
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
		if (movestart && Mathf.Abs(player.transform.position.x - transform.position.x)<10 && Mathf.Abs(player.transform.position.y - transform.position.y)<7) {
			if(facing_right){
				if(transform.position.x > endposition){
					Flip();
				}
				else{
					Vector2 pos = transform.position;
					pos.x = pos.x	+ speed* Time.deltaTime;
					transform.position = pos;
				}
			}
			else{
				if(transform.position.x < startposition){
					Flip();
				}
				else{
					Vector2 pos = transform.position;
					pos.x = pos.x - speed* Time.deltaTime;
					transform.position = pos;
				}
			}
		}
		
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facing_right = !facing_right;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void move(){
		movestart = true;
	}
	
}

