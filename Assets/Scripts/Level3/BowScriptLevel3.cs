using UnityEngine;
using System.Collections;

public class BowScriptLevel3 : MonoBehaviour {
	
	public float weaponvelocityx = -16;
	public GameObject weapon;
	public bool facing_right = true;
	public SpriteRenderer spriterenderer;
	public string weaponname = "none";
	GameObject player;
	PlayerLevel3 pl3;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pl3 = player.GetComponent<PlayerLevel3> ();
	}
	
	public void attack(){
		if (weaponname == "bow") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			weapon = (GameObject)Instantiate (Resources.Load ("arrow"));
			if (pl3.m_FacingRight) {
				weapon.transform.position = new Vector2 (player.transform.position.x + 0.5f, player.transform.position.y+0.5f);
			} else {
				weapon.transform.position = new Vector2 (player.transform.position.x - 0.5f, player.transform.position.y+0.5f);
			}

		} 
		else if (weaponname == "trident") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			weapon = (GameObject)Instantiate (Resources.Load ("tridentweapon"));
			Transform ts = weapon.GetComponent<Transform>();
			Vector3 arrowscale = ts.localScale;
			arrowscale.x *= -1;
			ts.localScale = arrowscale;
			if (pl3.m_FacingRight) {
				weapon.transform.position = new Vector2 (player.transform.position.x + 0.5f, player.transform.position.y+0.5f);
			} else {
				weapon.transform.position = new Vector2 (player.transform.position.x - 0.5f, player.transform.position.y+0.5f);
			}

		}
		else if(weaponname == "goli"){
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			weapon = (GameObject)Instantiate (Resources.Load ("goliweapon"));
			GoliScript gs = weapon.GetComponent<GoliScript>();
			pl3.PlaySound(3);
			if(pl3.uppressed){
				gs.speedx = 0;
				gs.speedy = 10;
				weapon.transform.position = new Vector2 (player.transform.position.x, player.transform.position.y+1f);
			}
			else{
				if(pl3.m_FacingRight){
					gs.speedx = 10;
					weapon.transform.position = new Vector2 (player.transform.position.x + 0.5f, player.transform.position.y+0.5f);
				}
				else{
					gs.speedx = -10;
					weapon.transform.position = new Vector2 (player.transform.position.x - 0.5f, player.transform.position.y+0.5f);
				}
			}

		}
		else if (weaponname == "gun") {
			spriterenderer = GetComponent<SpriteRenderer> ();
			spriterenderer.enabled = true;
			pl3.PlaySound(2);
			if(!pl3.bulletnogravity){
			 weapon = (GameObject)Instantiate (Resources.Load ("bulletlevel3"));
			}
			else{
				weapon = (GameObject)Instantiate (Resources.Load ("bulletnogravity"));
			}
			Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
			if(pl3.m_FacingRight){
				rb.velocity = new Vector2(Mathf.Abs(weaponvelocityx),0);
			}
			else{
				rb.velocity = new Vector2(weaponvelocityx,0);
			}
			if (pl3.m_FacingRight) {
				weapon.transform.position = new Vector2 (player.transform.position.x + 0.5f, player.transform.position.y+0.5f);
			} else {
				weapon.transform.position = new Vector2 (player.transform.position.x - 0.5f, player.transform.position.y+0.5f);
			}

		} else {
			spriterenderer.enabled = false;
		}
		if (facing_right && pl3.m_FacingRight) {

		} 
		else if (!facing_right && !pl3.m_FacingRight) {
			
		}
		else{
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
		facing_right = !facing_right;
	}
}
