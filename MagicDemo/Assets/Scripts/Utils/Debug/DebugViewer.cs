using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class DebugViewer : MonoBehaviour {
	[SerializeField]
	private TextMesh m_textOutput;
	[SerializeField, Range(0, 1)]
	private float m_textBrightness;

	private DateTime m_lastTextShownTime;
	private int m_secondsUntilTextHide;

	[UsedImplicitly]
	protected void Awake() {
		m_textOutput.richText = true;
		m_textOutput.text = "";
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
		m_secondsUntilTextHide = secondsUntilHide;
		ProcessDebugText(new List<DebugViewerLine>{text});
	}

	public void ShowLines(List<DebugViewerLine> text, int secondsUntilHide = 0) {
		m_secondsUntilTextHide = secondsUntilHide;
		ProcessDebugText(text);
	}

	private void ProcessDebugText(List<DebugViewerLine> lines) {
		StringBuilder textForOutput = new StringBuilder();
		foreach (DebugViewerLine line in lines) {
			textForOutput.Append(line.MakeText(m_textBrightness) + Environment.NewLine);
		}
		m_textOutput.text = textForOutput.ToString();
		m_lastTextShownTime = DateTime.Now;
	}

	private void CheckForTimeToHideText() {
		if (m_secondsUntilTextHide == 0) {
			return;
		}
		if (m_lastTextShownTime + new TimeSpan(0, 0, m_secondsUntilTextHide) <= DateTime.Now) {
			m_textOutput.text = "";
		}
	}

	public bool IsActive() {
		return gameObject.activeInHierarchy;
	}

}
