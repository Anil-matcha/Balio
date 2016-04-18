using UnityEngine;
using System.Collections;

public class IndicatorScriptLevel3 : MonoBehaviour {
	
	// Use this for initialization
	public float previousshowtime = 0;
	public GameObject[] setactive;
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if(Time.time - previousshowtime > 2){
				for(int i=0;i<setactive.Length;i++){
					setactive[i].SetActive(true);
				}
				PlayerLevel3 pl2 = other.gameObject.GetComponent<PlayerLevel3>();
				PlayerLevel3Control pl2c = other.gameObject.GetComponent<PlayerLevel3Control>();
				Rigidbody2D rb = pl2.GetComponent<Rigidbody2D>();
				Animator anim = pl2.m_Anim;
				rb.velocity = Vector2.zero;
				anim.SetFloat("Speed",0f);
				anim.SetBool("Ground",true);
				pl2c.playercanmove = false;
				previousshowtime = Time.time;
				pl2.showpanel();
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		for(int i=0;i<setactive.Length;i++){
			//setactive[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
