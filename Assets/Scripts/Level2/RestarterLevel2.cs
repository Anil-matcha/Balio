using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
namespace UnityStandardAssets._2D
{*/
public class RestarterLevel2 : MonoBehaviour
{
	public GameObject gameoverbutton;
	public Slider healthSlider;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			LoadNextLevel ();	
		} 
		else if (other.tag == "Box") {
			Destroy(other.gameObject);
		}
		else if (other.tag == "Goon") {
			Destroy(other.gameObject);
		}
	}
	
	void LoadNextLevel() {

	}
	
	public void playagain(){
		Application.LoadLevel (Application.loadedLevelName);
	}
	
}
//}

