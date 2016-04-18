using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*namespace UnityStandardAssets._2D
{*/
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        public bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		public float previouslasertime=0;
		public float previousfiretime=0;
		public Slider healthSlider;
		public Vector2 playerpos;
		public int goonhitindex = 0;
		public bool goonhitbool = false;
		public Vector2 goonhitvelocity = new Vector2(0,0);
		public float previousenemyfalltime = 0;
		public int timetogenerate = 5;
		public int hearts = 3;
		public float goonhitprevioustime=0;
		public float killprevioustime=0;
		public float startcount = 0;
		public float maxposx = -22;
		ArrayList checkpoints = new ArrayList();
		public int henpowercount = 0;
		public GameObject henpower;
		public Slider enemyhealthSlider;
		public int enemyhealth = 100;
		public float showenemyhealthposition = 410;
		GameObject enemyslider;
		public Button playagainbutton;
		public bool devtest = false;
		public float devtestchangetime = 0;
		public float deathposy = -20;
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
			checkpoints.Add (new Vector2(-22,-2));
			checkpoints.Add (new Vector2(52.5f,2f));
			checkpoints.Add (new Vector2(84f,4f));
			checkpoints.Add (new Vector2(162,4f));
			checkpoints.Add (new Vector2(192,13.5f));
			checkpoints.Add (new Vector2(275,17f));
			henpower = GameObject.FindGameObjectWithTag ("HenPower");
			if (henpower) {
				henpower.SetActive (false);
			}
			enemyslider = GameObject.FindGameObjectWithTag("EnemyHealthSlider");
			if (enemyslider) {
				enemyslider.SetActive (false);
				playagainbutton.gameObject.SetActive (false);
			}
        }

		void OnTriggerEnter2D(Collider2D other) {
			//print (other.gameObject.tag);
			if (other.gameObject.tag == "Car") {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			//other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100000,0));
			//Destroy(gameObject);
		} else if (other.gameObject.tag == "KillZone") {
			if(Time.time - killprevioustime > 1){
				killprevioustime = Time.time;
				LoadNextLevel();
			}
		}
		else if (other.gameObject.tag == "EnemyFire") {
			//print ("EnemyFire");
			int damage = 25;
			bloodSplat (gameObject);
			HealthManager hm = GetComponent<HealthManager>();
			if(hm.health - damage>0){
				damagebygoon (damage);
				goonhitvelocity = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
				goonhitbool = true;
				applyGoonHitForce (goonhitvelocity);
			}
			else{
				damagebygoon(hm.health);
				goonhitbool = false;
				goonhitvelocity = new Vector2(0,0);
			}
			goonhitprevioustime = Time.time;
			
			
		}
		}

	    public void killplayer(){
			if(Time.time - killprevioustime > 1){
				killprevioustime = Time.time;
				LoadNextLevel();
			}
		}

        private void FixedUpdate()
        {
			if (m_Rigidbody2D.velocity.y < -20) {
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,-20);
			}
            m_Grounded = false;
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject){
					//print (colliders[i].transform.position.y-transform.position.y);
                    m_Grounded = true;
				}
            }
			//print (m_Grounded);
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.isKinematic = false;
        }

	    public void applyGoonHitForce(Vector2 vel){
           if (goonhitindex < 20 && goonhitbool) {
				goonhitindex += 1;
				m_Rigidbody2D.AddForce (vel * 10);
		   } else {
				goonhitbool = false;
				goonhitindex = 0;
		   }
	 	}

        public void Move(float move, bool crouch, bool jump, bool laser,bool fire,bool henpowerbool,bool devtestbool)
        {
            // If crouching, check to see if the character can stand up
			if (devtestbool && Time.time - devtestchangetime > 2) {
				devtestchangetime = Time.time;
				devtest = !devtest;
			}
			//print (devtest);
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
					m_Grounded = true;
                }
            }

			if (laser && startcount>=60) {
				attacklaser();
			}

			if (fire && startcount>=60) {
				attackfire();	
			}

			if (henpowerbool) {
				if(henpowercount > 0){
					henpowercount--;
					GameObject hen = (GameObject)Instantiate (Resources.Load ("Hen"));
					hen.transform.position = new Vector2(transform.position.x,transform.position.y+2f);
					Rigidbody2D rb = hen.GetComponent<Rigidbody2D>();
					rb.AddForce(new Vector2(m_Rigidbody2D.velocity.x*10,m_Rigidbody2D.velocity.y*10));
					henpower.SetActive(false);
				}
			}
			//print ("presentfiretime"+presentfiretime);
			//print ("previousfiretime"+previousfiretime);
            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
				//transform.position = new Vector2(transform.position.x + move*m_MaxSpeed*Time.deltaTime,m_Rigidbody2D.velocity.y*Time.deltaTime);
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }

            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
				
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,0f);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce/50),ForceMode2D.Impulse);
				
            }
			/*
			if(!jump) {
				if(!m_Grounded){
					if(m_Rigidbody2D.velocity.y > 0){
						if(m_Rigidbody2D.velocity.y >3 && m_Rigidbody2D.velocity.y < 20){
							m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,3);
						}
					}
				}
			}
			*/
			playerpos = transform.position;
		    if (goonhitbool) {
			applyGoonHitForce(goonhitvelocity);
			}
			startcar ();
			if (startcount < 60) {
				startcount++;
			}
			showenemyhealth();
			if (transform.position.x > maxposx) {
				maxposx = transform.position.x;
			}
			if (transform.position.y < deathposy) {
				killplayer();
			}
		//print (hearts);
        }


	    public void attackfire(){
			if(Time.time-previousfiretime > 0.5){
				GameObject fireball = (GameObject)Instantiate(Resources.Load("fireball"));
				if(m_FacingRight){
					fireball.transform.position = transform.position+new Vector3(1f,0.5f,0.0f);
					fireball.GetComponent<Rigidbody2D>().AddForce(new Vector2(100,0));
				}
				else{
					fireball.transform.position = transform.position+new Vector3(-1f,0.5f,0.0f);
					fireball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100,0));
				}
				previousfiretime = Time.time;
			}
		}

	    public void attacklaser(){
			if(Time.time-previouslasertime > 0.5){
				GameObject blast = (GameObject)Instantiate(Resources.Load("blast"));
				LineRenderer linerenderer = blast.GetComponent<LineRenderer>();
				if(m_FacingRight){
					blast.transform.position = transform.position+new Vector3(0.5f,1f,0.0f);
					//fire.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000,0));
				}
				else{
					blast.transform.position = transform.position+new Vector3(-0.5f,1f,0.0f);
					//fire.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000,0));
				}
				//print ("attack");
				previouslasertime = Time.time;
			}
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
			if (other.gameObject.tag == "Coin") {
			GameObject player = GameObject.FindGameObjectWithTag ("Coin");
			player.SetActive (false);
		} else if (other.gameObject.tag == "Car") {
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		} else if (other.gameObject.tag == "Goon") {
			if( Mathf.Abs(transform.position.x - other.gameObject.transform.position.x ) <1 && (transform.position.y - other.gameObject.transform.position.y)<1.7 &&  (transform.position.y - other.gameObject.transform.position.y)>1){
				Destroy(other.gameObject);
				bloodSplat (other.gameObject);
			}
			else{
				if(Time.time - goonhitprevioustime > 0.5){
					int damage = 5;
					bloodSplat (other.gameObject);
					HealthManager hm = GetComponent<HealthManager>();
					if(hm.health - damage>0){
						damagebygoon (damage);
						goonhitvelocity = other.gameObject.GetComponent<Rigidbody2D> ().velocity * 20;
						goonhitbool = true;
						applyGoonHitForce (goonhitvelocity);
					}
					else{
						damagebygoon(hm.health);
						goonhitbool = false;
						goonhitvelocity = new Vector2(0,0);
					}
					goonhitprevioustime = Time.time;
				}
			}

		}
		else if (other.gameObject.tag == "Villain") {
			if(Time.time - goonhitprevioustime > 0.5){
				int damage = 15;
				bloodSplat (other.gameObject);
				HealthManager hm = GetComponent<HealthManager>();
				if(hm.health - damage>0){
					damagebygoon (damage);
					goonhitvelocity = other.gameObject.GetComponent<Rigidbody2D> ().velocity * 40;
					goonhitbool = true;
					applyGoonHitForce (goonhitvelocity);
				}
				else{
					damagebygoon(hm.health);
					goonhitbool = false;
					goonhitvelocity = new Vector2(0,0);
				}
				goonhitprevioustime = Time.time;
			}
		}
		else if (other.gameObject.tag == "Block") {
			GameObject hen = (GameObject)Instantiate (Resources.Load ("Hen"));
			hen.transform.position = new Vector2(other.gameObject.transform.position.x,other.gameObject.transform.position.y+2f);
			Rigidbody2D rb = hen.GetComponent<Rigidbody2D>();
			rb.AddForce(new Vector2(m_Rigidbody2D.velocity.x*10,m_Rigidbody2D.velocity.y*10));
			m_Rigidbody2D.AddForce(new Vector2(m_Rigidbody2D.velocity.x*-10,m_Rigidbody2D.velocity.y*-10));
			Destroy(other.gameObject);
		}
		}

	public void startcar(){
		if (transform.position.x > 77 && GameObject.FindGameObjectWithTag ("Car")) {
			GameObject Car = GameObject.FindGameObjectWithTag ("Car");
			CarScript cs = Car.GetComponent<CarScript> ();
			cs.start = true;
			if(Time.time - previousenemyfalltime >timetogenerate && cs.position.y > -2){
				timetogenerate = UnityEngine.Random.Range(3,8);
				GameObject enemy = (GameObject)Instantiate (Resources.Load ("Enemy"));
				enemy.transform.position = cs.position + new Vector2(-2f,2f);
				GoonScript gs = enemy.GetComponent<GoonScript>();
				Destroy(gs);
				enemy.AddComponent<TrainEnemyScript>();
				previousenemyfalltime = Time.time;
			}
		}
	}

	public void showenemyhealth(){
		if (transform.position.x > showenemyhealthposition && GameObject.FindGameObjectWithTag ("Car")) {
			enemyslider.SetActive(true);
		}
	}

	public void bloodSplat(GameObject other){
		GameObject bloodsplat = (GameObject)Instantiate (Resources.Load ("bloodsplatforfloor"));
		//print ("characterpos=" + transform.position);
		//print ("otherpos=" + other.transform.position);
		bloodsplat.transform.position = transform.position;
		m_Rigidbody2D.AddForce (transform.right * m_Rigidbody2D.velocity.x * -100);
		GameObject blood = (GameObject)Instantiate (Resources.Load ("BloodSplat"));
		blood.transform.position = transform.position;
		EllipsoidParticleEmitter[] ps = blood.GetComponentsInChildren<EllipsoidParticleEmitter> ();
		for (int i=0; i<ps.Length; i++) {
			ps [i].Emit ();
			ps [i].emit = false;
			//ps[i].enabled = false;
		}
		blood.transform.position = new Vector3 (other.transform.position.x - 2, 0, other.transform.position.z);
	}

	public void damagebygoon(int damage){
		if (!devtest) {
			HealthManager hm = GetComponent<HealthManager> ();
			if (hm.health <= 0) {
				LoadNextLevel ();
			} else {
				hm.health -= damage;
				//PlayerPrefs.SetInt("health",HealthManager.health);
				healthSlider.value = hm.health;
				if (hm.health <= 0) {
					LoadNextLevel ();
				}
			}
		}
	}

	void LoadNextLevel() {
		Camera2DFollow.followbool = false;
		if (hearts <= 0) {
			Application.LoadLevel ("Menu");
		} else {
			//print (hearts);
			if(!devtest){
				GameObject heart = GameObject.FindGameObjectWithTag ("Heart" + hearts);
				if(heart){
					heart.SetActive(false);
					hearts = hearts-1;
				}
			}
			HealthManager hm = GetComponent<HealthManager>();
			hm.health = 100;
			if(healthSlider){
				healthSlider.value = hm.health;
			}
		}

		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.angularVelocity = 0;
		Vector2 pos = new Vector2 (-22, -2);
		foreach(Vector2 checkpoint in checkpoints) {
			if(checkpoint.x > maxposx){
				break;
			}
			else{
				pos = checkpoint;
			}
		}
		transform.position = pos;
		goonhitbool = false;
		m_Grounded = false;
		GameObject Car = GameObject.FindGameObjectWithTag ("Car");
		if (Car) {
			CarScript cs = Car.GetComponent<CarScript> ();
			cs.reset ();
		}
	}

    }
    


//}
