using UnityEngine;
using System.Collections;

public class SpikesVerticalMoveScript : MonoBehaviour {

	public float maxposy;
	public float minposy;
	public float speedy;
	public bool facing_up = true;
	public float starttime;
	// Use this for initialization
	void Start () {
	
	}

	private void Flip()
	{
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
		starttime = UnityEngine.Random.Range (1f,3f);
	}


	// Update is called once per frame
	void Update () {
		if (Time.time > starttime) {
			if (facing_up) {
				if (transform.position.y > maxposy) {
					facing_up = false;
					Flip ();
				} else {
					transform.position = new Vector2 (transform.position.x, transform.position.y + speedy * Time.deltaTime);
				}
			} else {
				if (transform.position.y < minposy) {
					facing_up = true;
					Flip ();
				} else {
					transform.position = new Vector2 (transform.position.x, transform.position.y - speedy * Time.deltaTime);
				}
			}
		}
	}
}
