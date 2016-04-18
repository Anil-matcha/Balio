using UnityEngine;
using System.Collections;

public class BowScript : MonoBehaviour {

	public float weaponvelocitx = 10;
	public GameObject weapon;
	public float facing_right;
	public SpriteRenderer spriterenderer;
	public string weaponname = "none";
	GameObject player;
	PlayerLevel2 pl2;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pl2 = player.GetComponent<PlayerLevel2> ();
	}

	public void attack(){
		if (weaponname == "bow") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			weapon = (GameObject)Instantiate (Resources.Load ("arrow"));
		} 
		else if (weaponname == "trident") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			weapon = (GameObject)Instantiate (Resources.Load ("tridentweapon"));
			Transform ts = weapon.GetComponent<Transform>();
			Vector3 arrowscale = ts.localScale;
			arrowscale.x *= -1;
			ts.localScale = arrowscale;
		}
		else if (weaponname == "gun") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			weapon = (GameObject)Instantiate (Resources.Load ("bullet"));
		} else {
			spriterenderer.enabled = false;
		}
		if (pl2.m_FacingRight) {
			weapon.transform.position = new Vector2 (player.transform.position.x + 0.5f, player.transform.position.y);
		} else {
			weapon.transform.position = new Vector2 (player.transform.position.x - 0.5f, player.transform.position.y);
		}
		if (weaponvelocitx < 0) {
			Flip ();
		}
	}

	public void hidebow(){
		//if (weaponname == "bow") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = false;
		//}

	}

	// Update is called once per frame
	void Update () {
		if (weapon) {
			weapon.transform.position = new Vector2(weapon.transform.position.x + weaponvelocitx*Time.deltaTime,weapon.transform.position.y);
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		if (weapon) {
			Transform ts = weapon.GetComponent<Transform>();
			Vector3 arrowscale = ts.localScale;
			arrowscale.x *= -1;
			ts.localScale = arrowscale;
		}

	}
}
