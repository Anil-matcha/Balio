using UnityEngine;
using System.Collections;

public class FallingPlatformScriptLevel3 : MonoBehaviour {

	// Use this for initialization
	public GameObject[] platforms;
	public bool start = false;
	public float previousfalltime;
	public int index;
	public float fallrate = 0.35f;
	public ArrayList poslist = new ArrayList();
	public ArrayList rotlist = new ArrayList();
	void Start () {
		foreach (GameObject obj in platforms) {
			poslist.Add(obj.transform.position);
			rotlist.Add(obj.transform.rotation);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			start = true;
			previousfalltime = Time.time;
		}
	}

	// Update is called once per frame
	void Update () {
		if (start) {
			if(Time.time - previousfalltime > fallrate){
				if(index < platforms.Length){
					platforms[index].GetComponent<Rigidbody2D>().isKinematic = false;
					index++;
					previousfalltime = Time.time;
				}
			}
		}
	}

	public void reset(){
		int count = 0;
		foreach (GameObject obj in platforms) {
			obj.GetComponent<Rigidbody2D>().isKinematic = true;
			obj.transform.position = (Vector3)poslist[count];
			obj.transform.rotation = (Quaternion)rotlist[count];
			count++;
			start = false;
		}
		index = 0;
	}
}
