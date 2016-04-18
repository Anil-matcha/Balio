using UnityEngine;
using System.Collections;

public class FinalEnemyScript : MonoBehaviour {

	
	public float startposition = 51;
	public float endposition = 65;
	public bool facing_right = true;
	public float speedx = 0;
	float timer;
	private Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	public float previousfiretime=0;
	public float previoushittime=0;
	public float newenemyprevioustime=0;
	GameObject player;
	public float firerate = 100f;
	public float newenemyrate = 20f;
	public float viewdistance = 35f;
	ArrayList newenemylist = new ArrayList();
	public int newenemycount = 3;
	// Use this for initialization
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
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - newenemyprevioustime > newenemyrate && newenemylist.Count < newenemycount) {
			if (player.transform.position.x > startposition - viewdistance && player.transform.position.x < endposition + viewdistance && Mathf.Abs (player.transform.position.y - transform.position.y) < 2) {
				int choose = UnityEngine.Random.Range(1,7);
				bool fireenemy = false;
				if(choose<=2){
					fireenemy = true;
				}
				else{
					fireenemy = false;
				}
				if(!fireenemy){
					GameObject enemy = (GameObject)Instantiate (Resources.Load ("Enemy"));
					GoonScript gs = enemy.GetComponent<GoonScript>();
					gs.startposition = 420f;
					gs.endposition = 455f;
					//gs.facing_right = false;
					gs.transform.position = new Vector2(transform.position.x-1,transform.position.y-1);
					gs.speedx = -5;
					//gs.m_Anim.SetFloat("Speed",-5);
					newenemyprevioustime = Time.time;
					newenemylist.Add(enemy);
				}
				else{
					GameObject enemy = (GameObject)Instantiate (Resources.Load ("FireEnemy"));
					FireGoon fgs = enemy.GetComponent<FireGoon>();
					fgs.startposition = 420f;
					fgs.endposition = 455f;
					//gs.facing_right = false;
					fgs.transform.position = new Vector2(transform.position.x-1,transform.position.y-1);
					fgs.speedx = 5;
					//gs.m_Anim.SetFloat("Speed",-5);
					newenemyprevioustime = Time.time;
					newenemylist.Add(enemy);
				}
			}
		}

		ArrayList modifiedenemylist = new ArrayList();
		foreach (GameObject obj in newenemylist) {
			if(obj){
				modifiedenemylist.Add(obj);
				//newenemylist.Remove(obj);
			}
		}
		newenemylist = modifiedenemylist;
		//print (newenemylist.Count);
		if (Time.time - previoushittime > 2) {
			if (player.transform.position.x > startposition - viewdistance && player.transform.position.x < endposition + viewdistance && Mathf.Abs (player.transform.position.y - transform.position.y) < 2) {
				if (transform.position.x > player.transform.position.x && facing_right) {
					Flip ();
				} else if (transform.position.x < player.transform.position.x && !facing_right) {
					Flip ();
				} else {
					if (Time.time - previousfiretime > 4) {
						firerate = UnityEngine.Random.Range(2f,6f);
						GameObject enemyfireball = (GameObject)Instantiate (Resources.Load ("finalenemyfireball"));
						if (facing_right) {
							enemyfireball.transform.position = transform.position + new Vector3 (1f, 0f, 0.0f);
							enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (30, 0));
						} else {
							enemyfireball.transform.position = transform.position + new Vector3 (-1f, 0f, 0.0f);
							enemyfireball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-30, 0));
						}
						previousfiretime = Time.time;
					}
				}
			}
		}
		if (facing_right) {
			if(transform.position.x < endposition){
				m_Rigidbody2D.velocity = new Vector2 (speedx, 0);
			}
			else{
				Flip ();
				//facing_right = false;
				m_Rigidbody2D.velocity = new Vector2 (0, 0);
			}
		} else {
			if(transform.position.x > startposition){
				m_Rigidbody2D.velocity = new Vector2 (-speedx, 0);
			}
			else{
				Flip ();
				//facing_right = true;
				m_Rigidbody2D.velocity = new Vector2 (0, 0);
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
			Flip ();
			previoushittime = Time.time;
		}
	}

}
