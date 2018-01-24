using System.IO;
using UnityEngine;

class FileUtils {

	public static T LoadFromJson<T>(string name) {
		TextAsset settingsJson = Resources.Load<TextAsset>(name);
		T entity = JsonUtility.FromJson<T>(settingsJson.text);
		return (T)entity;
	}

	public static void WriteFileToPrivateDirectory(string fileName, string textToFile) {
		string fullPath = GetPrivateDirectory() + fileName;
		File.WriteAllText(fullPath, textToFile);
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
	}

	private static string GetPrivateDirectory() {
		string path = Application.dataPath + "/../../Private/TextOutput/";
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}
		return path;
	}

}