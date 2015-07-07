using UnityEngine;
using System.Collections;

public class SaveLoadMap : MonoBehaviour {
	IAddToMapElement[] mapElements;
	string mapName;

	void Awake()
	{
		Events.instance.AddListener<SaveGameEvent> (SaveData);
		Events.instance.AddListener<LoadGameEvent> (LoadData);
		mapElements = GetComponentsInChildren<IAddToMapElement> ();
		mapName = GetComponent<UniqueID> ().UniqueName;
	}
	
	void OnDestroy()
	{
		Events.instance.RemoveListener<SaveGameEvent> (SaveData);
		Events.instance.RemoveListener<LoadGameEvent> (LoadData);
	}
	
	void SaveData (SaveGameEvent e)
	{
		foreach (IAddToMapElement element in mapElements) {
			ES2.Save<bool>(element.ElementAdded, e.File + "?tag=SaveMap_" + mapName + ":" + element.ElementName + ":" + "elementAdded");
		}

		// Can't use GetComponentsInChildren() because it will do a depth first search and find things more than 1 child deep
		ISaveLoadable saver = null;
		for (int i = 0; i < transform.childCount; ++i) {
			saver = transform.GetChild(i).gameObject.GetComponent<ISaveLoadable>();
			if(saver != null)
				saver.Save(e.File);
		}
	}
	
	void LoadData (LoadGameEvent e)
	{
		foreach (IAddToMapElement element in mapElements) {
			if(ES2.Exists(e.File + "?tag=SaveMap_" + mapName + ":" + element.ElementName + ":" + "elementAdded") &&
			   ES2.Load<bool>(e.File + "?tag=SaveMap_" + mapName + ":" + element.ElementName + ":" + "elementAdded")){
				element.AddElement();
			}
		}

		// Can't use GetComponentsInChildren() because it will do a depth first search and find things more than 1 child deep
		ISaveLoadable loader = null;
		for (int i = 0; i < transform.childCount; ++i) {
			loader = transform.GetChild(i).gameObject.GetComponent<ISaveLoadable>();
			if(loader != null)
				loader.Load(e.File);
		}
	}
}
