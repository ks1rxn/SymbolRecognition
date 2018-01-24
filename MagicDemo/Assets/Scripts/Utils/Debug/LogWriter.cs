using System;
using System.IO;
using UnityEngine;

public class LogWriter {

	static LogWriter() {
		ClearFileInLogsDirectory("HWLastLog.txt");
	}

	public static void WriteLog(string text) {
#if UNITY_EDITOR
		AppendLog("HWLastLog.txt", text);
#endif
	}

	public static void WriteLog(object caller, string text) {
#if UNITY_EDITOR
		AppendLog("HWLastLog.txt", caller.GetType().Name + "::" + text);
#endif
	}

	private static void AppendLog(string fileName, string textToFile) {
		string fullPath = GetLogsDirectory() + fileName;
		File.AppendAllText(fullPath, DateTime.Now.ToString("HH:mm:ss.f") + "   " + textToFile + Environment.NewLine);
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
	}

	private static void ClearFileInLogsDirectory(string fileName) {
		string fullPath = GetLogsDirectory() + fileName;
		if (File.Exists(fullPath)) {
			File.Delete(fullPath);
		}
	}

	private static string GetLogsDirectory() {
		string path = Application.dataPath + "/../../Private/TextOutput/logs/";
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}
		return path;
	}

}
