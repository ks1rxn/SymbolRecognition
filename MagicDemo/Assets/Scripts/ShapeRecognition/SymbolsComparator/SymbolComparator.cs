using System.Collections.Generic;
using UnityEngine;

public class SymbolComparator {
	private List<SymbolTemplate> shapes;

	public SymbolComparator() {
		shapes = new List<SymbolTemplate>();

		shapes.Add(FileUtils.LoadFromJson<SymbolTemplate>("s"));
		shapes.Add(FileUtils.LoadFromJson<SymbolTemplate>("circle"));
		shapes.Add(FileUtils.LoadFromJson<SymbolTemplate>("line"));
	}

	public List<SymbolComparationResult> CompareCustomSymbolWithAllTemplates(List<Vector3> customSymbol) {
		LogWriter.WriteLog(this, "Got new shape for comparation");
		List<SymbolComparationResult> results = new List<SymbolComparationResult>(shapes.Count);
		foreach (SymbolTemplate exampleShape in shapes) {
			SymbolComparationResult symbolsDifference = CalculateValueOfSymbolsDifference(exampleShape, customSymbol);
			LogWriter.WriteLog(this, "Result with " + symbolsDifference.Type + " is " + symbolsDifference.Value);
			results.Add(symbolsDifference);
		}
		return results;
	}

	private static SymbolComparationResult CalculateValueOfSymbolsDifference(SymbolTemplate example, List<Vector3> playersShapePoints) {
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

}