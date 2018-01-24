using System.Collections.Generic;
using UnityEngine;

public class SymbolTemplate {
	public SymbolType Type;
	public List<Vector3> Points;

	public SymbolTemplate(SymbolType type, List<Vector3> points) {
		Points = points;
		Type = type;
	}

}

public enum SymbolType {
	Circle,
	S,
	Line
}
