using UnityEngine;
using System.Collections;

public class GoonScriptLevel2 : MonoBehaviour {
	
	public float startposition = 51;
	public float endposition = 65;
	public bool facing_right = true;
	float timer;
	public Animator m_Anim;            // Reference to the player's animator component.
	Rigidbody2D m_Rigidbody2D;
	public float speedx = 2f;
	public float minspeedx = 1f;
	public float maxspeedx = 3f;
	GameObject player;
	public float delta = 0.2f;
	public float deltay = 2f;
	public bool collisiontest = false;
	public float flipstarttime = 0;
	Transform parentpos;
	public GameObject platform;
	public GameObject[] killtomove;
	public int tridentcount = 0;
	public int tridentmaxcount = 5;
	public bool vertical = false;
	// Use this for initialization
	void Start () {
		timer = Time.time;
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Anim.SetBool ("Ground", true);
		m_Anim.SetBool ("Crouch",false);
		m_Anim.SetFloat ("vSpeed", 0.0f);
		speedx = 0;
		m_Anim.SetFloat("Speed", speedx);
		if (speedx < 0) {
			if(Time.time - flipstarttime > 1){
				flipstarttime = Time.time;
				Flip();
			}
		}
		player = GameObject.FindGameObjectWithTag ("Player");
		parentpos = gameObject.GetComponentInParent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.x > transform.position.x) {
			if (!facing_right) {
				Flip ();
			}
		} else {
			if (facing_right) {
				Flip ();
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
		if (other.gameObject.tag == "Arrow") {
			PlayerLevel2 pl2 = player.GetComponent<PlayerLevel2>();
			bloodSplat(gameObject);
			if(vertical){
				for(int i=0;i<killtomove.Length;i++){
					GameObject obj = killtomove[i];
					VerticalPlatformScript hl2 = obj.GetComponent<VerticalPlatformScript>();
					hl2.movestart = true;
				}
				pl2.attackname = "none";
				other.gameObject.SetActive(false);
			}
			else{
				for(int i=0;i<killtomove.Length;i++){
					GameObject obj = killtomove[i];
					HorizontalMovementScriptLevel2 hl2 = obj.GetComponent<HorizontalMovementScriptLevel2>();
					hl2.movestart = true;
				}
				pl2.attackname = "none";
				Destroy(other.gameObject);
			}
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "Bullet") {
			PlayerLevel2 pl2 = player.GetComponent<PlayerLevel2>();
			PlayerLevel2Control pl2c = player.GetComponent<PlayerLevel2Control>();
			bloodSplat(gameObject);
			pl2.m_Rigidbody2D.velocity = Vector2.zero;
			pl2c.playercanmove = false;
			Destroy(other.gameObject);
			StartCoroutine(LoadNextLevel());
		}
		if (other.gameObject.tag == "Trident") {
			PlayerLevel2 pl2 = player.GetComponent<PlayerLevel2>();
			bloodSplat(gameObject);

			if(tridentcount < tridentmaxcount ){
				tridentcount++;
				m_Rigidbody2D.AddForce(new Vector2(500,0));
				Destroy(other.gameObject);
			}
			else{
				pl2.attackname = "none";
				for(int i=0;i<killtomove.Length;i++){
					GameObject obj = killtomove[i];
					VerticalPlatformScript hl2 = obj.GetComponent<VerticalPlatformScript>();
					hl2.movestart = true;
				}
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
		}
		if (other.gameObject.tag != "Untagged") {
			//print (other.gameObject.tag);
		}
	}
	
	public void fliponhitbycar(){
		if (Time.time - flipstarttime > 1) {
			flipstarttime = Time.time;
			Flip ();
		}
	}

	public void bloodSplat(GameObject obj){
		GameObject bloodsplat = (GameObject)Instantiate (Resources.Load ("bloodsplatforfloor"));
		bloodsplat.transform.position = transform.position;
		GameObject blood = (GameObject)Instantiate (Resources.Load ("BloodSplat"));
		blood.transform.position = transform.position;
		EllipsoidParticleEmitter[] ps = blood.GetComponentsInChildren<EllipsoidParticleEmitter> ();
		for (int i=0; i<ps.Length; i++) {
			ps [i].Emit ();
			ps [i].emit = false;
			//ps[i].enabled = false;
		}
		blood.transform.position = new Vector3 (obj.transform.position.x - 2, 0, obj.transform.position.z);
	}

	private IEnumerator LoadNextLevel(){

		yield return new WaitForSeconds(2);
		Application.LoadLevel ("Menu");

	}
	
}

