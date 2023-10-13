using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.IO;

public class iconicLoader : MonoBehaviour { 
	void Start () {
		if(iconicData.ModuleList != null) return;

		Debug.Log("Starting iconicLoader.cs");
		TextAsset iconicDataJson = Resources.Load<TextAsset>("iconicData");
		iconicData.ModuleList = LoadJson(iconicDataJson.text);
	}

	public static iconicJson.iconicData ParseJson(String text)
	{
		return JsonUtility.FromJson<iconicJson.iconicData>(text);
	}

	public static OrderedDictionary LoadJson(String text) {
		OrderedDictionary d = new OrderedDictionary();

		iconicJson.iconicData j = ParseJson(text);
		foreach(iconicJson.Module m in j.modules) {
			List<string> s = new List<string>();
			s.Add(m.raw);
			foreach(string part in m.parts) s.Add(part);

			d.Add(m.key, s.ToArray());
		}

		d.Add(string.Empty, iconicData.BlankModule);

		return d;
	}
}
