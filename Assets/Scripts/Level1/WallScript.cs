using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	// Use this for initialization
	public float scaleprevioustime;
	public int count = 0;
	public bool countbool;
	public float dx;
	public int maxcount = 10;
	public float wallsizey = 1f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - scaleprevioustime > 1f) {
			float x = UnityEngine.Random.Range (0.1f, 0.4f);
			dx = (x - transform.localScale.x)/maxcount;
			//transform.localScale = new Vector2 (x, 1);
			scaleprevioustime = Time.time;
			count = 0;
			countbool = true;
		} else {
			if(countbool){
				if(count<10){
					count++;
					transform.localScale = new Vector2 (transform.localScale.x + dx, wallsizey);
				}
				else{
					count = 0;
					countbool = false;
				}
			}
		}
	}
}
