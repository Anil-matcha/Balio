using UnityEngine;
using System.Collections;

public class TrainEnemyScript : MonoBehaviour {

	public float speedx = 3;
	public Animator m_Anim;            // Reference to the player's animator component.
	public bool facing_right = false;
	Rigidbody2D m_Rigidbody2D;
	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		m_Anim.SetFloat("Speed", speedx);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2 (transform.position.x - speedx*Time.deltaTime,transform.position.y);

	}

	void OnCollisionEnter2D(Collision2D other) {

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
}
