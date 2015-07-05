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
	}
	
	void LoadData (LoadGameEvent e)
	{
		foreach (IAddToMapElement element in mapElements) {
			if(ES2.Exists(e.File + "?tag=SaveMap_" + mapName + ":" + element.ElementName + ":" + "elementAdded") &&
			   ES2.Load<bool>(e.File + "?tag=SaveMap_" + mapName + ":" + element.ElementName + ":" + "elementAdded")){
				element.AddElement();
			}
		}
	}
}
