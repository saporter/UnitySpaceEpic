using UnityEngine;
using System.Collections;
using System;

public class PrefabManager : MonoBehaviour, IPrefabManager {
	[SerializeField] GameObject[] shipSchematics; // Schematic prefabs with ISaveLoadable components
	[SerializeField] GameObject[] shipModules;	// IContainer prefabs

	#region IPrefabManager implementation

	public GameObject FindPrefabWithName (string prefabName)
	{
		foreach (GameObject prefab in shipSchematics) {
			if(prefab.GetComponent<ISaveLoadable>().Name == prefabName)
				return prefab;
		}

		foreach (GameObject prefab in shipModules) {
			if(prefab.GetComponent<IContainer>().Contents.name == prefabName)
				return prefab;
		}

		// Not found
		return null;
	}

	#endregion


}
