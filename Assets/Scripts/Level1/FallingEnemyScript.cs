using UnityEngine;
using System.Collections;

public class FallingEnemyScript : MonoBehaviour {

	bool facing_right = false;
	private Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	public float startposition = 102;
	public float endposition = 170;
	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	const float k_CeilingRadius = .01f; 
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	// Use this for initialization
	void Start () {
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", false);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		m_Anim.SetFloat("Speed", 10);
		m_GroundCheck = transform.Find("GroundCheck");
		m_CeilingCheck = transform.Find("CeilingCheck");
	}
	
	// Update is called once per frame
	void Update () {
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}
		m_Anim.SetBool("Ground", m_Grounded);
		if (facing_right) {
			if(transform.position.x < endposition){
				m_Rigidbody2D.velocity = new Vector2 (10, 0);
			}
			else{
				Flip ();
				//facing_right = false;
				m_Rigidbody2D.velocity = new Vector2 (10, 0);
			}
		} else {
			if(transform.position.x > startposition){
				m_Rigidbody2D.velocity = new Vector2 (-10, 0);
			}
			else{
				Flip ();
				//facing_right = true;
				m_Rigidbody2D.velocity = new Vector2 (-10, 0);
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

}
