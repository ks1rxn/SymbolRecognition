using System.Collections.Generic;
using UnityEngine;

public class SymbolAnalyzer {
	private static readonly SymbolAnalyzer instance;
	private readonly SymbolComparator symbolComparator;

	static SymbolAnalyzer()  {
		instance = new SymbolAnalyzer();
	}

	public static SymbolAnalyzer Instance {
		get {
			return instance;
		}
	}

	private SymbolAnalyzer() {
		symbolComparator = new SymbolComparator();
	}

	public SymbolAnalyzerResult Analyze(List<Vector3> points) {
		List<SymbolComparationResult> comparationResults = symbolComparator.CompareShapeWithAllExamples(points);
		SymbolComparationResult bestResult = GetBestResult(comparationResults);
		SymbolAnalyzerResult resultOfAnalyze = new SymbolAnalyzerResult(bestResult.Type, bestResult.Value);
		return resultOfAnalyze;
	}

	private static SymbolComparationResult GetBestResult(List<SymbolComparationResult> comparationResults) {
		if (comparationResults.Count == 0) {
			return null;
		}
		SymbolComparationResult bestResult = comparationResults[0];
		for (int i = 1; i != comparationResults.Count; i++) {
			if (comparationResults[i].Value < bestResult.Value) {
				bestResult = comparationResults[i];
			}
		}
		return bestResult;
	}

}
