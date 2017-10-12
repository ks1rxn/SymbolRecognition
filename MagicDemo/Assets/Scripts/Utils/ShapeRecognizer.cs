using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShapeRecognizer {
	private static Shape m_circle;
	private static Shape m_line;
	private static Shape m_s;

	static ShapeRecognizer() {
		m_s = LoadShape("S");
		m_circle = LoadShape("O");
		m_line = LoadShape("I");
	}

	public static float Analyze(List<Vector3> points) {
		ResizeShape(points);
//		WriteDataToFile(JsonUtility.ToJson(new Shape(points)));
//		foreach (Vector3 point in points) {
//			Game.EventService.SendMessage(new DebugSpellTrackerMessage(DebugSpellTrackerForm.Sphere, point, Quaternion.identity, 0.1f));
//		}
		float resultO = AnalyzeShape(new List<Vector3>(points), new List<Vector3>(m_circle.Points));
		float resultS = AnalyzeShape(new List<Vector3>(points), new List<Vector3>(m_s.Points));
		float resultI = AnalyzeShape(new List<Vector3>(points), new List<Vector3>(m_line.Points));
		Debug.Log("Circle: " + resultO);
		Debug.Log("Line: " + resultI);
		Debug.Log("S: " + resultS);
		return resultO;
	}


	private static void ResizeShape(List<Vector3> points) {
		Vector2 size = GetShapeWH(points);
		float maxSize = Mathf.Max(size.x, size.y);
		for (int i = 0; i != points.Count; i++) {
			points[i] = points[i] * (10 / maxSize);
		}
	}

	private static Vector2 GetShapeWH(List<Vector3> points) {
		float leftX = float.MaxValue;
		float rightX = float.MinValue;
		float topY = float.MinValue;
		float bottomY = float.MaxValue;

		foreach (Vector3 point in points) {
			leftX = Mathf.Min(leftX, point.x);
			rightX = Mathf.Max(rightX, point.x);
			topY = Mathf.Max(topY, point.y);
			bottomY = Mathf.Min(bottomY, point.y);
		}

		return new Vector2(Mathf.Abs(rightX - leftX), Mathf.Abs(topY - bottomY));
	} 

	private static float AnalyzeShape(List<Vector3> shape, List<Vector3> example) {
		List<float> distances  = new List<float>(32);
		for (int i = example.Count - 1; i != -1; i--) {
			int closetIndex = -1;
			float closestDist = float.MaxValue;
			for (int g = 0; g != shape.Count; g++) {
				float dist = Vector3.Distance(example[i], shape[g]);
				if (dist < closestDist) {
					closestDist = dist;
					closetIndex = g;
				}
			}
			shape.RemoveAt(closetIndex);
			example.RemoveAt(i);
			distances.Add(closestDist * closestDist);
		}
		float sq = 0;
		for (int i = 0; i != distances.Count; i++) {
			sq += distances[i];
		}
		sq /= Mathf.Sqrt(distances.Count);
		return sq;
	}

	public static void WriteDataToFile(string jsonString) {
		string path = Application.dataPath + "/Levels.json";
		File.WriteAllText(path, jsonString);
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh ();
#endif
	}

	private static Shape LoadShape(string name) {
		TextAsset settingsJson = Resources.Load<TextAsset>(name);
		Shape shape = JsonUtility.FromJson<Shape>(settingsJson.text);
		return shape;
	}

}
