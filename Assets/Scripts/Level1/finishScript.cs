using UnityEngine;
using System.Collections;

public class finishScript : MonoBehaviour {

	public GameObject gameoverbutton;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag=="Player"){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
			rb.velocity.Set(0,0);
			gameoverbutton.SetActive(true);
		}
	}
}
