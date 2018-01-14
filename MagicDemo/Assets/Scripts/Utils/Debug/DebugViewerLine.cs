using UnityEngine;

public class DebugViewerLine {
	private string m_text;
	private DebugViewerLineColor m_color;

	public DebugViewerLine(string text, DebugViewerLineColor color) {
		m_text = text;
		m_color = color;
	}

	public string MakeText(float brightness) {
		int brightnessRounded = Mathf.RoundToInt(Mathf.Clamp01(brightness) * 255);
		return "<color=\"#" + MakeColorString(m_color) + brightnessRounded.ToString("X2") + "\">" + m_text + "</color>";
	}

	private string MakeColorString(DebugViewerLineColor color) {
		switch (color) {
			case DebugViewerLineColor.Red:
				return "FF0000";
			case DebugViewerLineColor.Green:
				return "00FF00";
			case DebugViewerLineColor.Blue:
				return "0000FF";
			case DebugViewerLineColor.White:
			default:
				return "FFFFFF";
		}
	}

}

public enum DebugViewerLineColor {
	White,
	Red,
	Green,
	Blue
}
