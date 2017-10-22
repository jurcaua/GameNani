using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looker : MonoBehaviour {

	private RaycastHit vision;
	public float raylength;

	public GameObject previous;
	public float timer;

	//canvas
	public Canvas canvas;
	public Canvas c;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward * raylength, Color.red, 0f);

		Ray ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (ray, out vision, raylength, LayerMask.GetMask("Observable", "Default"))) {
			if (vision.collider.gameObject.layer == LayerMask.NameToLayer("Observable")) {
				if (vision.collider.gameObject == previous) {
					//time it
					timer += Time.deltaTime;
					if (LookDataManager.addObject (previous, Time.deltaTime) == 0) {
						//first time setup
						c = Instantiate(canvas, previous.transform);
						c.GetComponent<LookatPlayer> ().oname = previous.name;
						c.GetComponent<LookatPlayer> ().data = LookDataManager.dictionary[previous.name];
					}
					LookDataManager.dictionary [previous.name].GetAverageWhile(timer);
				} else {
					if (previous != null) {
						Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");
						timer = 0;
					}
					previous = vision.collider.gameObject;


				}

			}
		}
		else if (previous != null){
			Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");

			LookDataManager.dictionary [previous.name].UpdateAverage(timer);
//			if (LookDataManager.addObject (previous, timer) == 0) {
//				//first time setup
//				Canvas c = Instantiate(canvas, previous.transform);
//				c.GetComponent<LookatPlayer> ().oname = previous.name;
//				c.GetComponent<LookatPlayer> ().data = LookDataManager.dictionary [previous.name];
//			}

			previous = null;
			timer = 0;
		}
	}
}
