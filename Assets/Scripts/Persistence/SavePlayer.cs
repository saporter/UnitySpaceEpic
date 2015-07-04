using UnityEngine;
using System.Collections;

public class SavePlayer : MonoBehaviour {
	IDamageable playerShip;

	void Awake()
	{
		Events.instance.AddListener<SaveGameEvent> (SaveData);
		Events.instance.AddListener<LoadGameEvent> (LoadData);
		playerShip = GetComponent<IDamageable> ();
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<SaveGameEvent> (SaveData);
		Events.instance.RemoveListener<LoadGameEvent> (LoadData);
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
