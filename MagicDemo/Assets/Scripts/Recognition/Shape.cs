using System.Collections.Generic;
using UnityEngine;

public class Shape {
	public ShapeType Type;
	public List<Vector3> Points;

	public Shape(ShapeType type, List<Vector3> points) {
		Points = points;
		Type = type;
	}

}
