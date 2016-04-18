using UnityEngine;
using System.Collections;

public class GoonScript : MonoBehaviour {

	public float startposition = 51;
	public float endposition = 65;
	public bool facing_right = true;
	float timer;
	public Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	public float speedx = 2f;
	public float minspeedx = 1f;
	public float maxspeedx = 3f;
	GameObject player;
	public float delta = 0.2f;
	public float deltay = 2f;
	public bool collisiontest = false;
	public float flipstarttime = 0;
	// Use this for initialization
	void Start () {
		timer = Time.time;
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		speedx = UnityEngine.Random.Range (minspeedx,maxspeedx);
		m_Anim.SetFloat("Speed", speedx);
		if (speedx < 0) {
			if(Time.time - flipstarttime > 1){
				flipstarttime = Time.time;
				Flip();
			}
		}
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		if(player){
			if(Mathf.Abs (player.transform.position.x - transform.position.x) < delta && Mathf.Abs (player.transform.position.y - transform.position.y) < deltay){
				damageplayer();
			}
		    if (facing_right) {
				if(transform.position.x < endposition){
					transform.position = new Vector2(transform.position.x + speedx*Time.deltaTime,transform.position.y);
					//m_Rigidbody2D.velocity = new Vector2 (speedx, 0);
				}
				else{
					if(Time.time - flipstarttime > 1){
						Flip ();
						flipstarttime = Time.time;
					}
					//facing_right = false;
					//m_Rigidbody2D.velocity = new Vector2 (speedx, 0);
				}
			} else {
				if(transform.position.x > startposition){
					transform.position = new Vector2(transform.position.x - speedx*Time.deltaTime,transform.position.y);
					//m_Rigidbody2D.velocity = new Vector2 (-speedx, 0);
				}
				else{
					if(Time.time - flipstarttime > 1){
						Flip ();
						flipstarttime = Time.time;
					}
					//facing_right = true;
					//m_Rigidbody2D.velocity = new Vector2 (-speedx, 0);
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

	void OnCollisionEnter2D(Collision2D other) {
		if (collisiontest) {
			print (other.gameObject.tag);
		}
		GameObject Car = GameObject.FindGameObjectWithTag ("Car");
		if (Car) {
			CarScript cs = Car.GetComponent<CarScript> ();
			if (other.gameObject.tag == "Car") {
				if (transform.position.y < cs.position.y) {
					fliponhitbycar ();
				}
				other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			}
		}
		if (other.gameObject.tag == "Goon") {
			if(Time.time - flipstarttime > 1){
				Flip ();
				flipstarttime = Time.time;
			}
			//m_Rigidbody2D.velocity = new Vector2 (0, 0);
		}
		if (other.gameObject.tag == "Wall") {

			if(collisiontest){
			 print("Wall");
			}
			if(Time.time - flipstarttime > 1){
				Flip ();
				flipstarttime = Time.time;
				if(collisiontest){

				}
			}
			//m_Rigidbody2D.velocity = new Vector2 (0, 0);
		} if (other.gameObject.tag == "Player") {
			//print("Wall");
			if(Time.time - flipstarttime > 1){
				Flip ();
				flipstarttime = Time.time;
			}
			//m_Rigidbody2D.velocity = new Vector2 (0, 0);
		} else {
			if(Time.time - flipstarttime > 1){
				Flip ();
				flipstarttime = Time.time;
			}
		}
	}
    
	public void fliponhitbycar(){
		if (Time.time - flipstarttime > 1) {
			flipstarttime = Time.time;
			Flip ();
		}
	}

	public void damageplayer(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			PlatformerCharacter2D p2d = player.GetComponent<PlatformerCharacter2D> ();
			if (Time.time - p2d.goonhitprevioustime > 0.5) {
				int damage = 5;
				p2d.bloodSplat (player);
				HealthManager hm = player.GetComponent<HealthManager> ();
				if (hm.health - damage > 0) {
					p2d.damagebygoon (damage);
					p2d.goonhitvelocity = m_Rigidbody2D.velocity * 20;
					p2d.goonhitbool = true;
					p2d.applyGoonHitForce (p2d.goonhitvelocity);
				} else {
					p2d.damagebygoon (hm.health);
					p2d.goonhitbool = false;
					p2d.goonhitvelocity = new Vector2 (0, 0);
				}
				p2d.goonhitprevioustime = Time.time;
			}
		}
	}

}
