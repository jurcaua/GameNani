using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			Debug.Log ("total time: " + dictionary [e.name].TotalTime + " average time: " + dictionary [e.name].averageTime);
			return 1;
		}

	}


}

public class LookData {

	public float TotalTime;
	public float averageTime;
	public List<LookSession> list;

	public LookData(float duration) {
		TotalTime = duration;
		averageTime = duration;
		list = new List<LookSession> ();
		list.Add (new LookSession (duration));
	}

	public void UpdateTotal() {
		TotalTime += Time.deltaTime;

	}

	public void GetAverageWhile(float duration) {
		float avg = 0; int i = -1;
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
		list.Add (new LookSession (duration));

		float avg = 0; int i = -1;
		foreach (LookSession l in list) {
			avg += l.duration;
			i++;
		}
		averageTime = avg / i;
	}
}

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
