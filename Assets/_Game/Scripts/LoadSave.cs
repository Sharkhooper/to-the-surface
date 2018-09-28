using System.IO;
using UnityEngine;

[System.Serializable]
public static class LoadSave {
	public static bool DoesSaveFileExist => File.Exists(SaveFile);

	private static string SaveFile => $"{Application.persistentDataPath}/save.json";

	private struct SaveData {
		public int currentLevelSafe;
		public int highestLevelSafe;
	}

	public static void Save(int currentLevel, int highestLevel) {
		SaveData data = new SaveData {
			currentLevelSafe = currentLevel,
			highestLevelSafe = highestLevel
		};

		StreamWriter sw = new StreamWriter(SaveFile);
		sw.WriteLine(JsonUtility.ToJson(data));
		sw.Close();
	}

	public static void Load(out int currentLevel, out int highestLevel) {
		StreamReader sr = new StreamReader(SaveFile);
		SaveData save = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
		sr.Close();
		currentLevel = save.currentLevelSafe;
		highestLevel = save.highestLevelSafe;
	}
}
