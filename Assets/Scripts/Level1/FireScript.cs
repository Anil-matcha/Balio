using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {
	Rigidbody2D rb;
	Vector3 velocity = new Vector3(20,0,0);
	float starttime;
	int startindex = 0;
	GameObject player;
	// Use this for initialization
	void Start () {
		starttime = Time.time;
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = transform.position + Time.deltaTime * velocity;
		if (Time.time - starttime > 10) {
			Destroy (gameObject);
		} else {
			if(startindex<10){
				rb.AddForce(rb.velocity*20);
				startindex++;
			}
		}
	}

	public void bloodSplat(Collider2D other){
		GameObject bloodsplat = (GameObject)Instantiate (Resources.Load ("bloodsplatforfloor"));
		//print ("characterpos=" + transform.position);
		//print ("otherpos=" + other.transform.position);
		bloodsplat.transform.position = transform.position;
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

	void OnTriggerEnter2D(Collider2D other) {
		//print (other.gameObject.tag);
		if (other.gameObject.tag == "Car") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			//other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100000, 0));
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Platform") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Wall") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Box") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			Destroy (gameObject);
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Goon") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			bloodSplat (other);
			Destroy (gameObject);
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Villain") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			bloodSplat (other);
			Destroy (gameObject);
			damagevillain();
		}
		else if (other.gameObject.tag == "EnemyFire") {
			GameObject fireblast = (GameObject)Instantiate (Resources.Load ("Explosion"));
			fireblast.transform.position = transform.position;
			Destroy (gameObject);
			Destroy(other.gameObject);
		}
	}

	public void damagevillain(){
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			PlatformerCharacter2D p2d = player.GetComponent<PlatformerCharacter2D> ();
			p2d.enemyhealth = p2d.enemyhealth - 20;
			p2d.enemyhealthSlider.value = p2d.enemyhealth;
			GameObject villain = GameObject.FindGameObjectWithTag ("Villain");
			if (p2d.enemyhealth == 0) {
				Destroy (villain);
				p2d.playagainbutton.gameObject.SetActive(true);
				Destroy(player);
			} else {
				FinalEnemyScript fes = villain.GetComponent<FinalEnemyScript> ();
				fes.previousfiretime = 0;
			}
		}
	}
}
