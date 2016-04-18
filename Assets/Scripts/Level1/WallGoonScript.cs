using UnityEngine;
using System.Collections;

public class WallGoonScript : MonoBehaviour {
	
	public float startposition = 51;
	public float endposition = 65;
	public bool facing_right = true;
	public float speedx = 0;
	float timer;
	private Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	private float previousfiretime=0;
	public GameObject wall;
	public BoxCollider2D wallcollider;
	public float offset = 1f;
	GameObject player;
	// Use this for initialization
	void Start () {
		timer = Time.time;
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		m_Anim.SetFloat("Speed", speedx);
		wallcollider = wall.GetComponentsInChildren<BoxCollider2D> ()[0];
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > endposition) {
			transform.position = new Vector2(endposition-0.1f,transform.position.y);
		}
		if (transform.position.x < startposition) {
			transform.position = new Vector2(endposition+0.1f,transform.position.y);
		}
		if (player) {
			if (player.transform.position.x > startposition - 10 && player.transform.position.x < endposition + 10 && Mathf.Abs (player.transform.position.y - transform.position.y) < 20) {
				if (transform.position.x > player.transform.position.x && facing_right) {
					Flip ();
				} else if (transform.position.x < player.transform.position.x && !facing_right) {
					Flip ();
				} else {
					m_Anim.SetFloat ("Speed", 0);
					speedx = 0;
					//print ("Zone3");
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
			} else {
				m_Anim.SetFloat ("Speed", 0);
				speedx = 0;
			}
		}
		float posx = transform.position.x;
		float posy = wall.transform.position.y+wallcollider.bounds.size.y+offset;
		transform.position = new Vector2(posx,posy);
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
		
	}
	

}
