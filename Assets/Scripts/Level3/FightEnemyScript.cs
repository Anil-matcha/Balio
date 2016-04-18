using UnityEngine;
using System.Collections;

public class FightEnemyScript : MonoBehaviour {
	
	public bool facing_right = true;
	public Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	GameObject player;
	public float flipstarttime = 0;
	public float attackprevioustime;
	GameObject weapon;
	public bool start = false;
	public float crouchprevioustime;
	public bool jump;
	public Vector2 startpos;
	public int rangemax = 3;
	public bool crouch = false;
	public float shoottime = 4f;
	public float crouchtime = 3f;
	public float viewrange = 20f;
	public float attackmaxtime = 0.5f;
	public float waitbeforeattack = 2f;
	public float waitafterattack = 1f;
	public bool test = false;
	PlayerLevel3 pl3;
	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		m_Anim.SetFloat("Speed", 0);
		player = GameObject.FindGameObjectWithTag ("Player");
		pl3 = player.GetComponent<PlayerLevel3> ();
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
		Vector2 fwdpos = new Vector2(transform.position.x+0.5f,transform.position.y+0.5f);
		Vector2 bwdpos = new Vector2(transform.position.x-0.5f,transform.position.y+0.5f);
		RaycastHit2D hitfwd = Physics2D.Raycast(fwdpos, fwd, viewrange);  
		RaycastHit2D hitbwd = Physics2D.Raycast(bwdpos, bwd, viewrange);
		RaycastHit2D hitfwdcls = Physics2D.Raycast(fwdpos, fwd, 8f);  
		RaycastHit2D hitbwdcls = Physics2D.Raycast(bwdpos, bwd, 8f);
		if (hitfwd.collider != null) { 
			if (hitfwd.collider.gameObject.tag == "Player") {
				if(Time.time - attackprevioustime > shoottime && !pl3.crouchbool){
					if(!start){
						start = true;
						m_Rigidbody2D.velocity = new Vector2(0,0);
					}
					attackprevioustime = Time.time;
					StartCoroutine(attack(true));
				}
			}
		}
		if (hitfwdcls.collider != null) { 
			if (hitfwdcls.collider.gameObject.tag == "Bullet") {
				if(Time.time - crouchprevioustime > crouchtime){
					jump = true;
					crouchprevioustime = Time.time;
					StartCoroutine(enemycrouch(true));
				}
			}
		}
		if (hitbwd.collider != null) { 
			if (hitbwd.collider.gameObject.tag == "Player") {
				if(Time.time - attackprevioustime > shoottime && !pl3.crouchbool){
					if(!start){
						start = true;
						m_Rigidbody2D.velocity = new Vector2(0,0);
					}
					attackprevioustime = Time.time;
					StartCoroutine(attack(false));
				}
			}
		}
		if (hitbwdcls.collider != null) { 
			if (hitbwdcls.collider.gameObject.tag == "Bullet") {
				print("bullet");
				if(Time.time - crouchprevioustime > crouchtime){
					jump = true;
					crouchprevioustime = Time.time;
					StartCoroutine(enemycrouch(false));
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
		if (weapon) {
			EnemyWeaponScript ews = weapon.GetComponent<EnemyWeaponScript> ();
			//ews.setplayer (gameObject);
			if (ews.spriterenderer.enabled) {
				if (facing_right) {
					weapon.transform.position = new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.3f);
				} else {
					weapon.transform.position = new Vector2 (transform.position.x - 0.5f, transform.position.y + 0.3f);
				}
			}
		}
	}
	
	public void attackenemy(){
		
		if (Time.time - attackprevioustime > attackmaxtime) {
			if(facing_right){
				weapon.transform.position = new Vector2(transform.position.x+0.5f,transform.position.y);
				EnemyWeaponScript bs = weapon.GetComponent<EnemyWeaponScript>();
				bs.weaponvelocity = 15;
				bs.weaponname = "gun";
				bs.attack();
			}
			else{
				weapon.transform.position = new Vector2(transform.position.x-0.5f,transform.position.y);
				EnemyWeaponScript bs = weapon.GetComponent<EnemyWeaponScript>();
				bs.weaponvelocity = -15;
				bs.weaponname = "gun";
				bs.attack();
			}
		}
		
	}
	
	
	public IEnumerator attack(bool front){
		if (weapon && !crouch) {
			EnemyWeaponScript ews = weapon.GetComponent<EnemyWeaponScript> ();
			ews.spriterenderer.enabled = true;
			//int val = UnityEngine.Random.Range (1, 100);
			bool flipped = false;
			if ((front && !facing_right) || (!front && facing_right)) {
				flipped = true;
				Flip ();
				if (facing_right) {
					weapon.transform.position = new Vector2 (transform.position.x + 0.5f, transform.position.y+0.3f);
				} else {
					weapon.transform.position = new Vector2 (transform.position.x - 0.5f, transform.position.y+0.3f);
				}
			}
			yield return new WaitForSeconds(waitbeforeattack);
			attackenemy ();
			yield return new WaitForSeconds(waitafterattack);
			if (flipped) {
				Flip ();
			}
		}

	}

	public IEnumerator enemycrouch(bool front){
		//if (facing_right && front) {
			EnemyWeaponScript ews = weapon.GetComponent<EnemyWeaponScript> ();
			ews.spriterenderer.enabled = true;
			m_Rigidbody2D.velocity = Vector2.zero;
			//int val = UnityEngine.Random.Range (1, 100);
			m_Anim.SetBool ("Crouch", true);
			ews.hidebow ();
			ews.spriterenderer.enabled = false;
			crouch = true;
			yield return new WaitForSeconds (3f);
			crouch = false;
			m_Anim.SetBool ("Crouch", false);
			ews.spriterenderer.enabled = true;

		//}
		
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facing_right = !facing_right;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		/*if (weapon) {
			Vector3 theScale1 = weapon.transform.localScale;
			theScale1.x *= -1;
			weapon.transform.localScale = theScale1;
		}*/
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
			if(!crouch){
				pl3.PlaySound(6);
				bloodSplat(gameObject);
				Destroy(other.gameObject);
				Destroy(gameObject);
				Destroy(weapon);
			}

		}
	}
	
}