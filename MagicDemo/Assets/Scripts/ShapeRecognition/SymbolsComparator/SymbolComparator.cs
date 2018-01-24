using System.Collections.Generic;
using UnityEngine;

public class SymbolComparator {
	private const float RecognitionSquareDimensions = 10;
	private List<SymbolTemplate> shapes;

	public SymbolComparator() {
		shapes = new List<SymbolTemplate>();

		shapes.Add(FileUtils.LoadFromJson<SymbolTemplate>("s"));
		shapes.Add(FileUtils.LoadFromJson<SymbolTemplate>("circle"));
		shapes.Add(FileUtils.LoadFromJson<SymbolTemplate>("line"));
	}

	public List<SymbolComparationResult> CompareShapeWithAllExamples(List<Vector3> points) {
		LogWriter.WriteLog(this, "Got new shape for comparation");
		List<Vector3> resizedPoints = ResizeShapeToFitRecognitionSquare(points);
		List<SymbolComparationResult> comparationResults = ComparePlayersShapeWithAllExamples(resizedPoints);
		return comparationResults;
	}

	/// Resize shape, so it fits recognition square.
	/// If square is 10x10, then coordinates are -5..5, -5..5
	private static List<Vector3> ResizeShapeToFitRecognitionSquare(List<Vector3> points) {
		List<Vector3> resizedPoints = new List<Vector3>(points.Count);
		SymbolSize initialSize = GetWidthAndHeightOfShape(points);
		float maximumDimension = Mathf.Max(initialSize.Width, initialSize.Height);
		for (int i = 0; i != points.Count; i++) {
			resizedPoints.Add(points[i] * (RecognitionSquareDimensions / maximumDimension));
		}
		return resizedPoints;
	}

	private static SymbolSize GetWidthAndHeightOfShape(List<Vector3> points) {
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

	private List<SymbolComparationResult> ComparePlayersShapeWithAllExamples(List<Vector3> playersShape) {
		List<SymbolComparationResult> results = new List<SymbolComparationResult>(shapes.Count);
		foreach (SymbolTemplate exampleShape in shapes) {
			SymbolComparationResult symbolsDifference = CalculateValueOfShapesDifference(exampleShape, playersShape);
			LogWriter.WriteLog(this, "Result with " + symbolsDifference.Type + " is " + symbolsDifference.Value);
			results.Add(symbolsDifference);
		}
		return results;
	}

	private static SymbolComparationResult CalculateValueOfShapesDifference(SymbolTemplate example, List<Vector3> playersShapePoints) {
		List<float> distancesBetweenClosestPoints = GetDistancesBetweenClosestPointsOfLists(example.Points, playersShapePoints);
		float averageDistance = MathHelper.CalculateAverageValue(distancesBetweenClosestPoints);
		float dispertion = MathHelper.CalculateDispertionValue(distancesBetweenClosestPoints, averageDistance);
		const int coefficientToMakeResultReadable = 100;
		float comparationResultValue = dispertion * Mathf.Pow(averageDistance, 3) * coefficientToMakeResultReadable;
		SymbolComparationResult comparationResult = new SymbolComparationResult(example.Type, comparationResultValue);
		return comparationResult;
	}

	private static List<float> GetDistancesBetweenClosestPointsOfLists(List<Vector3> exampleShapePoints, List<Vector3> playersShapePoints) {
		List<float> distancesBetweenClosestPoints = new List<float>(128);
		for (int i = 0; i != playersShapePoints.Count; i++) {
			float closestDistance = float.MaxValue;
			for (int g = 0; g != exampleShapePoints.Count; g++) {
				float distanceBetweenPoints = Vector3.Distance(playersShapePoints[i], exampleShapePoints[g]);
				if (distanceBetweenPoints < closestDistance) {
					closestDistance = distanceBetweenPoints;
				}
			}
			distancesBetweenClosestPoints.Add(closestDistance);
		}
		return distancesBetweenClosestPoints;
	}

	private struct SymbolSize {
		public readonly float Width;
		public readonly float Height;

		public SymbolSize(float width, float height) {
			Width = width;
			Height = height;
		}
	}

}