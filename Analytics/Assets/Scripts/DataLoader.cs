using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour {

    public string folderPath;
	public List<string> games = new List<string> ();

	public Dropdown dp;
	public Button gamebutton;
	public GameObject sr;
	public GameObject sc;

	public GameObject legend;
	public GameObject graph;
	public GameObject swap;

	public GameObject buttonprefab;
	public string game;

	public List<string> objects = new List<string>();
	public List<KeyCode> kcs = new List<KeyCode>();

    void Awake() {
        folderPath = Application.persistentDataPath + "/../GameNani/GameNani/LoadData/";
        Debug.Log(folderPath);
    }

	void Start () {
        //if (PlayerPrefs.GetInt("loaded") == 0) {
          //  PlayerPrefs.SetInt("loaded", 1);
            if (Directory.Exists(folderPath)) {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files) {
                    if (Path.GetExtension(file) == ".JSON") {
                        Debug.Log("Found file:" + file);
                        OpenFile(file);
                    }
                }
            } else {
                Debug.Log("Directory does not exist!");
                Directory.CreateDirectory(folderPath);
            }
        //}
        FillDropDown();
        
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            DATA.sessions.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

	void OpenFile(string filePath) {
        string json = File.ReadAllText(filePath);
        PrintableData data = JsonUtility.FromJson<PrintableData>(json);

        ProcessData(data, filePath);
    }

    void ProcessData(PrintableData data, string filePath) {
        Debug.Log(Path.GetFileNameWithoutExtension(filePath).Substring("session".Length));
        DATA.AddSession(data.gameName, data.dateTime, Path.GetFileNameWithoutExtension(filePath).Substring("session".Length), data.keys, data.lookDatas, data.keyPressData.keycodes, data.keyPressData.keydatas);

		if (!games.Contains (data.gameName)) {
			games.Add (data.gameName);
		}
	}

	void FillDropDown() {
		dp.AddOptions (games);
	}

	public void Clear() {
		game = games [dp.value];
		sc.gameObject.SetActive (true);
		sr.gameObject.SetActive (true);


		//add data to it

		Transform content = sr.transform.GetChild (0);

		foreach (Session s in DATA.sessions) {
			if (s.gameName == game) {
				foreach (string o in new List<string>(s.lookData.dictionary.Keys)) {
					if (!objects.Contains(o)) {
						GameObject b = Instantiate (buttonprefab, content);
						b.GetComponentInChildren<Text>().text = o;
						objects.Add (o);
					}
				}
			}
		}
		

		dp.gameObject.SetActive (false);
		gamebutton.gameObject.SetActive (false);

		legend.SetActive (true);
		graph.SetActive (true);
		swap.SetActive (true);

		//loads the next page
	}

	public void ClearKey() {
		sc.transform.GetChild(0).GetComponent<BottomBar> ().key = true;
		game = games [dp.value];

		for (int i = sr.transform.GetChild(0).childCount - 1; i >= 0; i--) {
			Destroy (sr.transform.GetChild(0).GetChild(i).gameObject);
		}

		Transform content = sr.transform.GetChild (0);
		foreach (Session s in DATA.sessions) {
			if (s.gameName == game) {
				foreach (KeyCode kc in new List<KeyCode>(s.keyData.dictionary.Keys)) {
					if (!kcs.Contains (kc)) {
						GameObject b = Instantiate (buttonprefab, content.transform);
						b.GetComponentInChildren<Text>().text = kc.ToString();
						kcs.Add (kc);
					}
				}
			}
		}

		Destroy (swap);
	}
}

[System.Serializable]
public class PrintableData {

    // session data
    public string gameName;
    public string dateTime;

    // ordered pairs of keys and lookdata objects
    public List<string> keys;
    public List<LookData> lookDatas;

    // key data
    public PrintableKeyPressData keyPressData;

}

[System.Serializable]
public class PrintableKeyPressData {
    public List<KeyCode> keycodes;
    public List<Keydata> keydatas;
}
