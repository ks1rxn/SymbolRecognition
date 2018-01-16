using UnityEngine;

public class DebugViewerLine {
	private string text;
	private DebugViewerLineColor color;

	public DebugViewerLine(string text, DebugViewerLineColor color) {
		this.text = text;
		this.color = color;
	}

	public string MakeText(float brightness) {
		int brightnessRounded = Mathf.RoundToInt(Mathf.Clamp01(brightness) * 255);
		return "<color=\"#" + MakeColorString(color) + brightnessRounded.ToString("X2") + "\">" + text + "</color>";
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
