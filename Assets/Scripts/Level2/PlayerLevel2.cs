using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*namespace UnityStandardAssets._2D
{*/
public class PlayerLevel2 : MonoBehaviour
{
	[SerializeField] private float m_MaxSpeed = 30f;                    // The fastest the player can travel in the x axis.
	[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	
	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	public Animator m_Anim;            // Reference to the player's animator component.
	public Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public int hitindex = 0;
	public bool hitbool = false;
	public Vector2 hitvelocity = new Vector2(0,0);
	public float hitprevioustime=0;
	public float startcount = 0;
	public float maxposx = -100;
	ArrayList checkpoints = new ArrayList();
	public float deathposy = -20;
	public bool hanging = false;
	public float maxhitcount = 5;
	public float multiplyfactor = 40;
	GameObject hinge;
	public Vector2 velocity;
	public Vector2 previousposition;
	GameObject weapon;
	public float previousattacktime;
	public bool gliding = false;
	GameObject parachute;
	public bool up = false;
	public GameObject panel;
	public AudioClip[] audioclip;
	public GameObject enemy1;
	public string attackname = "none";
	public GameObject weaponimage;
	public GameObject rabbitimage;
	public Sprite[] weapons;
	public bool rabbitacquired = false;
	public GameObject[] resetplatforms;
	private void Awake()
	{
		// Setting up references.
		m_GroundCheck = transform.Find("GroundCheck");
		m_CeilingCheck = transform.Find("CeilingCheck");
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		previousposition = transform.position;
		panel.SetActive (false);
		rabbitimage.SetActive (false);
		checkpoints.Add (new Vector2 (-40,44.7f));
		checkpoints.Add (new Vector2 (175,53));
		checkpoints.Add (new Vector2 (420,75));
		checkpoints.Add (new Vector2 (862,35));
		checkpoints.Add (new Vector2 (893.87f,-9));
		checkpoints.Add (new Vector2 (1015,35));
	}

	void showweapon(){
		if (attackname == "none") {
			weaponimage.SetActive(false);
		}
		if (attackname == "bow") {
			weaponimage.SetActive(true);
			weaponimage.GetComponent<Image>().sprite = weapons[0];
		}
		if (attackname == "trident") {
			weaponimage.SetActive(true);
			weaponimage.GetComponent<Image>().sprite = weapons[1];
		}
		if (attackname == "gun") {
			weaponimage.SetActive(true);
			weaponimage.GetComponent<Image>().sprite = weapons[2];
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Hinge") {
			if(m_FacingRight){
				other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(200,0));
			}
			else{
				other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200,0));
			}
			hanging = true;
			hinge = other.gameObject;
		}
		else if (other.gameObject.tag == "Parachute") {
			parachute = other.gameObject;
			//Destroy (other.gameObject);
			gliding = true;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.drag = 1f;
		}
		else if (other.gameObject.tag == "Bow") {
			weapon = (GameObject)Instantiate (Resources.Load ("Bow"));
			BowScript bs = weapon.GetComponent<BowScript> ();
			bs.hidebow ();
			weapon.SetActive(true);
			attackname = "bow";
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Trident") {
			weapon = (GameObject)Instantiate (Resources.Load ("Trident"));
			BowScript bs = weapon.GetComponent<BowScript> ();
			bs.hidebow ();
			attackname = "trident";
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Gun") {
			weapon = (GameObject)Instantiate (Resources.Load ("Gun"));
			BowScript bs = weapon.GetComponent<BowScript> ();
			bs.hidebow ();
			weapon.SetActive(true);
			attackname = "gun";
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "KillZone") {
			reset();
		}
		else if (other.gameObject.tag == "Rabbit") {
			rabbitacquired = true;
			rabbitimage.SetActive(true);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Mushroom") {
			m_Rigidbody2D.velocity = Vector2.zero;
			hitbool = true;
			maxhitcount = 1;
			multiplyfactor = 2;
		}
	}
	
	public void killplayer(){
			LoadNextLevel();
	}
	
	private void FixedUpdate()
	{
		if (m_Rigidbody2D.velocity.y < -15) {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,-15);
		}
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject){
				m_Grounded = true;
			}
		}
		m_Anim.SetBool("Ground", m_Grounded);
		
		// Set the vertical animation
		m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}
	
	public void applyHitForce(Vector2 vel){
		if (hitindex < maxhitcount && hitbool) {
			if(hitindex == 0){
				PlaySound(0);
			}
			hitindex += 1;
			m_Rigidbody2D.AddForce (vel * multiplyfactor , ForceMode2D.Impulse);
		} else {
			hitbool = false;
			hitindex = 0;
		}
	}

	public void apply(Vector2 vel){
		if (hitindex < 3 && hitbool) {
			hitindex += 1;
			m_Rigidbody2D.AddForce (vel * 40);
		} else {
			hitbool = false;
			hitindex = 0;
		}
	}

	public void attackenemy(bool attack){
		if (attack && attackname!="none") {
			if (Time.time - previousattacktime > 0.5) {
				previousattacktime = Time.time;
				if(m_FacingRight){
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y);
					}
					BowScript bs = weapon.GetComponent<BowScript>();
					bs.weaponvelocitx = 15;
					bs.weaponname = attackname;
					bs.attack();
				}
				else{
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y);
					}
					BowScript bs = weapon.GetComponent<BowScript>();
					bs.weaponvelocitx = -15;
					bs.weaponname = attackname;
					bs.attack();
				}
			}
			else{
				if(attackname == "bow" || attackname == "gun"){
					if(m_FacingRight){
						weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y);
					}
					else{
						weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y);
					}
				}
			}
		} else {
			if (Time.time - previousattacktime > 0.5) {
				if(attackname == "bow" || attackname == "gun"){
					BowScript bs = weapon.GetComponent<BowScript>();
					bs.hidebow();
				}
			}			
			else{
				if(m_FacingRight){
					if(attackname == "bow"){
						weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y);
					}
				}
				else{
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y);
					}
				}
			}
		}
	}

	public void Move(float move, bool crouch, bool jump, bool attack,bool fire,bool henpowerbool,bool devtestbool,float movevertical)
	{
		attackenemy (attack);
		// If crouching, check to see if the character can stand up
		if (!crouch && m_Anim.GetBool ("Crouch")) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle (m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)) {
				crouch = true;
				m_Grounded = true;
			}
		}
		
		// Set whether or not the character is crouching in the animator
		m_Anim.SetBool ("Crouch", crouch);
		if (!gliding) {

		
			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl) {
				// Reduce the speed if crouching by the crouchSpeed multiplier
				move = (crouch ? move * m_CrouchSpeed : move);
			
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				m_Anim.SetFloat ("Speed", Mathf.Abs (move));
			
				// Move the character
				m_Rigidbody2D.velocity = new Vector2 (move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
				//transform.position = new Vector2(transform.position.x + move*m_MaxSpeed*Time.deltaTime,m_Rigidbody2D.velocity.y*Time.deltaTime);
				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight) {
					// ... flip the player.
					Flip ();
				}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight) {
					// ... flip the player.
					Flip ();
				}
			}
		
			// If the player should jump...
			if (m_Grounded && jump && m_Anim.GetBool ("Ground")) {
			
				// Add a vertical force to the player.
				m_Grounded = false;
				m_Anim.SetBool ("Ground", false);
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, 0f);
				m_Rigidbody2D.AddForce (new Vector2 (0f, m_JumpForce));
				PlaySound(0);
			}
			if (jump) {
				if (hanging) {
					hanging = false;
					hitbool = true;
					maxhitcount = 1;
					multiplyfactor = 1;
					float velocityx = velocity.x;
					float velocityy = velocity.y;
					if (velocityx < 3 && velocityx >= 0) {
						velocityx = 3;
					}
					if (velocityx > -3 && velocityx < 0) {
						velocityx = -3;
					}
					if (velocityy < 3 && velocityy >= 0) {
						velocityy = 3;
					}
					if (velocityy > -3 && velocityy < 0) {
						velocityy = -3;
					}
					if (velocityx > 6) {
						velocityx = 6;
					}
					if (velocityx < -6) {
						velocityx = -6;
					}
					if (velocityy > 4) {
						velocityy = 4;
					}
					if (velocityy < -4) {
						velocityy = -4;
					}
					//print (velocityx + " " + velocityy);
					if (m_FacingRight) {
						if (velocityx < 0) {
							velocityx = -velocityx;
						}
					} else {
						if (velocityx > 0) {
							velocityx = -velocityx;
						}
					}
					if (velocityy < 0) {
						velocityy = -velocityy;
					}
					if (m_FacingRight) {
						velocityx = 3;
					} else {
						velocityx = -3;
					}
					velocityy = 3;
					hitvelocity = new Vector2 (velocityx*10, velocityy*10);
				}
			}
		} else {
			if(transform.position.x > 860){
				m_Rigidbody2D.velocity = Vector2.zero;
				gliding = false;
				Destroy(parachute);
				//parachute = false;
				//parachute.transform.position = new Vector2(462,75);
			}
			else{
				m_Rigidbody2D.AddForce(new Vector2(10,50));
				m_Anim.SetFloat ("Speed", Mathf.Abs (move));
				m_Anim.SetBool("Ground",false);
				if(m_FacingRight){
					if(m_Rigidbody2D.velocity.x < 0){
						Flip();
					}
				}
				else{
					if(m_Rigidbody2D.velocity.x > 0){
						Flip();
					}
				}
				m_Rigidbody2D.AddForce(new Vector2(move*100,movevertical*100));
				if(Mathf.Abs(m_Rigidbody2D.velocity.x)>5){
					if(m_Rigidbody2D.velocity.x > 0){
						m_Rigidbody2D.velocity = new Vector2(5,m_Rigidbody2D.velocity.y);
					}
					else{
						m_Rigidbody2D.velocity = new Vector2(-5,m_Rigidbody2D.velocity.y);
					}
				}
				if(Mathf.Abs(m_Rigidbody2D.velocity.y)>5){
					if(m_Rigidbody2D.velocity.y > 0){
						m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,5);
					}
					else{
						m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,-5);
					}
				}
				if(parachute){
				  parachute.transform.position = new Vector2(transform.position.x,transform.position.y+2f);
				}
			}
		}
		//print (m_Rigidbody2D.velocity.x + " " + m_Rigidbody2D.velocity.y);
		/*else {
			if(!m_Grounded){
				if(m_Rigidbody2D.velocity.y > 0){
					if(m_Rigidbody2D.velocity.y >3){
						m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,3);
					}
				}
			}
		}
		*/
		velocity = new Vector2((transform.position.x-previousposition.x)/Time.deltaTime,(transform.position.y-previousposition.y)/Time.deltaTime); 
		//print (velocity.x + " "+ velocity.y);
		previousposition = transform.position;
		if (hitbool) {
			applyHitForce(hitvelocity);
		}
		if (hanging) {
			transform.position = new Vector2(hinge.transform.position.x,hinge.transform.position.y);
		}
		if (transform.position.x > maxposx) {
			maxposx = transform.position.x;
		}
		showweapon ();
	}
	
	

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Mushroom") {
			m_Rigidbody2D.velocity = Vector2.zero;
			hitbool = true;
			maxhitcount = 3;
			multiplyfactor = 40;
		} else if (other.gameObject.tag == "Spikes") {
			bloodSplat(gameObject);
			reset();
		}
	}

	public void bloodSplat(GameObject obj){
		GameObject bloodsplat = (GameObject)Instantiate (Resources.Load ("bloodsplatforfloor"));
		bloodsplat.transform.position = transform.position;
		GameObject blood = (GameObject)Instantiate (Resources.Load ("BloodSplat"));
		blood.transform.position = transform.position;
		EllipsoidParticleEmitter[] ps = blood.GetComponentsInChildren<EllipsoidParticleEmitter> ();
		for (int i=0; i<ps.Length; i++) {
			ps [i].Emit ();
			ps [i].emit = false;
			//ps[i].enabled = false;
		}
		blood.transform.position = new Vector3 (obj.transform.position.x - 2, 0, obj.transform.position.z);
	}

	public void damagebygoon(int damage){

	}
	
	void LoadNextLevel() {

	}

	void PlaySound(int clip)
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = audioclip[clip];
		audio.Play ();
	}﻿

			
	public void showpanel(){
		panel.SetActive (true);
	}

	public void hidepanel(){
		panel.SetActive (false);
	}

	void reset() {
		Camera2DFollow.followbool = false;
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.angularVelocity = 0;
		Vector2 pos = new Vector2 (-30, 45);
		int count = 0;
		foreach(Vector2 checkpoint in checkpoints) {
			count++;
			if(checkpoint.x > maxposx){
				break;
			}
			else{
				pos = checkpoint;
			}
		}
		//print (count + " " + (checkpoints.Count-1));
		if (count >= checkpoints.Count-1) {
			foreach(GameObject obj in resetplatforms){
				//obj.SetActive(true);
				//FallingPlatformScript fps = obj.GetComponent<FallingPlatformScript>();
				//fps.starttime = Time.time;
				ResetPlatform(obj);

			}
		}
		transform.position = pos;
		m_Grounded = false;
	}

	public void ResetPlatform(GameObject obj){
		//print ("reset");
		//yield return new WaitForSeconds(1f);
		obj.SetActive (true);
		FallingPlatformScript fps = obj.GetComponent<FallingPlatformScript>();
		fps.reset ();
	}

}



//}
