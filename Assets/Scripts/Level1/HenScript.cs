using UnityEngine;
using System.Collections;

public class HenScript : MonoBehaviour {

	public float speedx = 15;
	public float speedy = 10;
	public bool jump = false;
	public int count =0;
	public int maxcount =10;
	private Rigidbody2D m_Rigidbody2D;
	[SerializeField] public float m_JumpForce = 200f;                  // Amount of force added when the player jumps.
	private Animator anim;            // Reference to the player's animator component.
	// Use this for initialization
	public float goonhitprevioustime;
	public float wallhitprevioustime;
	public float destroystartx = 375;
	public float destroyendx = 416;
	void Start () {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		anim.SetFloat ("speed",speedx);
		jump = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (jump) {
			/*
			print ("jump");
			if (count < maxcount) {
				count++;
				Vector2 pos = transform.position;
				pos.x = pos.x + speedx * Time.deltaTime;
				pos.y = pos.y + speedy * Time.deltaTime;
				transform.position = pos;
			} else {
				jump = false;
			}
			*/
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			jump = false;
		} else {
			Vector2 pos = transform.position;
			pos.x = pos.x + speedx * Time.deltaTime;
			transform.position = pos;
		}
	}

	public void bloodSplat(GameObject other){
		GameObject bloodsplat = (GameObject)Instantiate (Resources.Load ("bloodsplatforfloor"));
		//print ("characterpos=" + transform.position);
		//print ("otherpos=" + other.transform.position);
		bloodsplat.transform.position = other.transform.position;
		//m_Rigidbody2D.AddForce (transform.right * m_Rigidbody2D.velocity.x * -100);
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

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.transform.position.x > destroystartx && other.gameObject.transform.position.x < destroyendx){
			ArrayList todestroy = new ArrayList();
			GameObject[] allgoon = GameObject.FindGameObjectsWithTag ("Goon");
			for (int i = 0; i<allgoon.Length; i++) {
				if(allgoon[i].transform.position.x >= destroystartx && allgoon[i].transform.position.x <= destroyendx){
					todestroy.Add(allgoon[i]);
				}
			}
			foreach(GameObject obj in todestroy){
				GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
				fireblast.transform.position = obj.transform.position;
				bloodSplat(obj);
				Destroy(obj);
			}
			Destroy(gameObject);
		}
		else{
			if (other.gameObject.tag == "Goon") {
				if(Time.time - goonhitprevioustime > 0.5f){
					print("Goonhit");
					jump = true;
					m_JumpForce = 800;
					m_Rigidbody2D.AddForce(new Vector2(-50f, 0));
					count = 0;
					goonhitprevioustime = Time.time;
				}
			}
			if (other.gameObject.tag == "Wall") {
				if(Time.time - wallhitprevioustime > 0.5f){
					//print("Wallhit");


					jump = true;
					m_JumpForce = 1200;
					m_Rigidbody2D.AddForce(new Vector2(-50f, 0));
					count = 0;

				}
			}

			if (other.gameObject.tag == "Player") {
				Destroy(gameObject);
				GameObject player = GameObject.FindGameObjectWithTag ("Player");
				PlatformerCharacter2D p2d = player.GetComponent<PlatformerCharacter2D>();
				p2d.henpowercount++;
				GameObject henPower = p2d.henpower;
				henPower.SetActive(true);
			}
		}
	}

}
