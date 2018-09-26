using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;


[System.Serializable]
public class LoadSave
{



	private int currentLevelSafe,highestLevelSafe;
	private static string SaveFile => $"{Application.persistentDataPath}/save.json";
	
	
	
	
	
	private struct SaveData
	{
		public int currentLevelSafe;
		public int highestLevelSafe;
	}

	
	
	    
	
	public void Safe(int currentLevel, int highestLevel)
	{
		
		
		SaveData data = new SaveData {
			currentLevelSafe = currentLevel,
				highestLevelSafe = highestLevel
		};

		
		StreamWriter sw = new StreamWriter(SaveFile);
		sw.WriteLine(JsonUtility.ToJson(data));
		sw.Close();
		
		
		
	}


	public void Load(out int currentLevel, out int highestLevel)
	{
		
		StreamReader sr = new StreamReader(SaveFile);
		SaveData save = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
		sr.Close();
		currentLevel = save.currentLevelSafe;
		highestLevel = save.highestLevelSafe;
	
		
		
	}


	public bool DoesSaveFileExist()
	{

		return File.Exists(SaveFile);
	}








}
