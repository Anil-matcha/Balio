using UnityEngine;
using System.Collections;

public class EncounterGoonScript : MonoBehaviour {

	public bool facing_right = true;
	public Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	GameObject player;
	public float flipstarttime = 0;
	public float attackprevioustime;
	GameObject weapon;
	public bool start = false;
	public float jumpprevioustime;
	public bool jump;
	public Vector2 startpos;
	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		m_Anim.SetFloat("Speed", 0);
		player = GameObject.FindGameObjectWithTag ("Player");
		m_Rigidbody2D.velocity = new Vector2 (0, 0);
		weapon = (GameObject)Instantiate (Resources.Load ("GunEnemy"));
		EnemyWeaponScript bs = weapon.GetComponent<EnemyWeaponScript>();
		bs.setplayer (gameObject);
		bs.hidebow();
		startpos = transform.position;
	}

	public void reset(){
		transform.position = startpos;
		start = false;
		m_Rigidbody2D.velocity = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
		Vector2 fwd;
		Vector2 bwd;
		fwd = transform.TransformDirection(Vector2.right);
		bwd = transform.TransformDirection(Vector2.left);
		Vector2 fwdpos = new Vector2(transform.position.x+0.5f,transform.position.y);
		Vector2 bwdpos = new Vector2(transform.position.x-0.5f,transform.position.y);
		RaycastHit2D hitfwd = Physics2D.Raycast(fwdpos, fwd, 8f);  
		RaycastHit2D hitbwd = Physics2D.Raycast(bwdpos, bwd, 8f);
		RaycastHit2D hitfwdcls = Physics2D.Raycast(fwdpos, fwd, 4f);  
		RaycastHit2D hitbwdcls = Physics2D.Raycast(bwdpos, bwd, 4f);
		if (hitfwd.collider != null) {
			if (hitfwd.collider.gameObject.tag == "Player") {
				if (Time.time - attackprevioustime > 3f) {
					if (!start) {
						start = true;
						m_Rigidbody2D.velocity = new Vector2 (0, 0);
					}
					StartCoroutine (attack (m_Rigidbody2D.velocity, true));
				}
			}
		}
		if (hitfwdcls.collider != null) { 
			if (hitfwdcls.collider.gameObject.tag == "Bullet") {
				if(Time.time - jumpprevioustime > 2f){
					jump = true;
					m_Rigidbody2D.velocity = Vector2.zero;
					m_Rigidbody2D.AddForce(new Vector2(0,15),ForceMode2D.Impulse);
					jumpprevioustime = Time.time;
				}
			}
		}
		if (hitbwd.collider != null) { 
			if (hitbwd.collider.gameObject.tag == "Player") {
				if(Time.time - attackprevioustime > 3f){
					if(!start){
						start = true;
						m_Rigidbody2D.velocity = new Vector2(0,0);
					}
					StartCoroutine(attack(m_Rigidbody2D.velocity,false));
				}
			}
		}
		if (hitbwdcls.collider != null) { 
			if (hitbwdcls.collider.gameObject.tag == "Bullet") {
				if(Time.time - jumpprevioustime > 2f){
					jump = true;
					m_Rigidbody2D.velocity = Vector2.zero;
					m_Rigidbody2D.AddForce(new Vector2(0,15),ForceMode2D.Impulse);
					jumpprevioustime = Time.time;
				}
			}
		}
		if (m_Rigidbody2D.velocity.y < 0.5f) {
			jump = false;
		}
		if (start && !jump) {
			if(facing_right){
				m_Rigidbody2D.velocity = new Vector2(0,m_Rigidbody2D.velocity.y);
			}
			else{
				m_Rigidbody2D.velocity = new Vector2(0,m_Rigidbody2D.velocity.y);
			}
		}
		if(Mathf.Abs(m_Rigidbody2D.velocity.y)>10){
			if(m_Rigidbody2D.velocity.y > 0){
				//m_Rigidbody2D.velocity = new Vector2(8,10);
			}
			else{
				//m_Rigidbody2D.velocity = new Vector2(8,-10);
			}
		}
		m_Anim.SetFloat("Speed", m_Rigidbody2D.velocity.x);
		EnemyWeaponScript ews = weapon.GetComponent<EnemyWeaponScript> ();
		if (ews.spriterenderer.enabled) {
			if(facing_right){
				weapon.transform.position = new Vector2 (transform.position.x + 0.5f, transform.position.y);
			}
			else{
				weapon.transform.position = new Vector2 (transform.position.x - 0.5f, transform.position.y);
			}
		}
	}

	public void attackenemy(){

			if (Time.time - attackprevioustime > 0.5) {
				attackprevioustime = Time.time;
				if(facing_right){
					weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y);
					EnemyWeaponScript bs = weapon.GetComponent<EnemyWeaponScript>();
					bs.weaponvelocity = 16;
					bs.weaponname = "gun";
					bs.attack();
				}
				else{
					weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y);
					EnemyWeaponScript bs = weapon.GetComponent<EnemyWeaponScript>();
					bs.weaponvelocity = -16;
					bs.weaponname = "gun";
					bs.attack();
				}
			}
			else{
				if(facing_right){
					weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y);
				}
				else{
					weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y);
				}
			}

		}


	public IEnumerator attack(Vector2 vel,bool front){
		m_Rigidbody2D.velocity = Vector2.zero;
		EnemyWeaponScript ews = weapon.GetComponent<EnemyWeaponScript> ();
		ews.spriterenderer.enabled = true;
		bool flipped = false;
		if ((front && !facing_right) || (!front && facing_right)) {
			flipped = true;
			Flip ();
			if (facing_right) {
				weapon.transform.position = new Vector2 (transform.position.x + 0.5f, transform.position.y);
			} else {
				weapon.transform.position = new Vector2 (transform.position.x - 0.5f, transform.position.y);
			}
		}
		attackenemy ();
		yield return new WaitForSeconds(5f);
		ews.spriterenderer.enabled = false;
		if (flipped) {
			Flip ();
		}
		m_Rigidbody2D.velocity = vel;
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

	void OnCollisionEnter2D(Collision2D other) {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Bullet") {
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevel3>().PlaySound(6);
			bloodSplat(gameObject);
			if(transform.position.x < 325){
				Destroy(other.gameObject);
				transform.position = new Vector2(player.transform.position.x+30,startpos.y);
			}
			else{
				Destroy(gameObject);
				Destroy(weapon);
			}
		}
	}

}
