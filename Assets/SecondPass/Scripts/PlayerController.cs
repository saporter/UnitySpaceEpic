using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	private IWeapon[] weapons;
	private IEngine[] engines;

	public IShipSystems systems;			// another component or gameobject must register itself in Awake()
	public INavigationSystem navigation;		// another component or gameobject must register itself in Awake()  
	public LayerMask floorMask;

	void Awake()
	{
		systems = GetComponent<IShipSystems> ();
		navigation = GetComponent<INavigationSystem> ();
		if (systems == null) {
			Debug.Log("IShipSystems is null.  Adding a default component.");
			systems = gameObject.AddComponent<PrototypeShipSystems>();
		}
		if (navigation == null) {
			Debug.Log("INavigationSystem is null.  Adding a default component.");
			navigation = gameObject.AddComponent<OrbitMover>();
		}

		weapons = GetComponentsInChildren<IWeapon> ();
		Events.instance.AddListener<EnemyModuleSelectedEvent> (FireOn);
	}

	void Update()
	{
		if (Input.GetMouseButton (1)) 
		{
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			if (Physics.Raycast (camRay, out floorHit, 300f, floorMask)) 
			{
				Events.instance.Raise(new PlacingMovementMarkerEvent(floorHit.point));
			}
		}
		else if(Input.GetMouseButtonUp (1))
		{
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			
			if (Physics.Raycast (camRay, out floorHit, 300f, floorMask)) 
			{
				systems.Target = GameObject.FindWithTag("Enemy");
				Events.instance.Raise(new MovementMarkerPlacedEvent(floorHit.point));
				navigation.Move(floorHit.point, systems);
			}
		}
	}

	void FireOn(EnemyModuleSelectedEvent e)
	{
		foreach (IWeapon w in weapons) {
			w.FireOn(e.target, systems);
		}
	}
}
