using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	public float speed = 0.5f;
	Renderer rd;
	// Use this for initialization
	void Start () {
		rd = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2 (Time.time*speed,0);
		rd.material.mainTextureOffset = offset;
	}
}
