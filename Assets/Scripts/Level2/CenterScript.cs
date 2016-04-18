using UnityEngine;
using System.Collections;

public class CenterScript : MonoBehaviour {

	// Use this for initialization
	public float rotation;
	void Start () {
		rotation = 0;
	}
	
	// Update is called once per frame
	void Update () {
		rotation = rotation+0.5f;
	}
}
