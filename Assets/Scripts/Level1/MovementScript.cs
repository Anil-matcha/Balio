using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public bool movestart = true;
	public bool downdirection = true;
	public float startposition = 2;
	public float endposition = -3;
	public float previoustime = 0;
	public float speed = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - previoustime > 3) {
			speed = UnityEngine.Random.Range(3,6);
		}
	    if (movestart) {
			if(downdirection ){
				if(transform.position.y > endposition){
					Vector2 pos = transform.position;
					pos.y = pos.y	- speed* Time.deltaTime;
					transform.position = pos;
				}
				else{
					downdirection = false;
				}
			}
			else{
				if(transform.position.y < startposition){
					Vector2 pos = transform.position;
					pos.y = pos.y + speed* Time.deltaTime;
					transform.position = pos;
				}
				else{
					downdirection = true;
				}
			}
		}
	}
}
