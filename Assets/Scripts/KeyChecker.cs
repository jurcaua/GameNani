using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyChecker : MonoBehaviour {

    public KeyCode debugKey;
    public bool checkAllKeys = false;
    public string debugOutput = "KeyPressed: {0}\nCount: {1}\nLongestPress: {2}";
    public GameObject debugCanvasPrefab;

    private GameObject debugCanvas;
    private TextMeshProUGUI debugText;
    private bool debugUI = false;
    private KeyCode[] keycodes;

    void Awake() {
        debugCanvas = Instantiate(debugCanvasPrefab) as GameObject;
        debugText = debugCanvas.GetComponentInChildren<TextMeshProUGUI>();
        debugCanvas.SetActive(debugUI);

        if (checkAllKeys) {
            keycodes = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];
        } else {
            keycodes = KeyPressDataManager.checkedKeys;
        }
    }

    void Update() {
        
        foreach (KeyCode key in keycodes) {
            if (Input.GetKeyDown(key)) {

                KeyPressDataManager.RegisterDown(key);

                UpdateDebugUI(key);

                if (key == debugKey) {
                    debugUI = !debugUI;
                    debugCanvas.SetActive(debugUI);
                }

            } else if (Input.GetKeyUp(key)) {

                Debug.Log("UP KEY EVENT: " + key);
                KeyPressDataManager.RegisterUp(key);

                UpdateDebugUI(key);

            }
        }
    }

    void UpdateDebugUI(KeyCode keycode) {
        debugText.text = string.Format(debugOutput,
            keycode.ToString(), KeyPressDataManager.CountOf(keycode), KeyPressDataManager.LongestHoldTimeOf(keycode));
    }

    void OnApplicationQuit() {
//        WWWForm form = new WWWForm();
//		form.AddField;
        //HTTP.Sync.POST("localhost",);
    }
}


