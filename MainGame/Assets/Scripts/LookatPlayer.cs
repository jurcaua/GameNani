using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LookatPlayer : MonoBehaviour {

	public LookData data;
	public string oname;
	public GameObject parent;
	public float height;

	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform);

		transform.position = parent.transform.position + new Vector3 (0, height + parent.GetComponent<Renderer> ().bounds.size.y , 0);
		transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = string.Format ("{0}\n<size=10>Total: {1}\nAverage: {2}\nLookedAt#: {3}</size=10>",
			oname, data.TotalTime.ToString ("F2"), data.averageTime.ToString ("F2"), data.lookedAt);
	}
}
