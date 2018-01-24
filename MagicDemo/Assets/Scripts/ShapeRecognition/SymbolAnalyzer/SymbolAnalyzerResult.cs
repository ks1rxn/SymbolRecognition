
public class SymbolAnalyzerResult {
	public const float MinPassValue = 20;

	private SymbolType symbolType;
	private float comparationValue;

	public SymbolAnalyzerResult(SymbolType symbolType, float comparationValue) {
		this.symbolType = symbolType;
		this.comparationValue = comparationValue;
	}

	public SymbolType SymbolType {
		get {
			return symbolType;
		}
	}

	public bool IsPassed() {
		return comparationValue < MinPassValue;
	}

}
