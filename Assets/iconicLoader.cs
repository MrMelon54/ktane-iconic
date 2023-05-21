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
		Debug.Log(iconicDataJson.text);

		OrderedDictionary d = new OrderedDictionary();

		iconicJson.iconicData j = JsonUtility.FromJson<iconicJson.iconicData>(iconicDataJson.text);
		foreach(iconicJson.Module m in j.modules) {
			List<string> s = new List<string>();
			s.Add(m.raw);
			foreach(string part in m.parts) s.Add(part);

			d.Add(m.key, s.ToArray());
		}

		d.Add(string.Empty, iconicData.BlankModule);

		iconicData.ModuleList = d;
	}
}
