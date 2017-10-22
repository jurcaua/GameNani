using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class Looker : MonoBehaviour {

	private RaycastHit vision;
	public float raylength;

	public GameObject previous;
	public float timer;

	//canvas
	public Canvas canvas;
	private Canvas c;
	public List<Canvas> canvases;

	public KeyCode key;

	public bool devMode;
	public Text devModeText;

	public string pathDir;

	public string gameName;
	public string dateTime;
	public int sessionNumber;
	// Use this for initialization
	void Start () {
		timer = 0;
		canvases = new List<Canvas> ();
		devMode = false;
		dateTime = System.DateTime.Now.ToString();
		pathDir = Application.persistentDataPath + "/GameNani/LoadData/";

		Debug.Log (pathDir);

		if (!Directory.Exists (pathDir)) {
			Directory.CreateDirectory (pathDir);
		}

		sessionNumber = Directory.GetFiles (pathDir).Length + 1;
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
				cv.gameObject.SetActive (true);
			}
			devModeText.text = "DevMode activated for Look";
		}

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (key)) {
			Switch ();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			PrintData ();
		}

		Debug.DrawRay (transform.position, transform.forward * raylength, Color.red, 0f);

		Ray ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (ray, out vision, raylength, LayerMask.GetMask("Observable", "Default"))) {
			if (vision.collider.gameObject.layer == LayerMask.NameToLayer ("Observable")) {
				if (vision.collider.gameObject == previous) {
					//time it
					timer += Time.deltaTime;
					LookDataManager.addObject (previous, Time.deltaTime);
					LookDataManager.dictionary [previous.name].GetAverageWhile (timer);
				} else {
					if (previous != null) {
						Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");
						LookDataManager.dictionary [previous.name].UpdateAverage (timer);
						timer = 0;
					}

					previous = vision.collider.gameObject;

					if (previous != null) {
						if (LookDataManager.addObject (previous, 0) == 0) {
							//first time setup
							c = Instantiate (canvas);
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
				Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");

				LookDataManager.dictionary [previous.name].UpdateAverage (timer);

				previous = null;
				timer = 0;
			}

		}
		else if (previous != null){
			Debug.Log ("Looked at: " + previous.name + " for " + timer + "secs");

			LookDataManager.dictionary [previous.name].UpdateAverage (timer);

			previous = null;
			timer = 0;
		}
	}

	public void PrintData() {
		string json = JsonUtility.ToJson (new PrintableData(LookDataManager.dictionary, gameName, dateTime));
		Debug.Log (json);

		StreamWriter writer0 = new StreamWriter (pathDir + "session" + sessionNumber + ".JSON", false);
		writer0.WriteLine (json);
		writer0.Close();
	}

	void OnApplicationQuit() {
		PrintData ();
	}
}
