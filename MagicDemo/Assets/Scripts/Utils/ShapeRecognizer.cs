using System.Collections.Generic;
using UnityEngine;

public class ShapeRecognizer {

	public static float Analyze(List<Vector3> points) {
		Vector3 center = CalculateCenter(points);
		//todo: not maxdist, but average dist from center to points
		//todo: change player points positions, not sample
		List<Vector3> circle = CreateCircle(center, MaxDist(points) / 2);
		float result = AnalyzeShape(new List<Vector3>(points), new List<Vector3>(circle)) / MaxDist(points);
		Debug.Log("Circle: " + result);
		return result;
	}

	private static Vector3 CalculateCenter(List<Vector3> points) {
		Vector3 center = new Vector3();
		for (int i = 0; i != points.Count; i++) {
			center += points[i];
		}
		center /= points.Count;
		return center;
	}

	private static List<Vector3> CreateCircle(Vector3 center, float radius) {
		List<Vector3> circle = new List<Vector3>(128);
		for (int i = 0; i != 128; i++) {
			float angle = 360 / 128f * i;
			Vector3 position = new Vector3(center.x + Mathf.Cos(angle * Mathf.PI / 180) * radius, center.y + Mathf.Sin(angle * Mathf.PI / 180) * radius, center.z);
			circle.Add(position);
		}
		return circle;
	}

	private static float MaxDist(List<Vector3> shape) {
		float max = 0;
		for (int i = 0; i != shape.Count; i++) {
			for (int g = 0; g != shape.Count; g++) {
				float d = Vector3.Distance(shape[i], shape[g]);
				if (d > max) {
					max = d;
				}
			}
		}
		return max;
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

}
