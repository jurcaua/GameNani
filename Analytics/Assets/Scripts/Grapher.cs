using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grapher : MonoBehaviour {
    
    public int resolution = 10;
    public Text xAxis;
    public Text yMin;
    public Text yMax;
    public float wideMult;
    public float xShift;
    public GameObject[] legendItems;

    private int currentResolution;
    private List<Vector3> points = new List<Vector3>();
    private LineRenderer[] r;

	float minX;
	float maxX;
	float minY;
	float maxY;

    void Start() {

        r = GetComponentsInChildren<LineRenderer>();
    }

    public void CreateObjectPoints(string objectName) {

		float[] minXs = new float[3];
		float[] maxXs = new float[3];
		float[] minYs = new float[3];
		float[] maxYs = new float[3];

		Graphable[] toGraph = new Graphable[3];

        r[0].gameObject.SetActive(true);
        legendItems[0].SetActive(true);
        legendItems[0].GetComponentInChildren<Text>().text = "Avg Time (s)";
        r[1].gameObject.SetActive(true);
        legendItems[1].SetActive(true);
        legendItems[1].GetComponentInChildren<Text>().text = "Total Time (s)";
        r[2].gameObject.SetActive(true);
        legendItems[2].SetActive(true);
        legendItems[2].GetComponentInChildren<Text>().text = "Times Looked At (# of times)";


		Debug.Log ("LENGTH LEGNTH: " + r.Length);
        for (int rendIndex = 0; rendIndex < r.Length; rendIndex++) {

            List<LookData> data = new List<LookData>();
            

            foreach (Session s in DATA.sessions) {
                if (s.lookData.dictionary.ContainsKey(objectName)) {
                    data.Add(s.lookData.dictionary[objectName]);
                }
            }

            float[] x = new float[data.Count];
            float[] y = new float[data.Count];

            Debug.Log(rendIndex + " -- " + r[rendIndex].name);
            for (int i = 0; i < data.Count; i++) {
                x[i] = i;

                if (rendIndex == 0) {
                    y[i] = data[i].averageTime;
                } else if (rendIndex == 1) {
                    y[i] = data[i].TotalTime;
                } else if (rendIndex == 2) {
                    y[i] = data[i].lookedAt;
                }
				PrintArray (y);
            }

			minXs[rendIndex] = Min (x);
			maxXs[rendIndex] = Max (x);
			minYs[rendIndex] = Min (y);
			maxYs[rendIndex] = Max (y);

			toGraph [rendIndex] = new Graphable (r[rendIndex], "Session", x, "", y, rendIndex);


            //Graph(r[rendIndex], "Session", x, "", y, rendIndex);
        }

		minX = Min (minXs);
		maxX = Max (maxXs);
		minY = Min (minYs);
		maxY = Min (maxYs);

		yMin.text = minY.ToString();
		yMax.text = maxY.ToString();

		Debug.Log ("X:" + minX + ", " + maxX + " --- Y:" + minY + ", " + maxY);

		for (int i = 0; i < toGraph.Length; i++) {
			Graphable g = toGraph[i];
			Graph (g.line, g.xLabel, g.x, g.yLabel, g.y, g.z);
		}
    }

	void PrintArray(float[] A){
		string temp = "Array: [";
		foreach (float f in A) {
			temp += f.ToString() + ", ";
			temp += "]";
		}
		Debug.Log (temp);
	}

    public void CreateKeyCodePoints(KeyCode keycode) {

        r[0].gameObject.SetActive(true);
        legendItems[0].SetActive(true);
        legendItems[0].GetComponentInChildren<Text>().text = "Longest Hold (s)";
        r[1].gameObject.SetActive(true);
        legendItems[1].SetActive(true);
        legendItems[1].GetComponentInChildren<Text>().text = "# of Times Pressed";
        r[2].gameObject.SetActive(false);
        legendItems[2].SetActive(false);

        for (int rendIndex = 0; rendIndex < r.Length - 1; rendIndex++) {

            List<Keydata> data = new List<Keydata>();


            foreach (Session s in DATA.sessions) {
                if (s.keyData.dictionary.ContainsKey(keycode)) {
                    data.Add(s.keyData.dictionary[keycode]);
                }
            }

            float[] x = new float[data.Count];
            float[] y = new float[data.Count];

            Debug.Log(rendIndex + " -- " + r[rendIndex].name);
            for (int i = 0; i < data.Count; i++) {
                x[i] = i;

                if (rendIndex == 0) {
                    y[i] = data[i].longestHold;
                } else if (rendIndex == 1) {
                    y[i] = data[i].count;
                }
            }

            Graph(r[rendIndex], "Session", x, "", y, rendIndex);
        }
    }

    private static float Linear(float x) {
        return x;
    }

    private static float Exponential(float x) {
        return x * x;
    }

    private static float Parabola(float x) {
        x = 2f * x - 1f;
        return x * x;
    }

    private static float Sine(float x) {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }

    public void Graph(LineRenderer line, string xLabel, float[] x, string yLabel, float[] y, float z) {

        if (x.Length != y.Length) {
            Debug.Log("x and y lists should be the same size!");
            return;
        }

        resolution = x.Length;
					/*
        float minX = Min(x);
        float maxX = Max(x);
        float minY = Min(y);
        float maxY = Max(y);*/

        points.Clear();
        for (int i = 0; i < resolution; i++) {
            Vector3 newPoint = new Vector3();
            
            newPoint.x = Mathf.InverseLerp(minX, maxX, x[i]) * wideMult - xShift;
            newPoint.y = Mathf.InverseLerp(minY, maxY, y[i]);
            newPoint.z = z;
            //Debug.Log(newPoint);

            points.Add(newPoint);
        }
		if (points.Count == 1) {
			Vector3 cornerCase = new Vector3();

			cornerCase.x = 1f * wideMult - xShift;
			cornerCase.y = points[0].y;
			cornerCase.z = z;

			points.Add(cornerCase);
		}

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());

        xAxis.text = xLabel;
    }

    float Max(float[] list) {
		
        float max = float.MinValue;
        for (int i = 0; i < list.Length; i++) {
			Debug.Log(list[i]);
            if (list[i] > max) {
                max = list[i];
            }
        }
        return max;
    }

    float Min(float[] list) {
		Debug.Log("Min");

        float min = float.MaxValue;
        for (int i = 0; i < list.Length; i++) {
            if (list[i] < min) {
                min = list[i];
            }
        }
        return min;
    }
}

public class Graphable{

	public LineRenderer line;
	public string xLabel;
	public float[] x;
	public string yLabel;
	public float[] y;
	public float z;

	public Graphable(LineRenderer _line, string _xLabel, float[] _x, string _yLabel, float[] _y, float _z){
		line = _line;
		xLabel = _xLabel;
		x = _x;
		yLabel = _yLabel;
		y = _y;
		z = _z;
	}
}
