using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public GameObject bottom;
	public string text;


	// Use this for initialization
	void Start () {
		bottom = GameObject.Find ("ScrollContent");
	}
	
	// Update is called once per frame
	void Update () {
		text = gameObject.GetComponentInChildren<Text> ().text;
	}

	public void Refresh() {


		bottom.GetComponent<BottomBar> ().Refresh (text);

		//for objects
		
	}
}
