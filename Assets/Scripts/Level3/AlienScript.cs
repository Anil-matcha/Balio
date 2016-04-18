using UnityEngine;
using System.Collections;

public class AlienScript : MonoBehaviour {

	GameObject player;
	public float speedy = 5;
	public bool facing_up = true;
	public bool set = false;
	public float distancerange = 10;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (transform.position.y - player.transform.position.y) > distancerange) {
			if(!set){
				if(transform.position.y > player.transform.position.y){
					facing_up  = false;
				}
				else{
					facing_up = true;
				}
				set = true;
			}
		} else {
			set = false;
		}
		if(facing_up){
			transform.position = new Vector2(transform.position.x , transform.position.y + speedy*Time.deltaTime);
		}
		else{
			transform.position = new Vector2(transform.position.x , transform.position.y - speedy*Time.deltaTime);
		}
	}
}
