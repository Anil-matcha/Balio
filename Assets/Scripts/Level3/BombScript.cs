using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

	// Use this for initialization
	GameObject player;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
			fireblast.transform.position = player.transform.position;
			other.gameObject.GetComponent<PlayerLevel3>().bloodSplat(other.gameObject);
			other.gameObject.GetComponent<PlayerLevel3>().reset();
			player.GetComponent<PlayerLevel3>().PlaySound(5);
		}
		if (other.gameObject.tag == "Goli") {
			Destroy(other.gameObject);
			gameObject.SetActive(false);
		}
	}
	 
	public void reset(){
		gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		if (player.transform.position.x - transform.position.x > 5) {
			GameObject fireblast = (GameObject)Instantiate(Resources.Load("Explosion"));
			fireblast.transform.position = player.transform.position;
			player.GetComponent<PlayerLevel3>().bloodSplat(player);
			player.GetComponent<PlayerLevel3>().reset();
			player.GetComponent<PlayerLevel3>().PlaySound(5);
		}
	}
}
