using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {

	ArrayList bombs = new ArrayList();
	// Use this for initialization
	void Start () {
		BombScript[] bombscripts= gameObject.GetComponentsInChildren<BombScript>();
		int index = 0;
		foreach(BombScript bs in bombscripts){
			bombs.Add(bombscripts[index].gameObject);
			index++;
		}
	}

	public void reset(){
		for (int i = 0; i<bombs.Count; i++) {
			((GameObject)bombs[i]).SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
