using UnityEngine;
using System.Collections;

// This script is applied to SimpleChassis schematic 
public class SaveLoadChassisSchematic : MonoBehaviour, ISaveLoadable {
	[SerializeField] string _name;
	[SerializeField] GameObject primaryWeapon;
	[SerializeField] GameObject secondaryWeapon;
	[SerializeField] GameObject engine;

	#region ISaveLoadable implementation
	public string Name {
		get {
			return _name; 
		}
	}

	public void Save (string file)
	{
		IContainer module = primaryWeapon.transform.childCount != 0 ? primaryWeapon.transform.GetChild(0).GetComponent<IContainer> () : null;
		if (module != null)
			ES2.Save<string> (module.Contents.name, file + "?tag=SaveLoadChassisSchematic_primary");
		else if (ES2.Exists (file + "?tag=SaveLoadChassisSchematic_primary")) {
			ES2.Delete(file + "?tag=SaveLoadChassisSchematic_primary");
		}
		module = secondaryWeapon.transform.childCount != 0 ? secondaryWeapon.transform.GetChild(0).GetComponent<IContainer> (): null;
		if (module != null)
			ES2.Save<string> (module.Contents.name, file + "?tag=SaveLoadChassisSchematic_secondary");
		else if (ES2.Exists (file + "?tag=SaveLoadChassisSchematic_secondary")) {
			ES2.Delete(file + "?tag=SaveLoadChassisSchematic_secondary");
		}
		module = engine.transform.childCount != 0 ? engine.transform.GetChild(0).GetComponent<IContainer> () : null;
		if (module != null)
			ES2.Save<string> (module.Contents.name, file + "?tag=SaveLoadChassisSchematic_engine");
		else if (ES2.Exists (file + "?tag=SaveLoadChassisSchematic_engine")) {
			ES2.Delete(file + "?tag=SaveLoadChassisSchematic_engine");
		}

	}
	public void Load (string file)
	{
		IPrefabManager prefabManager = GameManager.GM.GetComponentInChildren<IPrefabManager> ();

		GameObject module = null;
		if (ES2.Exists (file + "?tag=SaveLoadChassisSchematic_primary"))
			module = prefabManager.FindPrefabWithName(ES2.Load<string> (file + "?tag=SaveLoadChassisSchematic_primary"));
		if (module != null) {
			module = Instantiate<GameObject> (module);
			module.transform.SetParent(primaryWeapon.transform);
		}

		module = null;
		if (ES2.Exists (file + "?tag=SaveLoadChassisSchematic_secondary"))
			module = prefabManager.FindPrefabWithName(ES2.Load<string> (file + "?tag=SaveLoadChassisSchematic_secondary"));
		if (module != null) {
			module = Instantiate<GameObject> (module);
			module.transform.SetParent(secondaryWeapon.transform);
		}

		module = null;
		if (ES2.Exists (file + "?tag=SaveLoadChassisSchematic_engine"))
			module = prefabManager.FindPrefabWithName(ES2.Load<string> (file + "?tag=SaveLoadChassisSchematic_engine"));
		if (module != null) {
			module = Instantiate<GameObject> (module);
			module.transform.SetParent(engine.transform);
		}

	}

	#endregion


}
