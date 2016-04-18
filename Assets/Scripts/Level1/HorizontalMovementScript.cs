using UnityEngine;
using System.Collections;

public class HorizontalMovementScript : MonoBehaviour {

	public bool movestart = false;
	public bool downdirection = true;
	public float startposition = 2;
	public float endposition = -3;
	public float previoustime = 0;
	public float speed = 5;
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - previoustime > 3) {
			speed = UnityEngine.Random.Range(3,6);
		}
		if (movestart) {
			if(downdirection ){
				if(transform.position.x > endposition){
					downdirection = false;
				}
				else{
					if(player){
						if(Mathf.Abs(player.transform.position.y-transform.position.y)<=2 && player.transform.position.y>=transform.position.y && Mathf.Abs(player.transform.position.x-transform.position.x)<=4 && player.transform.position.x>=transform.position.x){
							float playerx = player.transform.position.x;
							Vector2 playerposnew = new Vector2(playerx+speed* Time.deltaTime,player.transform.position.y);
							player.transform.position = playerposnew;
						}
					}
					Vector2 pos = transform.position;
					pos.x = pos.x	+ speed* Time.deltaTime;
					transform.position = pos;
				}
			}
			else{
				if(transform.position.x < startposition){
					downdirection = true;
				}
				else{
					if(player){
						if(Mathf.Abs(player.transform.position.y-transform.position.y)<=2  && player.transform.position.y>=transform.position.y &&  Mathf.Abs(player.transform.position.x-transform.position.x)<=4 && player.transform.position.x>=transform.position.x){
							float playerx = player.transform.position.x;
							Vector2 playerposnew = new Vector2(playerx-speed* Time.deltaTime,player.transform.position.y);
							player.transform.position = playerposnew;
						}
					}
					Vector2 pos = transform.position;
					pos.x = pos.x - speed* Time.deltaTime;
					transform.position = pos;
				}
			}
		}
	}


}
