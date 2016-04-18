using UnityEngine;
using System.Collections;

public class PotScript : MonoBehaviour {

	public bool start;
	public float speedy;
	// Use this for initialization
	void Start () {
	
	}

	/*void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Goli") {
			Destroy(gameObject);
		}
	}*/

	// Update is called once per frame
	void Update () {
		if (start) {
			transform.position = new Vector2(transform.position.x , transform.position.y - speedy * Time.deltaTime);
		}
	}
}
