using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShapeRecognizer {
	private static List<Shape> m_shapes;

	static ShapeRecognizer() {
		m_shapes = new List<Shape>();

		m_shapes.Add(LoadShape("s"));
		m_shapes.Add(LoadShape("circle"));
		m_shapes.Add(LoadShape("line"));
	}

	public static ShapeRecognizerResult Analyze(List<Vector3> points) {
		ResizeShape(points);
		
		ShapeRecognizerResult result = new ShapeRecognizerResult();
		foreach (Shape shape in m_shapes) {
			float shapeResult = AnalyzeShape(shape.Type, points, shape.Points);
//			Debug.Log(shape.Type + ":" + shapeResult);
			if (shapeResult < result.Value) {
				result.Value = shapeResult;
				result.Type = shape.Type;
			}
		}

		return result;
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

	private static float AnalyzeShape(ShapeType type, List<Vector3> shape, List<Vector3> example) {
		List<float> pureDistances = new List<float>(128);
		for (int i = 0; i != shape.Count; i++) {
			int closestIndex = -1;
			float closestDist = float.MaxValue;
			for (int g = 0; g != example.Count; g++) {
				float dist = Vector3.Distance(shape[i], example[g]);
				if (dist < closestDist) {
					closestDist = dist;
					closestIndex = g;
				}
			}
//			if (type == ShapeType.Circle) {
//				foreach (Vector3 point in shape) {
//					Game.EventService.SendMessage(new DebugSpellTrackerMessage(DebugSpellTrackerForm.Sphere, point, Quaternion.identity, 0.1f));
//				}
//				foreach (Vector3 point in example) {
//					Game.EventService.SendMessage(new DebugSpellTrackerMessage(DebugSpellTrackerForm.Sphere, point, Quaternion.identity, 0.2f));
//				}
//				Game.EventService.SendMessage(new DebugSpellTrackerMessage(DebugSpellTrackerForm.Sphere, example[closestIndex] + (shape[i] - example[closestIndex]) / 2, Quaternion.identity, 0.05f));	
//			}
			pureDistances.Add(closestDist);
		}
//		WriteListToFile(type + "_dist_raw.txt", pureDistances);
//		pureDistances.Sort((x,y) => x.CompareTo(y));
//		WriteListToFile(type + "_dist_sorted.txt", pureDistances);

		float med = 0;
		for (int i = 0; i != pureDistances.Count; i++) {
			med += pureDistances[i];
		}
		med /= pureDistances.Count;

		float disp = 0;
		for (int i = 0; i != pureDistances.Count; i++) {
			disp += Mathf.Pow(pureDistances[i] - med, 2);
		}
		disp = Mathf.Sqrt(disp / pureDistances.Count) * 100;
//		Debug.Log(disp + " " + med * med * med);
		return disp * med * med * med;
	}

	private static void WriteListToFile(string fileName, List<float> values) {
		string text = "";
		int index = 0;
		for (int i = 0; i != values.Count; i++) {
			text += index + "," + values[i] + Environment.NewLine;
			index++;
		}
		WriteDataToFile(fileName, text);
	}

	private static void WriteDataToFile(string fileName, string jsonString) {
		string path = Application.dataPath + "/" + fileName;
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