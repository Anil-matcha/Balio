using UnityEngine;
using System.Collections;

public class EnemyFireScript : MonoBehaviour {
	Rigidbody2D rb;
	Vector3 velocity = new Vector3(20,0,0);
	float starttime;
	int startindex = 0;
	public bool finalenemy = false;
	// Use this for initialization
	void Start () {
		starttime = Time.time;
		rb = GetComponent<Rigidbody2D> ();
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
		/*
		if (other.gameObject.tag == "Player") {
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			PlatformerCharacter2D p2d = player.GetComponent<PlatformerCharacter2D>();
			p2d.bloodSplat(player);
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
		*/
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		//print (other.gameObject.tag);
		if (other.gameObject.tag == "Car") {
			GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
			fireblast.transform.position = transform.position;
			//other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100000, 0));
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Platform") {
			GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
			fireblast.transform.position = transform.position;
			Destroy(gameObject);
		}
		else if (other.gameObject.tag == "Box") {
			GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
			fireblast.transform.position = transform.position;
			Destroy(gameObject);
			Destroy(other.gameObject);
		}
		/*
		else if (other.gameObject.tag == "Player") {
			if(!finalenemy){
				GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
				fireblast.transform.position = transform.position;
			}
			else{
				GameObject fireblast = (GameObject)Instantiate(Resources.Load("Exploson10"));
				fireblast.transform.position = transform.position;
			}
			bloodSplat(other);
			Destroy(gameObject);
		}
		*/
	}
	
}
