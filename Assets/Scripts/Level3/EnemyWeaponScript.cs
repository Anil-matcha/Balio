using UnityEngine;
using System.Collections;

public class EnemyWeaponScript : MonoBehaviour {

	public GameObject weapon;
	public bool facing_right = true;
	public SpriteRenderer spriterenderer;
	public string weaponname = "none";
	GameObject player;
	EncounterGoonScript pl3;
	FightEnemyScript fe3;
	PoliceEnemyScript pe3;
	public float weaponvelocity = -16;
	public int id;
	// Use this for initialization
	void Start () {
		spriterenderer = GetComponent<SpriteRenderer> ();
	}

	public void setplayer(GameObject p){
		player = p;
		pl3 = player.GetComponent<EncounterGoonScript> ();
		fe3 = player.GetComponent<FightEnemyScript> ();
		pe3 = player.GetComponent<PoliceEnemyScript> ();
	}

	public void attack(){
		if (player) {
			if (weaponname == "gun") {
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevel3>().PlaySound(3);
				spriterenderer.enabled = true;
				if(pl3){
					weapon = (GameObject)Instantiate (Resources.Load ("bulletenemy"));
				}
				else if(fe3){
					weapon = (GameObject)Instantiate (Resources.Load ("enemybulletnogravity"));
				}
				else if(pe3){
					weapon = (GameObject)Instantiate (Resources.Load ("enemybulletnogravity"));
					BulletNogravity bnf = weapon.GetComponent<BulletNogravity>();
					bnf.id = id;
				}
				Rigidbody2D rb = weapon.GetComponent<Rigidbody2D> ();
				if (facing_right) {
					rb.velocity = new Vector2 (Mathf.Abs(weaponvelocity), 0);
					weapon.transform.position = new Vector2 (player.transform.position.x + 0.5f, player.transform.position.y+0.5f);
				} else {
					rb.velocity = new Vector2 (weaponvelocity, 0);
					weapon.transform.position = new Vector2 (player.transform.position.x - 0.5f, player.transform.position.y+0.5f);
				}
			}
			else {
				spriterenderer.enabled = false;
			}
		}
	}
	
	public void hidebow(){
		spriterenderer = GetComponent<SpriteRenderer> ();
		spriterenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		flipplayer ();
	}

	public void flipplayer(){
		if(pl3){
			if (facing_right && pl3.facing_right) {
				
			} else if (!facing_right && !pl3.facing_right) {
				
			} else {
				Flip ();
			}
		}
		else if(fe3){
			if (facing_right && fe3.facing_right) {
				
			} else if (!facing_right && !fe3.facing_right) {
				
			} else {
				Flip ();
			}
		}
		else if(pe3){
			if (facing_right && pe3.facing_right) {
				
			} else if (!facing_right && !pe3.facing_right) {
				
			} else {
				Flip ();
			}
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
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
