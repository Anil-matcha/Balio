using UnityEngine;
using System.Collections;

public class FallingPlatformScript : MonoBehaviour {

	Rigidbody2D rb;
	public float staytime = 1f;
	public float breaktime = 1f;
	public float breakstarttime;
	public bool fall = false;
	public bool facing_right = true;
	public Vector2 startpos;
	public float startrotationz = 0;
	public float starttime = 0;
	public float fallstarttime = 0;
	public bool fallstartbool = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		startpos = transform.position;
		Vector3 rtv = transform.rotation.eulerAngles;
		startrotationz = rtv.z;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {

			if (other.gameObject.transform.position.y > transform.position.y + 0.5 && Time.time - starttime > 2 && !fall) {
				StartCoroutine (Fall ());
			}
		} else if (other.gameObject.tag == "Spikes") {
			//StartCoroutine(Reset());
			Vector3 rtv = transform.rotation.eulerAngles;
			rtv.z = startrotationz;
			transform.rotation = Quaternion.Euler (rtv);
			transform.position = startpos;
			fall = false;
			starttime = Time.time;
			rb.isKinematic = true;
			fallstartbool = false;
			gameObject.SetActive(false);
		}
	}

	public void reset(){
		Vector3 rtv = transform.rotation.eulerAngles;
		rtv.z = startrotationz;
		transform.rotation = Quaternion.Euler (rtv);
		transform.position = startpos;
		fall = false;
		starttime = Time.time;
		rb.isKinematic = true;
		fallstartbool = false;
	}

	private IEnumerator Fall(){

		yield return new WaitForSeconds(staytime);
		fall = true;
		breakstarttime = Time.time;

	}

	// Update is called once per frame
	void Update () {
		if (fall) {
			if(Time.time - breakstarttime > 1){
				rb.isKinematic = false;
				fallstarttime = Time.time;
				fallstartbool = true;
			}
			else{
				Vector3 rtv = transform.rotation.eulerAngles;
				if(facing_right){
					rtv.z = rtv.z - 0.4f;
				}
				else{
					rtv.z = rtv.z + 0.4f;
				}
				transform.rotation = Quaternion.Euler (rtv);
			}
		}
		if (fallstartbool) {
			if(Time.time - fallstarttime > 5){
				Vector3 rtv = transform.rotation.eulerAngles;
				rtv.z = startrotationz;
				transform.rotation = Quaternion.Euler (rtv);
				transform.position = startpos;
				fall = false;
				starttime = Time.time;
				rb.isKinematic = true;
				fallstarttime = Time.time;
				fallstartbool = false;
				gameObject.SetActive(false);
			}
		}
	}


		
}
