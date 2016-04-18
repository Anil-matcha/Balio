using UnityEngine;
using System.Collections;

public class HorizontalMovementScriptLevel2 : MonoBehaviour {
	
	public bool movestart = false;
	public bool facing_right = true;
	public float startposition = 2;
	public float endposition = -3;
	public float previoustime = 0;
	public float speed = 15;
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
		if (pl2.gliding) {
			playerincontact = false;
		}
		if (movestart) {
			if(facing_right){
				if(transform.position.x > endposition){
					facing_right = false;
				}
				else{
					if(player){
						if(playerincontact){
							float playerx = player.transform.position.x;
							Vector2 playerposnew = new Vector2(playerx+speed* Time.deltaTime,player.transform.position.y);
							player.transform.position = playerposnew;
						}
					}
					Vector2 pos = transform.position;
					pos.x = pos.x	+ speed* Time.deltaTime;
					transform.position = pos;
				}
			}
			else{
				if(transform.position.x < startposition){
					facing_right = true;
				}
				else{
					if(player){
						if(playerincontact){
							float playerx = player.transform.position.x;
							Vector2 playerposnew = new Vector2(playerx-speed* Time.deltaTime,player.transform.position.y);
							player.transform.position = playerposnew;
						}
					}
					Vector2 pos = transform.position;
					pos.x = pos.x - speed* Time.deltaTime;
					transform.position = pos;
				}
			}
		}

		/*
		fwd = transform.TransformDirection(Vector2.right);
		bwd = transform.TransformDirection(Vector2.left);
		//print (PlatformerCharacter2D.playerpos);
		Vector2 fwdpos = new Vector2(transform.position.x+0.5f,transform.position.y);
		Vector2 bwdpos = new Vector2(transform.position.x-0.5f,transform.position.y);
		
		RaycastHit2D hitfwd = Physics2D.Raycast(fwdpos, fwd, 100f);  
		RaycastHit2D hitbwd = Physics2D.Raycast(bwdpos, bwd, 100f);  
		
		if (hitfwd.collider != null) { 
			if(hitfwd.collider.gameObject.tag=="Player"){
				playerfront = true;
				if(!facing_right){
					Flip ();
				}
			}
		}
		if (hitbwd.collider != null) { 
			if(hitbwd.collider.gameObject.tag=="Player"){
				playerback = true;
				if(facing_right){
					Flip ();
				}
			}
		}
		*/

	}

	public void move(){
		movestart = true;
	}
	
}
