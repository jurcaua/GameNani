﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Looker : MonoBehaviour {

	[Header("Settings")]

	public string gameName;
	private RaycastHit vision;
	public float maxDistance;

	GameObject previous;
	float timer;

	//canvas

	[Header("Debugging")]
	public Canvas DebugCanvas;
	private Canvas c;
	List<Canvas> canvases;

	public KeyCode DebugKey;
    public KeyCode RestartKey = KeyCode.R;

	bool devMode;
	Text devModeText;

	string pathDir;


	string dateTime;
	int sessionNumber;

	public bool debugLog = true;
	// Use this for initialization
	void Start () {
		timer = 0;
		canvases = new List<Canvas> ();
		devMode = false;
		dateTime = System.DateTime.Now.ToString();
		pathDir = Application.persistentDataPath + "/GameNani/LoadData/";

		if (debugLog) {
			Debug.Log (pathDir);
		}

		if (!Directory.Exists (pathDir)) {
			Directory.CreateDirectory (pathDir);
		}

		sessionNumber = Directory.GetFiles (pathDir).Length + 1;
		devModeText = transform.GetChild (0).GetChild (0).GetComponent<Text> ();
	}

	void Switch() {
		devMode = !devMode;
		if (!devMode) {
			foreach (Canvas cv in canvases) {
				cv.gameObject.SetActive (false);
			}
			devModeText.text = "";
		} else {
			foreach (Canvas cv in canvases) {
				cv.GetComponent<Animator> ().SetTrigger ("Dev");
				cv.gameObject.SetActive (true);
			}
			devModeText.text = "DevMode activated for Look";
		}

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (DebugKey)) {
			Switch ();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			PrintData ();
		}

        if (Input.GetKeyDown(RestartKey)) {
            PrintData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

		if (Input.GetKeyDown(KeyCode.Q)) {
			Ray setRay = new Ray (transform.position, transform.forward);
			if (Physics.Raycast (setRay, out vision, maxDistance)) {
				if (vision.collider.gameObject.layer != LayerMask.NameToLayer ("Observable")) {
					vision.collider.gameObject.layer = LayerMask.NameToLayer ("Observable");
				}
			}
		}

        Debug.DrawRay (transform.position, transform.forward * maxDistance, Color.red, 0f);

		Ray ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (ray, out vision, maxDistance, LayerMask.GetMask("Observable", "Default"))) {
			if (vision.collider.gameObject.layer == LayerMask.NameToLayer ("Observable")) {
				if (vision.collider.gameObject == previous) {
					//time it
					timer += Time.deltaTime;
					LookDataManager.addObject (previous, Time.deltaTime);
					LookDataManager.dictionary [previous.name].GetAverageWhile (timer);
				} else {
					if (previous != null) {
						if (debugLog) {
							Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");
						}
						LookDataManager.dictionary [previous.name].UpdateAverage (timer);
						timer = 0;
					}

					previous = vision.collider.gameObject;

					if (previous != null) {
						if (LookDataManager.addObject (previous, 0) == 0) {
							//first time setup
							c = Instantiate (DebugCanvas);
							c.GetComponent<LookatPlayer> ().parent = previous;
							c.GetComponent<LookatPlayer> ().oname = previous.name;
							c.GetComponent<LookatPlayer> ().data = LookDataManager.dictionary [previous.name];
							canvases.Add (c);

							if (!devMode) {
								c.gameObject.SetActive (false);
							}
						}
						LookDataManager.dictionary [previous.name].lookedAt++;
					}

				}

			} else if (previous != null){
				//non observable object
				if (debugLog) {
					Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");
				}

				LookDataManager.dictionary [previous.name].UpdateAverage (timer);

				previous = null;
				timer = 0;
			}

		}
		else if (previous != null){
			if (debugLog) {
				Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");
			}

			LookDataManager.dictionary [previous.name].UpdateAverage (timer);

			previous = null;
			timer = 0;
		}
	}

	public void PrintData() {
		string json = JsonUtility.ToJson (new PrintableData(LookDataManager.dictionary, gameName, dateTime));
		if (debugLog) {
			Debug.Log (json);
		}

		StreamWriter writer0 = new StreamWriter (pathDir + "session" + sessionNumber + ".JSON", false);
		writer0.WriteLine (json);
		writer0.Close();
	}

	void OnApplicationQuit() {
		PrintData ();
	}
}
