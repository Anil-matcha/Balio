using UnityEngine;
using System.Collections;

public class ReleaseScript : MonoBehaviour {

	// Use this for initialization
	Rigidbody2D[] allr;
	public float previousdroptime;
	public float droprate;
	public int index = 0;
	public bool start = false;
	void Start () {
		allr = GetComponentsInChildren<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - previousdroptime > droprate && start) {
			if(index < allr.Length){
				allr[index].isKinematic = false;
				index++;
				previousdroptime = Time.time;
			}
		}
	}
}
