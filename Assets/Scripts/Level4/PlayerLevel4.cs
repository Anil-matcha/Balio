using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*namespace UnityStandardAssets._2D
{*/
public class PlayerLevel4 : MonoBehaviour
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
	public float attackmaxtime = 0.5f;
	GameObject parachute;
	public bool up = false;
	public GameObject panel;
	public AudioClip[] audioclip;
	public string attackname = "none";
	public GameObject weaponimage;
	public Sprite[] weapons;
	public float hangstarttime;
	public bool bulletnogravity = false;
	public bool crouchbool = false;
	public bool uppressed = false;
	public GameObject PotsUI;
	public int potscount;
	public bool flying = false;
	private void Awake()
	{
		// Setting up references.
		m_GroundCheck = transform.Find("GroundCheck");
		m_CeilingCheck = transform.Find("CeilingCheck");
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		previousposition = transform.position;
		panel.SetActive (false);
		PotsUI.SetActive (false);
		checkpoints.Add (new Vector2 (0, 1));
		checkpoints.Add (new Vector2(131,3));
		checkpoints.Add (new Vector2(179,4));
		checkpoints.Add (new Vector2(255,2));
		checkpoints.Add (new Vector2(365,0));
		checkpoints.Add (new Vector2(400,11));
		checkpoints.Add (new Vector2(897,11));
		checkpoints.Add (new Vector2 (921, 7));
		checkpoints.Add (new Vector2 (1043, 5));
		checkpoints.Add (new Vector2(1090,-15));
	}
	
	public void showpotsUI(){
		PotsUI.SetActive (true);
	}
	
	public void addPot(){
		potscount++;
		Text t = PotsUI.GetComponentInChildren<Text> ();
		t.text = potscount+"";
	}
	
	void showweapon(){
		if (attackname == "none") {
			weaponimage.SetActive(false);
		}
		if (attackname == "gun") {
			weaponimage.SetActive (true);
			weaponimage.GetComponent<Image> ().sprite = weapons [0];
		} else if (attackname == "goli") {
			weaponimage.SetActive (true);
			weaponimage.GetComponent<Image> ().sprite = weapons [1];
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Hinge") {
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			if (m_FacingRight) {
				other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (10f, 0), ForceMode2D.Impulse);
			} else {
				other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-10f, 0), ForceMode2D.Impulse);
			}
			hangstarttime = Time.time;
			m_Rigidbody2D.velocity = Vector2.zero;
			//hanging = true;
			hinge = other.gameObject;
			HingeScriptLevel3 hl3 = hinge.GetComponent<HingeScriptLevel3> ();
			if (m_FacingRight) {
				hl3.facing_left = false;
			} else {
				hl3.facing_left = true;
			}
			hl3.start = true;
		} else if (other.gameObject.tag == "Heart") {
			transform.position = (Vector2)checkpoints[6];
		}
		else if (other.gameObject.tag == "Bow") {
			PlaySound (4);
			weapon = (GameObject)Instantiate (Resources.Load ("Bow"));
			BowScriptLevel3 bs = weapon.GetComponent<BowScriptLevel3> ();
			bs.hidebow ();
			weapon.SetActive (true);
			attackname = "bow";
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Goli") {
			PlaySound (4);
			weapon = (GameObject)Instantiate (Resources.Load ("Goli"));
			BowScriptLevel3 bs = weapon.GetComponent<BowScriptLevel3> ();
			bs.hidebow ();
			attackname = "goli";
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Idli") {
			PlaySound(4);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Gun") {
			//print ("Gun");
			PlaySound(4);
			weapon = (GameObject)Instantiate (Resources.Load ("GunLevel3"));
			BowScriptLevel3 bs = weapon.GetComponent<BowScriptLevel3> ();
			bs.hidebow ();
			weapon.SetActive(true);
			attackname = "gun";
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "KillZone") {
			reset();
		}
		else if (other.gameObject.tag == "Mushroom") {
			m_Rigidbody2D.velocity = Vector2.zero;
			hitbool = true;
			maxhitcount = 1;
			multiplyfactor = 2;
		}
		else if (other.gameObject.tag == "EnemyBullet") {
			if(!crouchbool){
				Destroy(other.gameObject);
				bloodSplat(gameObject);
				reset();
			}
		}
		else if (other.gameObject.tag == "Bullet") {
			PlaySound(4);
			BulletNogravity bn = other.gameObject.GetComponent<BulletNogravity>();
			FastBullet fb = other.gameObject.GetComponent<FastBullet>();
			if(bn){
				Destroy(other.gameObject);
				bulletnogravity = true;
			}
			else if(fb){
				Destroy(other.gameObject);
				bulletnogravity = true;
				attackmaxtime = 0.1f;
			}
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
		if (transform.position.x > maxposx) {
			maxposx = transform.position.x;
		}
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
			if (Time.time - previousattacktime > attackmaxtime) {
				previousattacktime = Time.time;
				if(m_FacingRight){
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y+0.5f);
					}
					BowScriptLevel3 bs = weapon.GetComponent<BowScriptLevel3>();
					bs.weaponvelocityx = 16;
					bs.weaponname = attackname;
					bs.attack();
				}
				else{
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y+0.5f);
					}
					BowScriptLevel3 bs = weapon.GetComponent<BowScriptLevel3>();
					bs.weaponvelocityx = -16;
					bs.weaponname = attackname;
					bs.attack();
				}
			}
			else{
				if(attackname == "bow" || attackname == "gun"){
					if(m_FacingRight){
						weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y+0.5f);
					}
					else{
						weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y+0.5f);
					}
				}
			}
		} else {
			if (Time.time - previousattacktime > attackmaxtime) {
				if(attackname == "bow" || attackname == "gun"){
					BowScriptLevel3 bs = weapon.GetComponent<BowScriptLevel3>();
					bs.hidebow();
				}
			}			
			else{
				if(m_FacingRight){
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y+0.5f);
					}
				}
				else{
					if(attackname == "bow" || attackname == "gun"){
						weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y+0.5f);
					}
				}
			}
		}
	}
	
	public void Move(float move, bool crouch, bool jump, bool attack,bool fire,bool henpowerbool,bool devtestbool,float movevertical)
	{
		if (movevertical < 0) {
			crouch = true;
		}
		if (movevertical <= 0) {
			uppressed = false;
		}else if (movevertical > 0) {
			uppressed = true;
		}
		if (!crouch) {
			attackenemy (attack);
		} else {
			attackenemy(false);
		}
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
		crouchbool = crouch;
		
		
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
			PlaySound (0);
		}
		if (!m_Grounded && flying) {
			m_Grounded = false;
			m_Anim.SetBool ("Ground", false);
			m_Anim.SetFloat ("Speed", Mathf.Abs (move));
			
			// Move the character
			//m_Rigidbody2D.velocity = new Vector2 (move * m_MaxSpeed, movevertical * m_MaxSpeed);
			float vx = m_Rigidbody2D.velocity.x;
			float vy = m_Rigidbody2D.velocity.y;
			int mag = 5;
            if(move > 0){
				m_Rigidbody2D.velocity = new Vector2(mag, m_Rigidbody2D.velocity.y);
			}
			else if(move<0){
				m_Rigidbody2D.velocity = new Vector2(-mag, m_Rigidbody2D.velocity.y);
			}
			if(movevertical > 0){
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, mag/2);
			}
			else if(movevertical<0){
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -mag/2);
			}
			//m_Rigidbody2D.AddForce(new Vector2(move*5, movevertical*5));
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
		velocity = new Vector2((transform.position.x-previousposition.x)/Time.deltaTime,(transform.position.y-previousposition.y)/Time.deltaTime); 
		previousposition = transform.position;
		if (hitbool) {
			applyHitForce(hitvelocity);
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
		else if (other.gameObject.tag == "Finish") {
			Application.LoadLevel("Menu");
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
		}
		blood.transform.position = new Vector3 (obj.transform.position.x - 2, 0, obj.transform.position.z);
	}
	
	void LoadNextLevel() {
		
	}
	
	public void PlaySound(int clip)
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
	
	public void encounterreset(){
		GameObject enc = GameObject.FindGameObjectWithTag ("EncounterEnemy");
		EncounterGoonScript es = enc.GetComponent<EncounterGoonScript> ();
		if (enc && es) {
			EncounterGoonScript engs = enc.GetComponent<EncounterGoonScript> ();
			engs.reset ();
		}
	}
	
	public void reset() {
		Camera2DFollow.followbool = false;
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.angularVelocity = 0;
		Vector2 pos = new Vector2 (0, 1);
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
		print (count);
		if (count == 4) {
			GameObject.FindGameObjectWithTag("Hen").GetComponent<FallingPlatformScriptLevel3>().reset();
		}
		if (count == 5) {
			encounterreset();
		}
		if (count == 10) {
			GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
			foreach(GameObject box in boxes){
				IceBoxBombPartScript ibs = box.GetComponent<IceBoxBombPartScript>();
				if(ibs){
					ibs.reset();
				}
			}
			GameObject rst = GameObject.FindGameObjectWithTag("Rabbit");
			BombScript[] allj = rst.GetComponentsInChildren<BombScript>();
			//print(allj.Length);
			rst.GetComponent<ResetScript>().reset();
			/*foreach(BombScript obj in allj){
				print ("obj");
				obj.gameObject.SetActive(true);
				//obj.reset();
			}*/
		}
		transform.position = pos;
		m_Grounded = false;
	}
	
}



//}
