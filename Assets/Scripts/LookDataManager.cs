using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrintableData {
	
	public List<string> keys;
	public List<LookData> lookDatas;
	public PrintableData(Dictionary<string, LookData> d) {
		keys = new List<string> (d.Keys);
		lookDatas = new List<LookData> ();
		foreach (string k in keys) {
			lookDatas.Add (d [k]);
		}
	}
}

public static class LookDataManager {
	
	public static Dictionary<string, LookData> dictionary = new Dictionary<string, LookData>();

	//return 0 if put in dictionary for first time
	//return 1 if updated
	public static int addObject(GameObject e, float duration) {
		if (!dictionary.ContainsKey (e.name)) {
			dictionary.Add (e.name, new LookData (/*maybe initialize*/duration));

			//everytime you look at an object for the first Time
			//debug
			Debug.Log ("Created new LookedObject: " + e.name);

			return 0;
		} else  {
			dictionary [e.name].UpdateTotal ();
			return 1;
		}

	}


}

[System.Serializable]
public class LookData {

	public float TotalTime;
	public float averageTime;
	public List<LookSession> list;
	public int lookedAt;

	public LookData(float duration) {
		TotalTime = duration;
		averageTime = duration;
		list = new List<LookSession> ();
		if (duration > 0) {
			list.Add (new LookSession (duration));
		}
	}

	public void UpdateTotal() {
		TotalTime += Time.deltaTime;

	}

	public void GetAverageWhile(float duration) {
		float avg = 0; int i = 0;
		foreach (LookSession l in list) {
			avg += l.duration;
			i++;
		}
		if (i == 0) {
			averageTime = duration;
		} else {
			averageTime = (avg + duration) / (i + 1); 
		}
	}

	public void UpdateAverage(float duration) {
		if (duration > 0) {
			list.Add (new LookSession (duration));

			float avg = 0;
			int i = 0;
			foreach (LookSession l in list) {
				avg += l.duration;
				i++;
			}
			averageTime = avg / i ;
			TotalTime = avg;
		}
		Debug.Log ("total time: " + TotalTime + " average time: " + averageTime);
	}
}

[System.Serializable]
public class LookSession {
	public float start;
	public float end;
	public float duration;
	public float attention;

	public LookSession (float d) {
		end = Time.time;
		duration = d;
		start = end - duration;
		attention = 0;
	}
	
}
