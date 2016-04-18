using UnityEngine;
using System.Collections;

public class TextScript : MonoBehaviour {

	public TextAsset textFile;
	public string[] textLines;
	// Use this for initialization
	void Start () {
		if (textFile != null) {
			char[] sep = ("\n").ToCharArray();
			textLines = textFile.text.Split(sep);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
