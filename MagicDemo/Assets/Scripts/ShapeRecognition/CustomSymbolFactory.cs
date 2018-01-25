using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CustomSymbolFactory {
	private const int SymbolPointsCount = 128;
	private const float RecognitionSquareDimension = 10;

	public static CustomSymbol CreateSymbol(List<Vector3> points) {
		List<Vector3> correctedCountPoints = CorrectPointsCount(points);
		Vector3 spellCenter = SymbolsHelper.CalculateSymbolCenter(correctedCountPoints);

		Basis basisBasedOnPlayerSight = CreateBasisBasedOnPlayerSight(spellCenter, Player.instance.headCollider.transform.position);
		Basis basisToProjectPointsToXyPlane = CreateBasisToProjectPointsToXyPlane(basisBasedOnPlayerSight);

		List<Vector3> poitnsProjectedToPlaneInPlayerSightBasis = MathHelper.ProjectPointsToPlane(correctedCountPoints, spellCenter, basisBasedOnPlayerSight.Forward);
		List<Vector3> pointsProjectedToXyPlane = ProjectPointsToXyPlane(poitnsProjectedToPlaneInPlayerSightBasis, spellCenter, basisToProjectPointsToXyPlane);
		List<Vector3> resizedPoints = ResizeShapeToFitRecognitionSquare(pointsProjectedToXyPlane);

		SymbolAnalyzerResult result = SymbolAnalyzer.Instance.Analyze(resizedPoints);

		CustomSymbol newSymbol = new CustomSymbol();
		newSymbol.Type = result.IsPassed() ? result.SymbolType : SymbolType.Unrecognized;
		newSymbol.Center = spellCenter;
		newSymbol.Orientation = basisBasedOnPlayerSight;
		newSymbol.Size = SymbolsHelper.GetWidthAndHeightOfShape(pointsProjectedToXyPlane).MaxDimension;

		return newSymbol;
	}

	private static List<Vector3> CorrectPointsCount(List<Vector3> points) {
		while (points.Count > SymbolPointsCount) {
			points = CombineClosestPoints(points);
		}
		while (points.Count < SymbolPointsCount) {
			points = AddExtraPoint(points);
		}
		return points;
	}

	private static Basis CreateBasisBasedOnPlayerSight(Vector3 spellCenter, Vector3 headPosition) {
		Vector3 targetVector = (headPosition - spellCenter).normalized;
		Vector3 targetUpVector = targetVector.y < 0 ? targetVector : -targetVector;
		targetUpVector.y = -(targetVector.x * targetUpVector.x + targetVector.z * targetUpVector.z) / targetVector.y;
		Vector3 targetRightVector = Vector3.Cross(targetVector, targetUpVector);
		Vector3.OrthoNormalize(ref targetVector, ref targetUpVector, ref targetRightVector);
		return new Basis(targetVector, targetUpVector, targetRightVector);
	}

	private static Basis CreateBasisToProjectPointsToXyPlane(Basis oldBasis) {
		Vector3 row1 = new Vector3(oldBasis.Right.x, oldBasis.Up.x, oldBasis.Forward.x);
		Vector3 row2 = new Vector3(oldBasis.Right.y, oldBasis.Up.y, oldBasis.Forward.y);
		Vector3 row3 = new Vector3(oldBasis.Right.z, oldBasis.Up.z, oldBasis.Forward.z);

		float det = row1.x * row2.y * row3.z + row2.x * row3.y * row1.z + row3.x * row1.y * row2.z - row1.x * row3.y * row2.z - row3.x * row2.y * row1.z - row2.x * row1.y * row3.z;
		Vector3 newForward = 1 / det * new Vector3(row3.z * row2.y - row3.y * row2.z, -(row3.z * row1.y - row3.y * row1.z), row2.z * row1.y - row2.y * row1.z);
		Vector3 newUp = 1 / det * new Vector3(-(row3.z * row2.x - row3.x * row2.z), row3.z * row1.x - row3.x * row1.z, -(row2.z * row1.x - row2.x * row1.z));
		Vector3 newRight = 1 / det * new Vector3(row3.y * row2.x - row3.x * row2.y, -(row3.y * row1.x - row3.x * row1.y), row2.y * row1.x - row2.x * row1.y);

		return new Basis(newForward, newUp, newRight);
	}

	private static List<Vector3> ProjectPointsToXyPlane(List<Vector3> points, Vector3 spellCenter, Basis XyPlaneBasis) {
		for (int i = 0; i != points.Count; i++) {
			Vector3 pointVector = points[i] - spellCenter;
			Vector3 pointNew = MathHelper.TranslatePointToNewBasis(pointVector, XyPlaneBasis.Forward, XyPlaneBasis.Up, XyPlaneBasis.Right);
			points[i] = pointNew;
		}
		return points;
	} 

	/// Resize shape, so it fits recognition square.
	/// If square is 10x10, then coordinates are -5..5, -5..5
	private static List<Vector3> ResizeShapeToFitRecognitionSquare(List<Vector3> points) {
		List<Vector3> resizedPoints = new List<Vector3>(points.Count);
		SymbolSize initialSize = SymbolsHelper.GetWidthAndHeightOfShape(points);
		for (int i = 0; i != points.Count; i++) {
			resizedPoints.Add(points[i] * (RecognitionSquareDimension / initialSize.MaxDimension));
		}
		return resizedPoints;
	}

	private static List<Vector3> AddExtraPoint(List<Vector3> points) {
		float maxD = float.MinValue;
		int maxIndex = int.MinValue;
		Vector3? point1 = null;
		Vector3? point2 = null;

		for (int i = 1; i != points.Count; i++) {
			float d = Vector3.Distance(points[i], points[i - 1]);
			if (d > maxD) {
				maxD = d;
				maxIndex = i;
				point1 = points[i];
				point2 = points[i - 1];
			}
		}

		if (point1 == null || point2 == null) {
			Debug.LogError("Too little points to add extra");
			return points;
		}

		Vector3 newPoint = (point2.Value - point1.Value) / 2 + point1.Value;
		points.Insert(maxIndex, newPoint);
		return points;
	}

	private static List<Vector3> CombineClosestPoints(List<Vector3> points) {
		float minD = float.MaxValue;
		int minIndex = int.MinValue;
		Vector3? point1 = null;
		Vector3? point2 = null;

		for (int i = 1; i != points.Count; i++) {
			float d = Vector3.Distance(points[i], points[i - 1]);
			if (d < minD) {
				minD = d;
				minIndex = i;
				point1 = points[i];
				point2 = points[i - 1];
			}
		}

		if (point1 == null || point2 == null) {
			Debug.LogError("Too little points to combine");
			return points;
		}

		Vector3 newPoint = (point2.Value - point1.Value) / 2 + point1.Value;
		points[minIndex] = newPoint;
		points.RemoveAt(minIndex - 1);
		return points;
	}

}
