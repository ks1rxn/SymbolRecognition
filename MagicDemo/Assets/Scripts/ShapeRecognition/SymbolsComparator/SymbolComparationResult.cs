
public class SymbolComparationResult { 	
	private SymbolType type;
	private float value;

	public SymbolComparationResult(SymbolType type, float value) {
		this.type = type;
		this.value = value;
	}

	public SymbolType Type {
		get {
			return type;
		}
		set {
			type = value;
		}
	}

	public float Value {
		get {
			return value;
		}
		set {
			this.value = value;
		}
	}

}
