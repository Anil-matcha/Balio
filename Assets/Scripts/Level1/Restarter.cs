using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
namespace UnityStandardAssets._2D
{*/
    public class Restarter : MonoBehaviour
    {
		public GameObject gameoverbutton;
		public Slider healthSlider;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player") {
			LoadNextLevel ();	
		} else if (other.tag == "Car") {
			GameObject hmv = GameObject.FindGameObjectWithTag ("HorizontalMoveBlock");
			HorizontalMovementScript hmvs = hmv.GetComponent<HorizontalMovementScript>();
			hmvs.movestart = true;
			Destroy(other.gameObject);
		}
		else if (other.tag == "Box") {
			Destroy(other.gameObject);
		}
		else if (other.tag == "Hen") {
			Destroy(other.gameObject);
		}
		else if (other.tag == "Goon") {
			Destroy(other.gameObject);
		}
        }

		void LoadNextLevel() {
			/*	
			Camera2DFollow.followbool = false;
			int hearts = PlatformerCharacter2D.hearts;
			if (hearts == 0) {
				Application.LoadLevel (Application.loadedLevelName);
			} else {
				GameObject heart = GameObject.FindGameObjectWithTag ("Heart" + hearts);
				//Destroy (heart);
				hearts = hearts-1;
				PlatformerCharacter2D.hearts = hearts;
				HealthManager.health = 100;
				healthSlider.value = HealthManager.health;
			}
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			Rigidbody2D m_Rigidbody2D = GetComponent<Rigidbody2D> ();
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.angularVelocity = 0;
			player.transform.position = new Vector2 (3, -2);
			PlatformerCharacter2D.goonhitbool = false;
			PlatformerCharacter2D.m_Grounded = false;
			//GameScript.DestroyandCreatePlayer ();
			*/
		}

	public void playagain(){
		Application.LoadLevel (Application.loadedLevelName);
	}

    }
//}
