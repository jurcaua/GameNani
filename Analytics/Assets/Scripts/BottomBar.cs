using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BottomBar : MonoBehaviour {

	public GameObject button;
	public GameObject button2;
    public DataLoader dl;
	public int childcount;
	public GameObject ldr;
	public GameObject other;

	public bool key;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Refresh(string s) {


		if (!key) {
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Destroy (transform.GetChild (i).gameObject);
			}

			int k = 0;
			foreach (Session ses in DATA.sessions) {
				if (ses.gameName == dl.game && ses.lookData.dictionary.ContainsKey (s)) {
					k++;
				}
			}
			foreach (Session ses in DATA.sessions) {
				if (ses.gameName == dl.game && ses.lookData.dictionary.ContainsKey (s)) {
					GameObject a = Instantiate (button, transform);
					a.GetComponentInChildren<Text> ().text = ses.sessionID;
					a.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (80, 1235 / k);
					a.GetComponent<BHscript> ().ld = ses.lookData.dictionary [s];
					a.GetComponent<BHscript> ().ldr = ldr;
					a.GetComponent<BHscript> ().bb = transform.parent.gameObject;
					a.GetComponent<BHscript> ().other = other;
				}
			}
            GameObject.Find("Graph").GetComponent<Grapher>().CreateObjectPoints(s);
        } else {
			Debug.Log ("KOOL");
			//when key
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Destroy (transform.GetChild (i).gameObject);
			}
			int k = 0;
            KeyCode thiskeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), s);
            foreach (Session ses in DATA.sessions) {
				Debug.Log (thiskeycode);
				if (ses.gameName == dl.game && ses.keyData.dictionary.ContainsKey (thiskeycode)) {
					k++;
				}
			}
			foreach (Session ses in DATA.sessions) {

				if (ses.gameName == dl.game && ses.keyData.dictionary.ContainsKey (thiskeycode)) {
					GameObject a = Instantiate (button2, transform);
					a.GetComponentInChildren<Text> ().text = ses.sessionID;
					a.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (80, 1235 / k);
				}
			}
            GameObject.Find("Graph").GetComponent<Grapher>().CreateKeyCodePoints(thiskeycode);
        }
			
	}
		
}
