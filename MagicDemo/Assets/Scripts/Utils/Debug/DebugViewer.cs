using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class DebugViewer : MonoBehaviour {
	[SerializeField]
	private TextMesh textOutput;
	[SerializeField, Range(0, 1)]
	private float textBrightness;

	private DateTime lastTextShownTime;
	private int secondsUntilTextHide;

	[UsedImplicitly]
	protected void Awake() {
		textOutput.richText = true;
		textOutput.text = "";
	}

	[UsedImplicitly]
	protected void OnEnable() {
		StartCoroutine(CheckForTimeToHideTextCoroutine());
	}

	private IEnumerator CheckForTimeToHideTextCoroutine() {
		while (true) {
			CheckForTimeToHideText();
			yield return new WaitForSecondsRealtime(1.0f);
		}
	}

	public void SetActive(bool active) {
		gameObject.SetActive(active);
	}

	public void ShowLine(DebugViewerLine text, int secondsUntilHide = 0) {
		secondsUntilTextHide = secondsUntilHide;
		ProcessDebugText(new List<DebugViewerLine>{text});
	}

	public void ShowLines(List<DebugViewerLine> text, int secondsUntilHide = 0) {
		secondsUntilTextHide = secondsUntilHide;
		ProcessDebugText(text);
	}

	private void ProcessDebugText(List<DebugViewerLine> lines) {
		StringBuilder textForOutput = new StringBuilder();
		foreach (DebugViewerLine line in lines) {
			textForOutput.Append(line.MakeText(textBrightness) + Environment.NewLine);
		}
		textOutput.text = textForOutput.ToString();
		lastTextShownTime = DateTime.Now;
	}

	private void CheckForTimeToHideText() {
		if (secondsUntilTextHide == 0) {
			return;
		}
		if (lastTextShownTime + new TimeSpan(0, 0, secondsUntilTextHide) <= DateTime.Now) {
			textOutput.text = "";
		}
	}

	public bool IsActive() {
		return gameObject.activeInHierarchy;
	}

}
