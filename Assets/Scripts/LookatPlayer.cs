using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookatPlayer : MonoBehaviour {

	public LookData data;
	public string oname;

	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform);

		transform.GetChild (0).GetComponent<Text>().text = oname + "\n" + data.TotalTime.ToString("F2") + "\n" + data.averageTime.ToString("F2");
	}
}
