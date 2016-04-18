using UnityEngine;
using System.Collections;

public class SnakeScript : MonoBehaviour {

	// Use this for initialization
	public float previousappeartime;
	Animator anim;
	bool appear = false;
	public bool facing_left = true;
	public float velocityx = 5;
	public float startposition = 5;
	public float endposition = 5;
	public float starttime;
	public int timetostart;
	public bool start = false;
	
	GameObject player;
	void Start () {
		previousappeartime = Time.time;
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		if(UnityEngine.Random.Range(0,10)%2 == 0){
			Flip ();
		}
		starttime = Time.time;
		timetostart = UnityEngine.Random.Range (0, 10);
	}
	
	// Update is called once per frame
	void Update () {
		//print (transform.position.x);
		if (Time.time - starttime > timetostart) {
			start = true;
		}
		if(start){
			if (Time.time - previousappeartime > 3) {
				if (!appear) {
					anim.SetBool ("Appear", true);
					gameObject.AddComponent<BoxCollider2D> ();
				} else {
					anim.SetBool ("Appear", false);
					BoxCollider2D p2d = gameObject.GetComponent<BoxCollider2D> ();
					Destroy (p2d);
				}
				appear = !appear;
				if (UnityEngine.Random.Range (0, 10) % 2 == 0) {
					Flip ();
				}
				previousappeartime = Time.time;
			}
			if (appear) {
				if (facing_left) {
					if(transform.position.x - velocityx * Time.deltaTime > startposition){
						transform.position = new Vector2 (transform.position.x - velocityx * Time.deltaTime, transform.position.y);
					}
					else{
						Flip();
					}
				} else {
					if(transform.position.x - velocityx * Time.deltaTime < endposition){
						transform.position = new Vector2 (transform.position.x + velocityx * Time.deltaTime, transform.position.y);
					}
					else{
						Flip ();
					}
				}
			}
			if (Mathf.Abs (player.transform.position.x - transform.position.x) < 4 && appear) {
				if (transform.position.x >= player.transform.position.x) {
					if (!facing_left) {
						Flip ();
					}
				} else {
					if (facing_left) {
						Flip ();
					}
				}
				anim.SetBool ("Attack", true);
			} else {
				anim.SetBool ("Attack", false);
			}
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facing_left = !facing_left;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Snake") {
			Flip();
		}
		else if (other.gameObject.tag == "Player") {
			Flip();
		}
	}
		
}
