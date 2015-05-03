using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	private IWeapon[] weapons;
	private IEngine[] engines;

	public IShipSystems systems;			

	void Awake()
	{
		systems = GetComponent<IShipSystems> ();
//		navigation = GetComponent<INavigationSystem> ();
		if (systems == null) {
			Debug.Log("IShipSystems is null.  Adding a default component.");
			systems = gameObject.AddComponent<PrototypeShipSystems>();
		}
//		if (navigation == null) {
//			Debug.Log("INavigationSystem is null.  Adding a default component.");
//			navigation = gameObject.AddComponent<OrbitMover>();
//		}

		weapons = GetComponentsInChildren<IWeapon> ();
		Events.instance.AddListener<EnemyModuleSelectedEvent> (FireOn);
	}



	void FireOn(EnemyModuleSelectedEvent e)
	{
		foreach (IWeapon w in weapons) {
			w.FireOn(e.target, systems);
		}
	}
}
