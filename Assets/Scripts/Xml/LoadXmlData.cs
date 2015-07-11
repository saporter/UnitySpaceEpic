using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LoadXmlData : MonoBehaviour, IDataLoader {
	[SerializeField] TextAsset GameAsset;
	bool readAssets = false;
	Dictionary<string, float> _floatVars = new Dictionary<string, float>();

	#region IDataLoader implementation

	public Dictionary<string, float> FloatVars {
		get {
			if(!readAssets)
				GetData();
			return _floatVars;
		}
	}

	#endregion




	// Use this for initialization
	void Awake () {
		if(!readAssets)
			GetData ();
	}

	void GetData()
	{
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (GameAsset.text);
		XmlNodeList classList = doc.GetElementsByTagName ("class");
		foreach (XmlNode classdata in classList) {
			XmlNodeList floats = classdata.ChildNodes[0].ChildNodes; // <Members> element, getting all <floats>
			foreach(XmlNode fVar in floats){
				_floatVars.Add(classdata.Attributes["name"].Value + "." + fVar.Attributes["name"].Value, float.Parse(fVar.InnerText)); 
			}
		}

		readAssets = true;
	}
}


