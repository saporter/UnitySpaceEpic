using UnityEngine;
using System.Collections;

/// <summary>
/// Intended to be attached to a generic Enemy Ship prefab
/// </summary>
public class SaveLoadEnemy : MonoBehaviour, ISaveLoadable {

	#region ISaveLoadable implementation
	public string Name {
		get {
			return GetComponent<UniqueID>().UniqueName; // Has no prefab
		}
	}

	public void Save (string file)
	{
		IDamageable ship = GetComponent<IDamageable> ();
		string myName = GetComponent<UniqueID> ().UniqueName;
		ES2.Save<float> (ship.CurrentHealth, file + "?tag=SaveLoadEnemy_" + myName + ":currentHealth");
	}

	public void Load (string file)
	{
		GetComponent<IDamageable> ().SetHealth (ES2.Load<float> (file + "?tag=SaveLoadEnemy_" + GetComponent<UniqueID> ().UniqueName + ":currentHealth"));
	}

	#endregion



}
