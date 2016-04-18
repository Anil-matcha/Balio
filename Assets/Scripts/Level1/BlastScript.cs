using UnityEngine;
using System.Collections;

public class BlastScript : MonoBehaviour {

	// Use this for initialization
	LineRenderer liner;
	public Vector2 pos;
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public double lengthOfLineRenderer = 0.2;
	int i = 1;
	public float starttime ;
	public float presenttime;
	public float previoustime;
	public bool destroybool = false;

	void Start () {
		liner = GetComponent<LineRenderer> ();
		liner.material = new Material(Shader.Find("Particles/Additive"));
		liner.SetColors(c1, c2);
		liner.SetWidth(2F, 4F);
		liner.SetVertexCount(2);
		starttime = Time.time;
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		presenttime = Time.time;
		if (destroybool) {
			if(presenttime-previoustime>5){
				Destroy(gameObject);
			}
		}
		transform.position = transform.position +  new Vector3(10,0,0)*Time.deltaTime;
		//RaycastHit hit;
		Vector2 fwd = transform.TransformDirection(Vector2.right);
		//print (PlatformerCharacter2D.playerpos);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, fwd, 100f);  

		if (hit.collider != null) { 
			if(hit.collider.gameObject.tag=="Car"){
				hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1200,0));
				presenttime = Time.time;
				previoustime = Time.time;
				destroybool = true;
			}
			else if(hit.collider.gameObject.tag == "Platform"){
				Destroy(gameObject);
			}
			else if(hit.collider.gameObject.tag == "Box"){
				Destroy(hit.collider.gameObject);
				Destroy(gameObject);
			}
			else if(hit.collider.gameObject.tag == "Enemy"){
				Destroy(gameObject);
			}
			//print (hit.distance+hit.collider.gameObject.tag);
			liner.SetPosition (0, new Vector2(hit.distance,0));
			//liner.SetPosition(1, new Vector3(transform.position.x+hit.distance,transform.position.y,0));  
		}  
		else {  
			liner.SetPosition (0, new Vector2(100f,0));
			//liner.SetPosition(1, new Vector3(transform.position.x+100f,transform.position.y,0));  
		}

        /*
		if(i < 20 && Time.time-starttime>0.01) {
			pos =  pos + new Vector2(Time.deltaTime,0);
			liner.SetPosition(1, pos);
			i++;
			starttime = Time.time;
		}
		*/
		//liner.SetPosition(pos);
	}
}
