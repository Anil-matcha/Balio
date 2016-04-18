using UnityEngine;
using System.Collections;

public class FireGoon : MonoBehaviour {

	public float startposition = 51;
	public float endposition = 65;
	public bool facing_right = true;
	public float speedx = 2;
	public float maxspeedx = 2;
	float timer;
	private Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	private float previousfiretime=0;
	private float previoushittime=0;
	public float delta = 0.2f;
	public float deltay = 2f;
	GameObject player;
	public bool raytest = false;

	// Use this for initialization
	void Start () {
		timer = Time.time;
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		m_Anim.SetFloat("Speed", speedx);
		player = GameObject.FindGameObjectWithTag ("Player");
		if (speedx < 0) {
			Flip();
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool playerback = false;
		bool playerfront = false;
		if (player) {
			//RaycastHit hit;
			Vector2 fwd;
			Vector2 bwd;
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
					if (Time.time - previoushittime > 2) {
						if (Mathf.Abs (player.transform.position.x - transform.position.x) < delta && Mathf.Abs (player.transform.position.y - transform.position.y) < deltay) {
							damageplayer ();
							speedx = 0;
							m_Anim.SetFloat("Speed" , 0);
						}
						else{
							speedx = maxspeedx;
							m_Anim.SetFloat("Speed" , maxspeedx);
						}
					}
					

						
							if (Time.time - previousfiretime > 2) {
								GameObject enemyfireball = (GameObject)Instantiate (Resources.Load ("enemyfireball"));
								if (facing_right) {
									enemyfireball.transform.position = transform.position + new Vector3 (1f, 0.5f, 0.0f);
									enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100, 0));
								} else {
									enemyfireball.transform.position = transform.position + new Vector3 (-1f, 0.5f, 0.0f);
									enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-100, 0));
								}
								previousfiretime = Time.time;
							}	
				}
			}
			if (hitbwd.collider != null) { 
				if(hitbwd.collider.gameObject.tag=="Player"){
					playerback = true;
					if(facing_right){
						Flip ();
					}
					if (Time.time - previoushittime > 2) {
						if (Mathf.Abs (player.transform.position.x - transform.position.x) < delta && Mathf.Abs (player.transform.position.y - transform.position.y) < deltay) {
							damageplayer ();
							speedx = 0;
							m_Anim.SetFloat("Speed" , 0);
						}
						else{
							speedx = maxspeedx;
							m_Anim.SetFloat("Speed" , maxspeedx);
						}
					}
					if (Time.time - previousfiretime > 2) {
						GameObject enemyfireball = (GameObject)Instantiate (Resources.Load ("enemyfireball"));
						if (facing_right) {
							enemyfireball.transform.position = transform.position + new Vector3 (1f, 0.5f, 0.0f);
							enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100, 0));
						} else {
							enemyfireball.transform.position = transform.position + new Vector3 (-1f, 0.5f, 0.0f);
							enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-100, 0));
						}
						previousfiretime = Time.time;
					}	
					
				
					//print(hitfwd.collider.gameObject.tag);
				}
			}
			/*
			if (Time.time - previoushittime > 2) {
				if (player.transform.position.x > startposition - 2 && player.transform.position.x < endposition + 2 && Mathf.Abs (player.transform.position.y - transform.position.y) < 2) {
					if (Mathf.Abs (player.transform.position.x - transform.position.x) < delta && Mathf.Abs (player.transform.position.y - transform.position.y) < deltay) {
						damageplayer ();
					}
					if (transform.position.x > player.transform.position.x && facing_right) {
						Flip ();
					} else if (transform.position.x < player.transform.position.x && !facing_right) {
						Flip ();
					} else {
						if (Time.time - previousfiretime > 0.5) {
							GameObject enemyfireball = (GameObject)Instantiate (Resources.Load ("enemyfireball"));
							if (facing_right) {
								enemyfireball.transform.position = transform.position + new Vector3 (1f, 0.5f, 0.0f);
								enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100, 0));
							} else {
								enemyfireball.transform.position = transform.position + new Vector3 (-1f, 0.5f, 0.0f);
								enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-100, 0));
							}
							previousfiretime = Time.time;
						}
					}
				}
			}
			*/
		}

		if (facing_right) {
			if(transform.position.x < endposition){
				transform.position = new Vector2(transform.position.x + speedx*Time.deltaTime,transform.position.y);
			}
			else{
				if(!playerfront){
					Flip ();
					speedx = maxspeedx;
					m_Anim.SetFloat("Speed" , maxspeedx);
				}
				else{
					m_Anim.SetFloat("Speed" , 0);
					speedx = 0;
				}
				//facing_right = false;
				//m_Rigidbody2D.velocity = new Vector2 (0, 0);
			}
		} else {
			if(transform.position.x > startposition){
				transform.position = new Vector2(transform.position.x - speedx*Time.deltaTime,transform.position.y);
				//m_Rigidbody2D.velocity = new Vector2 (-speedx, 0);
			}
			else{
				if(!playerback){
					Flip ();
					speedx = maxspeedx;
					m_Anim.SetFloat("Speed" , maxspeedx);
				}
				else{
					speedx = 0;
					m_Anim.SetFloat("Speed" , 0);
				}				//facing_right = true;
				//m_Rigidbody2D.velocity = new Vector2 (0, 0);
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
	
	void OnCollisionEnter2D(Collision2D other) {
		 if (other.gameObject.tag == "Player") {
			//Flip ();
			speedx = 0;
			m_Anim.SetFloat("Speed",0);
			previoushittime = Time.time;
		} else {
			Flip ();
		}
	}

	public void damageplayer(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		PlatformerCharacter2D p2d = player.GetComponent<PlatformerCharacter2D> ();
		if(Time.time - p2d.goonhitprevioustime > 0.5){
			int damage = 5;
			p2d.bloodSplat (player);
			HealthManager hm = player.GetComponent<HealthManager>();
			if(hm.health - damage>0){
				p2d.damagebygoon (damage);
				p2d.goonhitvelocity = m_Rigidbody2D.velocity * 20;
				p2d.goonhitbool = true;
				p2d.applyGoonHitForce (p2d.goonhitvelocity);
			}
			else{
				p2d.damagebygoon(hm.health);
				p2d.goonhitbool = false;
				p2d.goonhitvelocity = new Vector2(0,0);
			}
			p2d.goonhitprevioustime = Time.time;
		}
	}
}
