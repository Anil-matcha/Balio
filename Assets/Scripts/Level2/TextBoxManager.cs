using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextBoxManager : MonoBehaviour {

	public TextAsset textFile;
	public string[] textLines;
	public GameObject textBox;
	public Text thetext;
	public int currentline =0;
	public int endatline;
	public bool isTyping = false;
	public bool cancelTyping = false;
	public float typeSpeed = 0.2f;
	GameObject player;
	// Use this for initialization
	void Start () {
		if (textFile != null) {
			char[] sep = ("\n").ToCharArray();
			textLines = textFile.text.Split(sep);
		}
		for (int i=0; i<textLines.Length-1; i++) {
			//print(textLines[i]);
		}
		if (endatline == 0) {
			endatline = textLines.Length-1;
		}
		player = GameObject.FindGameObjectWithTag ("Player");
		//print (currentline);
		StartCoroutine(TextScroll(textLines[currentline]));
	}
	
	// Update is called once per frame
	void Update () {
		//thetext.text = textLines [currentline];
		if(Input.GetKeyDown(KeyCode.Return)){
			 if(!isTyping){
			 	currentline++;
				if(currentline>endatline){
					textBox.SetActive(false);
					PlayerLevel2Control pl2c = player.GetComponent<PlayerLevel2Control>();
					PlayerLevel2 pl2 = player.GetComponent<PlayerLevel2>();
					pl2c.playercanmove = true;
					pl2.hidepanel();
					//gameObject.SetActive(false);
				}
				else{
					//print(textLines[currentline]);
					StartCoroutine(TextScroll(textLines[currentline]));
				}
			 }
			 else{
				if(isTyping && !cancelTyping){
					cancelTyping = true;
				}
			 }
		}

	}

	public void start(){
		//StartCoroutine(TextScroll(textLines[currentline]));
	}

	private IEnumerator TextScroll(string lineOfText){
		//print (lineOfText);
		int letter = 0;
		thetext.text = "";
		isTyping = true;
		cancelTyping = false;
		while (isTyping && !cancelTyping && letter<lineOfText.Length-1) {
			thetext.text+=lineOfText[letter];
			letter++;
			yield return new WaitForSeconds(typeSpeed);
		}
		thetext.text = lineOfText;
		isTyping = false;
		cancelTyping = false;
	}
}
