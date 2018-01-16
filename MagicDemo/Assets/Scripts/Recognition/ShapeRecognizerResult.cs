
public class ShapeRecognizerResult {
	public const float MinPassValue = 20;

	private ShapeType type;
	private float value;

	public ShapeRecognizerResult() {
		type = ShapeType.Circle;
		value = float.MaxValue;
	}

	public ShapeType Type {
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

public enum ShapeType {
	Circle,
	S,
	Line
}
