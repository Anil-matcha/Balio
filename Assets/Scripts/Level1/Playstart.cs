using UnityEngine;
using System.Collections;

public class Playstart : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	public void onClick(){
		Application.LoadLevel ("Level1");
	}

	public void ViewLevels(){
		Application.LoadLevel ("Levels");
	}

	public void gotoLevel1(){
		Application.LoadLevel ("Level1");
	}

	public void gotoLevel2(){
		Application.LoadLevel ("Level2");
	}

	public void gotoLevel3(){
		Application.LoadLevel ("Level3");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
