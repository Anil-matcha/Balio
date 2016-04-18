using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/*
namespace UnityStandardAssets._2D
{
	*/
[RequireComponent(typeof (PlayerLevel2))]
public class PlayerLevel2Control : MonoBehaviour
{
	private PlayerLevel2 m_Character;
	private bool m_Jump;
	public bool devtest = false;
	public bool playercanmove = true;
	private void Awake()
	{
		m_Character = GetComponent<PlayerLevel2>();
	}
	
	
	private void Update()
	{
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			//m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			m_Jump = Input.GetKey(KeyCode.Space);
		}
	}
	
	
	private void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		bool attack = Input.GetKey (KeyCode.X);
		bool fire = Input.GetKey (KeyCode.Z);
		bool henpower = Input.GetKey (KeyCode.S);
		devtest = Input.GetKey (KeyCode.H);
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");
		// Pass all parameters to the character control script.
		//print (m_Jump);
		if (playercanmove) {
			m_Character.Move (h, crouch, m_Jump, attack, fire, henpower, devtest, v);
		}
		m_Jump = false;
		
	}



}
//}
