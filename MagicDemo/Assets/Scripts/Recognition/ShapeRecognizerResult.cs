
public class ShapeRecognizerResult {
	public const float MinPassValue = 5;

	private ShapeType m_type;
	private float m_value;

	public ShapeRecognizerResult() {
		m_type = ShapeType.Circle;
		m_value = float.MaxValue;
	}

	public ShapeType Type {
		get {
			return m_type;
		}
		set {
			m_type = value;
		}
	}

	public float Value {
		get {
			return m_value;
		}
		set {
			m_value = value;
		}
	}

}

public enum ShapeType {
	Circle,
	S,
	Line
}
