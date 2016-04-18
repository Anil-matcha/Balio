using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Vector2 checkpoint = new Vector2 (3, -2);
	GameObject player;
	PlatformerCharacter2D p2d;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		p2d = player.GetComponent<PlatformerCharacter2D> ();
	}

	public void DestroyandCreatePlayer(){
		print ("Destroy");
		LoadAgain ();
	}

	 void LoadAgain() {
		print ("LoadAgain");
		Time.timeScale = 0;
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.transform.position = checkpoint;
		Rigidbody2D playerbody = player.GetComponent<Rigidbody2D> ();
		playerbody.velocity = Vector2.zero;
		playerbody.angularVelocity = 0;
		p2d.goonhitbool = false;
		//Platformer2DUserControl.dead = true;
		GameObject killzone = GameObject.FindGameObjectWithTag ("KillZone");
		killzone.SetActive (false);
		//System.Threading.Thread.Sleep (1000);
		Time.timeScale = 1;
		killzone.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
