using UnityEngine;
using System.Collections;

/// <summary>
/// Will save GameObjects which are statically created in the editor.  Will not handle prefabs instantiated at runtime.
/// </summary>
public class SaveLoadChildGameObjects : MonoBehaviour, ISaveLoadable {
	private string[] childrenIds;

	void Awake()
	{
		UniqueID[] children = GetComponentsInChildren<UniqueID> (true);	// get children names, even if inactive
		childrenIds = new string[children.Length];
		for (int i = 0; i < childrenIds.Length; ++i) {
			childrenIds[i] = children[i].UniqueName;
		}
	}

	#region ISaveLoadable implementation

	public void Save (string file)
	{
		// Need to clear any previous save data for previously alive children first...
		foreach (string childId in childrenIds) {
			if(ES2.Exists(file + "?tag=SaveLoadChildGameObjects_" + childId))
				ES2.Delete(file + "?tag=SaveLoadChildGameObjects_" + childId);
		}

		// ...so I can resave the ones still left alive here
		for (int i = 0; i < transform.childCount; ++i) {
			// Save this child's name so we know it's alive
			ES2.Save<bool>(true, file + "?tag=SaveLoadChildGameObjects_" + transform.GetChild(i).gameObject.GetComponent<UniqueID>().UniqueName);
			// Have the child save itself
			transform.GetChild(i).gameObject.GetComponent<ISaveLoadable>().Save(file);
		}
	}

	public void Load (string file)
	{
		// The scene must reload itself for this method to work.  All original child elements are created, now we just Destroy the ones not alive during save
		for (int i = 0; i < transform.childCount; ++i) {
			if(ES2.Exists(file + "?tag=SaveLoadChildGameObjects_" + transform.GetChild(i).gameObject.GetComponent<UniqueID>().UniqueName)){
				transform.GetChild(i).gameObject.GetComponent<ISaveLoadable>().Load(file);
			}else{
				Destroy(transform.GetChild(i).gameObject);
			}
		}

	}

	public string Name {
		get {
			return ""; // Has no prefab
		}
	}

	#endregion



}
