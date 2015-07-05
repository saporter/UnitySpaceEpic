using UnityEngine;
using System.Collections;

public class SaveLoadPlayer : MonoBehaviour {
	IDamageable playerShip;

	void Awake()
	{
		Events.instance.AddListener<SaveGameEvent> (SaveData);
		Events.instance.AddListener<LoadGameEvent> (LoadData);
		playerShip = GetComponent<IDamageable> ();
	}

	void Start()
	{
		// Add schematic to shipMenu.  This is how our ship layout is interacted with by player
		GameObject schematic = GetComponent<IChassis> ().SchematicUIClone;
		schematic.transform.SetParent (GameManager.GM.ShipMenu.transform.GetChild(0).transform, false);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<SaveGameEvent> (SaveData);
		Events.instance.RemoveListener<LoadGameEvent> (LoadData);

		GameObject schematic = GetComponent<IChassis> ().SchematicUIClone;
		if(schematic != null)	// This happens when a new game is loaded (after already playing)
			Destroy(schematic);
	}

	void SaveData (SaveGameEvent e)
	{
		ES2.Save<Transform> (transform, e.File + "?tag=SavePlayer_transform");
		ES2.Save<float> (playerShip.CurrentHealth, e.File + "?tag=SavePlayer_currentHealth");
	}
	
	void LoadData (LoadGameEvent e)
	{
		ES2.Load<Transform> (e.File + "?tag=SavePlayer_transform", transform);
		float savedHealth = ES2.Load<float> (e.File + "?tag=SavePlayer_currentHealth");
		playerShip.ApplyDamage (playerShip.CurrentHealth - savedHealth);
	}
}
