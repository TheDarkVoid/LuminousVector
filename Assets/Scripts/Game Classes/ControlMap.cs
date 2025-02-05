using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ControlMap : DefaultData {
	public List<string> controlNames = new List<string>();
	public List<KeyCode> controlKeys = new List<KeyCode>();
	
	
	public void RegisterKey(string keyName, KeyCode key)
	{
		if(controlNames.Contains(keyName))
		{
			int index = controlNames.IndexOf(keyName);
			controlNames[index] = keyName;
			controlKeys[index] = key;
		}else
		{
			controlNames.Add(keyName);
			controlKeys.Add(key);
		}
	}
	public void Clear()
	{
		controlNames.Clear();
		controlKeys.Clear();
	}
	public bool GetKey(string keyName)
	{
		return Input.GetKey(GetKeyCode(keyName));
	}

	public bool GetKeyDown(string keyName)
	{
		return Input.GetKeyDown(GetKeyCode(keyName));
	}

	public bool GetKeyUp(string keyName)
	{
		return Input.GetKeyUp(GetKeyCode(keyName));
	}

	public KeyCode GetKeyCode(string keyName)
	{
		KeyCode key = KeyCode.None;
		if(!controlNames.Contains(keyName))
		{
			ControlMap tmp = RegisterDefaultControls(this);
			controlNames = tmp.controlNames;
			controlKeys = tmp.controlKeys;
		}else
		{
			key = controlKeys[controlNames.IndexOf(keyName)];
		}
		return key;
	}

	public void LoadData()
	{
		string dir = Application.dataPath;
		LoadMap(File.ReadAllLines(dir+"/controls.cfg"));
	}

	public void SaveData()
	{
		string dir = Application.dataPath;
		string[] controlData = GetMap();
		Debug.Log(dir);
		if(File.Exists(dir+"/controls.cfg"))
		{
			File.WriteAllLines(dir+"/controls.cfg", controlData);
		}else
		{
			StreamWriter controlFile = File.CreateText(dir+"/controls.cfg");
			foreach(string l in controlData)
			{
				controlFile.WriteLine(l);
			}
			controlFile.Flush();
			controlFile.Close();
		}
	}
	
	public string[] GetMap()
	{
		ArrayList map = new ArrayList();
		map.Add("#Key Configs, For Information of the Keycodes see: http://docs.unity3d.com/Documentation/ScriptReference/KeyCode.html");
		foreach(string n in controlNames)
		{
			int index = controlNames.IndexOf(n);
			map.Add(n + ":" + ((int)controlKeys[index]));
		}
		return (string[])map.ToArray(typeof(string));
	}
	
	public void LoadMap(string[] map)
	{
//		Debug.Log("Loading KeyMap");
		foreach(string k in map)
		{
			if(!k.Contains("#") && k.Contains(":"))
			{
				string[] split = k.Split(':');
				int keyID = 0;
				int.TryParse(split[1], out keyID);
				RegisterKey(split[0], (KeyCode)(keyID));
			}
		}
	}
}
