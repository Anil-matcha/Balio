using UnityEngine;
using System.Collections;

public class SunflowerScript : MonoBehaviour {

	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(player.transform.position, Vector3.up);

		if(Mathf.Abs(player.transform.position.x - transform.position.x) <10){
		}
	}
	void RotateLeft () {
		Quaternion theRotation = transform.localRotation;
		theRotation.y = 360*Time.deltaTime;
		transform.localRotation = theRotation;
	}
}
