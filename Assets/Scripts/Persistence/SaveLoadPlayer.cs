using UnityEngine;
using System.Collections;

public class SaveLoadPlayer : MonoBehaviour {

	void Awake()
	{
		Events.instance.AddListener<SaveGameEvent> (SaveData);
		Events.instance.AddListener<LoadGameEvent> (LoadData);
	}

	void Start()
	{
		// Add schematic to shipMenu.  This is how our ship layout is interacted with by player
		moveSchematicToUI();
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<SaveGameEvent> (SaveData);
		Events.instance.RemoveListener<LoadGameEvent> (LoadData);

		GameObject schematic = GetComponent<IChassis> ().SchematicUI;
		if(schematic != null)	// This happens when a new game is loaded (after already playing)
			Destroy(schematic);
	}

	void SaveData (SaveGameEvent e)
	{
		IDamageable playerShip = GetComponent<IDamageable> ();
		ISaveLoadable schematicSaver = GetComponent<IChassis> ().SchematicUI.GetComponent<ISaveLoadable> ();

		// Save Transform and health
		ES2.Save<Transform> (transform, e.File + "?tag=SavePlayer_transform");
		ES2.Save<float> (playerShip.CurrentHealth, e.File + "?tag=SavePlayer_currentHealth");

		// Save schematic
		ES2.Save<string> (schematicSaver.Name, e.File + "?tag=SavePlayer_schematic");
		schematicSaver.Save (e.File);
	}
	
	void LoadData (LoadGameEvent e)
	{
		IPrefabManager prefabManager = GameManager.GM.GetComponentInChildren<IPrefabManager> ();
		IDamageable playerShip = GetComponent<IDamageable> ();
		IChassis chassis = GetComponent<IChassis> ();

		ES2.Load<Transform> (e.File + "?tag=SavePlayer_transform", transform);
		playerShip.SetHealth (ES2.Load<float> (e.File + "?tag=SavePlayer_currentHealth"));

		// Load schematic
		if (chassis.SchematicUI != null) {
			Destroy (chassis.SchematicUI);
		}
		GameObject schematic = prefabManager.FindPrefabWithName (ES2.Load<string> (e.File + "?tag=SavePlayer_schematic"));
		schematic = Instantiate<GameObject> (schematic);
		schematic.GetComponent<ISaveLoadable> ().Load (e.File);
		chassis.SchematicUI = schematic;
	}

	private void moveSchematicToUI()
	{
		GameObject schematic = GetComponent<IChassis> ().SchematicUI;
		schematic.transform.SetParent (GameManager.GM.ShipMenu.transform.GetChild(0).transform, false);
	}
}
