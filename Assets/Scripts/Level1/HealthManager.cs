using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	// Use this for initialization
	public int health = 100;
	public GameObject[] hearts;
	void Start () {
		/*
		if (PlayerPrefs.HasKey ("health")) {
			health = PlayerPrefs.GetInt("health");
		}
		*/
		//print (health);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
