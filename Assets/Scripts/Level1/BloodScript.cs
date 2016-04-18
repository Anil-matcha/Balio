using UnityEngine;
using System.Collections;

public class BloodScript : MonoBehaviour {

	SpriteRenderer sp;
	float opacity;
	Color c;
	// Use this for initialization
	void Start () {
		opacity = 0;
		sp = GetComponent<SpriteRenderer> ();
		c = sp.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > 1) {
			c = new Color(c.r, c.g, c.b, opacity);
			opacity += 20*Time.deltaTime;
			//print (opacity);
			/*
			print (100/Time.time);
			Color tmp = sp.color;
			tmp.a = 100/Time.time;
			sp.color = tmp;
			//sp.color.a = 100/Time.time;
			if(Time.time>20){
				Destroy(gameObject);
			}
			*/
		}
	}
}
