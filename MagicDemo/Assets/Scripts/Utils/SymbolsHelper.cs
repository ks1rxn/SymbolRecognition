using System.Collections.Generic;
using UnityEngine;

class SymbolsHelper {

	public static Vector3 CalculateSymbolCenter(List<Vector3> points) {
		Vector3 center = Vector3.zero;
		for (int i = 0; i != points.Count; i++) {
			center += points[i];
		}
		center /= points.Count;
		return center;
	}

	public static SymbolSize GetWidthAndHeightOfShape(List<Vector3> points) {
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

		return new SymbolSize(Mathf.Abs(rightX - leftX), Mathf.Abs(topY - bottomY));
	}

 }

public class Basis {
	public Vector3 Forward;
	public Vector3 Up;
	public Vector3 Right;

	public Basis(Vector3 forward, Vector3 up, Vector3 right) {
		Forward = forward;
		Up = up;
		Right = right;
	}
}

public class SymbolSize {
	public readonly float Width;
	public readonly float Height;

	public SymbolSize(float width, float height) {
		Width = width;
		Height = height;
	}

	public float MaxDimension {
		get {
			return Mathf.Max(Width, Height);
		}
	}

}