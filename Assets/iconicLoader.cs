using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using UnityEngine;

public class iconicLoader : MonoBehaviour { 
	public static bool ListReady = false;
	public TextAsset data;
	void Start () {
		if(iconicData.ModuleList != null) return;

		Debug.Log("Starting iconicLoader.cs");
		iconicData.ModuleList = LoadJson(data.text);
		ListReady = true;
	}

	public static iconicJson.iconicData ParseJson(string text)
	{
		return JsonConvert.DeserializeObject<iconicJson.iconicData>(text);
	}

	public static OrderedDictionary LoadJson(string text) {
		OrderedDictionary d = new OrderedDictionary();

		iconicJson.iconicData j = ParseJson(text);
		foreach(iconicJson.Module m in j.modules) {
			List<string> s = new List<string>
            {
                m.raw
            };
			foreach(string part in m.parts) s.Add(part);

			d.Add(m.key, s.ToArray());
		}

		d.Add(string.Empty, iconicData.BlankModule);

		return d;
	}
}
